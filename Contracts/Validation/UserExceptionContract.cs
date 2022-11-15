namespace CodeChops.DomainDrivenDesign.Contracts.Validation;

public record ValidationExceptionContract : Contract
{
	public required IErrorCode ErrorCode { get; init; }
	public required string Message { get; init; }
	public required IReadOnlyList<object?> Parameters { get; init; }
}
