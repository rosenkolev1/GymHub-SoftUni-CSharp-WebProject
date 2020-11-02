using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class ProductImage
    {
        public ProductImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Foreign Keys
        [Required]
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        //Simple Properties
        //Path to picture
        [Required]
        public string Picture { get; set; }
    }
}