namespace EcoHaul.SharedKernel;

public sealed record ValidationErrorDetail(string PropertyName, string Code, string Description);

public sealed record ValidationError : Error
{
    public ValidationError(ValidationErrorDetail[] errors)
        : base(
            "Validation.General",
            "One or more validation errors occurred.",
            ErrorType.Validation)
    {
        Errors = errors;
    }

    public ValidationErrorDetail[] Errors { get; }

    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results
            .Where(result => result.IsFailure)
            .Select(result => new ValidationErrorDetail(string.Empty, result.Error.Code, result.Error.Description))
            .ToArray());
}
