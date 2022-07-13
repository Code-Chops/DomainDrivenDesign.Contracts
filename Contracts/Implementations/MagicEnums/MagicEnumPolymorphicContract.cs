using CodeChops.DomainDrivenDesign.Contracts.Polymorphism;
using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.Implementations.MagicEnums;

public record MagicEnumPolymorphicContract : PolymorphicContract
{
	public string SpecificTypeId { get; } = null!;
	public string Name { get; init; } = null!;
	
	public MagicEnumPolymorphicContract(IMagicEnum magicEnum)
	{
		if (magicEnum is null) throw new ArgumentNullException(nameof(magicEnum));

		this.SpecificTypeId = GetNameWithoutGenericParameters(magicEnum);
		this.Name = magicEnum.ToString()!;
	}

	// ReSharper disable once UnusedMember.Global
	public MagicEnumPolymorphicContract()
	{
	}

	private static string GetNameWithoutGenericParameters(IMagicEnum magicEnum)
	{
		var name = magicEnum.GetType().Name;
		var indexOfLessThan = name.IndexOf('<');
		
		return indexOfLessThan == -1
			? name
			: name[..indexOfLessThan];
	}
}