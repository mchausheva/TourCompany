using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TourCompany.Controllers;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Models;

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
        public async Task Get_Destinations()
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
