using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.API.Commands;

namespace Product.API.Controllers;


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

    [HttpPost("checkout")]  
    public async Task<IActionResult> CheckoutProduct(CheckoutProductCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        _logger.LogInformation($"Sent command with Name: {command.Name}");
        return Ok();
    }
}
