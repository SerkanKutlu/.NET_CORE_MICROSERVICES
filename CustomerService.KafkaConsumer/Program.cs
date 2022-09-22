using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.Kafka;
using CustomerService.KafkaConsumer;
using CustomerService.KafkaConsumer.Consumers;
using CustomerService.KafkaConsumer.Repository;
using GenericMongo;
using Microsoft.Extensions.Options;
using Serilog;
using Log = CustomerService.KafkaConsumer.Models.Log;

var host = Host.CreateDefaultBuilder(args);
host.ConfigureServices(services =>
{
    var environmentName = Environment.GetEnvironmentVariables()["DOTNET_ENVIRONMENT"];
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{environmentName}.json",optional:false)
        .Build();
    services.AddHostedService<Worker>();
    services.AddGenericMongo<Log>(settings =>
    {
        settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
        settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
        settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
    });
    //Kafka
    services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
    services.AddSingleton<IKafkaSettings>(provider=>provider.GetRequiredService<IOptions<KafkaSettings>>().Value);
    services.AddSingleton<LogRepository>();
    services.AddSingleton<KafkaConsumer>();
    services.AddSingleton<IKafkaPublisher,KafkaPublisher>();
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