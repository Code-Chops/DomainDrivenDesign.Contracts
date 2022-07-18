using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.MagicEnums;

public class EntityWithMagicEnumMock : DomainObject
{
	public int Age { get; init; }
	public MagicEnumWrapperContractMock Wrapper { get; init; } = null!;
}

public record EntityContract(int Age, MagicEnumWrapperContractMock Wrapper) : Contract;


public record EntityAdapter : Adapter<EntityContract, EntityWithMagicEnumMock>
{
	protected internal override Type GetDomainObjectType() => typeof(EntityWithMagicEnumMock);
	protected internal override EntityWithMagicEnumMock ConvertContractToDomainObject(Contract contract)
		=> contract is EntityContract entityContract ? new EntityWithMagicEnumMock() { Age = entityContract.Age, Wrapper = entityContract.Wrapper} : null!;

	protected internal override EntityContract ConvertDomainObjectToContract(IDomainObject domainObject)
		=> domainObject is EntityWithMagicEnumMock entity ? new EntityContract(entity.Age, entity.Wrapper) : null!;
}