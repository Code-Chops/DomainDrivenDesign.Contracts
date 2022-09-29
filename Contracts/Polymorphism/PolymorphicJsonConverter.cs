using System.Collections.Immutable;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

/// <summary>
/// This polymorphic domain converter can convert:
/// <list type="bullet">
/// <item>Domain object(s) to contract(s) (et vice versa): 1 to 1, many to 1. Using a <see cref="Adapter{TPolymorphicContract}"/>.</item>
/// <item>Domain objects to JSON directly (et vice versa). Using a <see cref="Adapter{TPolymorphicContract}"/>.</item>
/// </list>
/// </summary>
public class PolymorphicJsonConverter : JsonConverter<PolymorphicContract>
{
	private ImmutableDictionary<string, PolymorphicContract> ContractsByImplementationId { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(PolymorphicContract));

	public PolymorphicJsonConverter(IEnumerable<PolymorphicContract>? contracts = null)
	{
		contracts ??= Array.Empty<PolymorphicContract>();
		this.ContractsByImplementationId = contracts.ToImmutableDictionary(contract => contract.ImplementationId ?? throw new Exception($"Contract {contract} has an implementation ID of invalid value null."));
	}

	/// <summary>
	/// Reads the JSON and deserializes it to fit the correct <see cref="PolymorphicContract"/>, based on the <see cref="PolymorphicContract.ImplementationId"/> JSON-property.
	/// </summary>
	public override PolymorphicContract? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Check for null values
		if (reader.TokenType == JsonTokenType.Null) return default;

		if (reader.TokenType != JsonTokenType.StartObject) 
			throw new JsonException($"Error during deserialization of {typeToConvert.Name} using {nameof(PolymorphicJsonConverter)}: Unexpected token found in JSON: {reader.TokenType}. Expected: {JsonTokenType.StartObject}.");

		// Copy the current state from reader (it's a struct).
		var readerAtStart = reader;

		// Read the contract static implementation id from our JSON document.
		using var jsonDocument = JsonDocument.ParseValue(ref reader);
		var id = jsonDocument.RootElement.GetProperty(nameof(PolymorphicContract.ImplementationId)).GetString();

		// See if that class can be deserialized or not.
		if (String.IsNullOrEmpty(id))
			throw new NotSupportedException($"Error during deserialization of {typeToConvert.Name} using {nameof(PolymorphicJsonConverter)}: ID is empty.");

		if (!this.ContractsByImplementationId.TryGetValue(id, out var contract))
		{
			// It is possible that the ID refers to a MagicEnum. In that case the name of the enum should be stripped, and only the member name should be used.
			if (!this.ContractsByImplementationId.TryGetValue(id[(id.IndexOf('.') + 1)..], out contract))
				throw new NotSupportedException($"Error during deserialization of {typeToConvert.Name} using {nameof(PolymorphicJsonConverter)}: unable to deserialize because ID {id} is not found in the provided contracts.");
		}

		// Deserialize it.
		var value = JsonSerializer.Deserialize(ref readerAtStart, contract.GetType(), options);
		if (value is null) throw new JsonException($"Error during deserialization of {typeToConvert.Name} using {nameof(PolymorphicJsonConverter)}.");
				
		return (PolymorphicContract)value;
	}

	public override void Write(Utf8JsonWriter writer, PolymorphicContract objectToWrite, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
}