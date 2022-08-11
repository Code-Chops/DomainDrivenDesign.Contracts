using System.Reflection;
using CodeChops.DomainDrivenDesign.Contracts.Converters.MagicEnums;
using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters.Numbers;

public class MagicEnumJsonConverterFactory : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert) 
		=> typeToConvert.IsAssignableTo(typeof(IMagicEnum));

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var converter = (JsonConverter)Activator.CreateInstance(
			type: typeof(MagicEnumJsonConverter<>).MakeGenericType(typeToConvert),
			bindingAttr: BindingFlags.Instance | BindingFlags.Public,
			binder: null,
			args: new[] { this.MagicEnums },
			culture: null)!;

		return converter;
	}
	
	private List<IMagicEnum> MagicEnums { get; }

	public MagicEnumJsonConverterFactory(IEnumerable<IMagicEnum>? magicEnums = null)
	{
		this.MagicEnums = magicEnums?.ToList() ?? new List<IMagicEnum>();
	}

}