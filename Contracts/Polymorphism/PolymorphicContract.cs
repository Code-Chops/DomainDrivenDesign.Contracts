namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

public abstract record PolymorphicContract : Contract
{
	public override string ToString() => $"{this.GetType().Name} with {nameof(this.TypeId)}: {this.TypeId}.";

	/// <summary>
	/// This type ID is used to find the correct contract for deserialization.
	/// Has to be an auto-implemented property: in order to retrieve the type ID in a static context and in order to serialize it. 
	/// </summary>
	public string TypeId => this.GetType().Name;
}