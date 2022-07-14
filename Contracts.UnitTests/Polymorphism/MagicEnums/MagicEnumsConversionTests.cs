using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.MagicEnums;

public class MagicEnumsConversionTests
{
	private static MagicEnumContract Contract { get; } = new(MagicEnumMock1.Value2);
	private const string Json = @$"{{""{nameof(MagicEnumContract.AdapterId)}"":""{nameof(MagicEnumMock1)}"",""{nameof(MagicEnumContract.Name)}"":""{nameof(MagicEnumMock1.Value2)}"",""{nameof(MagicEnumContract.TypeId)}"":""{nameof(MagicEnumContract)}""}}";
	private JsonSerializerOptions JsonSerializerOptions { get; }

	public MagicEnumsConversionTests()
	{
		this.JsonSerializerOptions = new()
		{
			WriteIndented = false, 
			Converters =
			{
				new AdaptingJsonConverter(new[] { new MagicEnumAdapter<MagicEnumMock1>() }),
			}
		};
	}

	[Fact]
	public void Conversion_ToDomainModel_IsCorrect()
	{
		var magicEnum = JsonSerializer.Deserialize<MagicEnumMock1>(Json, this.JsonSerializerOptions)!;
		Assert.NotNull(magicEnum);
		
		Assert.Equal(typeof(MagicEnumMock1), magicEnum.GetType());
		Assert.Equal(MagicEnumMock1.Value2.Name, magicEnum.Name);
		Assert.Equal(MagicEnumMock1.Value2.Value.ToString(), magicEnum.Value.ToString());
	}
	
	[Fact]
	public void Deserialization_MagicEnum_Is_Correct()
	{
		var contract = JsonSerializer.Deserialize<MagicEnumContract>(Json, this.JsonSerializerOptions)!;
		Assert.NotNull(contract);

		Assert.Equal(typeof(MagicEnumContract), contract.GetType());
		Assert.Equal(MagicEnumMock1.Value2.Name, contract.Name);
	}

	[Fact]
	public void Serialization_MagicEnum_Is_Correct()
	{
		var json = JsonSerializer.Serialize(Contract, this.JsonSerializerOptions);
        
		Assert.Equal(Json, json);
	}

	[Fact]
	public void Deserialization_WithMultipleAdapters_Should_Work()
	{
		var jsonSerializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = false, 
			Converters =
			{
				new AdaptingJsonConverter(new Adapter[] { new MagicEnumAdapter<MagicEnumMock1>(), new MagicEnumAdapter<MagicEnumMock2>() }),
			}
		};
		
		var magicEnum = JsonSerializer.Deserialize<MagicEnumMock1>(Json, jsonSerializerOptions)!;
		Assert.NotNull(magicEnum);
		
		Assert.Equal(typeof(MagicEnumMock1), magicEnum.GetType());
		Assert.Equal(MagicEnumMock1.Value2.Name, magicEnum.Name);
		Assert.Equal(MagicEnumMock1.Value2.Value.ToString(), magicEnum.Value.ToString());
	}
}