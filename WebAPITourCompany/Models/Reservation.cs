using MessagePack;

namespace WebAPITourCompany.Models
{
    [MessagePackObject]
    public class Reservation
    {
        [Key(0)]
        public int ReservationId { get; set; }
        [Key(1)]
        public int CustomerId { get; set; }
        [Key(2)]
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        [Key(3)]
        public int CityId { get; set; }
        [Key(4)]
        public int Days { get; set; }
        [Key(5)]
        public int NumberOfPeople { get; set; }
        [Key(6)]
        public string PromoCode { get; set; }
        [Key(7)]
        public decimal TotalPrice { get; set; }
    }
}
