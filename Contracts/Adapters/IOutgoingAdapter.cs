namespace CodeChops.Contracts.Adapters;

public interface IOutgoingAdapter : IAdapter
{
	Contract ConvertToContract(object @object);
}