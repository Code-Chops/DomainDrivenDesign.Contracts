using System.Text.Json;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Mocks;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;

public class PolymorphicJsonConversionTests
{
    private Point3DMock Point3D { get; } = new(1, 2, 3);
    private const string Point3DJson = @"{""Z"":3,""X"":1,""Y"":2,""TypeId"":""Point3DMock""}";
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = false }; 
    
    private static List<PolymorphicContract> Contracts { get; } = PointMock.Implementations.GetValues().Select(value => (PolymorphicContract)value).ToList();
    private static PolymorphicConverter PolymorphicConverter { get; } = new(JsonSerializerOptions, new List<PolymorphicAdapter>(), Contracts);
    
    [Fact]
    public void Deserialization_Is_Correct()
    {
        var point3D = PolymorphicConverter.Deserialize<PointMock>(Point3DJson);

        Assert.Equal(typeof(Point3DMock), point3D.GetType());
    }

    [Fact]
    public void Serialization_Is_Correct()
    {
        var json = PolymorphicConverter.Serialize(this.Point3D);
        
        Assert.Equal(Point3DJson, json);
    }
}