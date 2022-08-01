namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Identities;

public record IdentityMock : Id<IdentityMock, uint>
{
	public IdentityMock(uint value) 
		: base(value)
	{
	}
}