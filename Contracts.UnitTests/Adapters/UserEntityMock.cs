namespace CodeChops.Contracts.UnitTests.Adapters;

[GenerateIdentity(name: nameof(UserEntityMockId))]
public partial class UserEntityMock(string name, ulong id) : Entity<UserEntityMockId>(new UserEntityMockId(id))
{
	public string Name { get; } = name;
}