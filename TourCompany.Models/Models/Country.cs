using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class Country
    {
        [Key(0)]
        public int CountryId { get; set; }
        [Key(1)]
        public string CountryName { get; set; }
    }
}
