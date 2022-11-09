using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class Customer
    {
        [Key(1)]
        public int CustomerId { get; set; }
        [Key(2)]
        public string CustomerName { get; set; }
        [Key(3)]
        public string Email { get; set; }
        [Key(4)]
        public string Telephone { get; set; }

    }
}
