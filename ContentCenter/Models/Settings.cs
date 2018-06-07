
namespace ContentCenter.Models
{
    public class Settings
    {
        public Logging Logging { get; set; }       
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
        public SQLServer SQLServer { get; set; }
        public Certificate Certificate { get; set; }
        public MapCatalog mapCatalog { get; set; }
        public MapCatalog ImageCatalog { get; set; }
        public UpdateCatalog UpdateCatalog { get; set; }
        public SocketServer SocketServer { get; set; }
        public Advenced Advenced { get; set; }
    }

}
