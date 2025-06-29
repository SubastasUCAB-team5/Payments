using Microsoft.Extensions.Configuration;
using PaymentsMS.Core.Service;
using Stripe;
using System.Threading.Tasks;

namespace PaymentsMS.Infrastructure.Gateways
{
    public class StripePaymentGateway : IPaymentGateway
    {
        public StripePaymentGateway(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:ApiKey"];
        }

        public async Task<string> CreateCustomer(string email, string name)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
            };

            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(customerOptions);

            return customer.Id;
        }

        public async Task<string> AttachPaymentMethod(string customerId, string paymentMethodId)
        {
            var paymentMethodAttachOptions = new PaymentMethodAttachOptions
            {
                Customer = customerId,
            };

            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.AttachAsync(paymentMethodId, paymentMethodAttachOptions);

            return paymentMethod.Id;
        }
    }
}
