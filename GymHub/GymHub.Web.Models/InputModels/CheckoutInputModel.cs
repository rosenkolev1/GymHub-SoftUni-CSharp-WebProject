using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class CheckoutInputModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[0-9]{10}", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Postcode { get; set; }

        public string CompanyName { get; set; }


        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethodId { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
