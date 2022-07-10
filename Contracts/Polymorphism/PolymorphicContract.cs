namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

/// <summary>
/// The base of a polymorphic model with a manually provided type discriminator.
/// </summary>
public abstract record PolymorphicContract : IPolymorphicContract
{
	/// <summary>
	/// This type ID is used to find the correct contract for deserialization. 
	/// </summary>
	public string TypeId => this.GetType().Name;
}

public interface IPolymorphicContract : IContract
{
	string TypeId { get; }
}