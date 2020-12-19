﻿using AutoMapper;
using GymHub.Common;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.SaleService
{
    public class SaleService : DeleteableEntityService, ISaleService
    {
        private readonly ICountryService countryService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IPaymentMethodService paymentMethodService;

        public SaleService(ApplicationDbContext context)
            :base(context)
        {
            
        }

        public SaleService(ApplicationDbContext context, IMapper mapper, ICountryService countryService, IProductService productService, ICartService cartService, IPaymentMethodService paymentMethodService)
            :base(context)
        {
            this.countryService = countryService;
            this.productService = productService;
            this.cartService = cartService;
            this.paymentMethodService = paymentMethodService;
        }

        private async Task AddProductsToSaleAsync(Sale sale, List<CheckoutProductViewModel> purchasedProducts)
        {
            //Add the products to the sale
            await this.context.Entry(sale).Collection(x => x.Products).LoadAsync();
            foreach (var product in purchasedProducts)
            {
                sale.Products.Add(new ProductSale { ProductId = product.Id, Quantity = product.Quantity, Sale = sale });               
            }

            await this.context.SaveChangesAsync();
        }

        private async Task SubtractQuantityInStockWithPurchaseQuantity(List<CheckoutProductViewModel> purchasedProducts)
        {
            var productIds = purchasedProducts.Select(x => x.Id);
            var products = this.context.Products
                .Where(x => productIds.Contains(x.Id));

            foreach (var product in products)
            {
                product.QuantityInStock -= purchasedProducts.First(x => x.Id == product.Id).Quantity;
            }

            await this.context.SaveChangesAsync();
        }

        private async Task<Sale> CreateNewSaleAsync(CheckoutInputModel inputModel, string userId)
        {
            var newSale = new Sale
            {
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                AdditionalInformation = inputModel.AdditionalInformation,
                Address = inputModel.Address,
                City = inputModel.City,
                CompanyName = inputModel.CompanyName,
                EmailAddress = inputModel.Email,
                PaymentMethodId = inputModel.PaymentMethodId,
                PhoneNumber = inputModel.PhoneNumber,
                Postcode = inputModel.Postcode,
                PurchasedOn = DateTime.UtcNow,
                UserId = userId,
                Country = this.countryService.GetCountryByCode(inputModel.CountryCode),
                SaleStatus = this.GetSaleStatus(GlobalConstants.PendingSaleStatus),
                Municipality = inputModel.Municipality
            };

            if(this.paymentMethodService.GetPaymentMethod(inputModel.PaymentMethodId) == GlobalConstants.PaymentMethodDebitOrCreditCard && inputModel.PaymentIntentId != null)
            {
                newSale.PaymentIntentId = inputModel.PaymentIntentId;
            }

            await this.context.AddAsync(newSale);
            await this.context.SaveChangesAsync();

            return newSale;
        }

        public List<ProductSale> CreateProductSales(List<BuyProductInputModel> inputModels)
        {
            throw new NotImplementedException();
        }

        public async Task CheckoutAsync(CheckoutInputModel inputModel, string userId, List<CheckoutProductViewModel> purchasedProducts)
        {
            //Do this first to avoid concurrency failures and clashes
            await this.SubtractQuantityInStockWithPurchaseQuantity(purchasedProducts);

            var newSale = await this.CreateNewSaleAsync(inputModel, userId);

            await this.AddProductsToSaleAsync(newSale, purchasedProducts);

            await this.cartService.ClearCartAsync(userId);

            await this.context.SaveChangesAsync();
        }

        public List<SaleInfoViewModel> GetSalesForUser(string userId)
        {
            return this.context.Sales
                .Where(x => x.UserId == userId)
                .Select(x => new SaleInfoViewModel
                {
                    BillingAccount = x.User.UserName,
                    ReceivingAccount = x.User.UserName,
                    PaymentStatus = x.SaleStatus.Name,
                    Id = x.Id,
                    PaymentMethod = x.PaymentMethod,
                    PurchasedOn = x.PurchasedOn,
                    TotalPayment = x.Products.Sum(x => x.Product.Price * x.Quantity)
                })
                .ToList();
        }

        public SaleDetailsViewModel GetSaleDetailsViewModel(string saleId)
        {
            return this.context.Sales
                .Where(x => x.Id == saleId)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Select(x => new SaleDetailsViewModel
                {
                    SaleStatus = x.SaleStatus.Name,
                    Address = x.Address,
                    EmailAddress = x.EmailAddress,
                    CompanyName = x.CompanyName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    AdditionalInformation = x.AdditionalInformation,
                    Country = x.Country,
                    ProductsSale = x.Products,
                    Postcode = x.Postcode,
                    City = x.City,
                    PaymentMethod = x.PaymentMethod,
                    PhoneNumber = x.PhoneNumber,
                    PurchasedOn = x.PurchasedOn,
                    Municipality = x.Municipality
                })
                .First();
        }

        public List<SaleInfoViewModel> GetSalesForAllUsers()
        {
            return this.context.Sales
                .Select(x => new SaleInfoViewModel
                {
                    BillingAccount = x.User.UserName,
                    ReceivingAccount = x.User.UserName,
                    PaymentStatus = x.SaleStatus.Name,
                    Id = x.Id,
                    PaymentMethod = x.PaymentMethod,
                    PurchasedOn = x.PurchasedOn,
                    TotalPayment = x.Products.Sum(x => x.Product.Price * x.Quantity)
                })
                .ToList();
        }

        public SaleStatus GetSaleStatus(string name)
        {
            return this.context.SaleStatuses.First(x => x.Name == name);
        }

        public async Task AddSaleStatusAsync(SaleStatus saleStatus)
        {
            await this.context.SaleStatuses.AddAsync(saleStatus);
            await this.context.SaveChangesAsync();
        }

        public bool SaleStatusExists(string id)
        {
            return this.context.SaleStatuses.Any(x => x.Id == id);
        }

        public List<SaleStatus> GetAllSaleStatuses()
        {
            return this.context.SaleStatuses.ToList();
        }

        public bool SaleExists(string saleId)
        {
            return this.context.Sales.Any(x => x.Id == saleId);
        }

        public async Task ChangeSaleStatusAsync(string saleId, string saleStatusId)
        {
            this.context.Sales.First(x => x.Id == saleId).SaleStatusId = saleStatusId;

            await this.context.SaveChangesAsync();
        }

        public string GetPaymentIntentId(string saleId)
        {
            return this.context.Sales
                .Where(x => x.Id == saleId)
                .Select(x => x.PaymentIntentId)
                .FirstOrDefault();
        }

        public SaleStatus GetSaleStatusById(string saleStatusId)
        {
            return this.context.SaleStatuses.First(x => x.Id == saleStatusId);
        }
    }
}
