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
