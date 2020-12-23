using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductCarouselViewModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}
