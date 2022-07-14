using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.Identities;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests;

public class UserEntityMock : DomainObject
{
	public IId GetId() => new Id<int>(0);
	
	public string Name { get; }

	public UserEntityMock(string name)
	{
		this.Name = name;
	}
}