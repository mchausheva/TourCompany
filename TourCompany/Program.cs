using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TourCompany.BL.CommandHandlers;
using TourCompany.BL.Kafka;
using TourCompany.Extentions;
using TourCompany.HealthChecks;
using TourCompany.Middleware;
using TourCompany.Models.Configurations;

var logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);

builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection(nameof(KafkaConfig)));

builder.Services.Configure<MongoDbConfiguration>(
    builder.Configuration.GetSection(nameof(MongoDbConfiguration)));


// Add services to the container.
builder.Services.RegisterRepositories()
                .RegisterServices()
                .AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://thetourcompany.co.uk/about/"), name: "About The Tour Company")
                .AddCheck<KafkaHealthCheck>("Kafka Settings")
                .AddCheck<SqlHealthCheck>("SQL Server")
                .AddCheck<MongoHealthCheck>("MongoDB");

// Add MediatR
builder.Services.AddMediatR(typeof(GetAllDestinationsCommandHandler).Assembly);


//App builder below
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

//app.MapHealthChecks("/health");
app.RegisterHealthCkecks();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();
