using MediatR;
using PaymentsMS.Core.Service;
using PaymentsMS.Application.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace PaymentsMS.Application.Handlers.Queries
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, List<PaymentsMS.Core.DTOs.PaymentMethodDto>>
    {
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger<GetPaymentMethodsQueryHandler> _logger;

        public GetPaymentMethodsQueryHandler(IPaymentGateway paymentGateway, ILogger<GetPaymentMethodsQueryHandler> logger)
        {
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public async Task<List<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetPaymentMethodsQueryHandler.Handle: Get payment methods.");
                return await _paymentGateway.ListPaymentMethodsAsync(request.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPaymentMethodsQueryHandler.Handle: Get payment methods failed.", ex);
                throw;
            }
        }
    }
}
