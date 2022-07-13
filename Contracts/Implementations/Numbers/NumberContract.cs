using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.Implementations.Numbers;

public abstract record NumberContract<TNumber> : NumberContract
    where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
{
    public TNumber Value { get; init; }
}

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record NumberContract : PolymorphicContract;

public partial record NumberDoubleContract : NumberContract<double>;
public partial record NumberFloatContract : NumberContract<float>;
public partial record NumberIntContract : NumberContract<int>;