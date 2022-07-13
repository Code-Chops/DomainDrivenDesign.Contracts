using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts;

/// <summary>
/// Provides a means to convert a domain model to a contract and vice versa.
/// </summary>
public abstract record Adapter
{
	/// <summary>
	/// Used by the converter for retrieving the correct adapter in order to convert the domain object to a contract.
	/// </summary>
	protected internal abstract Type GetDomainObjectType();
	
	protected internal abstract IContract ConvertDomainObjectToContract(IDomainObject domainObject);
	protected internal abstract IDomainObject ConvertContractToDomainObject(IContract contract);
}