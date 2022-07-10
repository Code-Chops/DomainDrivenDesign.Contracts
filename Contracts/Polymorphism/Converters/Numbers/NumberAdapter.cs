using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.GenericMath;
using CodeChops.Identities;
using CodeChops.ImplementationDiscovery;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Converters.Numbers;

[DiscoverImplementations]
public abstract partial class NumberAdapter<TNumber, TContract> : PolymorphicAdapter<TContract>
    where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
    where TContract : NumberContract<TNumber>, IHasStaticTypeId<Id<string>>, new()
{
    protected internal override IDomainObject ConvertContractToDomainObject(IContract contract)
    {
        var number = ((TContract)contract).Value;

        return new Number<TNumber>(number);
    }

    protected internal override TContract ConvertDomainObjectToContract(IDomainObject entity) 
        => new() { Value = (Number<TNumber>)entity };
    
    public override Type GetDomainObjectType() => typeof(Number<TNumber>);
}