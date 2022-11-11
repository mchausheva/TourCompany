using TourCompany.Models.Models;
using TourCompany.Models.Requests;
using TourCompany.Models.Responses;

namespace TourCompany.DL.Interfaces
{
    public interface IReservationRepository
    {
        public Task<IEnumerable<Reservation>> GetAll();
        public Task<Reservation?> GetById(int reservationId, int customerId);
        public Task<Reservation> CreateReservation(Reservation reservation);
        public Task<Reservation> UpdateReservation(int reservationId, Reservation reservation);
        public Task<Reservation?> DeleteReservationById(int reservationId, int customerId);
    }
}
