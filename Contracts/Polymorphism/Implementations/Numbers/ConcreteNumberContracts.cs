namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers;

public partial record NumberDecimalContract	: NumberContract<decimal>;
public partial record NumberDoubleContract	: NumberContract<double>;
public partial record NumberFloatContract	: NumberContract<float>;
public partial record NumberIntContract		: NumberContract<int>;
public partial record NumberUIntContract	: NumberContract<uint>;
public partial record NumberLongContract	: NumberContract<long>;
public partial record NumberULongContract	: NumberContract<ulong>;
public partial record NumberShortContract	: NumberContract<short>;
public partial record NumberUShorContract	: NumberContract<ushort>;