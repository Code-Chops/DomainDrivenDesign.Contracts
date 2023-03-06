namespace CodeChops.Contracts;

/// <summary>
/// Provides a way to convert a domain object to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract record Adapter<TObject, TContract> : Adapter<TContract> 
	where TContract : Contract
{
	/// <summary>
	/// The contract is tightly coupled to 1 domain object type.
	/// </summary>
	protected internal override Type GetDomainObjectType() => typeof(TObject);
}

/// <summary>
/// Provides a way to convert a domain object to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract record Adapter<TContract> : Adapter 
	where TContract : Contract
{
	public override string ObjectName { get; } = typeof(TContract).Name;

	protected internal override Type GetContractType() => typeof(TContract);
	public abstract override TContract ConvertObjectToContract(object o);
}

/// <summary>
/// Provides a way to convert an entity model to a contract and vice versa, using a domain object with a type discriminator.
/// </summary>
public abstract record Adapter
{
	public override string ToString() => $"{this.GetType().Name} {{ {nameof(this.ObjectName)} = {this.ObjectName} }}";
	
	public abstract string ObjectName { get; }

	/// <summary>
	/// Used by the converter for retrieving the correct adapter in order to convert the domain object to a contract.
	/// </summary>
	protected internal abstract Type GetDomainObjectType();
	
	/// <summary>
	/// Used for retrieving the correct adapter in order to convert the contract to a domain object.
	/// </summary>
	protected internal abstract Type GetContractType();
	
	public abstract Contract ConvertObjectToContract(object o);
	public abstract object ConvertContractToObject(Contract contract);
}