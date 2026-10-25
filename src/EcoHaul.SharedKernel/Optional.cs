namespace EcoHaul.SharedKernel;

public readonly record struct Optional<T>
{
    public bool IsSpecified { get; init; }

    public T? Value { get; init; }

    public static Optional<T> Specified(T? value) => new()
    {
        IsSpecified = true,
        Value = value
    };
}
