using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Converters;
using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Converters.Numbers;

public class NumberConversionTests
{
	private static Number<int> NumberInt { get; } = new(7);
	private const string NumberIntJson = @"{""Value"":7}";
	private static Number<double> NumberDouble { get; } = new(3.12);
	private const string NumberDoubleJson = @"{""Value"":3.12}";

	private static NumberWrapperMock NumberWrapper { get; } = new(NumberInt, NumberDouble);
	private const string NumberWrapperJson = @"{""IntNumber"":{""Value"":7},""DoubleNumber"":{""Value"":3.12}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false };
	private static PolymorphicConverter PolymorphicConverter { get; } = new(JsonSerializerOptions, new List<PolymorphicAdapter>(), new List<PolymorphicContract>());
    
	[Fact]
	public void Deserialization_NumberInt_Is_Correct()
	{
		var number = PolymorphicConverter.Deserialize<Number<int>>(NumberIntJson);

		Assert.Equal(typeof(Number<int>), number.GetType());
		Assert.Equal(NumberInt.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberInt_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberInt);
        
		Assert.Equal(NumberIntJson, json);
	}
	
	[Fact]
	public void Deserialization_NumberDouble_Is_Correct()
	{
		var number = PolymorphicConverter.Deserialize<Number<double>>(NumberDoubleJson);

		Assert.Equal(typeof(Number<double>), number.GetType());
		Assert.Equal(NumberDouble.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberDouble_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberDouble);
        
		Assert.Equal(NumberDoubleJson, json);
	}
	
	[Fact]
	public void Deserialization_NumberWrapper_Is_Correct()
	{
		var numberWrapper = PolymorphicConverter.Deserialize<NumberWrapperMock>(NumberWrapperJson);

		Assert.Equal(typeof(NumberWrapperMock), numberWrapper.GetType());
		Assert.Equal(NumberDouble.Value, numberWrapper.DoubleNumber.Value);
		Assert.Equal(NumberInt.Value, numberWrapper.IntNumber.Value);	
	}
	
	[Fact]
	public void Serialization_NumberWrapper_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberWrapper);
        
		Assert.Equal(NumberWrapperJson, json);
	}
}