namespace CodeChops.Contracts.UnitTests.Adapters;

[GenerateIdentity]
public partial class UserEntityMock : Entity
{
	public string Name { get; }

	public UserEntityMock(string name)
	{
		this.Name = name;
	}
}