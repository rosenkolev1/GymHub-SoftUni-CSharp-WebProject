using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        public List<PaymentMethod> GetAllPaymentMethods();
        public bool PaymentMethodExists(string paymentMethodName, bool hardCheck = false);
        public Task AddAsync(string paymentMethodName);
    }
}
