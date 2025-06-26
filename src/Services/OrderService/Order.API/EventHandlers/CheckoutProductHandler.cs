using Rebus.Handlers;
using Shared.Messaging.Events.IntegrationEvents;

namespace Order.API.EventHandlers;

public class CheckoutProductHandler : IHandleMessages<ProductCheckoutEvent>
{
    private readonly ILogger _logger;


    public CheckoutProductHandler()
    {
        //_logger = logger;
    }

    public async Task Handle(ProductCheckoutEvent message)
    {
        Console.WriteLine($"Received message, ID: {message.Id}, Name:  {message.Name} at {message.OcurredOn}");
    }
}