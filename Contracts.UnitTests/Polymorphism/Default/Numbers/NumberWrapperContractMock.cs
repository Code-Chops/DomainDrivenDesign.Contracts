using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Default.Numbers;

public class NumberWrapperContractMock
{
	public Number<int> IntNumber { get; }
	public Number<double> DoubleNumber { get; }

	public NumberWrapperContractMock(Number<int> intNumber, Number<double> doubleNumber)
	{
		this.IntNumber = intNumber;
		this.DoubleNumber = doubleNumber;
	}
}