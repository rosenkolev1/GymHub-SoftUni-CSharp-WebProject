using System.Collections.Generic;

namespace GymHub.Web.Models.ViewModels
{
    public class AllSalesInfoViewModel
    {
        public List<SaleInfoViewModel> SaleInfoViewModels { get; set; }
        public List<SaleFilterOptionViewModel> SaleFilterOptions { get; set; }
    }
}
