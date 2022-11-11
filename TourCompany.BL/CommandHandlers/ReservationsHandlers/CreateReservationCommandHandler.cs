using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.BL.Kafka;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Models;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers.ReservationsHandlers
{
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationResponse>
    {
        private readonly ILogger<CreateReservationCommandHandler> _logger;
        private readonly IReservationRepository _reservationRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;
        private readonly ReservationProducer _producer;

        public CreateReservationCommandHandler(ILogger<CreateReservationCommandHandler> logger, IReservationRepository reservationRepository,
                                                IDestinationRepository destinationRepository, IMapper mapper, ReservationProducer producer)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
            _destinationRepository = destinationRepository;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task<ReservationResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDate = request.reservationRequest.ReservationDate;

                var validDate = DateTime.Compare(requestDate, DateTime.Today);

                if (validDate < 0)
                {
                    return new ReservationResponse
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "This date has passed. Please enter a valid date."
                    };
                }

                var tempRreservation = _mapper.Map<Reservation>(request.reservationRequest);

                var reservation = UpdateTotalPrice(_destinationRepository, tempRreservation).Result;

                var result = await _reservationRepository.CreateReservation(reservation);

                await _producer.SendMessage(result, cancellationToken);

                return new ReservationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You Successfully Created Your Reservation!",
                    Reservation = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The Reservation Failed --> {ex.Message}");
                return new ReservationResponse
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "The Reservation Failed!"
                };
            }
        }

        public async Task<Reservation> UpdateTotalPrice(IDestinationRepository destination, Reservation reservation)
        {
            var city = await destination.GetCityById(reservation.CityId);
            var defaultPrice = city.PricePerNight * reservation.NumberOfPeople * reservation.Days;

            switch (reservation.PromoCode)
            {
                case "PROMOCODE5%":
                    reservation.TotalPrice = defaultPrice - (defaultPrice * 0.05m);
                    break;

                case "PROMOCODE10%":
                    reservation.TotalPrice = defaultPrice - (defaultPrice * 0.1m);
                    break;

                case "PROOCODE15%":
                    reservation.TotalPrice = defaultPrice - (defaultPrice * 0.15m);
                    break;

                case "":
                    reservation.TotalPrice = defaultPrice;
                    break;

                default:
                    reservation.TotalPrice = defaultPrice;
                    break;
            }
            return reservation;
        }
    }
}
