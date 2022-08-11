using CodeChops.DomainDrivenDesign.Contracts.Converters.Numbers;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;

public class CustomPolymorphicJsonConversionTests
{
    private static Point3DContractMock Point3D { get; } = new(1, 2, 3);
    private const string ExpectedJson = $@"{{""{nameof(Point3D.Z)}"":3,""{nameof(PolymorphicContract.TypeId)}"":""{nameof(PointContractEnum)}.{nameof(Point3DContractMock)}"",""{nameof(Point3D.X)}"":1,""{nameof(Point3D.Y)}"":2}}";
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        WriteIndented = false, 
        Converters =
        {
            new PolymorphicJsonConverter(PointContractEnum.GetValues().Select(value => value.UninitializedInstance)), new MagicEnumJsonConverterFactory(new [] { new PointContractEnum()}), new IdentityJsonConverterFactory()
        }
    };

    [Fact]
    public void Deserialization_Is_Correct()
    {
        var point3D = JsonSerializer.Deserialize<PointContract>(ExpectedJson, JsonSerializerOptions)!;
        Assert.NotNull(point3D);
        
        Assert.Equal(typeof(Point3DContractMock), point3D.GetType());
    }

    [Fact]
    public void Serialization_Is_Correct()
    {
        var json = JsonSerializer.Serialize<PointContract>(Point3D, JsonSerializerOptions);
        
        Assert.Equal(ExpectedJson, json);
    }
}