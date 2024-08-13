using Nowadays.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Nowadays.API.Exceptions;
using Nowadays.API.Extensions;
using Nowadays.API.Extensions.JwtConf;
using Nowadays.API.LoggingConfigurations;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Text;
using Nowadays.API.Middlewares.ExceptionHandlerMiddleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Start - add Swagger with Auth 
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Nowadays - Issue Tracking",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer TokenTest321 \"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// End - Add Swagger with Auth 

builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddAPIRegistration(builder.Configuration);



//FOR SERILOG
//builder.Host.UseSerilog(new MainConfiguration().ConfigureLogger(builder, builder.Configuration)); // I make all the configurations in the Main Configuration section and just call it in Program.cs.
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    var mainConfiguration = new MainConfiguration();
    var logger = mainConfiguration.ConfigureLogger(builder, builder.Configuration);
    loggerConfiguration.WriteTo.Logger(logger);
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandlingMiddleware();

app.UseHttpsRedirection();

// If we are in a development environment, I send it this way because I want to see the details. However, when I upload the same application to PROD, I will only see the message part instead of all the details.
//app.ConfigureExceptionHandling(app.Environment.IsDevelopment());

//FOR SERILOG
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
