using MessagePack;

namespace TourCompany.Models.Requests
{
    [MessagePackObject]
    public class CustomerRequest
    {
        [Key(0)]
        public string CustomerName { get; set; }
        [Key(1)]
        public string Email { get; set; }
        [Key(2)]
        public string Telephone { get; set; }
    }
}
