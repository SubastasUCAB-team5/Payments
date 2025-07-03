using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentsMS.Application.Handlers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(IPaymentGateway paymentGateway, ILogger<CreateCustomerCommandHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("CreateCustomerCommandHandler.Handle: Create customer.");
                return _paymentGateway.CreateCustomer(request.Email, request.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateCustomerCommandHandler.Handle: Create customer failed.", ex);
                throw;
            }
        }
    }
}
