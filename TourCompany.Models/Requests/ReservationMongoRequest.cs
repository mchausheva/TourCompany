using MessagePack;

namespace TourCompany.Models.Requests
{
    [MessagePackObject]
    public class ReservationMongoRequest
    {
        [Key(0)]
        public MongoDB.Bson.ObjectId id { get; set; }
        [Key(1)]
        public int ReservationId { get; set; }
        [Key(2)]
        public int CustomerId { get; set; }
        [Key(3)]
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        [Key(4)]
        public int CityId { get; set; }
        [Key(5)]
        public int Days { get; set; }
        [Key(6)]
        public int NumberOfPeople { get; set; }
        [Key(7)]
        public string PromoCode { get; set; }
        [Key(8)]
        public decimal TotalPrice { get; set; }
    }
}
