using Rebus.Handlers;
using Shared.Messaging.Events.IntegrationEvents;

namespace Order.API.EventHandlers;

public class CheckoutProductEventHandler : IHandleMessages<ProductCheckoutEvent>
{
    private readonly ILogger _logger;


    public CheckoutProductEventHandler()
    {
        //_logger = logger;
    }

    public async Task Handle(ProductCheckoutEvent message)
    {
        Console.WriteLine($"Received message,\nID: {message.Id}\nName:  {message.Name}\nDescription: {message.Description}\nPrice: {message.Price} at {message.OcurredOn.AddHours(2)}");
    }
}