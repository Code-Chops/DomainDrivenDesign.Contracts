using System.Reflection;
using CodeChops.DomainDrivenDesign.DomainModeling.Identities;

namespace CodeChops.DomainDrivenDesign.Contracts.Converters.Identities;

public class IdentityJsonConverterFactory : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert) 
		=> !typeToConvert.IsAbstract && typeToConvert.IsAssignableTo(typeof(Id));

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var genericBaseId = GetGenericBaseId(typeToConvert);
		var idPrimitive = genericBaseId.GetGenericArguments().First();
		var converter = (JsonConverter)Activator.CreateInstance(
			type: typeof(IdentityJsonConverter<,>).MakeGenericType(typeToConvert, idPrimitive),
			bindingAttr: BindingFlags.Instance | BindingFlags.Public,
			binder: null,
			args: null,
			culture: null)!;

		return converter;
		
		static Type GetGenericBaseId(Type type)
		{
			while (type.BaseType != null)
			{
				if (type.BaseType == typeof(Id))
				{
					return type;
				}
				type = type.BaseType;
			}
			throw new InvalidOperationException($"Primitive of was {type.Name} was not found.");
		}
	}
}