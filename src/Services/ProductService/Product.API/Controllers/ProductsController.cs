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
        return new ProductCheckoutEvent(Guid.NewGuid(), this.Name, this.Description, this.OrderItems, this.OrderDate,
            this.Price);
    }
}

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IBus _bus;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IBus bus, ILogger<ProductsController> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    [HttpPost]  
    public async Task<IActionResult> CheckoutProduct(CheckoutProductCommand command)
    {
        await _bus.Send(command);
        _logger.LogInformation($"Sent command with Id: {command.Id}, Name: {command.Name}");
        return Ok();
    }
}
