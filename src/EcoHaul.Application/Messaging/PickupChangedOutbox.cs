using System.Text.Json;
using System.Text.Json.Serialization;
using EcoHaul.Domain;

namespace EcoHaul.Application.Messaging;

public static class PickupChangedOutbox
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public static OutboxMessage Create(Pickup pickup)
    {
        var change = new PickupChanged(
            pickup.Id,
            pickup.CustomerId,
            pickup.RequestedOn,
            pickup.Status);

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = OutboxMessageType.PickupChanged,
            Payload = JsonSerializer.Serialize(change, JsonOptions),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
