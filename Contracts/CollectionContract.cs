using System.Collections;

namespace CodeChops.Contracts;

public record CollectionContract<TElementContract> : Contract, IReadOnlyList<TElementContract>
	where TElementContract : Contract
{
	public IReadOnlyList<TElementContract> Values { get; }

	public PagingContract Paging { get; }
	
	public int Count => this.Values.Count;

	public CollectionContract(IReadOnlyList<TElementContract> values, PagingContract paging)
	{
		this.Values = values;
		this.Paging = paging;
	}

	public virtual TElementContract this[int index] 
		=> Validator.Get<TElementContract>.Default.GuardIndexInRange(this.Values, index, errorCode: null)!;
	
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	public IEnumerator<TElementContract> GetEnumerator() => this.Values.GetEnumerator();
}