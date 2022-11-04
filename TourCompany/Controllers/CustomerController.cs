using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TourCompany.Models.MediatR.Customers;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Models;
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
            if (customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var result = await _mediator.Send(new GetCustomerReservationsCommand(customerId));

            if (result == null) return NotFound($"Not Found Customer or Reservations");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(nameof(GetCustomer))]
        public async Task<IActionResult> GetCustomer(int customerId)
        {
            if (customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var result = await _mediator.Send(new GetCustomerByIdCommand(customerId));

            if (result == null) return NotFound($"Not Found Customer with Id = {customerId}");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CustomerRequest customerRequest)
        {
            var result = await _mediator.Send(new CreateAccountCommand(customerRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut(Name = "UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(int customerId, [FromBody] CustomerRequest customerRequest)
        {
            if (customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var getResult = await _mediator.Send(new GetCustomerByIdCommand(customerId));
            if (getResult == null) return NotFound($"Not Found Customer with Id = {customerId}");

            var result = await _mediator.Send(new UpdateAccountCommand(customerId, customerRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(Name = "DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(int customerId)
        {
            if (customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var getResult = await _mediator.Send(new GetCustomerByIdCommand(customerId));
            if (getResult == null) return NotFound($"Not Found Customer with Id = {customerId}");


            var result = await _mediator.Send(new DeleteAccountCommand(customerId));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
