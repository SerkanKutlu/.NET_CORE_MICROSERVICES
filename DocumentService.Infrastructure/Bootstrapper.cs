using Core.Entity;
using Core.Helpers;
using Core.Interfaces;
using Core.Middlewares;
using DocumentService.Infrastructure.Hangfire;
using DocumentService.Infrastructure.Kafka;
using DocumentService.Infrastructure.Repository;
using GenericMongo;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DocumentService.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection RegisterComponents(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services,configuration);
        AddServices(services,configuration);
        AddHelpers(services);
        AddConfigurations(services,configuration);
        return services;
    }
    
    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
    public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard();
        return app;
    }

    private static void AddRepositories(IServiceCollection services,IConfiguration configuration)
    {
        services.AddGenericMongo<DocumentEntity>(settings =>
        {
            settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
            settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
            settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
        }, collection =>
        {
            var options = new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.FromSeconds(0),
                Name = "ExpireIndex"
            };
            var indexModel = new CreateIndexModel<DocumentEntity>("{ExpireAt:1}",options);
            collection.Indexes.CreateOne(indexModel);
        });
        services.AddScoped<IDocumentRepository, DocumentRepository>();
       
    }
    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDocumentService, Services.DocumentService>();
        services.AddHttpClient();
        
        //Hangfire
        var migrationOptions = new MongoMigrationOptions
        {   
            MigrationStrategy =new ThrowMongoMigrationStrategy(),// new MigrateMongoMigrationStrategy(), 
            BackupStrategy = new NoneMongoBackupStrategy()
        };
        var storageOptions = new MongoStorageOptions
        {
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection ,
            //Watch is for replica sets only. 
            MigrationOptions = migrationOptions
        };
        services.AddHangfire(config => config.UseMongoStorage(configuration.GetSection("MongoSettings")["HangfireConnectionString"],storageOptions));
        services.AddHangfireServer();
       
        
        //Kafka
        services.AddSingleton<IKafkaPublisher, KafkaPublisher>();
        
        //Hanfire
        services.AddSingleton<JobService>();
    }
    

    private static void AddHelpers(IServiceCollection services)
    {
        services.AddScoped<IAuthHelper, AuthHelper>();
        services.AddScoped<IFileHelper, FileHelper>();
    }

    private static void AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        //Kafka
        services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
        services.AddSingleton<IKafkaSettings>(provider=>provider.GetRequiredService<IOptions<KafkaSettings>>().Value);

    }

  
}