namespace TourCompany.Models.Configurations
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public int AutoOffsetReset { get; set; }
        public string GroupId { get; set; }
        public KafkaConfig()
        {
        }
    }
}
