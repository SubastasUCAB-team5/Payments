using System.Threading.Tasks;
using System.Collections.Generic;
using PaymentsMS.Core.DTOs;

namespace PaymentsMS.Core.Service
{
    public interface IPaymentGateway
    {
        Task<string> CreateCustomer(string email, string name);
        Task<string> AttachPaymentMethod(string customerId, string paymentMethodId);
        Task<bool> DetachPaymentMethodAsync(string customerId, string paymentMethodId);
        Task<List<PaymentMethodDto>> ListPaymentMethodsAsync(string customerId);
        Task<bool> SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId);
        Task<string> CreatePaymentIntent(string customerId, long amount, string currency);
    }
}
