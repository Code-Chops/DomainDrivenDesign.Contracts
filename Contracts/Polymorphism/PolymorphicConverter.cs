using System.Text.Json;
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
public class PolymorphicConverter : IPolymorphicConverter
{
	private JsonSerializerOptions SerializerOptions { get; }
	private IReadOnlyDictionary<Type, PolymorphicAdapter> AdaptersByContractType { get; }
	private IReadOnlyDictionary<Type, PolymorphicAdapter> AdaptersByDomainObjectType { get; }

	public PolymorphicConverter(JsonSerializerOptions options, IEnumerable<PolymorphicAdapter> adapters, IEnumerable<PolymorphicContract> contracts)
	{
		this.SerializerOptions = options;

		this.SerializerOptions.Converters.Add(new PolymorphicJsonConverter(contracts)); // This class is hard-coupled to the PolymorphicJsonConverter.

		var adapterList = adapters as List<PolymorphicAdapter> ?? adapters.ToList();
		this.AdaptersByContractType = adapterList.ToDictionary(adapter => adapter.GetContractType() ?? throw new Exception($"Contract type on {adapter.GetType().Name} is null."));

		this.AdaptersByDomainObjectType = adapterList.ToDictionary(adapter => adapter.GetDomainObjectType() ?? throw new Exception($"Domain object type on {adapter.GetType().Name} is null."));
	}

	/// <summary>
	/// Uses an injected <see cref="PolymorphicAdapter"/> to convert the domain object to a contract.
	/// </summary>
	/// <exception cref="KeyNotFoundException">When the correct <see cref="PolymorphicAdapter"/> has not been found using the type of the domain object as key.</exception>
	public IEnumerable<PolymorphicContract> ConvertDomainObjectToContract(params IDomainObject[] domainObjects)
	{
		var domainObjectsByType = domainObjects
			.GroupBy(domainObject => domainObject.GetType())
			.ToDictionary(group => group.Key, group => group.ToList());
		
		var contracts = new List<PolymorphicContract>();
		foreach (var (domainObjectType, objects) in domainObjectsByType)
		{
			if (!this.AdaptersByDomainObjectType.TryGetValue(domainObjectType, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(PolymorphicAdapter)} found for {domainObjectType.Name} in {nameof(PolymorphicConverter)}.");
			
			var modelsOfType = objects.Select(domainObject => (PolymorphicContract)adapter.ConvertDomainObjectToContract(domainObject));
			contracts.AddRange(modelsOfType);
		}

		return contracts;
	}

	/// <summary>
	/// Serializes a contract to JSON.
	/// </summary>
	public string Serialize(object contract)
	{
		var json = JsonSerializer.Serialize(contract, this.SerializerOptions);
		return json;
	}

	/// <summary>
	/// Uses an injected <see cref="PolymorphicAdapter"/> to convert the contract to a domain object.
	/// </summary>
	/// <exception cref="KeyNotFoundException">When the correct <see cref="PolymorphicAdapter"/> has not been found using the type of the contract as key.</exception>
	public IEnumerable<IDomainObject> ConvertContractToDomainObject(params PolymorphicContract[] contracts)
	{
		var contractsByType = contracts.GroupBy(contract => contract.GetType())
			.ToDictionary(group => group.Key, group => group.ToList());
		
		var domainObjects = new List<IDomainObject>();
		foreach (var (contractType, contractsOfType) in contractsByType)
		{
			if (!this.AdaptersByContractType.TryGetValue(contractType, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(PolymorphicAdapter)} found for {contractType.Name} in {nameof(PolymorphicConverter)}.");
			var entitiesOfType = contractsOfType.Select(contract => adapter.ConvertContractToDomainObject(contract));
			domainObjects.AddRange(entitiesOfType);
		}

		return domainObjects;
	}
	
	/// <summary>
	/// Deserializes JSON to a contract, by finding a contract that has the correct <see cref="PolymorphicContract.TypeId"/>-property.
	/// </summary>
	public TContract Deserialize<TContract>(string json)
	{
		var contract = (TContract)JsonSerializer.Deserialize(json, typeof(TContract), this.SerializerOptions)!;
		return contract;
	}
	
	/// <summary>
	/// Deserializes JSON to a contract using a stream, by finding a contract that has the correct <see cref="PolymorphicContract.TypeId"/>-property.
	/// </summary>
	public TContract Deserialize<TContract>(Stream responseStream)
	{
		var contract = (TContract?)JsonSerializer.Deserialize(responseStream, typeof(TContract), this.SerializerOptions);
		if (contract is null) throw new FormatException($"Error during deserialization to {nameof(IDomainObject)}.");

		return contract;
	}
}