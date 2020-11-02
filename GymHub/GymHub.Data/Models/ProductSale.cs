using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class ProductSale
    {
        //Foreign Keys
        [Required]
        [ForeignKey("Sale")]
        public string SaleId { get; set; }
        public virtual Sale Sale { get; set; }
        [Required]
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        //Simple properties
        [Required]
        public int Quantity { get; set; }
    }
}