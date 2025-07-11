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

        /// <summary>
        /// Crea un nuevo cliente en Stripe.
        /// </summary>
        /// <param name="email">El correo electrónico del cliente.</param>
        /// <param name="name">El nombre del cliente.</param>
        /// <returns>El ID del cliente de Stripe.</returns>
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

        /// <summary>
        /// Adjunta un método de pago a un cliente en Stripe.
        /// </summary>
        /// <param name="customerId">El ID del cliente de Stripe.</param>
        /// <param name="paymentMethodId">El ID del método de pago de Stripe.</param>
        /// <returns>El ID del método de pago adjuntado.</returns>
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

        /// <summary>
        /// Desvincula un método de pago de un cliente en Stripe.
        /// </summary>
        /// <param name="customerId">El ID del cliente de Stripe (no usado directamente por Stripe para detach, pero útil para contexto).</param>
        /// <param name="paymentMethodId">El ID del método de pago a desvincular.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario, false.</returns>
        public async Task<bool> DetachPaymentMethodAsync(string customerId, string paymentMethodId)
        {
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.DetachAsync(paymentMethodId);
            return paymentMethod != null;
        }

        /// <summary>
        /// Lista los métodos de pago asociados a un cliente en Stripe.
        /// </summary>
        /// <param name="customerId">El ID del cliente de Stripe.</param>
        /// <returns>Una lista de objetos PaymentMethodDto.</returns>
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

        /// <summary>
        /// Establece un método de pago como predeterminado para un cliente en Stripe.
        /// </summary>
        /// <param name="customerId">El ID del cliente de Stripe.</param>
        /// <param name="paymentMethodId">El ID del método de pago a establecer como predeterminado.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario, false.</returns>
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

        /// <summary>
        /// Procesa un pago utilizando Stripe Payment Intents.
        /// </summary>
        /// <param name="customerId">El ID del cliente de Stripe.</param>
        /// <param name="paymentMethodId">El ID del método de pago a utilizar.</param>
        /// <param name="amount">El monto del pago en la unidad más pequeña de la moneda (ej. centavos para USD).</param>
        /// <param name="currency">La moneda del pago (ej. "usd").</param>
        /// <returns>True si el pago fue exitoso, de lo contrario, false.</returns>
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
