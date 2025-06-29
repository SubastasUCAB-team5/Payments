
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentsMS.Application.Commands;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;
using PaymentsMS.Application.Queries;


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

        [HttpDelete("payment-method")]
        public async Task<IActionResult> DeletePaymentMethod([FromQuery] string customerId, [FromQuery] string paymentMethodId)
        {
            var command = new DeletePaymentMethodCommand { CustomerId = customerId, PaymentMethodId = paymentMethodId };
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Payment method detached successfully." });
            }
            return BadRequest(new { message = "Failed to detach payment method." });
        }

        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] string customerId)
        {
            var query = new GetPaymentMethodsQuery { CustomerId = customerId };
            var paymentMethods = await _mediator.Send(query);
            return Ok(paymentMethods);
        }
    }
}
