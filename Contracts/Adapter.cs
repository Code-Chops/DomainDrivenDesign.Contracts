using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts;

/// <summary>
/// Provides a means to convert a domain model to a contract and vice versa.
/// </summary>
public abstract class Adapter
{
	protected internal abstract IContract ConvertDomainObjectToContract(IDomainObject entity);
	protected internal abstract IDomainObject ConvertContractToDomainObject(IContract contract);
}