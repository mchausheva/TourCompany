using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourCompany.Models.MediatR;

namespace TourCompany.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DestinationController : ControllerBase
    {
        private readonly ILogger<DestinationController> _logger;
        private readonly IMediator _mediator;

        public DestinationController(ILogger<DestinationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(Name = "GetAllDestinations")]
        public async Task<IActionResult> GetAllDestinations()
        {
            _logger.LogInformation("Destination Controller - GET");
            return Ok(await _mediator.Send(new GetAllDestinationCommand()));
        }
    }
}