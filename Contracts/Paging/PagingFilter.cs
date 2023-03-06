namespace CodeChops.Contracts.Paging;

/// <summary>
/// For filtering out one page of a collection. 
/// </summary>
public record PagingFilter
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
	
	// ReSharper disable VirtualMemberCallInConstructor
	public PagingFilter(int page, int? size = null)
	{
		this.Page = page;
		this.Size = size ?? this.DefaultSize;

		if (this.Size <= 0) this.Size = 1;
		if (this.Size > this.MaximumSize) this.Size = this.MaximumSize;
	}
}