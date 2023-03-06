using System.Collections;

namespace CodeChops.Contracts;

public record CollectionContract<TElementContract> : Contract, IReadOnlyList<TElementContract>
	where TElementContract : Contract
{
	public IReadOnlyList<TElementContract> Values { get; }

	public int Count => this.Values.Count;

	public CollectionContract(IReadOnlyList<TElementContract> values)
	{
		this.Values = values;
	}

	public virtual TElementContract this[int index] 
		=> Validator.Get<TElementContract>.Default.GuardIndexInRange(this.Values, index, errorCode: null)!;
	
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	public IEnumerator<TElementContract> GetEnumerator() => this.Values.GetEnumerator();
}