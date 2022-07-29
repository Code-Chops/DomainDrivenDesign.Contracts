using CodeChops.DomainDrivenDesign.DomainModeling.Attributes;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Adapters;

[GenerateEntityId]
public partial class UserEntityMock : Entity
{
	public string Name { get; }

	public UserEntityMock(string name)
	{
		this.Name = name;
	}
}