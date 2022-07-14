using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.MagicEnums;

public record MagicEnumContract : PolymorphicContract, IHasMultipleAdaptersContract
{
	public string AdapterId { get; init; } = null!;
	public string Name { get; init; } = null!;
	
	public MagicEnumContract(IMagicEnum magicEnum)
	{
		if (magicEnum is null) throw new ArgumentNullException(nameof(magicEnum));

		this.AdapterId = GetNameWithoutGenericParameters(magicEnum);
		this.Name = magicEnum.ToString()!;
	}

	// ReSharper disable once UnusedMember.Global
	public MagicEnumContract()
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