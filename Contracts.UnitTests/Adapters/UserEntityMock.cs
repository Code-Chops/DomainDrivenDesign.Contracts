namespace CodeChops.Contracts.UnitTests.Adapters;

[GenerateIdentity(name: nameof(UserEntityMockId))]
public partial class UserEntityMock : Entity<UserEntityMockId>
{
	public string Name { get; }

	public UserEntityMock(string name)
	{
		this.Name = name;
	}
}