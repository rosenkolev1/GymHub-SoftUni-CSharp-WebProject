using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Data.Models
{
    public class ProductCategory : IDeletableEntity
    {
        public ProductCategory()
        {
            this.Id = Guid.NewGuid().ToString();
        }
              
        [Key]
        public string Id { get; set; }

        //Foreign Keys
        [ForeignKey(nameof(Product))]
        [Required]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey(nameof(Category))]
        [Required]
        public string CategoryId { get; set; }

        public Category Category { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
