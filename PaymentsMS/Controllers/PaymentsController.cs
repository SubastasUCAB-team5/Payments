
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentsMS.Application.Commands;
using System.Threading.Tasks;
using PaymentsMS.Core.DTOs;
using PaymentsMS.Application.Queries;
using System;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using PaymentsMS.Examples.Commands;

namespace PaymentsMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("customer")]
        [SwaggerRequestExample(typeof(CreateCustomerCommand), typeof(CreateCustomerCommandExample))]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            if (command == null)
            {
                return BadRequest(new { message = "Fallo al crear el cliente." });
            }

            var customerId = await _mediator.Send(command);
            if (customerId == null)
            {
                return BadRequest(new { message = "Fallo al crear el cliente." });
            }

            return Ok(new { customerId });
        }

        [HttpPost("payment-method")]
        [SwaggerRequestExample(typeof(AttachPaymentMethodCommand), typeof(AttachPaymentMethodCommandExample))]
        public async Task<IActionResult> AttachPaymentMethod([FromBody] AttachPaymentMethodCommand command)
        {
            if (command == null)
            {
                return BadRequest(new { message = "Fallo al crear el metodo de pago." });
            }

            var paymentMethodId = await _mediator.Send(command);
            if (paymentMethodId == null)
            {
                return BadRequest(new { message = "Fallo al crear el metodo de pago." });
            }

            return  Ok(new { paymentMethodId });
        }

        [HttpDelete("payment-method")]
        public async Task<IActionResult> DeletePaymentMethod([FromQuery] string customerId, [FromQuery] string paymentMethodId)
        {
            var command = new DeletePaymentMethodCommand { CustomerId = customerId, PaymentMethodId = paymentMethodId };
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Metodo de pago eliminado exitosamente." });
            }
            return BadRequest(new { message = "Fallo al eliminar el metodo de pago." });
        }

        [HttpGet("payment-methods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] string customerId)
        {
            var query = new GetPaymentMethodsQuery { CustomerId = customerId };
            var paymentMethods = await _mediator.Send(query);
            return Ok(paymentMethods);
        }

        [HttpPatch("default-payment-method")]
        [SwaggerRequestExample(typeof(SetDefaultPaymentMethodCommand), typeof(SetDefaultPaymentMethodCommandExample))]
        public async Task<IActionResult> SetDefaultPaymentMethod([FromBody] SetDefaultPaymentMethodCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Metodo de pago por defecto establecido exitosamente." });
            }
            return BadRequest(new { message = "Fallo al establecer el metodo de pago por defecto." });
        }

        [HttpPost("process-payment")]
        [SwaggerRequestExample(typeof(ProcessPaymentCommand), typeof(ProcessPaymentCommandExample))]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            if (command == null)
            {
                return BadRequest(new { message = "Fallo al procesar el pago." });
            }

            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Pago procesado exitosamente." });
            }
            return BadRequest(new { message = "Fallo al procesar el pago." });
        }
    }
}
