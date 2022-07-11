namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

public abstract record PolymorphicContract : IContract
{
	public override string ToString() => $"{this.GetType().Name} with {nameof(this.TypeId)}: {this.TypeId}.";

	public string TypeId => this.GetType().Name;
}