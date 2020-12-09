using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CheckoutProductViewModel> ProductsInfo { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public decimal ShippingPrice => TotalPrice < 150 ? 6 : 0;

        public List<Country> Countries { get; set; } 

        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}
