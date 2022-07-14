using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers;
using CodeChops.GenericMath;
using NumberDoubleContract = CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers.NumberDoubleContract;
using NumberIntContract = CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers.NumberIntContract;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Numbers;

public class NumberConversionTests
{
	private static Number<int> NumberInt { get; } = new(7);
	private const string NumberIntJson = @$"{{""Value"":7,""TypeId"":""{nameof(NumberIntContract)}""}}";
	private static Number<double> NumberDouble { get; } = new(3.12);
	private const string NumberDoubleJson = @$"{{""Value"":3.12,""TypeId"":""{nameof(NumberDoubleContract)}""}}";

	private static NumberWrapperContractMock NumberWrapperContract { get; } = new() { IntNumber = NumberInt, DoubleNumber = NumberDouble };
	private const string NumberWrapperJson = @$"{{""{nameof(NumberWrapperContractMock.IntNumber)}"":{{""{nameof(NumberContract<int>.Value)}"":7,""{nameof(PolymorphicContract.TypeId)}"":""{nameof(NumberIntContract)}""}},""{nameof(NumberWrapperContractMock.DoubleNumber)}"":{{""{nameof(NumberContract<int>.Value)}"":3.12,""{nameof(PolymorphicContract.TypeId)}"":""{nameof(NumberDoubleContract)}""}}}}";
	private JsonSerializerOptions JsonSerializerOptions { get; }

	public NumberConversionTests()
	{
		this.JsonSerializerOptions = new()
		{
			WriteIndented = false, 
			Converters =
			{
				new AdaptingJsonConverter()
			}
		};
	}
	
	[Fact]
	public void Deserialization_NumberInt_Is_Correct()
	{
		var number = JsonSerializer.Deserialize<Number<int>>(NumberIntJson, this.JsonSerializerOptions);

		Assert.Equal(typeof(Number<int>), number.GetType());
		Assert.Equal(NumberInt.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberInt_Is_Correct()
	{
		var json = JsonSerializer.Serialize(NumberInt, this.JsonSerializerOptions);
        
		Assert.Equal(NumberIntJson, json);
	}
	
	[Fact]
	public void Deserialization_NumberDouble_Is_Correct()
	{
		var number = JsonSerializer.Deserialize<Number<double>>(NumberDoubleJson, this.JsonSerializerOptions);

		Assert.Equal(typeof(Number<double>), number.GetType());
		Assert.Equal(NumberDouble.Value, number.Value);
	}

	[Fact]
	public void Serialization_NumberDouble_Is_Correct()
	{
		var json = JsonSerializer.Serialize(NumberDouble, this.JsonSerializerOptions);
        
		Assert.Equal(NumberDoubleJson, json);
	}
	
	[Fact]
	public void Deserialization_NumberWrapper_Is_Correct()
	{
		var numberWrapper = JsonSerializer.Deserialize<NumberWrapperContractMock>(NumberWrapperJson, this.JsonSerializerOptions)!;
		Assert.NotNull(numberWrapper);
		
		Assert.Equal(typeof(NumberWrapperContractMock), numberWrapper.GetType());
		Assert.Equal(NumberDouble.Value, numberWrapper.DoubleNumber.Value);
		Assert.Equal(NumberInt.Value, numberWrapper.IntNumber.Value);	
	}
	
	[Fact]
	public void Serialization_NumberWrapper_Is_Correct()
	{
		var json = JsonSerializer.Serialize(NumberWrapperContract, this.JsonSerializerOptions);
        
		Assert.Equal(NumberWrapperJson, json);
	}
}