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
        private readonly ILogger<ProcessPaymentCommandHandler> _logger;

        public ProcessPaymentCommandHandler(IPaymentGateway paymentGateway, ILogger<ProcessPaymentCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("ProcessPaymentCommandHandler.Handle: Process payment.");
                return await _paymentGateway.ProcessPayment(request.CustomerId, request.PaymentMethodId, request.Amount, request.Currency);
            }
            catch (Exception ex)
            {
                _logger.LogError("ProcessPaymentCommandHandler.Handle: Process payment failed.", ex);
                throw;
            }
        }
    }
}
