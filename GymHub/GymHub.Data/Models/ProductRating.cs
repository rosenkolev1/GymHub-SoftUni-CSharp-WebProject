using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymHub.Data.Models
{
    public class ProductRating
    {
        //Foreign Keys
        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Product")]
        [Required]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        //Simple properties
        [Required]
        public double Rating { get; set; }
    }
}
