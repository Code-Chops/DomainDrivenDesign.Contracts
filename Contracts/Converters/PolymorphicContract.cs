namespace CodeChops.Contracts.Converters;

public abstract record PolymorphicContract : Contract
{
	public override string ToString() => $"{this.GetType().Name} with {nameof(this.ImplementationId)}: {this.ImplementationId}.";

	/// <summary>
	/// This implementation ID is used to find the correct contract for deserialization.
	/// Has to be an auto-implemented property: in order to retrieve the implementation ID in a static context and in order to serialize it. 
	/// </summary>
	public string ImplementationId => this.GetType().Name;
}