namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Mocks;

public partial record Point3DMock(int X, int Y, int Z) : Point2DMock(X, Y);