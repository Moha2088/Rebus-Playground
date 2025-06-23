namespace Shared.Messaging.Events.IntegrationEvents;

public record IntegrationEvent
{
    public Guid Guid = Guid.NewGuid();

    public DateTimeOffset OcurredOn => DateTimeOffset.Now;

    public string EventType => GetType().AssemblyQualifiedName!;
}