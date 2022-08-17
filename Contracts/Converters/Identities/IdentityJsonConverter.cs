using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using CodeChops.DomainDrivenDesign.DomainModeling.Identities;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters.Identities;

public class IdentityJsonConverter<TId, TValue> : JsonConverter<TId>
	where TId : Id<TId, TValue>
	where TValue : IEquatable<TValue>, IComparable<TValue>
{
	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAssignableTo(typeof(TId));
	
	private static JsonConverter<TValue> DefaultConverter { get; } = (JsonConverter<TValue>)new JsonSerializerOptions().GetConverter(typeof(TValue));
	
	public override TId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Check for null values
		if (reader.TokenType == JsonTokenType.Null) return default;
		
		if (reader.TokenType is not JsonTokenType.String and not JsonTokenType.Number) 
			throw new JsonException($"Unexpected token found in JSON: {reader.TokenType}. Expected: {JsonTokenType.String}.");

		var id = (TId)FormatterServices.GetUninitializedObject(typeof(TId));
		var propertyInfo = typeof(Id<TId, TValue>).GetProperty("_value", BindingFlags.Instance | BindingFlags.NonPublic)!;
		var primitiveValue = DefaultConverter.Read(ref reader, typeof(TValue), options)!;
		
		propertyInfo.SetValue(id, primitiveValue, BindingFlags.Instance | BindingFlags.NonPublic, binder: null, index: null, CultureInfo.InvariantCulture );

		return id;
	}

	public override void Write(Utf8JsonWriter writer, TId id, JsonSerializerOptions options) 
		=> DefaultConverter.Write(writer, (TValue)id, options);
}