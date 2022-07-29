namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Adapters;

public record UserAdapterMock : Adapter<UserContractMock, UserEntityMock>
{
	protected internal override UserContractMock ConvertDomainObjectToContract(IDomainObject domainObject)
		=> new(((UserEntityMock)domainObject).Name);

	protected internal override UserEntityMock ConvertContractToDomainObject(Contract contract)
		=> new(((UserContractMock)contract).Name);
}