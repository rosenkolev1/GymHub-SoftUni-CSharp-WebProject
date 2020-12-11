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
        public string Model { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cannot buy this many products.")]
        public int Quantity{ get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price of product is invalid")]
        public decimal SinglePrice { get; set; }
    }
}
