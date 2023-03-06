namespace CodeChops.Contracts.Validation;

public record ValidationExceptionAdapter : Adapter<ValidationException, ValidationExceptionContract>
{
	public override ValidationExceptionContract ConvertObjectToContract(object o)
		=> o is ValidationException exception 
			? new ValidationExceptionContract()
				{
					ErrorCode = exception.ErrorCode,
					Parameters = exception.ExternalMessage.Parameters, 
					Message = exception.ExternalMessage.Message,
				}
			: throw new InvalidOperationException($"Cannot convert {nameof(ValidationException)} to {nameof(ValidationExceptionContract)}. Domain object is not of type {typeof(ValidationException)}.");

	public override object ConvertContractToObject(Contract contract) 
		=> contract is ValidationExceptionContract validationContract 
			? new ValidationException(validationContract.ErrorCode, new ValidationExceptionMessage((validationContract.Message, validationContract.Parameters.ToImmutableList())!))
			: throw new InvalidOperationException($"Cannot convert {nameof(ValidationExceptionContract)} to {nameof(ValidationException)}. Contract is not of type {typeof(ValidationExceptionContract)}.");
}