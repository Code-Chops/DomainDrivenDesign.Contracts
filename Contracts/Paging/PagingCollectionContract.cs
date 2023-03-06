namespace CodeChops.Contracts.Paging;

public record PagedCollectionContract<TElementContract> : CollectionContract<TElementContract>
	where TElementContract : Contract
{
	public PagingContract Paging { get; }

	public PagedCollectionContract(IReadOnlyList<TElementContract> values, PagingContract paging)
		: base(values)
	{
		this.Paging = paging;
	}
}