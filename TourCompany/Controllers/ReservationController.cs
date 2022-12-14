using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Requests;

namespace TourCompany.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IMediator _mediator;

        public ReservationController(ILogger<ReservationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(Name = "GetReservation")]
        public async Task<IActionResult> GetReservation(int reservationId, int customerId)
        {
                var result = await _mediator.Send(new GetReservationCommand(reservationId, customerId));

                if (result == null) return NotFound($"Not Found Reservation with Id = {reservationId} and Customer Id = {customerId}");

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "CreateReservation")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest reservationRequest) 
        {
            var result = await _mediator.Send(new CreateReservationCommand(reservationRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut(Name = "UpdateReservation")]
        public async Task<IActionResult> UpdateReservation(int reservationId, int customerId, [FromBody] ReservationRequest reservationRequest)
        {
            var result = await _mediator.Send(new UpdateReservationCommand(reservationId, reservationRequest));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(Name = "DeleteReservation")]
        public async Task<IActionResult> DeleteReservation(int reservationId, int customerId)
        {
            var result = await _mediator.Send(new DeleteReservationCommand(reservationId, customerId));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
