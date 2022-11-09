using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.DL.Repositories;
using TourCompany.Models.MediatR.Reservations;
using TourCompany.Models.Models;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers.ReservationsHandlers
{
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ReservationResponse>
    {
        private readonly ILogger<UpdateReservationCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;
        public readonly IDestinationRepository _destinationRepository;

        public UpdateReservationCommandHandler(ILogger<UpdateReservationCommandHandler> logger, IMapper mapper, IReservationRepository reservationRepository, IDestinationRepository destinationRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
            _destinationRepository = destinationRepository;
        }

        public async Task<ReservationResponse> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Command Handler - UPDATE Reservation");

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

                var tempReservation = _mapper.Map<Reservation>(request.reservationRequest);

                var reservation = UpdateTotalPrice(_destinationRepository, tempReservation).Result;

                var result = await _reservationRepository.UpdateReservation(request.reservationId, reservation);

                if (result == null)
                {
                    return new ReservationResponse
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        Message = "The reservation could not be updated."
                    };
                }

                return new ReservationResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = "You Successfully Updated Your Reservation!",
                    Reservation = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"The updation failed.");
                return new ReservationResponse
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "The updation failed!"
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

                default:
                    reservation.TotalPrice = defaultPrice;
                    break;
            }
            return reservation;
        }
    }
}
