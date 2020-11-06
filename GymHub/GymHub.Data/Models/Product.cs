using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Collections
        public virtual ICollection<ProductImage> AdditionalImages { get; set; }
        public virtual ICollection<ProductCart> ProductCarts { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
        public virtual ICollection<ProductRating> ProductRatings { get; set; }

        //Simple properties
        [Required]
        public string Name { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string MainImage { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        //Warranty in days
        [Required]
        public int Warranty { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
