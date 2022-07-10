using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Converters.Mocks;

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record PointMock : PolymorphicContract;