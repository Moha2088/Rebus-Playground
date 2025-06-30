

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Serilog;
using Shared.Configurations;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddMediatR(opt =>
{
    var assembly = typeof(Program).Assembly;
    opt.RegisterServicesFromAssembly(assembly);

});

#region ElasticSearch

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var sinkOptions = ElasticSearchConfigurations.ConfigureElastic(builder.Configuration, environment!);

Log.Logger = new LoggerConfiguration()
.Enrich.FromLogContext()
.Enrich.WithProperty("Environment", environment)
.MinimumLevel.Information()
.WriteTo.Debug()
.WriteTo.Console()
.WriteTo.Elasticsearch(sinkOptions)
.CreateLogger();

builder.Host.UseSerilog();

#endregion

#region RebusConfig Pub

var services = new ServiceCollection();
services.AddLogging();

builder.Services.AddRebus(configure =>
            configure.Transport(transport =>
                transport.UseRabbitMqAsOneWayClient(builder.Configuration["RabbitMQ:ConnectionString"]))
            .Routing(route =>
                route.TypeBased().MapAssemblyOf<Program>("product-queue"))
            .Logging(logging => logging.Console())
            .Options(opt =>
            {
                opt.RetryStrategy(maxDeliveryAttempts: 10);
                opt.SetBusName("Product MessageBus");
            }));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
