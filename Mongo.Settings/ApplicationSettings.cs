namespace Mongo.Settings
{

    public class ApplicationSettings
    {
        public Appsettings AppSettings { get; set; }

    }
    public class Appsettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string SendGridSecret { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public bool EnableSendMail { get; set; }
    }
}
