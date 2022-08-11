using CodeChops.DomainDrivenDesign.Contracts.Converters.Numbers;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Geometry;

public class PointTests
{
	private static PointMock PointMock { get; } = new() { Point = (1, 7) };
	private const string Json = "{\"Point\":{\"X\":1,\"Y\":7}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = false,
		Converters =
		{
			new PolymorphicJsonConverter(PointContractEnum.GetValues().Select(value => value.UninitializedInstance)), new NumberJsonConverterFactory()
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