using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

/// <summary>
/// This polymorphic converter can convert:
/// <list type="bullet">
/// <item>Domain object(s) to contract(s) (et vice versa): 1 to 1, many to 1. Using a <see cref="PolymorphicAdapter"/>.</item>
/// <item>Domain objects to JSON directly (et vice versa). Using a <see cref="PolymorphicAdapter"/>.</item>
/// <item>Serialize / deserialize a contract from / to JSON.</item>
/// </list>
/// </summary>
public interface IPolymorphicConverter
{
	/// <summary>
	/// Uses an injected <see cref="PolymorphicAdapter"/> to convert the domain object to a contract.
	/// </summary>
	/// <exception cref="KeyNotFoundException">When the correct <see cref="PolymorphicAdapter"/> has not been found using the type of the domain object as key.</exception>
	IEnumerable<PolymorphicContract> ConvertDomainObjectToContract(params IDomainObject[] domainObjects);

	/// <summary>
	/// Serializes a contract to JSON.
	/// </summary>
	string Serialize(object contract);

	/// <summary>
	/// Uses an injected <see cref="PolymorphicAdapter"/> to convert the contract to a domain object.
	/// </summary>
	/// <exception cref="KeyNotFoundException">When the correct <see cref="PolymorphicAdapter"/> has not been found using the type of the contract as key.</exception>
	IEnumerable<IDomainObject> ConvertContractToDomainObject(params PolymorphicContract[] contracts);

	/// <summary>
	/// Deserializes JSON to a contract, by finding a contract that has the correct <see cref="PolymorphicContract.TypeId"/>-property.
	/// </summary>
	TContract Deserialize<TContract>(string json);

	/// <summary>
	/// Deserializes JSON to a contract using a stream, by finding a contract that has the correct <see cref="PolymorphicContract.TypeId"/>-property.
	/// </summary>
	TContract Deserialize<TContract>(Stream responseStream);
}