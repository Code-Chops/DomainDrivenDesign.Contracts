using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Default.Numbers;

public class NumberConversionTests
{
	private static Number<int> NumberInt { get; } = new(7);
	private const string NumberIntJson = @"{""Value"":7}";
	private static Number<double> NumberDouble { get; } = new(3.12);
	private const string NumberDoubleJson = @"{""Value"":3.12}";

	private static NumberWrapperContractMock NumberWrapperContract { get; } = new(NumberInt, NumberDouble);
	private const string NumberWrapperJson = @$"{{""{nameof(NumberWrapperContractMock.IntNumber)}"":{{""Value"":7}},""{nameof(NumberWrapperContractMock.DoubleNumber)}"":{{""Value"":3.12}}}}";
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
		var numberWrapper = PolymorphicConverter.Deserialize<NumberWrapperContractMock>(NumberWrapperJson);

		Assert.Equal(typeof(NumberWrapperContractMock), numberWrapper.GetType());
		Assert.Equal(NumberDouble.Value, numberWrapper.DoubleNumber.Value);
		Assert.Equal(NumberInt.Value, numberWrapper.IntNumber.Value);	
	}
	
	[Fact]
	public void Serialization_NumberWrapper_Is_Correct()
	{
		var json = PolymorphicConverter.Serialize(NumberWrapperContract);
        
		Assert.Equal(NumberWrapperJson, json);
	}
}