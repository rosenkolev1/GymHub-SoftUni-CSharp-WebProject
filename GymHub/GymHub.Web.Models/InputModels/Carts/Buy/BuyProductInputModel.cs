using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class BuyProductInputModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price of product is invalid")]
        public decimal SinglePrice { get; set; }
    }
}
