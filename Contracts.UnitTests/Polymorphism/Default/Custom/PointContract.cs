using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Default.Custom;

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record PointContract : PolymorphicContract;