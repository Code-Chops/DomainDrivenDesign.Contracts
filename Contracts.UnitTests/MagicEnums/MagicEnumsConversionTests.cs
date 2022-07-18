﻿using CodeChops.DomainDrivenDesign.Contracts.Converters.MagicEnums;
using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.MagicEnums;

public class MagicEnumsConversionTests
{
	private const string Json = @$"""{nameof(MagicEnumMock1)}.{nameof(MagicEnumMock1.Value2)}""";
	private const string WrapperJson = @$"{{""Enum"":""{nameof(MagicEnumMock2)}.{nameof(MagicEnumMock2.Value3)}""}}";
		
	private JsonSerializerOptions JsonSerializerOptions { get; }
	
	public MagicEnumsConversionTests()
	{
		this.JsonSerializerOptions = new()
		{
			WriteIndented = false,
			Converters = { new MagicEnumJsonConverter(new IMagicEnum[]{ new MagicEnumMock1(), new MagicEnumMock2()}) }
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
	public void Deserialization_MagicEnum_IsCorrect()
	{
		var contract = JsonSerializer.Deserialize<IMagicEnum>(Json, this.JsonSerializerOptions)!;
		Assert.NotNull(contract);

		Assert.Equal(typeof(MagicEnumMock1), contract.GetType());
		Assert.Equal(MagicEnumMock1.Value2.Name, contract.ToString());
	}

	[Fact]
	public void Serialization_MagicEnum_IsCorrect()
	{
		var json = JsonSerializer.Serialize(MagicEnumMock1.Value2, this.JsonSerializerOptions);
        
		Assert.Equal(Json, json);
	}

	[Fact]
	public void Serialization_MagicEnum_WithWrapper_ShouldWork()
	{
		var wrapper = new MagicEnumWrapperContractMock() { Enum = MagicEnumMock2.Value3 };
		var json = JsonSerializer.Serialize(wrapper, this.JsonSerializerOptions);
		
		Assert.Equal(WrapperJson, json);
	}
	
	[Fact]
	public void Deserialization_MagicEnum_WithWrapper_ShouldWork()
	{
		var magicEnum = JsonSerializer.Deserialize<MagicEnumWrapperContractMock>(WrapperJson, this.JsonSerializerOptions)!;
		Assert.NotNull(magicEnum);
		
		Assert.Equal(typeof(MagicEnumWrapperContractMock), magicEnum.GetType());
		Assert.Equal(MagicEnumMock2.Value3.Name, magicEnum.Enum.Name);
	}
}