
using Order.API.EventHandlers;
using Rebus.Config;
using Serilog;
using Shared.Configurations;
using Shared.Messaging.Events.IntegrationEvents;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

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

#region RebusConfig Sub

builder.Services.AutoRegisterHandlersFromAssemblyOf<Program>();

builder.Services.AddLogging();
builder.Services.AddTransient<CheckoutProductEventHandler>();

builder.Services.AddRebus(configure =>
    configure.Transport(transport =>
        transport.UseRabbitMq(builder.Configuration["RabbitMQ:ConnectionString"], "product-queue"))
    .Logging(logging => logging.Console())
    .Options(opt => opt.SetBusName("Order MessageBus")),
    
    onCreated: async bus =>
    {
        await bus.Subscribe<ProductCheckoutEvent>();
    });

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
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
