using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(opt =>
{
    var assembly = typeof(Program).Assembly;

    opt.RegisterServicesFromAssembly(assembly);

});

#region RebusConfig Pub

builder.Services.AddRebus(configure =>
            configure.Transport(transport =>
                transport.UseRabbitMqAsOneWayClient(builder.Configuration["RabbitMQ:ConnectionString"]))
            .Routing(route =>
                route.TypeBased().MapAssemblyOf<Program>("product-queue"))
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
