namespace CodeChops.Contracts.Paging;

/// <summary>
/// The contract of a page with a default (maximum page) size that can be overriden.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public record PagingContract : Contract
{
	protected virtual int DefaultSize => 20;

	/// <summary>
	/// The page number (starting with page 0).
	/// </summary>
	public int Page { get; } 
	
	/// <summary>
	/// The page size.
	/// </summary>
	public int Size { get; }

	[JsonConstructor]
	// ReSharper disable VirtualMemberCallInConstructor
	public PagingContract(int page, int? size)
	{
		this.Page = page;
		this.Size = size ?? this.DefaultSize;
	}
}