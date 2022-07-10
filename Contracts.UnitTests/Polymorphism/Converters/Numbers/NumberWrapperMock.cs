using CodeChops.GenericMath;

namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Converters.Numbers;

public class NumberWrapperMock
{
	public Number<int> IntNumber { get; }
	public Number<double> DoubleNumber { get; }

	public NumberWrapperMock(Number<int> intNumber, Number<double> doubleNumber)
	{
		this.IntNumber = intNumber;
		this.DoubleNumber = doubleNumber;
	}
}