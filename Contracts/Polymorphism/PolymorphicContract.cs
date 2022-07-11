namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

public abstract record PolymorphicContract : IContract
{
	public string TypeId { get; } = null!;
}