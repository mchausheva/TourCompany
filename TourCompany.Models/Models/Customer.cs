using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class Customer
    {
        [Key(0)]
        public int CustomerId { get; set; }
        [Key(1)]
        public string CustomerName { get; set; }
        [Key(2)]
        public string Email { get; set; }
        [Key(3)]
        public string Telephone { get; set; }

    }
}
