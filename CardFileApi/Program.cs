using BLL.Dependencies;
using BLL.Validation.Validators;
using CardFileApi;
using CardFileApi.Extensions;
using DAL.Dependencies;
using FluentValidation.AspNetCore;
using NLog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Set up configuration
var configuration = new ConfigurationBuilder().
    SetBasePath(Directory.GetCurrentDirectory()).
    AddJsonFile("appsettings.json", false).
    Build();

// Set up configuration for logger service
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining(typeof(TextMaterialValidator));
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.ConfigureSqlContext(configuration);
builder.Services.ConfigureAuthentication(configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureJwt();
builder.Services.ConfigureHttpContextAccessor();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureDALServices();
builder.Services.ConfigureBLLServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }

