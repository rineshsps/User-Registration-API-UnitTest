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
    }
}
