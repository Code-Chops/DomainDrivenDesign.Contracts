using System.Text.Json;
using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Converters;

public class PolymorphicConverter
{
	/// <summary>
	/// Options without polymorphic converters.
	/// </summary>
	private JsonSerializerOptions SerializerOptions { get; }
	private IReadOnlyDictionary<Type, PolymorphicAdapter> AdaptersByContractType { get; }
	private IReadOnlyDictionary<Type, PolymorphicAdapter> AdaptersByDomainObjectType { get; }

	public PolymorphicConverter(JsonSerializerOptions options, IEnumerable<PolymorphicAdapter> adapters, IEnumerable<IPolymorphicContract> contracts)
	{
		this.SerializerOptions = options;

		this.SerializerOptions.Converters.Add(new PolymorphicJsonConverter(contracts, options)); // This class is hard-coupled to the PolymorphicJsonConverter.

		var adapterList = adapters as List<PolymorphicAdapter> ?? adapters.ToList();
		this.AdaptersByContractType = adapterList.ToDictionary(adapter => adapter.GetContractType());
		this.AdaptersByDomainObjectType = adapterList.ToDictionary(adapter => adapter.GetDomainObjectType());
	}

	public string ConvertEntitiesToJson(IEnumerable<IDomainObject> domainObjects)
	{
		var domainObjectsByType = domainObjects
			.GroupBy(domainObject => domainObject.GetType())
			.ToDictionary(group => group.Key, group => group.ToList());
		
		var contracts = new List<object>();
		foreach (var (type, objects) in domainObjectsByType)
		{
			if (!this.AdaptersByDomainObjectType.TryGetValue(type, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(PolymorphicAdapter)} found for {type.Name}.");
			
			var modelsOfType = objects.Select(entity => adapter.ConvertDomainObjectToContract(entity));
			contracts.AddRange(modelsOfType);
		}

		var json = this.Serialize(contracts);
		return json;
	}

	public string Serialize<TContract>(TContract contract)
	{
		var json = JsonSerializer.Serialize(contract, this.SerializerOptions);
		return json;
	}
	
	public TContract Deserialize<TContract>(string json)
	{
		var contract = (TContract?)JsonSerializer.Deserialize(json, typeof(TContract), this.SerializerOptions);
		if (contract is null) throw new FormatException($"Error during deserialization to {nameof(IDomainObject)}.");

		return contract;
	}
	
	public TContract Deserialize<TContract>(Stream responseStream)
	{
		var contract = (TContract?)JsonSerializer.Deserialize(responseStream, typeof(TContract), this.SerializerOptions);
		if (contract is null) throw new FormatException($"Error during deserialization to {nameof(IDomainObject)}.");

		return contract;
	}
	
	public IEnumerable<IDomainObject> ConvertJsonToEntities(Stream responseStream)
	{
		var contractsByType = this.Deserialize<IEnumerable<PolymorphicContract>>(responseStream)
			.GroupBy(contract => contract.GetType())
			.ToDictionary(group => group.Key, group => group.ToList());
		
		var domainObjects = new List<IDomainObject>();
		foreach (var (type, contractsOfType) in contractsByType)
		{
			if (!this.AdaptersByContractType.TryGetValue(type, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(PolymorphicAdapter)} found for {type.Name}.");
			var entitiesOfType = contractsOfType.Select(contract => adapter.ConvertContractToDomainObject(contract));
			domainObjects.AddRange(entitiesOfType);
		}

		return domainObjects;
	}
}