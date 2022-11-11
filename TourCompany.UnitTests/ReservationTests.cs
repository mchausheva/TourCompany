using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TourCompany.AutoMapper;
using TourCompany.BL.Kafka;
using TourCompany.Controllers;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Models;

namespace TourCompany.UnitTests
{
    public class ReservationTests
    {
        private List<Reservation> _reservations = new List<Reservation>()
        {
            new Reservation()
            {
                ReservationId = 12001,
                CustomerId = 2005,
                ReservationDate = DateTime.Today,
                CityId = 101,
                Days = 2,
                NumberOfPeople = 3,
                PromoCode = ""
            },
            new Reservation()
            {
                ReservationId = 12002,
                CustomerId = 2004,
                ReservationDate = DateTime.Today,
                CityId = 103,
                Days = 2,
                NumberOfPeople = 2,
                PromoCode = ""
            },
            new Reservation()
            {
                ReservationId = 12003,
                CustomerId = 2006,
                ReservationDate = DateTime.Today,
                CityId = 107,
                Days = 3,
                NumberOfPeople = 2,
                PromoCode = ""
            }
        };

        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediator;

        private readonly Mock<ILogger<ReservationController>> _loggerReservationControllerMock;
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        public readonly Mock<IDestinationRepository> _destinationRepositoryMock;

        public ReservationTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });
            _mapper = mockMapperConfig.CreateMapper();
            _mediator = new Mock<IMediator>();

            _loggerReservationControllerMock = new Mock<ILogger<ReservationController>>();
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _destinationRepositoryMock = new Mock<IDestinationRepository>();
        }

        [Fact]
        public async Task Reservation_Get_Ok()
        {
            //set_up
            var reservatioId = 12001;
            var customerId = 2005;
            
            _mediator.Setup(x => x.Send(new GetReservationCommand(reservatioId, customerId), new CancellationToken()))
                     .ReturnsAsync(_reservations.First(x => x.ReservationId == reservatioId && x.CustomerId == customerId));

            //inject
            var controller = new ReservationController(_loggerReservationControllerMock.Object, _mediator.Object);

            //act
            var result = await controller.GetReservation(reservatioId, customerId);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var reservation = okObjectResult.Value as Reservation;
            Assert.NotNull(reservation);
            Assert.Equal(reservatioId, reservation.ReservationId);
        }

        [Fact]
        public async Task Reservation_Get_NotOk()
        {
            //set_up
            var reservatioId = 12003;
            var customerId = 2003;

            _mediator.Setup(x => x.Send(new GetReservationCommand(reservatioId, customerId), new CancellationToken()))
                     .ReturnsAsync(_reservations.FirstOrDefault(x => x.ReservationId == reservatioId && x.CustomerId == customerId));

            //inject
            var controller = new ReservationController(_loggerReservationControllerMock.Object, _mediator.Object);

            //act
            var result = await controller.GetReservation(reservatioId, customerId);

            //assert
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);
        }
    }
}
