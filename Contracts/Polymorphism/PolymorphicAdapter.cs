using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

/// <summary>
/// Provides a way to convert an entity model (of concrete types) to a contract and vice versa, using a domain object with a type discriminator.
/// Whereby 1 contract is tightly coupled to 1 domain object type.
/// </summary>
public abstract record PolymorphicAdapter<TPolymorphicContract, TDomainObject> : Adapter
	where TPolymorphicContract : IContract
	where TDomainObject : DomainObject
{
	/// <summary>
	/// The contract is tightly coupled to 1 domain object type.
	/// </summary>
	protected internal override Type GetDomainObjectType() => typeof(TDomainObject);

	protected internal abstract override TDomainObject ConvertContractToDomainObject(IContract contract);
}

/// <summary>
/// Provides a way to convert an entity model (with a concrete contract type) to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
/// <typeparam name="TPolymorphicContract">The contract.</typeparam>
public abstract record PolymorphicAdapter<TPolymorphicContract> : PolymorphicAdapter
	where TPolymorphicContract : PolymorphicContract
{
	public override Type GetContractType() => typeof(TPolymorphicContract);

	protected internal abstract override TPolymorphicContract ConvertDomainObjectToContract(IDomainObject domainObject);
}

/// <summary>
/// Provides a way to convert an entity model to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract record PolymorphicAdapter : Adapter
{
	/// <summary>
	/// Used for retrieving the correct polymorphic adapter in order to convert the contract to a domain object.
	/// </summary>
	public abstract Type GetContractType();
}