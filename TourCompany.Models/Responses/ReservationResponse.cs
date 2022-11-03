using TourCompany.Models.Models;

namespace TourCompany.Models.Responses
{
    public class ReservationResponse : BaseResponse
    {
        public Reservation Reservation { get; set; }
    }
}
