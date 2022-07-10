using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Converters;
using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Converters.Numbers;

public class NumberConversionTests
{
	private static Number<int> NumberInt { get; } = new(7);
	private const string ExpectedNumberIntJson = @"{""Value"":7}";
	private static Number<double> NumberDouble { get; } = new(3.12);
	private const string ExpectedNumberDoubleJson = @"{""Value"":3.12}";
    
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false };
	// private static List<PolymorphicContract> Contracts { get; } = PointMock.Implementations.GetValues().Select(value => (PolymorphicContract)value).ToList();
	private static PolymorphicConverter PolymorphicConverter { get; } = new(JsonSerializerOptions, new List<PolymorphicAdapter>(), new List<IPolymorphicContract>());
    
	[Fact]
	public void Deserialization_NumberInt_Is_Correct()
	{
		var number = PolymorphicConverter.Deserialize<Number<int>>(ExpectedNumberIntJson);

		Assert.Equal(typeof(Number<int>), number.GetType());
		Assert.Equal(NumberInt.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberInt_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberInt);
        
		Assert.Equal(ExpectedNumberIntJson, json);
	}
	
	[Fact]
	public void Deserialization_NumberDouble_Is_Correct()
	{
		var number = PolymorphicConverter.Deserialize<Number<double>>(ExpectedNumberDoubleJson);

		Assert.Equal(typeof(Number<double>), number.GetType());
		Assert.Equal(NumberDouble.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberDouble_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberDouble);
        
		Assert.Equal(ExpectedNumberDoubleJson, json);
	}
}