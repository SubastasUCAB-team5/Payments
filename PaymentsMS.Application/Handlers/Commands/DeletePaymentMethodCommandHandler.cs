using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;

        public DeletePaymentMethodCommandHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            return await _paymentGateway.DetachPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
        }
    }
}
