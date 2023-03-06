namespace CodeChops.Contracts.Paging;

public record PagedCollectionContract<TElementContract> : CollectionContract<TElementContract>
	where TElementContract : Contract
{
	public PageContract Page { get; }

	public PagedCollectionContract(IReadOnlyList<TElementContract> values, PageContract page)
		: base(values)
	{
		this.Page = page;
	}
}