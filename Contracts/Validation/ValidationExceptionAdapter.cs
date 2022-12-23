namespace CodeChops.DomainDrivenDesign.Contracts.Validation;

public record ValidationExceptionAdapter : Adapter<ValidationException, ValidationExceptionContract>
{
	protected internal override ValidationExceptionContract ConvertDomainObjectToContract(IDomainObject domainObject)
		=> domainObject is ValidationException exception 
			? new ValidationExceptionContract()
				{
					ErrorCode = exception.ErrorCode,
					Parameters = exception.ExternalMessage.Parameters, 
					Message = exception.ExternalMessage.Message,
				}
			: throw new InvalidOperationException($"Cannot convert {nameof(ValidationException)} to {nameof(ValidationExceptionContract)}. Domain object is not of type {typeof(ValidationException)}.");

	protected internal override IDomainObject ConvertContractToDomainObject(Contract contract) 
		=> contract is ValidationExceptionContract validationContract 
			? new ValidationException(validationContract.ErrorCode, new ValidationExceptionMessage(message: validationContract.Message, parameters: validationContract.Parameters.ToList()))
			: throw new InvalidOperationException($"Cannot convert {nameof(ValidationExceptionContract)} to {nameof(ValidationException)}. Contract is not of type {typeof(ValidationExceptionContract)}.");
}