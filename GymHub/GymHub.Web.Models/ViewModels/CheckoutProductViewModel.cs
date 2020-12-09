using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class CheckoutProductViewModel 
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 2147483647, ErrorMessage = "Cannot buy this many products.")]
        public int Quantity{ get; set; }

        [Required]
        public decimal SinglePrice { get; set; }
    }
}
