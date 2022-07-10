using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism;

internal class PolymorphicJsonConverter : JsonConverter<IPolymorphicContract>
{
	/// <summary>
	/// Options without polymorphic converters.
	/// </summary>
	private JsonSerializerOptions SerializerOptions { get; }
	private Dictionary<string, IPolymorphicContract> ContractsByTypeId { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(IPolymorphicContract));

	internal PolymorphicJsonConverter(IEnumerable<IPolymorphicContract> contracts, JsonSerializerOptions serializerOptions)
	{
		this.SerializerOptions = serializerOptions;
		this.ContractsByTypeId = contracts.ToDictionary(contract => contract.TypeId);
	}

	/// <summary>
	/// Reads the JSON and deserializes it to fit the correct <see cref="IPolymorphicContract"/>, based on the <see cref="IPolymorphicContract.TypeId"/> JSON-property.
	/// </summary>
	public override IPolymorphicContract? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Check for null values
		if (reader.TokenType == JsonTokenType.Null) return default;

		if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException($"Unexpected token found in JSON: {reader.TokenType}. Expected: {JsonTokenType.StartObject}.");

		// Copy the current state from reader (it's a struct).
		var readerAtStart = reader;

		// Read the contract static type id from our JSON document.
		using var jsonDocument = JsonDocument.ParseValue(ref reader);
		var id = jsonDocument.RootElement.GetProperty(nameof(IPolymorphicContract.TypeId)).GetString();

		// See if that class can be deserialized or not.
		if (String.IsNullOrEmpty(id) || !this.ContractsByTypeId.TryGetValue(id, out var contract))
		{
			throw new NotSupportedException($"{id ?? "<unknown>"} can not be deserialized");
		}

		// Deserialize it.
		var value = (IPolymorphicContract?)JsonSerializer.Deserialize(ref readerAtStart, contract.GetType(), options) ?? throw new JsonException("Error during retrieval of JSON value.");
		return value;
	}

	public override void Write(Utf8JsonWriter writer, IPolymorphicContract objectToWrite, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
}