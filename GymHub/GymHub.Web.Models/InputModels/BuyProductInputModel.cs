using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class BuyProductInputModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal SinglePrice { get; set; }
    }
}
