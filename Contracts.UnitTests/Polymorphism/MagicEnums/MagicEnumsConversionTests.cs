using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Implementations;
using CodeChops.DomainDrivenDesign.Contracts.Implementations.MagicEnums;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.MagicEnums;

public class MagicEnumsConversionTests
{
	private static MagicEnumPolymorphicContract Contract { get; } = new MagicEnumPolymorphicContract(MagicEnumMock.Value2);
	private const string Json = @$"{{""{nameof(MagicEnumPolymorphicContract.SpecificTypeId)}"":""{nameof(MagicEnumMock)}"",""{nameof(MagicEnumPolymorphicContract.Name)}"":""{nameof(MagicEnumMock.Value2)}"",""{nameof(MagicEnumPolymorphicContract.TypeId)}"":""{nameof(MagicEnumPolymorphicContract)}""}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = false, 
		Converters =
		{
			new ContractDomainJsonConverter(new PolymorphicJsonConverter(new[] { Contract }), new[] { new MagicEnumAdapter<MagicEnumMock>() })
		}
	};

	[Fact]
	public void Conversion_ToDomainModel_IsCorrect()
	{
		var magicEnum = JsonSerializer.Deserialize<MagicEnumMock>(Json, JsonSerializerOptions)!;
		Assert.NotNull(magicEnum);
		
		Assert.Equal(typeof(MagicEnumMock), magicEnum.GetType());
		Assert.Equal(MagicEnumMock.Value2.Name, magicEnum.Name);
		Assert.Equal(MagicEnumMock.Value2.Value.ToString(), magicEnum.Value.ToString());
	}
	
	[Fact]
	public void Deserialization_MagicEnum_Is_Correct()
	{
		var contract = JsonSerializer.Deserialize<MagicEnumPolymorphicContract>(Json, JsonSerializerOptions)!;
		Assert.NotNull(contract);

		Assert.Equal(typeof(MagicEnumPolymorphicContract), contract.GetType());
		Assert.Equal(MagicEnumMock.Value2.Name, contract.Name);
	}

	[Fact]
	public void Serialization_MagicEnum_Is_Correct()
	{
		var json = JsonSerializer.Serialize(Contract, JsonSerializerOptions);
        
		Assert.Equal(Json, json);
	}
}