using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.Redis;
using GenericMongo;
using Microsoft.Extensions.Options;
using RedisSub;
using RedisSub.Entities;
using RedisSub.Interfaces;
using RedisSub.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json",optional:false)
            .Build();
        services.AddHostedService<Worker>();
        //Redis
        services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));
        services.AddSingleton<IRedisSettings>(provider=>provider.GetRequiredService<IOptions<RedisSettings>>().Value);
        services.AddSingleton<IRedisService, RedisService>();
        services.AddGenericMongo<Log>(settings =>
        {
            settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
            settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
            settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
        });
        services.AddSingleton<LogRepository>();
        services.AddSingleton<IRedisSubscriber, RedisSubscriber>();
        services.AddSingleton<IRedisPublisher, RedisPublisher>();
    })
    .Build();

await host.RunAsync();