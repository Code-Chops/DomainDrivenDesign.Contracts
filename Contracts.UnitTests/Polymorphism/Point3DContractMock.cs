namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism;

public partial record Point3DContractMock(int X, int Y, int Z) : Point2DContractMock(X, Y);