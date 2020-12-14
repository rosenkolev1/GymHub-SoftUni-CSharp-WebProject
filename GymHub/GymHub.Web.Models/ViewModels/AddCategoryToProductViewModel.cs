using System.Collections.Generic;

namespace GymHub.Web.Models.ViewModels
{
    public class AddCategoryToProductViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public int Counter { get; set; }
        public string SelectedCategory { get; set; }
    }
}
