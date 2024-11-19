namespace CodeChops.Contracts.Paging;

/// <summary>
/// The contract of a page with a default (maximum page) size that can be overriden.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record PagingContract : Contract
{
	/// <summary>
	/// The page number (starting with page 0).
	/// <remarks>Add [FromQuery(Name = "page")]</remarks>
	/// </summary>
	public virtual int Page { get; init; }

	/// <summary>
	/// The page size.
	/// </summary>
	/// <remarks>Add [FromQuery(Name = "size")]</remarks>
	public int Size { get; init; }

	protected virtual int MaximumSize => 1000;
	protected virtual int DefaultSize => 20;

	// ReSharper disable VirtualMemberCallInConstructor
	public PagingContract()
	{
		if (this.Page < 0)
			this.Page = 0;

		if (this.Size <= 0)
			this.Size = this.DefaultSize;

		if (this.Size > this.MaximumSize)
			this.Size = this.MaximumSize;
	}
}