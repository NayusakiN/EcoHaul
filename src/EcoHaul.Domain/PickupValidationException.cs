namespace EcoHaul.Domain;

public sealed class PickupValidationException(string message) : Exception(message);
