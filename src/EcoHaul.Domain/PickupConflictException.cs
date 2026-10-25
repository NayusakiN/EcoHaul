namespace EcoHaul.Domain;

public sealed class PickupConflictException(string message) : Exception(message);
