namespace CodeChops.Contracts.UnitTests.Adapters;

public record UserAdapterMock : Adapter<UserEntityMock, UserContractMock>
{
	public override UserContractMock ConvertObjectToContract(object domainObject)
		=> new(((UserEntityMock)domainObject).Name);

	public override UserEntityMock ConvertContractToObject(Contract contract)
		=> new(((UserContractMock)contract).Name);
}