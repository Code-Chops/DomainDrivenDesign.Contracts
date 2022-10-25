namespace CodeChops.DomainDrivenDesign.Contracts.Contracts;

public abstract record Contract
{
	public override string ToString() => this.GetType().Name;
}