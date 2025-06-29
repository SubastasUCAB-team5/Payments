
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentsMS.Application.Commands;
using System.Threading.Tasks;

namespace PaymentsMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var customerId = await _mediator.Send(command);
            return Ok(new { customerId });
        }

        [HttpPost("payment-method")]
        public async Task<IActionResult> AttachPaymentMethod([FromBody] AttachPaymentMethodCommand command)
        {
            var paymentMethodId = await _mediator.Send(command);
            return Ok(new { paymentMethodId });
        }
    }
}
