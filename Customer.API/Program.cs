using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();

#region SuppressDefaultValidation

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

#endregion
#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#endregion




#endregion





var app = builder.Build();


#region Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion


