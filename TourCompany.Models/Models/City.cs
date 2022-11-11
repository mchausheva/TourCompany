using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class City
    {
        [Key(0)]
        public int CityId { get; set; }
        [Key(1)]
        public string CityName { get; set; }
        [Key(2)]
        public decimal PricePerNight { get; set; }
        [Key(3)]
        public int CountryId { get; set; }
    }
}
