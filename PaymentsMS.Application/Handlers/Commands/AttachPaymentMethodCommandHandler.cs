using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class AttachPaymentMethodCommandHandler : IRequestHandler<AttachPaymentMethodCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway;

        public AttachPaymentMethodCommandHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public Task<string> Handle(AttachPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            return _paymentGateway.AttachPaymentMethod(request.CustomerId, request.PaymentMethodId);
        }
    }
}
