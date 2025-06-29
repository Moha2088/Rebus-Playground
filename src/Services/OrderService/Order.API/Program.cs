
using Order.API.EventHandlers;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Shared.Messaging.Events.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
