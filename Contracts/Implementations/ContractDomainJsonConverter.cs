using System.Text.Json;
using System.Text.Json.Serialization;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.DomainDrivenDesign.DomainModeling;

namespace CodeChops.DomainDrivenDesign.Contracts.Implementations;

/// <summary>
/// This contract to domain converter can convert domain objects to JSON (et vice versa).
/// Uses an injected <see cref="PolymorphicJsonConverter"/> and <see cref="Adapter{TPolymorphicContract}"/>(s).
/// </summary>
public class ContractDomainJsonConverter : JsonConverter<IDomainObject>
{
	private PolymorphicJsonConverter PolymorphicJsonConverter { get; }
	private IReadOnlyDictionary<Type, Adapter> AdaptersByContractType { get; }
	private IReadOnlyDictionary<Type, Adapter> AdaptersByDomainObjectType { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAssignableTo(typeof(IDomainObject));

	public ContractDomainJsonConverter(PolymorphicJsonConverter polymorphicJsonConverter, IEnumerable<Adapter> adapters)
	{
		this.PolymorphicJsonConverter = polymorphicJsonConverter;
		
		var adapterList = adapters as List<Adapter> ?? adapters.ToList();
		this.AdaptersByContractType = adapterList.ToDictionary(adapter => adapter.GetContractType() ?? throw new Exception($"Contract type on {adapter.GetType().Name} is null."));
		this.AdaptersByDomainObjectType = adapterList.ToDictionary(adapter => adapter.GetDomainObjectType() ?? throw new Exception($"Domain object type on {adapter.GetType().Name} is null."));
	}

	public override IDomainObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Deserialize it using the PolymorphicJsonConverter.
		var contract = this.PolymorphicJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException("Error during retrieval of JSON value.");
		var contractType = contract.GetType();
		
		// Convert it to a domain object using the correct adapter.
		if (!this.AdaptersByContractType.TryGetValue(contractType, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(Adapter)} found for {contractType.Name} in {nameof(ContractDomainJsonConverter)}.");
		var domainObject = adapter.ConvertContractToDomainObject(contract);
		
		return domainObject;
	}

	public override void Write(Utf8JsonWriter writer, IDomainObject domainObject, JsonSerializerOptions options)
	{
		var domainObjectType = domainObject.GetType();
		if (!this.AdaptersByDomainObjectType.TryGetValue(domainObjectType, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(Adapter)} found for {domainObjectType.Name} in {nameof(ContractDomainJsonConverter)}.");
		var contract = (PolymorphicContract)adapter.ConvertDomainObjectToContract(domainObject);

		this.PolymorphicJsonConverter.Write(writer, contract, options);
	}
}