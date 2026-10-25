using EcoHaul.Application.Abstractions.Messaging;

namespace EcoHaul.Application.Photos;

public sealed record AddPhotoCommand(
    Guid PickupId,
    Guid CustomerId,
    string FileName,
    string ContentType,
    Stream Content) : ICommand<Guid>;

public sealed record ReplacePhotoCommand(
    Guid PickupId,
    Guid PhotoId,
    Guid CustomerId,
    string? ETag,
    string FileName,
    string ContentType,
    Stream Content) : ICommand;

public sealed record OpenPhotoCommand(Guid PickupId, Guid PhotoId, Guid CustomerId) : IQuery<string>;
