namespace CodeChops.DomainDrivenDesign.Contracts;

public abstract record Contract
{
	public override string ToString() => this.GetType().Name;
}