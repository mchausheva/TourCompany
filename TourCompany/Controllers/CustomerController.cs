using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.Requests;

namespace TourCompany.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetCustomerReservations))]
        public async Task<IActionResult> GetCustomerReservations(int customerId)
        {
            var result = await _mediator.Send(new GetCustomerReservationsCommand(customerId));

            if (result == null) return NotFound($"Not Found Customer or Reservations");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetCustomerById))]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var result = await _mediator.Send(new GetCustomerByIdCommand(customerId));

            if (result == null) return NotFound($"Not Found Customer with Id = {customerId}");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(CreateAccount))]
        public async Task<IActionResult> CreateAccount([FromBody] CustomerRequest customerRequest)
        {
            var result = await _mediator.Send(new CreateAccountCommand(customerRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut(nameof(UpdateAccount))]
        public async Task<IActionResult> UpdateAccount(int customerId, [FromBody] CustomerRequest customerRequest)
        {
            var result = await _mediator.Send(new UpdateAccountCommand(customerId, customerRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(nameof(DeleteAccount))]
        public async Task<IActionResult> DeleteAccount(int customerId)
        {
            var result = await _mediator.Send(new DeleteAccountCommand(customerId));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
