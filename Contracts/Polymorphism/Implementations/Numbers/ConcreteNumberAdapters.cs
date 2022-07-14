namespace CodeChops.DomainDrivenDesign.Contracts.Polymorphism.Implementations.Numbers;

public partial record NumberDecimalAdapter	: NumberAdapter<decimal,	NumberDecimalContract>;
public partial record NumberDoubleAdapter	: NumberAdapter<double,		NumberDoubleContract>;
public partial record NumberFloatAdapter	: NumberAdapter<float,		NumberFloatContract>;
public partial record NumberIntAdapter		: NumberAdapter<int,		NumberIntContract>;
public partial record NumberUIntAdapter		: NumberAdapter<uint,		NumberUIntContract>;
public partial record NumberLongAdapter		: NumberAdapter<long,		NumberLongContract>;
public partial record NumberULongAdapter	: NumberAdapter<ulong,		NumberULongContract>;
public partial record NumberShortAdapter	: NumberAdapter<short,		NumberShortContract>;
public partial record NumberUShortAdapter	: NumberAdapter<ushort,		NumberUShorContract>;