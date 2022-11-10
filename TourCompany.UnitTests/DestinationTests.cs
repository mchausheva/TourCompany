using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TourCompany.Controllers;
using TourCompany.Models.MediatR;

namespace TourCompany.UnitTests
{
    public class DestinationTests
    {
        private readonly Mock<ILogger<DestinationController>> _loggerDestinationControllerMock;
        private readonly Mock<IMediator> _mediator;

        public DestinationTests()
        {
            _loggerDestinationControllerMock = new Mock<ILogger<DestinationController>>();
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Get_Destinations_Ok()
        {
            //set up
            _mediator.Setup(x => x.Send(new Mock<GetAllDestinationCommand>(), new CancellationToken()));

            //inject
            var controller = new DestinationController(_loggerDestinationControllerMock.Object, _mediator.Object);

            //act
            var result = await controller.GetAllDestinations();

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }
    }
}
