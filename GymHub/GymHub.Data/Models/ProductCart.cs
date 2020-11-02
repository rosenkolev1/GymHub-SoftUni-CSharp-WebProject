using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymHub.Data.Models
{
    public class ProductCart
    {
        //Foreign Keys
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [Required]
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        //Simple properties
        [Required]
        public int Quantity { get; set; }
    }
}
