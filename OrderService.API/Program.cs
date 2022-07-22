using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderService.Common;
using OrderService.Common.Exceptions;
using OrderService.Core;
using OrderService.Data;
using OrderService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Services

builder.Services.AddControllers();

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#endregion
#region SuppressDefaultValidation

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var messages = context.ModelState.Values
            .Where(x => x.ValidationState == ModelValidationState.Invalid)
            .SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();
        throw new InvalidModelException(string.Join($" , ", messages));
    };
});

#endregion
#region AdditionalServices

builder.Services.AddDataExtensions(builder.Configuration);
builder.Services.AddRepositoryExtensions(builder.Configuration);
builder.Services.AddCommonExtensions(builder.Configuration);
builder.AddSeriLogConfiguration();
builder.Services.AddCoreExtensions(builder.Configuration);
#endregion
#endregion





var app = builder.Build();

#region Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


#endregion
