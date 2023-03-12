namespace CodeChops.Contracts.Adapters;

public interface IIncomingAdapter : IAdapter
{
	object ConvertToObject(Contract contract);
}