namespace TourCompany.Models.Requests
{
    public class ReservationRequest
    {
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public int CityId { get; set; }
        public int Days { get; set; }
        public int NumberOfPeople { get; set; }
        public string PromoCode { get; set; }
    }
}
