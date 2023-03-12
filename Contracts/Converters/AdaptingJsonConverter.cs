using System.Runtime.Serialization;
using CodeChops.Contracts.Adapters;

namespace CodeChops.Contracts.Converters;

/// <summary>
/// This contract to object converter can convert objects to JSON (et vice versa) using adapters.
/// Uses an injected <see cref="PolymorphicJsonConverter"/> and <see cref="Adapter{TObject,TContract}"/>(s).
/// </summary>
public sealed class AdaptingJsonConverter : JsonConverter<object>
{
	private PolymorphicJsonConverter PolymorphicJsonConverter { get; }
	private ImmutableDictionary<string, IBidirectionalAdapter> AdaptersByDomainObjectName { get; }
	private ImmutableDictionary<Type, IBidirectionalAdapter> AdaptersByDomainObjectType { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> !typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(IDomainObject));

	public AdaptingJsonConverter(IEnumerable<IBidirectionalAdapter>? adapters = null)
	{
		adapters ??= Array.Empty<IBidirectionalAdapter>();
		var adapterList = adapters as List<IBidirectionalAdapter> ?? adapters.ToList();

		var polymorphicContracts = adapterList
			.Select(adapter => FormatterServices.GetUninitializedObject(adapter.ContractType))
			.OfType<PolymorphicContract>();
		
		this.PolymorphicJsonConverter = new PolymorphicJsonConverter(polymorphicContracts);
		
		this.AdaptersByDomainObjectName = adapterList.ToImmutableDictionary(adapter => adapter.ContractName ?? throw new Exception($"Contract type on {adapter.GetType().Name} is null."));
		this.AdaptersByDomainObjectType = adapterList.ToImmutableDictionary(adapter => adapter.ObjectType ?? throw new Exception($"Object type on {adapter.GetType().Name} is null."));
	}

	public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var contract = GetContract(ref reader);
		var contractType = contract.GetType();
		
		if (!this.AdaptersByDomainObjectName.TryGetValue(contractType.Name, out var adapter)) 
			throw new KeyNotFoundException($"No injected implementation of {nameof(IBidirectionalAdapter)} found for {contractType.Name} in {nameof(AdaptingJsonConverter)}.");
		
		// Convert it to a domain object using the correct adapter.
		var domainObject = adapter.ConvertToObject(contract);
		return domainObject;


		Contract GetContract(ref Utf8JsonReader reader)
		{
			if (this.AdaptersByDomainObjectType.TryGetValue(typeToConvert, out var adapter))
			{
				var contract = JsonSerializer.Deserialize(ref reader, adapter.ContractType, options) 
					?? throw new JsonException("Error during retrieval of JSON value.");
				
				return (Contract)contract;
			}
		
			// Deserialize it using the PolymorphicJsonConverter.
			var polymorphicContract = this.PolymorphicJsonConverter.Read(ref reader, typeToConvert, options) 
				?? throw new JsonException("Error during retrieval of polymorphic JSON value.");
			
			return polymorphicContract;
		}
	}

	public override void Write(Utf8JsonWriter writer, object domainObject, JsonSerializerOptions options)
	{
		var domainObjectType = domainObject.GetType();
		
		if (!this.AdaptersByDomainObjectType.TryGetValue(domainObjectType, out var adapter)) 
			throw new KeyNotFoundException($"No injected implementation of {nameof(IBidirectionalAdapter)} found for {domainObjectType.Name} in {nameof(AdaptingJsonConverter)}.");
		
		var contract = adapter.ConvertToContract(domainObject);

		JsonSerializer.Serialize(writer, contract, contract.GetType(), options);
	}
}