using CustomerService.Consumer.Consumers;
using MassTransit;
using MassTransit.MultiBus;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Logger

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


#endregion

#region MassTransit

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LogAtCreateConsumer>();
    x.AddConsumer<LogAtUpdateConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
        cfg.UseRetry(r=>r.Interval(5,TimeSpan.FromSeconds(10)));
        
    });
});
// builder.Services.AddMassTransit(x =>
// {
//     x.AddConsumer<LogAtCreateConsumer>();
//     x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
//     {
//         cfg.Host(new Uri("rabbitmq://localhost"),h =>
//         {
//             h.Username("guest");
//             h.Password("guest");
//         });
//         cfg.ReceiveEndpoint("createQueue.Customer", ep =>
//         {
//             ep.UseMessageRetry(r => r.Interval(2, 100));
//             ep.ConfigureConsumer<LogAtCreateConsumer>(provider);
//         });
//     }));
// });

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();