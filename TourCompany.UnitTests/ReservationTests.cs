using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TourCompany.AutoMapper;
using TourCompany.BL.CommandHandlers.ReservationsHandlers;
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
        private readonly Mock<ILogger<GetReservationCommandHandler>> _loggerGetReservationHandlerMock;
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        public readonly Mock<IDestinationRepository> _destinationRepositoryMock;

        public ReservationTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMappings());
            });
            _mapper = mockMapperConfig.CreateMapper();

            _loggerGetReservationHandlerMock = new Mock<ILogger<GetReservationCommandHandler>>();
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _destinationRepositoryMock = new Mock<IDestinationRepository>();
        }

        [Fact]
        public async Task Reservation_Get_Ok()
        {
            //set_up
            var reservatioId = 12001;
            var customerId = 2005;

            _reservationRepositoryMock.Setup(x => x.GetById(reservatioId, customerId))
                            .ReturnsAsync(_reservations.First(x => x.ReservationId == reservatioId && x.CustomerId == customerId));

            //inhect
            var command = new GetReservationCommand(reservatioId, customerId);
            var handler = new GetReservationCommandHandler(_loggerGetReservationHandlerMock.Object, _reservationRepositoryMock.Object);

            //act
            var result = await handler.Handle(command, new CancellationToken());
            
            //assert
            Assert.NotNull(result);
            Assert.Equal(reservatioId, result.ReservationId);
        }
    }
}