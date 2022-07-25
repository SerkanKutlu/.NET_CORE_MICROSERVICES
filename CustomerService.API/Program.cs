using System.Text;
using CustomerService.Common;
using CustomerService.Common.Exceptions;
using CustomerService.Core;
using CustomerService.Data;
using CustomerService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;


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

builder.Services.AddDataExtensions(builder.Configuration);
builder.Services.AddCommonExtensions(builder.Configuration);
builder.Services.AddRepositoryExtensions(builder.Configuration);
builder.AddSeriLogConfiguration();
builder.Services.AddCoreExtensions(builder.Configuration);
#endregion

#region Authentication



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
    };
});


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion


