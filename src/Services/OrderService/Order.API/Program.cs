
using Order.API.EventHandlers;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region RebusConfig Sub
var logger = app.Services.GetRequiredService<ILogger>();
using var activator = new BuiltinHandlerActivator();
activator.Register(() => new CheckoutProductHandler(logger));

var subscriber = Configure.With(activator)
    .Transport(transport => transport.UseRabbitMq(builder.Configuration["RabbitMQ:ConnectionString"], "product-queue"))
    .Start();

#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
