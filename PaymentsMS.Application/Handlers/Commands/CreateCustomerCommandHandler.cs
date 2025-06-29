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

        public CreateCustomerCommandHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            return _paymentGateway.CreateCustomer(request.Email, request.Name);
        }
    }
}
