using AutoMapper;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.SaleService
{
    public class SaleService : DeleteableEntityService, ISaleService
    {
        private readonly ICountryService countryService;
        private readonly IProductService productService;
        private readonly ICartService cartService;

        public SaleService(ApplicationDbContext context, IMapper mapper, ICountryService countryService, IProductService productService, ICartService cartService)
            :base(context)
        {
            this.countryService = countryService;
            this.productService = productService;
            this.cartService = cartService;
        }

        private async Task AddProductsToSaleAsync(Sale sale, List<ProductCartViewModel> purchasedProducts)
        {
            //Add the products to the sale
            await this.context.Entry(sale).Collection(x => x.Products).LoadAsync();
            foreach (var product in purchasedProducts)
            {
                sale.Products.Add(new ProductSale { ProductId = product.Id, Quantity = product.Quantity, Sale = sale });
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
                Country = this.countryService.GetCountryByCode(inputModel.CountryCode)
            };

            await this.context.AddAsync(newSale);
            await this.context.SaveChangesAsync();

            return newSale;
        }

        public List<ProductSale> CreateProductSales(List<BuyProductInputModel> inputModels)
        {
            throw new NotImplementedException();
        }

        public async Task CheckoutAsync(CheckoutInputModel inputModel, string userId, List<ProductCartViewModel> purchasedProducts)
        {
            var newSale = await this.CreateNewSaleAsync(inputModel, userId);
            await this.AddProductsToSaleAsync(newSale, purchasedProducts);

            await this.cartService.ClearCartAsync(userId);

            await this.context.SaveChangesAsync();
        }
    }
}
