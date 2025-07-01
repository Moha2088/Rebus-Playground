using MediatR;
using Shared.Messaging.Events.IntegrationEvents;

namespace Product.API.Commands;

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