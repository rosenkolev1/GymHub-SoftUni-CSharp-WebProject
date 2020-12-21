using AutoMapper;
using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using GymHub.Services;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.SaleService;
using GymHub.Web.Controllers.BaseControllers;
using GymHub.Web.Helpers.NotificationHelpers;
using GymHub.Web.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    [Area("Administration")]
    public class SalesController : BaseSaleController
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

        public ActionResult Search(List<SaleFilterOptionViewModel> SaleFilterOptions)
        {
            SaleFilterOptions =  SaleFilterOptions.DistinctBy(x => x.FilterName).ToList();

            var saleInfoViewModels = this.saleService.GetSalesForAllUsers(SaleFilterOptions);

            if (SaleFilterOptions == null || SaleFilterOptions.Count < 4) SaleFilterOptions = new List<SaleFilterOptionViewModel>(this.allSaleFilterOptions);

            var allSalesInfoViewModels = new AllSalesInfoViewModel
            {
                SaleFilterOptions = SaleFilterOptions,
                SaleInfoViewModels = saleInfoViewModels
            };

            return this.View("/Views/Sales/All.cshtml", allSalesInfoViewModels);
        }

        public IActionResult ChangeSaleStatus(string saleId)
        {
            if (this.saleService.SaleExists(saleId) == false)
            {
                return this.NotFound();
            }

            if (this.TempData[GlobalConstants.ErrorsFromPOSTRequest] != null)
            {
                ModelStateHelper.MergeModelStates(this.TempData, this.ModelState);
            }

            var viewModel = this.saleService.GetAllSaleStatuses();
            var inputModel = new ChangeSaleStatusInputModel { SaleId = saleId };

            if (this.TempData["NewSaleStatusId"] != null)
            {
                inputModel.NewSaleStatusId = this.TempData["NewSaleStatusId"].ToString();
            }

            var complexModel = new ComplexModel<ChangeSaleStatusInputModel, List<SaleStatus>> { ViewModel = viewModel, InputModel = inputModel };

            return this.View(complexModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSaleStatus(ComplexModel<ChangeSaleStatusInputModel, List<SaleStatus>> complexModel)
        {
            if (this.ModelState.IsValid == false)
            {
                //Set up temp data with model state and input model
                this.TempData[GlobalConstants.ErrorsFromPOSTRequest] = ModelStateHelper.SerialiseModelState(this.ModelState);
                this.TempData["NewSaleStatusId"] = complexModel.InputModel.NewSaleStatusId;

                return this.RedirectToAction(nameof(ChangeSaleStatus), new { saleId = complexModel.InputModel.SaleId });
            }

            //Check if status and sale exist
            if (this.saleService.SaleStatusExists(complexModel.InputModel.NewSaleStatusId) == false)
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
            if (newSaleStatus.Name == GlobalConstants.ConfirmedSaleStatus)
            {
                var paymentIntentId = this.saleService.GetPaymentIntentId(complexModel.InputModel.SaleId);
                if (paymentIntentId != null)
                {
                    var paymentIntent = await this.paymentIntentService.GetAsync(paymentIntentId);
                    if (paymentIntent.Status == "requires_capture")
                    {
                        await this.paymentIntentService.CaptureAsync(paymentIntentId);
                    }
                }
            }
            //Set free the payment intent and the funds if the paymentStatus is declined
            else if (newSaleStatus.Name == GlobalConstants.DeclinedSaleStatus)
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

        [HttpPost]
        public async Task<IActionResult> Refund(string saleId)
        {
            if (this.saleService.SaleExists(saleId) == false)
            {
                NotificationHelper.SetNotification(this.TempData, NotificationType.Error, "Sale doesn't exist");
                return this.RedirectToAction("Search", "Sales", new { area = ""});
            }

            var paymentIntentId = this.saleService.GetPaymentIntentId(saleId);
            await this.refundService.CreateAsync(new RefundCreateOptions { PaymentIntent = paymentIntentId });
            await this.saleService.ChangeSaleStatusAsync(saleId, this.saleService.GetSaleStatus(GlobalConstants.RefundedSaleStatus).Id);

            NotificationHelper.SetNotification(this.TempData, NotificationType.Success, "Sale successfully refunded");

            return this.RedirectToAction("Search", "Sales");
        }
    }
}
