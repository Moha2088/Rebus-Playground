using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Shared.Messaging.Events.IntegrationEvents;

namespace Product.API.Controllers;

public record CheckoutProductCommand(
        String Name,
        string Description,
        List<string> OrderItems,
        DateTimeOffset OrderDate,
        decimal Price) : IRequest
{
    public ProductCheckoutEvent FromCommand()
    {
        return new ProductCheckoutEvent(this.Name, this.Description, this.OrderItems, this.OrderDate,
            this.Price);
    }
}

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private ISender _sender;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ISender sender, ILogger<ProductsController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpPost]  
    public async Task<IActionResult> CheckoutProduct(CheckoutProductCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        _logger.LogInformation($"Sent command with Name: {command.Name}");
        return Ok();
    }
}
