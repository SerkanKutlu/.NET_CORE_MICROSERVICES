
using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.Rabbit;
using CustomerService.RabbitConsumer;
using CustomerService.RabbitConsumer.Consumers;
using CustomerService.RabbitConsumer.Models;
using CustomerService.RabbitConsumer.Repository;
using GenericMongo;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args);

host.ConfigureServices(services =>
{
    var environmentName = Environment.GetEnvironmentVariables()["DOTNET_ENVIRONMENT"];
    var configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{environmentName}.json", optional: false)
        .Build();
    services.AddHostedService<Worker>();
    services.AddGenericMongo<Log>(settings =>
    {
        settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
        settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
        settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
    });
    //Rabbit
    services.Configure<RabbitSettings>(configuration.GetSection(nameof(RabbitSettings)));
    services.AddSingleton<IRabbitSettings>(provider => provider.GetRequiredService<IOptions<RabbitSettings>>().Value);

    services.AddSingleton<LogRepository>();
    services.AddSingleton<RabbitConsumer>();

});


await host.Build().RunAsync();