using Microsoft.Extensions.Configuration;
using PaymentsMS.Core.Service;
using Stripe;
using System.Threading.Tasks;
using System.Collections.Generic;
using PaymentsMS.Core.DTOs;

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

        public async Task<bool> DetachPaymentMethodAsync(string customerId, string paymentMethodId)
        {
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.DetachAsync(paymentMethodId);
            return paymentMethod != null;
        }

        public async Task<List<PaymentMethodDto>> ListPaymentMethodsAsync(string customerId)
        {
            var service = new PaymentMethodService();
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card", 
            };
            StripeList<PaymentMethod> paymentMethods = await service.ListAsync(options);
            var result = new List<PaymentMethodDto>();
            foreach (var pm in paymentMethods.Data)
            {
                result.Add(new PaymentMethodDto
                {
                    Id = pm.Id,
                    Brand = pm.Card.Brand,
                    Last4 = pm.Card.Last4,
                    ExpMonth = pm.Card.ExpMonth,
                    ExpYear = pm.Card.ExpYear
                });
            }
            return result;
        }

        public async Task<bool> SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId)
        {
            var customerService = new CustomerService();
            var updateOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId,
                },
            };
            var customer = await customerService.UpdateAsync(customerId, updateOptions);
            return customer != null && customer.InvoiceSettings?.DefaultPaymentMethod?.Id == paymentMethodId;
        }

        public async Task<bool> ProcessPayment(string customerId, string paymentMethodId, long amount, string currency)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Customer = customerId,
                PaymentMethod = paymentMethodId,
                OffSession = true, 
                Confirm = true,    
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return paymentIntent.Status == "succeeded";
        }
    }
}
