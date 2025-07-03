using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommand, bool>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<DeletePaymentMethodCommandHandler> _logger;

        public DeletePaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<DeletePaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("DeletePaymentMethodCommandHandler.Handle: Delete payment method.");
                return await _paymentGateway.DetachPaymentMethodAsync(request.CustomerId, request.PaymentMethodId);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeletePaymentMethodCommandHandler.Handle: Delete payment method failed.", ex);
                throw;
            }
        }
    }
}
