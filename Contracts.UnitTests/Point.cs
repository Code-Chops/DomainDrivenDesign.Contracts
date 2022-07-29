using CodeChops.DomainDrivenDesign.Contracts.Converters.Numbers;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;
using CodeChops.Geometry.Space;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests;

public class PointMock
{
	public Point<double> Point { get; init; }
}

public class Tests
{
	private static PointMock PointMock { get; } = new PointMock() { Point = (1, 7) };
	private const string Json = $"{{\"Point\":{{\"X\":1,\"Y\":7}}}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false, Converters = { new PolymorphicJsonConverter(PointContract.Implementations.GetValues()), new NumberJsonConverterFactory() }};

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