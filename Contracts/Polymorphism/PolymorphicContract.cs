namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

public abstract record PolymorphicContract : IContract
{
	public override string ToString() => $"{this.GetType().Name} with {nameof(this.TypeId)}: {this.TypeId}.";

	/// <summary>
	/// This type ID is used to find the correct contract for deserialization. 
	/// </summary>
	public string TypeId => this.GetType().Name;
}