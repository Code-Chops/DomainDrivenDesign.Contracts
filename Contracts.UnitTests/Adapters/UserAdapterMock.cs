namespace CodeChops.Contracts.UnitTests.Adapters;

public record UserAdapterMock : Adapter<UserEntityMock, UserContractMock>
{
	protected internal override UserContractMock ConvertObjectToContract(object domainObject)
		=> new(((UserEntityMock)domainObject).Name);

	protected internal override UserEntityMock ConvertContractToObject(Contract contract)
		=> new(((UserContractMock)contract).Name);
}