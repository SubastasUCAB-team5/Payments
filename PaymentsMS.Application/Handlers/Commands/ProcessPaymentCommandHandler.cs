using MediatR;
using PaymentsMS.Application.Commands;
using PaymentsMS.Core.Service;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;

        public ProcessPaymentCommandHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            return await _paymentGateway.ProcessPayment(request.CustomerId, request.PaymentMethodId, request.Amount, request.Currency);
        }
    }
}
