using GymHub.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        public List<PaymentMethod> GetAllPaymentMethods();
        public bool PaymentMethodExistsByName(string paymentMethodName, bool hardCheck = false);
        public bool PaymentMethodExistsById(string paymentMethodId);
        public Task AddAsync(string paymentMethodName);
        public string GetPaymentMethod(string paymentMethodId);
    }
}
