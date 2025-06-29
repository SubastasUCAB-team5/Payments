using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;

namespace PaymentsMS.Application.Handlers.Queries
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentsMS.Core.DTOs.PaymentMethodDto>>
    {
        private readonly IPaymentGateway _paymentGateway;

        public GetPaymentMethodsQueryHandler(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            return await _paymentGateway.ListPaymentMethodsAsync(request.CustomerId);
        }
    }
}
