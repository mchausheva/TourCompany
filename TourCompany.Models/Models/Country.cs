using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class Country
    {
        [Key(1)]
        public int CountryId { get; set; }
        [Key(2)]
        public string CountryName { get; set; }
    }
}
