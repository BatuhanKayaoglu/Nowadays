using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace Nowadays.API.LoggingConfigurations
{
    public class MainConfiguration
    {
        public Logger ConfigureLogger(WebApplicationBuilder builder, IConfiguration configuration)
        {
            Logger log = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                     .Enrich.With(new CustomUserNameColumn()) // In order to send data into the 'UserName' parameter that I added to the SQL Logs table.
                .CreateLogger();
            return log;
        }
    }
}
