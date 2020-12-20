using System.Collections.Generic;

namespace GymHub.Web.Models.ViewModels
{
    public class AllProductsViewModel
    {
        public List<ProductViewModel> ProductViewModels { get; set; }
        public PaginationViewModel PaginationViewModel { get; set; }
    }
}
