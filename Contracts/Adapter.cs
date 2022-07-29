using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts;

/// <summary>
/// Provides a way to convert an entity model (of concrete types) to a contract and vice versa, using a domain object with a type discriminator.
/// Whereby 1 contract is tightly coupled to 1 domain object type.
/// </summary>
public abstract record Adapter<TContract, TDomainObject> : Adapter<TContract>
	where TContract : Contract
	where TDomainObject : IDomainObject
{
	/// <summary>
	/// The contract is tightly coupled to 1 domain object type.
	/// </summary>
	protected internal override Type GetDomainObjectType() => typeof(TDomainObject);
}

/// <summary>
/// Provides a way to convert an entity model (with a concrete contract type) to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
/// <typeparam name="TContract">The contract.</typeparam>
public abstract record Adapter<TContract> : Adapter 
	where TContract : Contract
{
	public override string Id { get; } = typeof(TContract).Name;

	protected internal override Type GetContractType() => typeof(TContract);

	protected internal abstract override TContract ConvertDomainObjectToContract(IDomainObject domainObject);
}

/// <summary>
/// Provides a way to convert an entity model to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract record Adapter
{
	public abstract string Id { get; }

	/// <summary>
	/// Used by the converter for retrieving the correct adapter in order to convert the domain object to a contract.
	/// </summary>
	protected internal abstract Type GetDomainObjectType();
	
	/// <summary>
	/// Used for retrieving the correct adapter in order to convert the contract to a domain object.
	/// </summary>
	protected internal abstract Type GetContractType();
	
	protected internal abstract Contract ConvertDomainObjectToContract(IDomainObject domainObject);
	protected internal abstract IDomainObject ConvertContractToDomainObject(Contract contract);
}