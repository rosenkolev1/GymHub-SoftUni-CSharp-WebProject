using GymHub.Common;
using GymHub.Data.Models;
using GymHub.Web.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class AddProductInputModel
    {
        public string Id { get; set; }

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

        public List<string> AdditionalImages{ get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price is either too high or is below or equal to 0")]
        public decimal Price { get; set; }

        [Required]
        [MinLength(GlobalConstants.ProductNameLengthMin)]
        public string Description { get; set; }

        [Required]
        public int Warranty { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        public string ShortDescription { get; set; }

        public bool IsAdding { get; set; } = true;

        [Required]
        public List<string> CategoriesIds { get; set; } = new List<string>();
    }
}
