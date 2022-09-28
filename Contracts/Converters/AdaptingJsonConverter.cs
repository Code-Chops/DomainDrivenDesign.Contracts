using System.Collections.Immutable;
using System.Runtime.Serialization;
using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters;

/// <summary>
/// This contract to domain converter can convert domain objects to JSON (et vice versa).
/// Uses an injected <see cref="PolymorphicJsonConverter"/> and <see cref="Adapter{TPolymorphicContract}"/>(s).
/// </summary>
public class AdaptingJsonConverter : JsonConverter<IDomainObject>
{
	private PolymorphicJsonConverter PolymorphicJsonConverter { get; }
	private ImmutableDictionary<string, Adapter> AdaptersByDomainObjectName { get; }
	private ImmutableDictionary<Type, Adapter> AdaptersByDomainObjectType { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> !typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(IDomainObject));

	public AdaptingJsonConverter(IEnumerable<Adapter>? adapters = null)
	{
		adapters ??= Array.Empty<Adapter>();
		var adapterList = adapters as List<Adapter> ?? adapters.ToList();

		var polymorphicContracts = adapterList
			.Select(adapter => FormatterServices.GetUninitializedObject(adapter.GetContractType()))
			.OfType<PolymorphicContract>();
		
		this.PolymorphicJsonConverter = new PolymorphicJsonConverter(polymorphicContracts);
		
		this.AdaptersByDomainObjectName = adapterList.ToImmutableDictionary(adapter => adapter.DomainObjectName ?? throw new Exception($"Contract type on {adapter.GetType().Name} is null."));
		this.AdaptersByDomainObjectType = adapterList.ToImmutableDictionary(adapter => adapter.GetDomainObjectType() ?? throw new Exception($"Domain object type on {adapter.GetType().Name} is null."));
	}

	public override IDomainObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var contract = GetContract(ref reader);
		var contractType = contract.GetType();
		
		if (!this.AdaptersByDomainObjectName.TryGetValue(contractType.Name, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(Adapter)} found for {contractType.Name} in {nameof(AdaptingJsonConverter)}.");
		
		// Convert it to a domain object using the correct adapter.
		var domainObject = adapter.ConvertContractToDomainObject(contract);
		return domainObject;


		Contract GetContract(ref Utf8JsonReader reader)
		{
			if (this.AdaptersByDomainObjectType.TryGetValue(typeToConvert, out var adapter))
			{
				var contract = JsonSerializer.Deserialize(ref reader, adapter.GetContractType(), options) ?? throw new JsonException("Error during retrieval of JSON value.");
				return (Contract)contract;
			}
		
			// Deserialize it using the PolymorphicJsonConverter.
			var polymorphicContract = this.PolymorphicJsonConverter.Read(ref reader, typeToConvert, options) ?? throw new JsonException("Error during retrieval of polymorphic JSON value.");
			return polymorphicContract;
		}
	}

	public override void Write(Utf8JsonWriter writer, IDomainObject domainObject, JsonSerializerOptions options)
	{
		var domainObjectType = domainObject.GetType();
		if (!this.AdaptersByDomainObjectType.TryGetValue(domainObjectType, out var adapter)) throw new KeyNotFoundException($"No injected {nameof(Adapter)} found for {domainObjectType.Name} in {nameof(AdaptingJsonConverter)}.");
		var contract = adapter.ConvertDomainObjectToContract(domainObject);

		JsonSerializer.Serialize(writer, contract, contract.GetType(), options);
	}
}