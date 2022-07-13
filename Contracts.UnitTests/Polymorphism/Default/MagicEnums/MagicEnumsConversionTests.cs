using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Implementations.MagicEnums;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Default.MagicEnums;

public class MagicEnumsConversionTests
{
	private PolymorphicContract Contract { get; }
	private const string Json = @$"{{""{nameof(MagicEnumPolymorphicContract.SpecificTypeId)}"":""{nameof(MagicEnumMock)}"",""{nameof(MagicEnumPolymorphicContract.Name)}"":""{nameof(MagicEnumMock.Value2)}"",""{nameof(MagicEnumPolymorphicContract.TypeId)}"":""{nameof(MagicEnumPolymorphicContract)}""}}";
	private JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false };
	private PolymorphicConverter PolymorphicConverter { get; }
	private PolymorphicAdapter Adapter { get; } = new MagicEnumPolymorphicAdapter<MagicEnumMock>();

	public MagicEnumsConversionTests()
	{
		this.Contract = new MagicEnumPolymorphicContract(MagicEnumMock.Value2);
		this.PolymorphicConverter = new(this.JsonSerializerOptions, new[] { this.Adapter }, new[] { this.Contract });
	}

	[Fact]
	public void Conversion_ToDomainModel_IsCorrect()
	{
		var contract = new MagicEnumPolymorphicContract(MagicEnumMock.Value2);
		var magicEnum = (MagicEnumMock)this.PolymorphicConverter.ConvertContractToDomainObject(contract).Single();
		
		Assert.Equal(typeof(MagicEnumMock), magicEnum.GetType());
		Assert.Equal(MagicEnumMock.Value2.Name, magicEnum.Name);
		Assert.Equal(MagicEnumMock.Value2.Value.ToString(), magicEnum.Value.ToString());
	}
	
	[Fact]
	public void Deserialization_MagicEnum_Is_Correct()
	{
		var contract = (MagicEnumPolymorphicContract)this.PolymorphicConverter.Deserialize<PolymorphicContract>(Json);

		Assert.Equal(typeof(MagicEnumPolymorphicContract), contract.GetType());
		Assert.Equal(MagicEnumMock.Value2.Name, contract.Name);
	}

	[Fact]
	public void Serialization_MagicEnum_Is_Correct()
	{
		var json = this.PolymorphicConverter.Serialize(this.Contract);
        
		Assert.Equal(Json, json);
	}
}