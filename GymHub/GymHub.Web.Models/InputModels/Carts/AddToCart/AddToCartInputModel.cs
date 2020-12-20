using System;
using System.ComponentModel.DataAnnotations;

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
