using MessagePack;

namespace TourCompany.Models.Requests
{
    [MessagePackObject]
    public class CustomerMongoRequest
    {
        [Key(0)]
        public MongoDB.Bson.ObjectId id { get; set; }
        [Key(1)]
        public int CustomerId { get; set; }
        [Key(2)]
        public string CustomerName { get; set; }
        [Key(3)]
        public string Email { get; set; }
        [Key(4)]
        public string Telephone { get; set; }
        [Key(5)]
        public int ReservationCount { get; set; }
    }
}
