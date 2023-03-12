using System.Collections;
using CodeChops.Contracts.Paging;

namespace CodeChops.Contracts;

public record CollectionContract<TElementContract> : PagingContract, IReadOnlyList<TElementContract>
	where TElementContract : Contract
{
	public required IReadOnlyList<TElementContract> Values { get; init; }

	public int Count => this.Values.Count;

	public virtual TElementContract this[int index] 
		=> Validator.Get<TElementContract>.Default.GuardIndexInRange(this.Values, index, errorCode: null)!;
	
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	public IEnumerator<TElementContract> GetEnumerator() => this.Values.GetEnumerator();
}