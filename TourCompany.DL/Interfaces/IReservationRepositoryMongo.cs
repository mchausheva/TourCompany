using TourCompany.Models.Models;
using TourCompany.Models.Requests;

namespace TourCompany.DL.Interfaces
{
    public interface IReservationRepositoryMongo
    {
        public Task<ReservationMongoRequest> GetById(int reservationId);
        public Task<ReservationMongoRequest> SaveReservation(Reservation reservation);
        public Task<ReservationMongoRequest> UpdateReservation(Reservation reservation);
        public Task<ReservationMongoRequest?> DeleteReservationById(int reservationId, int customerId);
    }
}
