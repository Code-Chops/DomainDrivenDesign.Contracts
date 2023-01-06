using CodeChops.Contracts.Converters;

namespace CodeChops.Contracts.UnitTests.Adapters;

public class AdaptingJsonConverterTests
{
	private static UserEntityMock Entity { get; } = new("Max");
	private const string Json = @$"{{""{nameof(UserContractMock.Name)}"":""Max""}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = false, 
		Converters =
		{
			new AdaptingJsonConverter(new[] { new UserAdapterMock() })
		}
	};
	
	[Fact]
	public void AdaptingJsonConverter_Serialize_WithoutPolymorphism_ShouldWork()
	{
		var json = JsonSerializer.Serialize(Entity, JsonSerializerOptions);
		
		Assert.Equal(Json, json);
	}
	
	[Fact]
	public void AdaptingJsonConverter_Deserialize_ToDomainObject_WithoutPolymorphism_ShouldWork()
	{
		var entity = JsonSerializer.Deserialize<UserEntityMock>(Json, JsonSerializerOptions);
		
		Assert.Equal(Entity.Name, entity?.Name);
	}
	
	[Fact]
	public void AdaptingJsonConverter_Deserialize_ToContract_WithoutPolymorphism_ShouldWork()
	{
		var contract = JsonSerializer.Deserialize<UserContractMock>(Json, JsonSerializerOptions);
		
		Assert.Equal(Entity.Name, contract?.Name);
	}
}