namespace TourCompany.Models.Models
{
    public class Destination
    {
        //public string CountryName { get; set; }
        //public int CityId { get; set; }
        //public string CityName { get; set; }

        public Country Country { get; set; }
        public List<City> Cities { get; set; }
    }
}
