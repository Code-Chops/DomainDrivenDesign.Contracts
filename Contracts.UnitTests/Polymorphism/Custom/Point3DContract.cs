namespace CodeChops.DomainDrivenDesign.Contracts.UnitTests.Polymorphism.Custom;

public partial record Point3DContract(int X, int Y, int Z) : Point2DContract(X, Y);