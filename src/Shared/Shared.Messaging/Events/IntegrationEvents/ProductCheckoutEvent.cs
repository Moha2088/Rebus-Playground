namespace Shared.Messaging.Events.IntegrationEvents;

public record ProductCheckoutEvent(String Name, string Description, List<string> OrderItems, DateTimeOffset OrderDate, decimal Price) : IntegrationEvent;