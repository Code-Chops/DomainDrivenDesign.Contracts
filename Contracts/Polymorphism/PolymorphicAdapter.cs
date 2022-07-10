using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.Identities;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

/// <summary>
/// Provides a way to convert an entity model (of concrete types) to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
/// <typeparam name="TPolymorphicContract">The contract.</typeparam>
/// <typeparam name="TDomainObject">The domain entity or aggregate model that is discoverable.</typeparam>
public abstract class PolymorphicAdapter<TPolymorphicContract, TDomainObject> : PolymorphicAdapter<TPolymorphicContract>
	where TPolymorphicContract : PolymorphicContract, IHasStaticTypeId<Id<string>>
	where TDomainObject : DomainObject
{
	public override Type GetDomainObjectType() => typeof(TDomainObject);
	
	protected internal abstract override TDomainObject ConvertContractToDomainObject(IContract contract);
}

/// <summary>
/// Provides a way to convert an entity model (with a concrete contract type) to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
/// <typeparam name="TPolymorphicContract">The contract.</typeparam>
public abstract class PolymorphicAdapter<TPolymorphicContract> : PolymorphicAdapter
	where TPolymorphicContract : PolymorphicContract, IHasStaticTypeId<Id<string>>
{
	public override Type GetContractType() => typeof(TPolymorphicContract);
	
	protected internal abstract override TPolymorphicContract ConvertDomainObjectToContract(IDomainObject entity);
}

/// <summary>
/// Provides a way to convert an entity model to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract class PolymorphicAdapter : Adapter
{
	/// <summary>
	/// Used for retrieving the correct polymorphic adapter in order to convert the contract to a domain object.
	/// </summary>
	public abstract Type GetContractType();
	
	/// <summary>
	/// Used for retrieving the correct polymorphic adapter in order to convert the domain object to a contract.
	/// </summary>
	public abstract Type GetDomainObjectType();
}