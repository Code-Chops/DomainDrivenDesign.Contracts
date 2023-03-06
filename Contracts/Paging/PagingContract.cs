namespace CodeChops.Contracts.Paging;

/// <summary>
/// The contract of a page with a default (maximum page) size that can be overriden.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record PagingContract : Contract
{
	/// <summary>
	/// The page number (starting with page 0).
	/// </summary>
	public int Page { get; }
	
	/// <summary>
	/// The page size.
	/// </summary>
	public int Size { get; }
	
	protected virtual int MaximumSize => 1000;
	protected virtual int DefaultSize => 20;
	
	[JsonConstructor]
	// ReSharper disable VirtualMemberCallInConstructor
	public PagingContract(int page, int? size = null)
	{
		this.Page = page;
		this.Size = size ?? this.DefaultSize;

		if (this.Size <= 0) this.Size = 1;
		if (this.Size > this.MaximumSize) this.Size = this.MaximumSize;
	}
}