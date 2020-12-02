using GymHub.Common;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class AddProductInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.ProductNameLengthMax)]
        [MinLength(GlobalConstants.ProductNameLengthMin)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ProductNameLengthMax)]
        [MinLength(GlobalConstants.ProductNameLengthMin)]
        public string Model { get; set; }

        [Required]
        public string MainImage { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price is either too high or is below or equal to 0")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ProductNameLengthMax)]
        [MinLength(GlobalConstants.ProductNameLengthMin)]
        public string Description { get; set; }

        [Required]
        public int Warranty { get; set; }

        [Required]
        public int QuantityInStock { get; set; }
    }
}
