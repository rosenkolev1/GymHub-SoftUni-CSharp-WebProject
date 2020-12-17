using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class AllProductsViewModel
    {
        public List<ProductViewModel> ProductViewModels { get; set; }
        public PaginationViewModel PaginationViewModel { get; set; }
    }
}
