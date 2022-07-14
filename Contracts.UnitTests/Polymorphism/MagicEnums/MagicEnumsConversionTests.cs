using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.MagicEnums;

public class MagicEnumsConversionTests
{
	private static MagicEnumContract Contract { get; } = new(MagicEnumMock.Value2);
	private const string Json = @$"{{""{nameof(MagicEnumContract.SpecificTypeId)}"":""{nameof(MagicEnumMock)}"",""{nameof(MagicEnumContract.Name)}"":""{nameof(MagicEnumMock.Value2)}"",""{nameof(MagicEnumContract.TypeId)}"":""{nameof(MagicEnumContract)}""}}";
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = false, 
		Converters =
		{
			new AdaptingJsonConverter(new[] { new MagicEnumAdapter<MagicEnumMock>() }),
			new PolymorphicJsonConverter(new [] { Contract}),
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
		var contract = JsonSerializer.Deserialize<MagicEnumContract>(Json, JsonSerializerOptions)!;
		Assert.NotNull(contract);

		Assert.Equal(typeof(MagicEnumContract), contract.GetType());
		Assert.Equal(MagicEnumMock.Value2.Name, contract.Name);
	}

	[Fact]
	public void Serialization_MagicEnum_Is_Correct()
	{
		var json = JsonSerializer.Serialize(Contract, JsonSerializerOptions);
        
		Assert.Equal(Json, json);
	}
}