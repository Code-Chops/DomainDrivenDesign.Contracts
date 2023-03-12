using CodeChops.Contracts.Adapters;

namespace CodeChops.Contracts.Validation;

public sealed record ValidationExceptionAdapter : Adapter<ValidationException, ValidationExceptionContract>
{
	public override ValidationExceptionContract ConvertToContract(ValidationException exception) 
		=> new()
	{
		ErrorCode = exception.ErrorCode,
		Parameters = exception.ExternalMessage.Parameters,
		Message = exception.ExternalMessage.Message,
	};

	public override ValidationException ConvertToObject(ValidationExceptionContract contract) 
		=> new(contract.ErrorCode, new ValidationExceptionMessage((contract.Message, contract.Parameters.ToImmutableList())!));
}