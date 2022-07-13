using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.GenericMath;
using CodeChops.Identities;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.Implementations.Numbers;

[DiscoverImplementations]
public abstract partial record NumberAdapter<TNumber, TContract> : PolymorphicAdapter<TContract>
    where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
    where TContract : NumberContract<TNumber>, IHasStaticTypeId<Id<string>>, new()
{
    protected internal override Type GetDomainObjectType() => typeof(Number<TNumber>);
    
    protected internal override IDomainObject ConvertContractToDomainObject(IContract contract)
    {
        var number = ((TContract)contract).Value;
        return new Number<TNumber>(number);
    }

    protected internal override TContract ConvertDomainObjectToContract(IDomainObject domainObject) 
        => new() { Value = (Number<TNumber>)domainObject };
}