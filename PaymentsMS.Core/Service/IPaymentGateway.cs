using System.Threading.Tasks;

namespace PaymentsMS.Core.Service
{
    public interface IPaymentGateway
    {
        Task<string> CreateCustomer(string email, string name);
        Task<string> AttachPaymentMethod(string customerId, string paymentMethodId);
    }
}
