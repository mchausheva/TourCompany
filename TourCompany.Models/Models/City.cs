namespace TourCompany.Models.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public decimal PricePerNight { get; set; }
        public int CountryId { get; set; }
    }
}
