using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using TourCompany.DL.Interfaces;
using TourCompany.Models.MediatR;
using TourCompany.Models.Models;
using TourCompany.Models.Responses;

namespace TourCompany.BL.CommandHandlers
{
    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ReservationResponse>
    {
        private readonly ILogger<UpdateReservationCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;

        public UpdateReservationCommandHandler(ILogger<UpdateReservationCommandHandler> logger, IMapper mapper, IReservationRepository reservationRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationResponse> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
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

                var reservation = _mapper.Map<Reservation>(request.reservationRequest);
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
                _logger.LogWarning($"The reservation had failed.");
                throw;
            }
        }
    }
}
