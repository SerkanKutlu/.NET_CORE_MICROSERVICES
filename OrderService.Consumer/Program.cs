using Microsoft.Extensions.Options;
using OrderService.Consumer;
using OrderService.Consumer.Consumers;
using OrderService.Consumer.Interfaces;
using OrderService.Consumer.Mongo;
using OrderService.Consumer.Services;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
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
        services.AddSingleton<KafkaConsumer>();
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