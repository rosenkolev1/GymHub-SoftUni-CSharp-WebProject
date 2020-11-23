using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class ProductRating : IDeletableEntity
    {
        public ProductRating()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Product")]
        [Required]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("ProductComment")]
        [Required]
        public string ProductCommentId { get; set; }
        public virtual ProductComment ProductComment { get; set; }


        //Simple properties
        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
