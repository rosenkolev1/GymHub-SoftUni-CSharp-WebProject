using AutoMapper;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.SaleService;
using GymHub.Web.AuthorizationPolicies;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymHub.Web.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly ISaleService saleService;
        private readonly IProductService productService;
        private readonly ICountryService countryService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly SessionService sessionService;
        private readonly PaymentIntentService paymentIntentService;
        private readonly RefundService refundService;

        public SalesController
            (ISaleService saleService, IProductService productService, ICountryService countryService,
            IPaymentMethodService paymentMethodService, ICartService cartService, IUserService userService, IMapper mapper,
            SessionService sessionService, PaymentIntentService paymentIntentService, RefundService refundService)
        {
            this.saleService = saleService;
            this.productService = productService;
            this.countryService = countryService;
            this.paymentMethodService = paymentMethodService;
            this.cartService = cartService;
            this.userService = userService;
            this.mapper = mapper;
            this.sessionService = sessionService;
            this.refundService = refundService;
            this.paymentIntentService = paymentIntentService;
        }

        public IActionResult Checkout()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var cardProducts = this.cartService.GetAllProductsForCheckoutViewModel(currentUserId);

            //Check if the user has any products in their cart
            if (cardProducts.Count == 0)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "You do not have any products in your cart");

                return this.RedirectToAction("All", "Carts");
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                ProductsInfo = cardProducts,
                TotalPrice = cardProducts.Sum(x => x.SinglePrice * x.Quantity),
                Countries = this.countryService.GetAllCountries(),
                PaymentMethods = this.paymentMethodService.GetAllPaymentMethods()
            };

            var complexModel = new ComplexModel<CheckoutInputModel, CheckoutViewModel>
            {
                ViewModel = checkoutViewModel
            };

            if(this.TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(this.TempData, this.ModelState);
            }

            if(this.TempData[GlobalConstants.InputModelFromPOSTRequestType]?.ToString() == nameof(CheckoutInputModel))
            {
                complexModel.InputModel = JsonSerializer.Deserialize<CheckoutInputModel>(this.TempData[GlobalConstants.InputModelFromPOSTRequest]?.ToString());
            }

            return this.View(complexModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ComplexModel<CheckoutInputModel, CheckoutViewModel> complexModel)
        {
            this.TempData[GlobalConstants.InputModelFromPOSTRequest] = JsonSerializer.Serialize(complexModel.InputModel);
            this.TempData[GlobalConstants.InputModelFromPOSTRequestType] = nameof(CheckoutInputModel);

            //Validate model without checking the database
            if(this.ModelState.IsValid == false)
            {
                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "An error occured while processing your request");

                return this.Json(new { RedirectPath = $"/Sales/Checkout" });
            }

            //Check if the payment method exists
            if(this.paymentMethodService.PaymentMethodExistsById(complexModel.InputModel.PaymentMethodId) == false)
            {
                this.ModelState.AddModelError("", "This payment method doesn't exist");

                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                //Set notification
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Selected payment method doesn't exist");

                return this.Json(new { RedirectPath = $"/Sales/Checkout" });
            }

            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var cartProducts = this.cartService.GetAllProductsForCheckoutViewModel(currentUserId);

            var inputModel = complexModel.InputModel;

            //Check if the user has any products in their cart
            if (this.cartService.GetNumberOfProductsInCart(currentUserId) == 0)
            {
                //Set notification
                NotificationHelper.SetNotification(TempData, NotificationType.Error, "You do not have any products in your cart");

                return this.Json(new { RedirectPath = $"/Carts/All" });
            }

            //Check if the products' quantities exceed the amount of units in stock
            foreach (var product in cartProducts)
            {
                if(product.Quantity > product.QuantityInStock)
                {
                    this.ModelState.AddModelError("", $"{product.Name}:{product.Model} has less units available than you are currently trying to buy");

                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, $"{product.Name}:{product.Model} has less units available than you are currently trying to buy");
                }
            }

            if(this.ModelState.IsValid == false)
            {
                //Serialize errors from modelstate
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);

                return this.Json(new { RedirectPath = $"/Carts/All" });
            }

            var paymentMethod = this.paymentMethodService.GetPaymentMethod(complexModel.InputModel.PaymentMethodId);

            if (paymentMethod == GlobalConstants.PaymentMethodDebitOrCreditCard)
            {
                //Set the temp data for a successful sale
                var confirmSaleToken = SetTempDataForSale(complexModel.InputModel, cartProducts);

                //Setup Stripe payment session
                var sessionId = await CreateStripeSession(cartProducts, confirmSaleToken);

                //If it is null for whatever reason, then just return an error notification and redirect
                if (sessionId == null)
                {
                    //Clear temp data with purchase info
                    this.TempData.Clear();

                    //Set notification
                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Something went wrong with the purchase. Purchase reverted. You have not been charged.");

                    return this.Json(new { RedirectPath = $"/Carts/All" });
                }

                return this.Json(new { id = sessionId, PaymentMethod = GlobalConstants.PaymentMethodDebitOrCreditCard });
            }
            else if(paymentMethod == GlobalConstants.PaymentMethodCashOnDelivery)
            {
                //Set the temp data for a successful sale
                var confirmSaleToken = SetTempDataForSale(complexModel.InputModel, cartProducts);

                return this.Json(new { PaymentMethod = GlobalConstants.PaymentMethodCashOnDelivery, RedirectPath = $"/Sales/CheckoutSuccess?confirmSaleToken={confirmSaleToken}&paymentMethod={GlobalConstants.PaymentMethodCashOnDelivery}" });
            }


            //If we have gotten this far, then the payment method was one of the other two, which are not supported at this moment
            NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "These two payments are not currently supported.");
            return this.Json(new { });
        }

        private string SetTempDataForSale(CheckoutInputModel saleInfo, List<CheckoutProductViewModel> products)
        {
            this.TempData[GlobalConstants.SaleInfo] = JsonSerializer.Serialize(saleInfo);
            var confirmSaleToken = Guid.NewGuid().ToString();
            this.TempData[GlobalConstants.ConfirmSaleToken] = confirmSaleToken;

            return confirmSaleToken;
        }

        private async Task<string> CreateStripeSession(List<CheckoutProductViewModel> products, string confirmSaleToken)
        {
            var currentDomain = $"{this.Request.Scheme}://{this.Request.Host}";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                    {
                      "card",
                    },
                LineItems = products
                    .Select(product => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(product.SinglePrice * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                                Images = product.ImagesUrls.Select(image => {

                                    if (image.StartsWith("/")) return $"{currentDomain}{image}";
                                    else return image;

                                }).Take(8).ToList(),
                                Description = product.Description
                            },
                        },
                        Quantity = product.Quantity
                    }).ToList(),
                Mode = "payment",
                SuccessUrl = currentDomain + $"/Sales/CheckoutSuccess?confirmSaleToken={confirmSaleToken}&paymentMethod={GlobalConstants.PaymentMethodDebitOrCreditCard}",
                CancelUrl = currentDomain + $"/Sales/CheckoutCancel",
                PaymentIntentData = new SessionPaymentIntentDataOptions { CaptureMethod = "manual"}
            };

            var session = await this.sessionService.CreateAsync(options, new RequestOptions { IdempotencyKey = Guid.NewGuid().ToString() });

            this.TempData[GlobalConstants.PaymentIntentId] = session.PaymentIntentId;

            return session.Id;
        }

        public async Task<IActionResult> CheckoutCancel()
        {
            //Clear sale temp data info
            this.TempData.Clear();

            var paymentIntentId = this.TempData[GlobalConstants.PaymentIntentId]?.ToString();

            if (paymentIntentId != null)
            {
                //Cancel the fund capture
                await this.paymentIntentService.CancelAsync(paymentIntentId);
            }

            NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "You have canceled your payment");

            return this.RedirectToAction(nameof(Checkout));
        }

        public async Task<IActionResult> CheckoutSuccess(string confirmSaleToken, string paymentMethod)
        {
            if(this.TempData[GlobalConstants.ConfirmSaleToken]?.ToString() == confirmSaleToken)
            {
                var currentUserId = this.userService.GetUserId(this.User.Identity.Name);
                var saleInfo = JsonSerializer.Deserialize<CheckoutInputModel>(this.TempData[GlobalConstants.SaleInfo]?.ToString());
                var saleProductsInfo = this.cartService.GetAllProductsForCheckoutViewModel(currentUserId);

                var anyItemExceedsStock = saleProductsInfo.Any(x => x.Quantity > x.QuantityInStock);
                string paymentIntentId = this.TempData[GlobalConstants.PaymentIntentId]?.ToString();

                //Check if the products wanted are in stock for when the user is buying with a credit or debit card
                if (paymentIntentId != null && anyItemExceedsStock)
                {
                    //Cancel the fund capture
                    await this.paymentIntentService.CancelAsync(paymentIntentId);

                    //Set notifications
                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "One or more of your ordered items are out of stock. Could not proceed to checkout. You have not been charged.");

                    return this.RedirectToAction("All", "Carts");
                }
                //If you aren't buying with a debit or credit card
                else if(anyItemExceedsStock)
                {
                    //Set notifications
                    NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "One or more of your ordered items are out of stock. Could not proceed to checkout. You have not been charged.");

                    return this.RedirectToAction("All", "Carts");
                }

                //Capture the funds
                if (paymentIntentId != null && paymentMethod == GlobalConstants.PaymentMethodDebitOrCreditCard)
                {
                    saleInfo.PaymentIntentId = paymentIntentId;
                }

                await this.saleService.CheckoutAsync(saleInfo, currentUserId, saleProductsInfo);

                NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "You successfully completed your order. You can review your order information in the profile tab");

                return this.RedirectToAction("All", "Carts");
            }
            else
            {
                return this.NotFound();
            }
        }

        public IActionResult All()
        {
            var currentUserId = this.userService.GetUserId(this.User.Identity.Name);

            var saleInfoViewModels = this.saleService.GetSalesForUser(currentUserId);

            return this.View(saleInfoViewModels);
        }


        public IActionResult Details(string saleId)
        {
            var saleDetails = this.saleService.GetSaleDetailsViewModel(saleId);

            return this.View(saleDetails);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public ActionResult Search()
        {
            var saleInfoViewModels = this.saleService.GetSalesForAllUsers();

            return this.View("/Views/Sales/All.cshtml", saleInfoViewModels);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        public IActionResult ChangeSaleStatus(string saleId)
        {
            if (this.saleService.SaleExists(saleId) == false)
            {
                return this.NotFound();
            }

            if(this.TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(this.TempData, this.ModelState);
            }

            var viewModel = this.saleService.GetAllSaleStatuses();
            var inputModel = new ChangeSaleStatusInputModel { SaleId = saleId };

            if(this.TempData["NewSaleStatusId"] != null)
            {
                inputModel.NewSaleStatusId = this.TempData["NewSaleStatusId"].ToString();
            }

            var complexModel = new ComplexModel<ChangeSaleStatusInputModel, List<SaleStatus>> { ViewModel = viewModel, InputModel = inputModel };

            return this.View(complexModel);
        }

        [Authorize(Policy = nameof(AuthorizeAsAdminHandler))]
        [HttpPost]
        public async Task<IActionResult> ChangeSaleStatus(ComplexModel<ChangeSaleStatusInputModel, List<SaleStatus>> complexModel)
        {
            if(this.ModelState.IsValid == false)
            {
                //Set up temp data with model state and input model
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);
                this.TempData["NewSaleStatusId"] = complexModel.InputModel.NewSaleStatusId;

                return this.RedirectToAction(nameof(ChangeSaleStatus), new { saleId = complexModel.InputModel.SaleId });
            }

            //Check if status and sale exist
            if(this.saleService.SaleStatusExists(complexModel.InputModel.NewSaleStatusId) == false)
            {
                this.ModelState.AddModelError("InputModel.NewSaleStatusId", "This sale status doesn't exist");
            }

            if (this.ModelState.IsValid == false)
            {
                //Set up temp data with model state and input model
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);
                this.TempData["NewSaleStatusId"] = complexModel.InputModel.NewSaleStatusId;

                return this.RedirectToAction(nameof(ChangeSaleStatus), new { saleId = complexModel.InputModel.SaleId });
            }

            //Capture the payment intent and officially complete the sale and charge the customer if the paymentStatus is confirmed
            var newSaleStatus = this.saleService.GetSaleStatusById(complexModel.InputModel.NewSaleStatusId);
            if(newSaleStatus.Name == GlobalConstants.ConfirmedSaleStatus)
            {
                var paymentIntentId = this.saleService.GetPaymentIntentId(complexModel.InputModel.SaleId);
                if (paymentIntentId != null)
                {
                    var paymentIntent = await this.paymentIntentService.GetAsync(paymentIntentId);
                    if(paymentIntent.Status == "requires_capture")
                    {
                        await this.paymentIntentService.CaptureAsync(paymentIntentId);
                    }
                }
            }
            //Set free the payment intent and the funds if the paymentStatus is declined
            else if(newSaleStatus.Name == GlobalConstants.DeclinedSaleStatus)
            {
                var paymentIntentId = this.saleService.GetPaymentIntentId(complexModel.InputModel.SaleId);
                if (paymentIntentId != null)
                {
                    var paymentIntent = await this.paymentIntentService.GetAsync(paymentIntentId);
                    if (paymentIntent.Status == "requires_capture")
                    {
                        await this.paymentIntentService.CancelAsync(paymentIntentId);
                    }
                }
            }

            await this.saleService.ChangeSaleStatusAsync(complexModel.InputModel.SaleId, complexModel.InputModel.NewSaleStatusId);

            //Set up success notification
            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, $"You have successfully changed sale status");

            return this.RedirectToAction(nameof(Search));
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        [HttpPost]
        public async Task<IActionResult> Refund(string saleId)
        {
            if(this.saleService.SaleExists(saleId) == false)
            {
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Sale doesn't exist");
                return this.RedirectToAction(nameof(All));
            }

            var paymentIntentId = this.saleService.GetPaymentIntentId(saleId);
            await this.refundService.CreateAsync(new RefundCreateOptions { PaymentIntent = paymentIntentId });
            await this.saleService.ChangeSaleStatusAsync(saleId, this.saleService.GetSaleStatus(GlobalConstants.RefundedSaleStatus).Id);

            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "Sale successfully refunded");

            return this.RedirectToAction(nameof(All));
        }
    }
}
