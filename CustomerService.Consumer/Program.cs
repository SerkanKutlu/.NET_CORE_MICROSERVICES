using CustomerService.Consumer;
using CustomerService.Consumer.Consumers;
using MassTransit;
using Serilog;
var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices(services =>
{
    services.AddHostedService<Worker>();
    services.AddSingleton<LogConsumer>(sp =>
        new LogConsumer("customerExchange", "*.log","logQueue",sp.GetRequiredService<ILogger<LogConsumer>>()));
});

host.ConfigureLogging(loggingBuilder =>
{
    var environmentName = Environment.GetEnvironmentVariables()["DOTNET_ENVIRONMENT"];
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{environmentName}.json")
        .Build();
    
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(logger);
});

await host.Build().RunAsync();