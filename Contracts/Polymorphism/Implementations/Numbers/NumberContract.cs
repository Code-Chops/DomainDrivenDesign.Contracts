using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers;

public abstract record NumberContract<TNumber> : NumberContract
    where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
{
    public TNumber Value { get; init; }
}

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record NumberContract : PolymorphicContract;