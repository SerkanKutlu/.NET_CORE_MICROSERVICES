using CustomerService.Common;
using CustomerService.Common.Exceptions;
using CustomerService.Data;
using CustomerService.Logger;
using CustomerService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
 

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();

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
#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#endregion
#region AdditionalServices

builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddCommonExtensions(builder.Configuration);
builder.Services.AddRepositoryExtensions(builder.Configuration);
#endregion
#region SeriLog

builder.AddSeriLogConfiguration();

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


