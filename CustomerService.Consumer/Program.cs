using CustomerService.Consumer;
using CustomerService.Consumer.Consumers;
using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Mongo;
using CustomerService.Consumer.Services;
using Microsoft.Extensions.Options;
using Serilog;
var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices(services =>
{
    var environmentName = Environment.GetEnvironmentVariables()["DOTNET_ENVIRONMENT"];
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{environmentName}.json",optional:false)
        .Build();
    services.AddHostedService<Worker>();
    services.AddSingleton<IConsumerService, ConsumerService>();
    services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
    services.AddSingleton<IMongoService,MongoService>();
    services.AddSingleton(sp =>
        new LogConsumer("customer.log", "*.log","queue.log",sp.GetRequiredService<IConsumerService>()));
    
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