namespace CodeChops.Contracts.Adapters;

/// <summary>
/// Provides a way to convert an object to a contract and/or vice versa.
/// </summary>
public interface IAdapter
{
	string ContractName { get; }
	Type ContractType { get; }
	Type ObjectType { get; }
}