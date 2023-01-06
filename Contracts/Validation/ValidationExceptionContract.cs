namespace CodeChops.Contracts.Validation;

public record ValidationExceptionContract : Contract
{
	public required string ErrorCode { get; init; }
	public required string Message { get; init; }
	public required IReadOnlyList<object?> Parameters { get; init; }
}
