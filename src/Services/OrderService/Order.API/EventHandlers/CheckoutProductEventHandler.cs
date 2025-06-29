using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Pipeline;
using Shared.Messaging.Events.IntegrationEvents;

namespace Order.API.EventHandlers;

public class CheckoutProductEventHandler : IHandleMessages<ProductCheckoutEvent>
{
    private readonly ILogger<CheckoutProductEventHandler> _logger;


    public CheckoutProductEventHandler(ILogger<CheckoutProductEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductCheckoutEvent message)
    {
        _logger.LogInformation($"Received message,\nID: {message.Id}\nName:  {message.Name}\nDescription: {message.Description}\nPrice: {message.Price} at {message.OcurredOn.AddHours(2)}");
    }
}