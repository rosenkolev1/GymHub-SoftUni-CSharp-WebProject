using GymHub.Data.Models;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.SaleService
{
    public interface ISaleService
    {
        public List<ProductSale> CreateProductSales(List<BuyProductInputModel> inputModels);
        public Task CheckoutAsync(CheckoutInputModel inputModel, string userId, List<ProductCartViewModel> purchasedProducts);
    }
}
