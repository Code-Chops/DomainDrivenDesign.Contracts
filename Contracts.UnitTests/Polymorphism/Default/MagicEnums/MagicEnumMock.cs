using CodeChops.MagicEnums;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Default.MagicEnums;

public record MagicEnumMock : MagicEnum<MagicEnumMock>
{
	public static MagicEnumMock Value1 { get; } = CreateMember(1);
	public static MagicEnumMock Value2 { get; } = CreateMember(7);
}