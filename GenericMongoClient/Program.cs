using GenericMongo;
using GenericMongo.Bases;
using GenericMongoClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGenericMongo<Student>(services =>
{
    services.CollectionName = "GenericClientCollection";
    services.ConnectionString = "mongodb://root:155202Asd...@localhost:27017";
    services.DatabaseName = "GenericClientCollectionDb";
});
builder.Services.AddSingleton<IStudentRepository,StudentRepository>();
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