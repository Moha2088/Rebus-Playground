namespace Shared.Messaging.Events.IntegrationEvents;

public record ProductCheckoutEvent(Guid Id, String Name, string Description, List<string> OrderItems, DateTimeOffset OrderDate, decimal Price) : IntegrationEvent;