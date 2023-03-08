namespace CodeChops.Contracts;

/// <inheritdoc cref="Adapter"/>
public abstract record Adapter<TObject, TContract> : Adapter, IAdapter<TObject, TContract>
	where TObject : notnull
	where TContract : Contract
{
	public override string ContractName { get; } = typeof(TContract).Name;

	public override Type ContractType => typeof(TContract);
	public override Type ObjectType => typeof(TObject);

	public abstract TContract ConvertToContract(TObject @object);
	public sealed override Contract ConvertToContract(object @object)
		=> this.ConvertToContract((TObject)@object);
	
	public abstract TObject ConvertToObject(TContract contract);
	public sealed override object ConvertToObject(Contract contract)
		=> this.ConvertToObject((TContract)contract);
}

/// <inheritdoc />
public abstract record Adapter : IAdapter
{
	public override string ToString() => $"{this.GetType().Name} {{ {nameof(this.ContractName)} = {this.ContractName} }}";

	public abstract string ContractName { get; }
	public abstract Type ContractType { get; }
	public abstract Type ObjectType { get; }

	public abstract Contract ConvertToContract(object @object);
	public abstract object ConvertToObject(Contract contract);
}

