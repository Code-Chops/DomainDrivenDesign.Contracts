using System.Reflection;
using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters.Numbers;

public class NumberJsonConverterFactory : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert) 
		=> !typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(INumber));

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var numberType = typeToConvert.GetGenericArguments()[0];
		var converter = (JsonConverter)Activator.CreateInstance(
			type: typeof(NumberJsonConverter<>).MakeGenericType(numberType),
			bindingAttr: BindingFlags.Instance | BindingFlags.Public,
			binder: null,
			args: null,
			culture: null)!;

		return converter;
	}
}