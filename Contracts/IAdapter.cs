namespace CodeChops.Contracts;

/// <inheritdoc />
public interface IAdapter<TObject, TContract> : IAdapter
	where TObject : notnull
	where TContract : Contract
{
	TContract ConvertToContract(TObject @object);
	TObject ConvertToObject(TContract contract);
}

/// <summary>
/// Provides a way to convert an object to a contract and vice versa.
/// </summary>
public interface IAdapter
{
	string ContractName { get; }
	Type ContractType { get; }
	Type ObjectType { get; }
	
	Contract ConvertToContract(object @object);
	object ConvertToObject(Contract contract);
}