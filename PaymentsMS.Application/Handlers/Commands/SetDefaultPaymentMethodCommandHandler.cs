using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class SetDefaultPaymentMethodCommandHandler : IRequestHandler<SetDefaultPaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;

        public SetDefaultPaymentMethodCommandHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public async Task<bool> Handle(SetDefaultPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            return await _paymentGateway.SetDefaultPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
        }
    }
}
