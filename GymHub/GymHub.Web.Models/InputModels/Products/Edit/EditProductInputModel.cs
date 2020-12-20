using GymHub.Common;
using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.InputModels
{
    public class EditProductInputModel
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

        public string MainImage { get; set; }

        public List<string> AdditionalImages { get; set; }

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

        public List<string> CategoriesIds { get; set; } = new List<string>();

        public List<string> CategoriesNames { get; set; } = new List<string>();

        public bool ImagesAsFileUploads { get; set; }

        public EditProductImageUploadInputModel MainImageUploadInfo { get; set; }

        public List<EditProductImageUploadInputModel> AdditionalImagesUploadsInfo { get; set; }

    }
}
