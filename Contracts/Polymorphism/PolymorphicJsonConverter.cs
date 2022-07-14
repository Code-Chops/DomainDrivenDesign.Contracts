﻿using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

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
	private ImmutableDictionary<string, PolymorphicContract> ContractsByTypeId { get; }

	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(PolymorphicContract));

	public PolymorphicJsonConverter(IEnumerable<PolymorphicContract> contracts)
	{
		this.ContractsByTypeId = contracts.ToImmutableDictionary(contract => contract.TypeId ?? throw new Exception($"Contract {contract} has an ID of null."));
	}

	/// <summary>
	/// Reads the JSON and deserializes it to fit the correct <see cref="PolymorphicContract"/>, based on the <see cref="PolymorphicContract.TypeId"/> JSON-property.
	/// </summary>
	public override PolymorphicContract? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Check for null values
		if (reader.TokenType == JsonTokenType.Null) return default;

		if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException($"Unexpected token found in JSON: {reader.TokenType}. Expected: {JsonTokenType.StartObject}.");

		// Copy the current state from reader (it's a struct).
		var readerAtStart = reader;

		// Read the contract static type id from our JSON document.
		using var jsonDocument = JsonDocument.ParseValue(ref reader);
		var id = jsonDocument.RootElement.GetProperty(nameof(PolymorphicContract.TypeId)).GetString();

		// See if that class can be deserialized or not.
		if (String.IsNullOrEmpty(id) || !this.ContractsByTypeId.TryGetValue(id, out var contract))
		{
			throw new NotSupportedException($"{id ?? "<unknown>"} can not be deserialized");
		}

		// Deserialize it.
		var value = (PolymorphicContract?)JsonSerializer.Deserialize(ref readerAtStart, contract.GetType(), options) ?? throw new JsonException("Error during retrieval of JSON value.");
		return value;
	}

	public override void Write(Utf8JsonWriter writer, PolymorphicContract objectToWrite, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, objectToWrite, objectToWrite.GetType(), options);
}