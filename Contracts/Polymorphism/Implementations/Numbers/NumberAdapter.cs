using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.GenericMath;
using CodeChops.Identities;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers;

public abstract record NumberAdapter<TNumber, TContract> : NumberAdapter
    where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
    where TContract : NumberContract<TNumber>, IHasStaticTypeId<Id<string>>, new()
{
    public override string Id => typeof(TContract).Name;
    protected internal override Type GetDomainObjectType() => typeof(Number<TNumber>);
    protected internal override Type GetContractType() => typeof(TContract);
    
    protected internal override IDomainObject ConvertContractToDomainObject(Contract contract)
    {
        var number = ((TContract)contract).Value;
        return new Number<TNumber>(number);
    }

    protected internal override TContract ConvertDomainObjectToContract(IDomainObject domainObject) 
        => new() { Value = (Number<TNumber>)domainObject };
}

[DiscoverImplementations(generateIdsForImplementations: true)]
public abstract partial record NumberAdapter : Adapter;