using MediatR;
using PaymentsMS.Core.Service;
using Microsoft.Extensions.Logging;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class SetDefaultPaymentMethodCommandHandler : IRequestHandler<SetDefaultPaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<SetDefaultPaymentMethodCommandHandler> _logger;

        public SetDefaultPaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<SetDefaultPaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public async Task<bool> Handle(SetDefaultPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("SetDefaultPaymentMethodCommandHandler.Handle: Set default payment method.");
                return await _paymentGateway.SetDefaultPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
            }
            catch (Exception ex)
            {
                _logger.LogError("SetDefaultPaymentMethodCommandHandler.Handle: Set default payment method failed.", ex);
                throw;
            }
        }
    }
}
