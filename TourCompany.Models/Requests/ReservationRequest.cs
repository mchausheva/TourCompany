using MessagePack;

namespace TourCompany.Models.Requests
{
    [MessagePackObject]
    public class ReservationRequest
    {
        [Key(0)]
        public int CustomerId { get; set; }
        [Key(1)]
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        [Key(2)]
        public int CityId { get; set; }
        [Key(3)]
        public int Days { get; set; }
        [Key(4)]
        public int NumberOfPeople { get; set; }
        [Key(5)]
        public string PromoCode { get; set; }
    }
}
