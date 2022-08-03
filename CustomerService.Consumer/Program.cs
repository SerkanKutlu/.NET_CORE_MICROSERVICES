using CustomerService.Consumer;
using CustomerService.Consumer.Consumers;
using MassTransit;
using Serilog;
var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices(services =>
{
    services.AddHostedService<Worker>();
    services.AddSingleton<LogConsumer>(sp =>
        new LogConsumer("topicExchange", "*.route","queue.log",sp.GetRequiredService<ILogger<LogConsumer>>()));
    services.AddSingleton<ExtraConsumer>(sp =>
        new ExtraConsumer("topicExchange", "*.route","queue.extra",sp.GetRequiredService<ILogger<ExtraConsumer>>()));
});

host.ConfigureLogging(loggingBuilder =>
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(logger);
});

await host.Build().RunAsync();