namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Adapters;

[GenerateStronglyTypedId]
public partial class UserEntityMock : Entity
{
	public string Name { get; }

	public UserEntityMock(string name)
	{
		this.Name = name;
	}
}