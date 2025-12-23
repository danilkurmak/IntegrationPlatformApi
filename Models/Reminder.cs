namespace IntegrationPlatformApi.Models
{
    public class Reminder
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public DateTime dateTime { get; set; }
        public string priority { get; set; } = "";
        public string status { get; set; } = "";
    }
}
