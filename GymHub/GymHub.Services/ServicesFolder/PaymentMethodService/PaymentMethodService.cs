using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.PaymentMethodService
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly ApplicationDbContext context;
        
        public PaymentMethodService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(string paymentMethodName)
        {
            await this.context.AddAsync(new PaymentMethod { Name = paymentMethodName });
            await this.context.SaveChangesAsync();
        }

        public List<PaymentMethod> GetAllPaymentMethods()
        {
            return this.context.PaymentMethods.ToList();
        }

        public string GetPaymentMethod(string paymentMethodId)
        {
            return this.context.PaymentMethods.Where(x => x.Id == paymentMethodId)
                .Select(x => x.Name).First();
        }

        public bool PaymentMethodExistsById(string paymentMethodId)
        {
            return this.context.PaymentMethods.Any(x => x.Id == paymentMethodId);
        }

        public bool PaymentMethodExistsByName(string paymentMethodName, bool hardCheck = false)
        {
            return this.context.PaymentMethods.IgnoreAllQueryFilters(hardCheck).Any(x => x.Name == paymentMethodName);
        }
    }
}
