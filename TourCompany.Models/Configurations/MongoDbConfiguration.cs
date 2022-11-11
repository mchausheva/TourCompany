namespace TourCompany.Models.Configurations
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string CustomerCollectionName { get; set; }
    }
}
