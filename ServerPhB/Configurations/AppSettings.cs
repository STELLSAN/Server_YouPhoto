using System;

namespace ServerPhB.Configurations
{
    public class AppSettings
    {
        public string SomeSetting { get; set; }
        public LoggingSettings Logging { get; set; }
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }
    }

    public class LogLevelSettings
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }
}
