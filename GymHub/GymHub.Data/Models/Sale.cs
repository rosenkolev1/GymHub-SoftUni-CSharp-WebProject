using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GymHub.Data.Models
{
    public class Sale
    {
        public Sale()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Foreign Keys
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        //Collections
        public virtual ICollection<ProductSale> Products { get; set; }

        //Simple properties
        [Required]
        public decimal TotalPrice => this.Products.Select(p => p.Product.Price * p.Quantity).Sum();
        [Required]
        public DateTime PurchasedOn { get; set; }
    }
}
