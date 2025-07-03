using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class AttachPaymentMethodCommandHandler : IRequestHandler<AttachPaymentMethodCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<AttachPaymentMethodCommandHandler> _logger;

        public AttachPaymentMethodCommandHandler(IPaymentGateway paymentGateway, ILogger<AttachPaymentMethodCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public Task<string> Handle(AttachPaymentMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("AttachPaymentMethodCommandHandler.Handle: Attach payment method to customer.");
                return _paymentGateway.AttachPaymentMethod(request.CustomerId, request.PaymentMethodId);
            }
            catch (Exception ex)
            {
                _logger.LogError("AttachPaymentMethodCommandHandler.Handle: Attach payment method to customer failed.", ex);
                throw;
            }
        }   
    }
}
