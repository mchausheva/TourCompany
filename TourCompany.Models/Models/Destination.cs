using MessagePack;

namespace TourCompany.Models.Models
{
    [MessagePackObject]
    public class Destination
    {
        //public string CountryName { get; set; }
        //public int CityId { get; set; }
        //public string CityName { get; set; }

        [Key(0)]
        public Country Country { get; set; }
        [Key(1)]
        public List<City> Cities { get; set; }
    }
}
