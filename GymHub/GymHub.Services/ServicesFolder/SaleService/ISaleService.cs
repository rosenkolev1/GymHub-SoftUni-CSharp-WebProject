using GymHub.Data.Models;
using GymHub.Data.Models.Enums;
using GymHub.Web.Models.InputModels;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.SaleService
{
    public interface ISaleService
    {
        public List<ProductSale> CreateProductSales(List<BuyProductInputModel> inputModels);
        public Task CheckoutAsync(CheckoutInputModel inputModel, string userId, List<CheckoutProductViewModel> purchasedProducts);
        public List<SaleInfoViewModel> GetSalesForUser(string userId, List<SaleFilterOptionViewModel> filterOptions);
        public SaleDetailsViewModel GetSaleDetailsViewModel(string saleId);
        public List<SaleInfoViewModel> GetSalesForAllUsers(List<SaleFilterOptionViewModel> filterOptions);
        public SaleStatus GetSaleStatus(string name);
        public Task AddSaleStatusAsync(SaleStatus saleStatus);
        public bool SaleStatusExists(string id);
        public List<SaleStatus> GetAllSaleStatuses();
        public bool SaleExists(string saleId);
        public Task ChangeSaleStatusAsync(string saleId, string saleStatusId);

        public string GetPaymentIntentId(string saleId);
        public SaleStatus GetSaleStatusById(string saleStatusId);
    }
}
