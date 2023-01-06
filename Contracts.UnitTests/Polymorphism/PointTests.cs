using CodeChops.Contracts.Polymorphism;
using CodeChops.GenericMath.Serialization.Json;

namespace CodeChops.Contracts.UnitTests.Polymorphism;

public class PointTests
{
	private static PointMock PointMock { get; } = new() { Point = (1D, 7D) };
	private const string Json = "{\"Point\":{\"X\":1,\"Y\":7}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = false,
		Converters =
		{
			new PolymorphicJsonConverter(PointContractEnum.GetInstances()), new NumberJsonConverterFactory()
		}
	};

	[Fact]
	public void Deserialization_Point_IsCorrect()
	{
		var contract = JsonSerializer.Deserialize<PointMock>(Json, JsonSerializerOptions)!;
		Assert.NotNull(contract);

		Assert.Equal(typeof(PointMock), contract.GetType());
		Assert.Equal(PointMock.Point, contract.Point);
		Assert.Equal(PointMock.Point.X.GetType(), contract.Point.X.GetType());
	}
	
	[Fact]
	public void Serialization_Point_IsCorrect()
	{
		var json = JsonSerializer.Serialize(PointMock, JsonSerializerOptions);
		Assert.Equal(Json, json);
	}
}