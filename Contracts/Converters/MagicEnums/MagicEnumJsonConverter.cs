using System.Collections.Immutable;
using System.Reflection;
using CodeChops.MagicEnums;
using Microsoft.Extensions.Caching.Memory;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters.MagicEnums;

public class MagicEnumJsonConverter : JsonConverter<IMagicEnum>
{
	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAssignableTo(typeof(IMagicEnum));
	
	private static IMemoryCache GetSingleMemberMethodCache { get; } = new MemoryCache(new MemoryCacheOptions());
	private ImmutableDictionary<string, IMagicEnum> EnumsByName { get; }
	private const char EnumDelimiter = '.';

	public MagicEnumJsonConverter(IEnumerable<IMagicEnum>? magicEnums = null)
	{
		magicEnums ??= Array.Empty<IMagicEnum>();
		this.EnumsByName = magicEnums.ToImmutableDictionary(magicEnum => magicEnum.GetType().Name);
	}

	public override IMagicEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Check for null values
		if (reader.TokenType == JsonTokenType.Null) return default;

		if (reader.TokenType != JsonTokenType.String) throw new JsonException($"Unexpected token found in JSON: {reader.TokenType}. Expected: {JsonTokenType.String}.");
		
		var enumIdentifier = reader.GetString() ?? throw new JsonException($"Unable to retrieve enum identifier when trying to deserialize {typeToConvert.GetType().Name}.");
		var delimiterIndex = enumIdentifier.IndexOf(EnumDelimiter);
		if (delimiterIndex == -1) throw new JsonException($"No MagicEnum identifier delimiter ('{EnumDelimiter}') found in {enumIdentifier}.");

		var enumName = enumIdentifier[..delimiterIndex];
		var enumMemberName = enumIdentifier[(delimiterIndex + 1)..];

		//
		// var expectedEnumName = GetNameWithoutGenericParameters(typeToConvert.Name); 
		// if (enumName != expectedEnumName) throw new JsonException($"Incorrect enum name '{enumName}'. Expected '{expectedEnumName}'.");
		
		if (!GetSingleMemberMethodCache.TryGetValue(enumName, out MethodInfo? getSingleMemberMethod))
		{
			getSingleMemberMethod = RetrieveGetSingleMemberMethod(typeToConvert);

			// Can't find the method on the runtime type, find the injected enum.
			if (getSingleMemberMethod is null)
			{
				if (!this.EnumsByName.TryGetValue(enumName, out var injectedEnum))
				{
					throw new JsonException($"Error while deserializing JSON for {typeToConvert.Name}. Unable to find the concrete MagicEnum. Did you forget to inject enum {enumName}?");
				}

				getSingleMemberMethod = RetrieveGetSingleMemberMethod(injectedEnum.GetType());
				if (getSingleMemberMethod is null)
				{
					throw new JsonException($"Error while deserializing JSON for {typeToConvert.Name}. Unable to find the concrete MagicEnum. Did you inject the wrong enum {enumName}?");
				}
			}
			
			GetSingleMemberMethodCache.Set(enumName, getSingleMemberMethod);
		}
		
		var magicEnum = (IMagicEnum)getSingleMemberMethod!.Invoke(obj: null, parameters: new object?[] { enumMemberName })!;
		return magicEnum;


		static MethodInfo? RetrieveGetSingleMemberMethod(Type type)
		{
			var method = type
				.GetMethods(bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.SingleOrDefault(method => method.Name == nameof(MagicEnumDummy.GetSingleMember) && method.GetParameters().Any(parameter => parameter.ParameterType == typeof(string)));

			return method;
		}
	}
	
	private static string GetNameWithoutGenericParameters(string name)
	{
		var indexOfLessThan = name.IndexOf('<');
		
		return indexOfLessThan == -1
			? name
			: name[..indexOfLessThan];
	}

	public override void Write(Utf8JsonWriter writer, IMagicEnum magicEnum, JsonSerializerOptions options)
	{
		var enumIdentifier = $"{GetNameWithoutGenericParameters(magicEnum.GetType().Name)}.{magicEnum}";
		writer.WriteStringValue(enumIdentifier);
	}
	
	// ReSharper disable once ClassNeverInstantiated.Local
	private record MagicEnumDummy : MagicEnum<MagicEnumDummy>;
}