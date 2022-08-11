using CodeChops.Geometry.Space.Directions.Strict;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.MagicEnums;

public record MagicEnumWrapperContractMock
{
	public MagicEnumMock2 Enum { get; init; }
	public IStrictDirection Direction { get; init; }

	public MagicEnumWrapperContractMock(MagicEnumMock2 @enum, IStrictDirection direction)
	{
		this.Enum = @enum;
		this.Direction = direction;
	}
}