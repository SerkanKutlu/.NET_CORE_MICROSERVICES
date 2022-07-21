using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CustomerService.Logger;

public static class LoggerExtensions
{
    public static void AddSeriLogConfiguration(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
            .Enrich
            .FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

    }
}