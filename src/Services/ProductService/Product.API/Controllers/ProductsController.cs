using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Shared.Messaging.Events.IntegrationEvents;

namespace Product.API.Controllers;

public record CheckoutProductCommand(
        Guid Id,
        String Name,
        string Description,
        List<string> OrderItems,
        DateTimeOffset OrderDate,
        decimal Price)
{
    public ProductCheckoutEvent FromCommand()
    {
        return new ProductCheckoutEvent(this.Id, this.Name, this.Description, this.OrderItems, this.OrderDate,
            this.Price);
    }
}

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IBus _bus;

    public ProductsController(IBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> CheckoutProduct(CheckoutProductCommand command)
    {
        var checkoutEvent = command.FromCommand();
        await _bus.Publish(checkoutEvent);
        return Ok();
    }
}
