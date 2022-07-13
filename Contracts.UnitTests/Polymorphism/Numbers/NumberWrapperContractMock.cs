using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Numbers;

public class NumberWrapperContractMock
{
	public Number<int> IntNumber { get; init; }
	public Number<double> DoubleNumber { get; init; }
}