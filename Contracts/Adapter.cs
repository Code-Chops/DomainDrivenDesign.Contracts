namespace CodeChops.Contracts;

/// <inheritdoc cref="Adapter"/>
public abstract record Adapter<TObject, TContract> : Adapter
	where TObject : class
	where TContract : Contract
{
	public override string ContractName { get; } = typeof(TContract).Name;

	public override Type ContractType => typeof(TContract);
	public override Type ObjectType => typeof(TObject);

	public abstract TContract ConvertToContract(TObject @object);
	protected internal sealed override Contract ConvertToContract(object @object)
		=> this.ConvertToContract(@object as TObject ?? NotNullGuard<TObject>.ThrowException<TObject>(this.GetType().Name, typeof(TObject).Name, errorCode: null));
	
	public abstract TObject ConvertToObject(TContract contract);
	protected internal sealed override object ConvertToObject(Contract contract)
		=> this.ConvertToObject(contract as TContract ?? NotNullGuard<TContract>.ThrowException<TContract>(this.GetType().Name, typeof(TContract).Name, errorCode: null));
}

/// <summary>
/// Provides a way to convert an object to a contract and vice versa.
/// </summary>
public abstract record Adapter
{
	public override string ToString() => $"{this.GetType().Name} {{ {nameof(this.ContractName)} = {this.ContractName} }}";

	public abstract string ContractName { get; }
	public abstract Type ContractType { get; }
	public abstract Type ObjectType { get; }

	protected internal abstract Contract ConvertToContract(object @object);
	protected internal abstract object ConvertToObject(Contract contract);
}

