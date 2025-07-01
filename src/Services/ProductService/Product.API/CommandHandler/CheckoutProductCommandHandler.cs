using MediatR;
using Product.API.Commands;
using Rebus.Bus;

namespace Product.API.CommandHandler;

public class CheckoutProductCommandHandler : IRequestHandler<CheckoutProductCommand>
{
    private readonly IBus _bus;
    private readonly ILogger<CheckoutProductCommandHandler> _logger;


    public CheckoutProductCommandHandler(IBus bus, ILogger<CheckoutProductCommandHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }


    public async Task Handle(CheckoutProductCommand command, CancellationToken cancellationToken)
    {
        var checkoutEvent = command.FromCommand();
        await _bus.Publish(checkoutEvent);
        _logger.LogInformation($"Published event with Id: {checkoutEvent.Id}, Name: {checkoutEvent.Name}, at: {checkoutEvent.OcurredOn}");
    }
}