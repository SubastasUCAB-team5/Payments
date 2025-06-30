using MediatR;

namespace PaymentsMS.Application.Commands
{
    public class ProcessPaymentCommand : IRequest<string>
    {
        public string CustomerId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
