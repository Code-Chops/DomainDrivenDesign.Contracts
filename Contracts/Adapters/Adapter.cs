namespace CodeChops.Contracts.Adapters;

/// <summary>
/// Provides a way to convert an object to a contract and vice versa.
/// </summary>
public abstract record Adapter<TObject, TContract> : IBidirectionalAdapter
	where TObject : class
	where TContract : Contract
{
	public override string ToString() => $"{this.GetType().Name} {{ {nameof(this.ContractName)} = {this.ContractName} }}";
	
	public string ContractName { get; } = typeof(TContract).Name;

	public Type ContractType => typeof(TContract);
	public Type ObjectType => typeof(TObject);

	public abstract TContract ConvertToContract(TObject @object);
	Contract IOutgoingAdapter.ConvertToContract(object @object)
		=> this.ConvertToContract(@object as TObject ?? NotNullGuard<TObject>.ThrowException<TObject>(this.GetType().Name, typeof(TObject).Name, errorCode: null));
	
	public abstract TObject ConvertToObject(TContract contract);
	object IIncomingAdapter.ConvertToObject(Contract contract)
		=> this.ConvertToObject(contract as TContract ?? NotNullGuard<TContract>.ThrowException<TContract>(this.GetType().Name, typeof(TContract).Name, errorCode: null));
}