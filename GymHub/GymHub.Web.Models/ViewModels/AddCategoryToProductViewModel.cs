using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class AddCategoryToProductViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public int Counter { get; set; }
        public string SelectedCategory { get; set; }
    }
}
