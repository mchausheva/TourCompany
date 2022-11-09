using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using TourCompany.BL.Kafka;
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
        private readonly ReservationProducer _producer;

        public ReservationController(ILogger<ReservationController> logger, IMediator mediator, ReservationProducer reservationProducer)
        {
            _logger = logger;
            _mediator = mediator;
            _producer = reservationProducer;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(Name = "GetReservation")]
        public async Task<IActionResult> GetReservation(int reservationId, int customerId)
        {
                if (reservationId <= 0 || customerId <= 0)
                {
                    _logger.LogInformation("Id must be greater than 0");
                    return BadRequest($"Parameter id must be greater than 0");
                }

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
            
            await _producer.SendMessage(result.Reservation, new CancellationToken());

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut(Name = "UpdateReservation")]
        public async Task<IActionResult> UpdateReservation(int reservationId, int customerId, [FromBody] ReservationRequest reservationRequest)
        {
            if (reservationId <= 0 || customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var getResult = await _mediator.Send(new GetReservationCommand(reservationId, customerId));
            if (getResult == null) return NotFound($"Not Found Reservation with Id = {reservationId} and Customer Id = {customerId}");


            var result = await _mediator.Send(new UpdateReservationCommand(reservationId, reservationRequest));

            await _producer.SendMessage(result.Reservation, new CancellationToken());

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(Name = "DeleteReservation")]
        public async Task<IActionResult> DeleteReservation(int reservationId, int customerId)
        {
            if (reservationId <= 0 || customerId <= 0)
            {
                _logger.LogInformation("Id must be greater than 0");
                return BadRequest($"Parameter id must be greater than 0");
            }

            var getResult = await _mediator.Send(new GetReservationCommand(reservationId, customerId));
            if (getResult == null) return NotFound($"Not Found Reservation with Id = {reservationId} and Customer Id = {customerId}");


            var result = await _mediator.Send(new DeleteReservationCommand(reservationId, customerId));

            if (result.HttpStatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
