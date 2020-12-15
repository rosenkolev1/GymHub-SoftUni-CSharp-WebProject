using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductSaleDetailsViewModel
    {
        public string SaleId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
