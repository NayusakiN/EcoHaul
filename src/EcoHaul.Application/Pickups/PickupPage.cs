namespace EcoHaul.Application.Pickups;

public sealed record PickupPage(
    IReadOnlyList<PickupResponse> Items,
    int Offset,
    int Limit,
    int Total)
{
    public const int DefaultLimit = 20;

    public const int MaxLimit = 100;
}
