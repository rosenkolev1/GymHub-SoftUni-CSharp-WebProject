using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymHub.Data.Models
{
    public class Sale : IDeletableEntity
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

        [Required]
        [ForeignKey(nameof(Country))]
        public string CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        [ForeignKey(nameof(PaymentMethod))]
        public string PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        //Collections
        public virtual ICollection<ProductSale> Products { get; set; }

        [Required]
        public DateTime PurchasedOn { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public string AdditionalInformation { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
