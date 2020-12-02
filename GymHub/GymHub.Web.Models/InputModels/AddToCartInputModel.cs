using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.InputModels
{
    public class AddToCartInputModel
    {
        [Required]
        public string ProductId { get; set; } 
        [Range(1, 2147483647, ErrorMessage = "Cannot buy this many products.")]
        public int Quantity { get; set; }
    }
}
