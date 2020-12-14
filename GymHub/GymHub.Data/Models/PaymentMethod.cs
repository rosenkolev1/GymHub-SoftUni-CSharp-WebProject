using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Data.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
