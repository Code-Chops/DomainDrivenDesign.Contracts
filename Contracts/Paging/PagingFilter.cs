namespace CodeChops.Contracts.Paging;

/// <inheritdoc cref="PagingFilter"/>
public record PagingFilter<TSelf> : PagingFilter, IHasDefault<PagingFilter<TSelf>>
	where TSelf : PagingFilter<TSelf>, IPagingFilter
{
	public static PagingFilter<TSelf> NoPaging { get; } = new(page: 0);
	public static PagingFilter<TSelf> Default { get; } = new(page: 0, size: null);

	/// <summary>
	/// Get paging without page limit.
	/// </summary>
	private PagingFilter(int page)
		: base(page, null)
	{
	}
	
	/// <param name="page">0-based.</param>
	/// <param name="size">When not provided, it will take <see cref="IPagingFilter.DefaultSize"/> as value.</param>
	// ReSharper disable VirtualMemberCallInConstructor
	public PagingFilter(int page, int? size)
		: base(page, size ?? TSelf.DefaultSize)
	{
	}
}

/// <summary>
/// For filtering out a page of a collection. 
/// </summary>
public abstract record PagingFilter
{
	/// <summary>
	/// The page number (starting with page 0).
	/// </summary>
	public int Page { get; } 
	
	/// <summary>
	/// The page size. Null means no size limit.
	/// </summary>
	public int? Size { get; }

	public int Offset => this.Page * (this.Size ?? 0);
	
	protected PagingFilter(int page, int? size)
	{
		this.Page = page;
		this.Size = size;
	}
}