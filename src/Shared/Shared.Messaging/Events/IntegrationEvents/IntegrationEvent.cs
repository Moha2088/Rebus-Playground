namespace Shared.Messaging.Events.IntegrationEvents;

public record IntegrationEvent
{
    public Guid Id = Guid.NewGuid();

    public DateTimeOffset OcurredOn => DateTimeOffset.Now;

    public string EventType => GetType().AssemblyQualifiedName!;
}