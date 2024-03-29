using CodeChops.Contracts.Converters;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.Contracts.UnitTests.Polymorphism;

[DiscoverImplementations(generateImplementationIds: true)]
public abstract partial record PointContract : PolymorphicContract;

public partial record Point2DContractMock(int X, int Y) : PointContract;
public partial record Point3DContractMock(int X, int Y, int Z) : Point2DContractMock(X, Y);