using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Custom;

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record PointContract : PolymorphicContract;