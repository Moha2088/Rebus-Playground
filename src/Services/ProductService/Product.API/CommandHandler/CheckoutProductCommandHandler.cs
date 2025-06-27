using MediatR;
using Product.API.Controllers;
using Rebus.Bus;

namespace Product.API.CommandHandler;

public class CheckoutProductCommandHandler : IRequestHandler<CheckoutProductCommand>
{
    private readonly IBus _bus;


    public CheckoutProductCommandHandler(IBus bus)
    {
        _bus = bus;
    }


    public async Task Handle(CheckoutProductCommand command, CancellationToken cancellationToken)
    {
        var checkoutEvent = command.FromCommand();
        await _bus.Publish(checkoutEvent);
        Console.WriteLine($"Published event with Id: {checkoutEvent.Id}, Name: {checkoutEvent.Name}, at: {checkoutEvent.OcurredOn}");
    }
}