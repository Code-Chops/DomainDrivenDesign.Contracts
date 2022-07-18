using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record PointContract : PolymorphicContract;