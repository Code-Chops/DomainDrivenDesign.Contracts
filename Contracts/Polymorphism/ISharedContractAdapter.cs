namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

/// <summary>
/// A contract can be used by multiple adapters. This factory gives the correct adapter by providing a contract.
/// </summary>
public interface ISharedContractAdapter
{
	public static abstract Adapter GetAdapterFromContract(PolymorphicContract contract);
}