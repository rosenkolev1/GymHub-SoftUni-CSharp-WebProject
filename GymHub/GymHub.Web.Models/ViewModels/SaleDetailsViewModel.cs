using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class SaleDetailsViewModel
    {
        public string Id { get; set; }

        public string CountryId { get; set; }

        public Country Country { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        //Collections
        public virtual ICollection<ProductSale> ProductsSale { get; set; }

        public DateTime PurchasedOn { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string SaleStatus { get; set; }

        public string AdditionalInformation { get; set; }

        public decimal TotalPrice { get; set; }

        public string Municipality { get; set; }
    }
}
