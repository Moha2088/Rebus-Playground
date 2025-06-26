using Product.API.Controllers;
using Rebus.Bus;
using Rebus.Handlers;

namespace Product.API.CommandHandler;

public class CheckoutProductCommandHandler : IHandleMessages<CheckoutProductCommand>
{
    private readonly IBus _bus;


    public CheckoutProductCommandHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task Handle(CheckoutProductCommand command)
    {
        var checkoutEvent = command.FromCommand();
        await _bus.Publish(checkoutEvent);
        Console.WriteLine($"Published event with Id: {checkoutEvent.Id}, Name: {checkoutEvent.Name}");
    }
}
