using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Custom;

public class CustomPolymorphicJsonConversionTests
{
    private static Point3DContract Point3D { get; } = new(1, 2, 3);
    private const string ExpectedJson = $@"{{""{nameof(Point3D.Z)}"":3,""{nameof(Point3D.X)}"":1,""{nameof(Point3D.Y)}"":2,""{nameof(PolymorphicContract.TypeId)}"":""{nameof(Point3DContract)}""}}";
    
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false, Converters = { new PolymorphicJsonConverter(PointContract.Implementations.GetValues()) }};

    [Fact]
    public void Deserialization_Is_Correct()
    {
        var point3D = JsonSerializer.Deserialize<PointContract>(ExpectedJson, JsonSerializerOptions)!;
        Assert.NotNull(point3D);
        
        Assert.Equal(typeof(Point3DContract), point3D.GetType());
    }

    [Fact]
    public void Serialization_Is_Correct()
    {
        var json = JsonSerializer.Serialize(Point3D, JsonSerializerOptions);
        
        Assert.Equal(ExpectedJson, json);
    }
}