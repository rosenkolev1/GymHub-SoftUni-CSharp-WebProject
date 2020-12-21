using GymHub.Web.Models.ViewModels.Products.All;
using System.Collections.Generic;

namespace GymHub.Web.Models.ViewModels
{
    public class AllProductsViewModel
    {
        public List<ProductViewModel> ProductViewModels { get; set; }
        public PaginationViewModel PaginationViewModel { get; set; }
        public List<ProductFilterOptionsViewModel> ProductFilterOptions { get; set; }
    }
}
