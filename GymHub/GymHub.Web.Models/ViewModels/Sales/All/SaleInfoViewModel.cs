using GymHub.Data.Models;
using System;

namespace GymHub.Web.Models.ViewModels
{
    public class SaleInfoViewModel
    {
        public string Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PurchasedOn { get; set; }

        public decimal TotalPayment { get; set; }

        public string BillingAccount { get; set; }

        public string ReceivingAccount { get; set; }

        public string PaymentStatus { get; set; }
    }
}
