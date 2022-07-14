using System.Reflection;
using CodeChops.DomainDrivenDesign.DomainModeling;
using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

public record MagicEnumAdapter<TMagicEnum> : Adapter<MagicEnumContract>
	where TMagicEnum : IMagicEnum
{
	protected internal override Type GetDomainObjectType() => typeof(TMagicEnum);
	
	protected internal override MagicEnumContract ConvertDomainObjectToContract(IDomainObject domainObject)
	{
		var contract = new MagicEnumContract((IMagicEnum)domainObject);
		return contract;
	}
	
	protected internal override IDomainObject ConvertContractToDomainObject(Contract contract)
	{
		var magicEnumContract = contract as MagicEnumContract ?? throw new ArgumentException($"Contract should be of type {nameof(MagicEnumContract)} in {nameof(MagicEnumAdapter<TMagicEnum>)}."); 

		var magicEnum = (IMagicEnum)GetSingleMemberMethod.Invoke(obj: null, parameters: new object?[] { magicEnumContract.Name })!;
		return magicEnum;
	}
	
	// ReSharper disable once ClassNeverInstantiated.Local
	private record MagicEnumDummy : MagicEnum<MagicEnumDummy>;

	private static MethodInfo GetSingleMemberMethod { get; } = typeof(TMagicEnum).GetMethods(bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
		.Single(method => method.Name == nameof(MagicEnumDummy.GetSingleMember) && method.GetParameters().Any(parameter => parameter.ParameterType == typeof(string)));
}