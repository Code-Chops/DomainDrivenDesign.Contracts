namespace CodeChops.Contracts.UnitTests.Adapters;

public record UserAdapterMock : Adapter<UserEntityMock, UserContractMock>
{
	public override UserContractMock ConvertToContract(UserEntityMock domainObject)
		=> new(domainObject.Name);

	public override UserEntityMock ConvertToObject(UserContractMock contract)
		=> new(contract.Name);
}