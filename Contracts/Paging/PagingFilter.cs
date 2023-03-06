namespace CodeChops.Contracts.Paging;

/// <inheritdoc cref="CodeChops.Contracts.Paging.PagingFilter" />
public record PagingFilter<TSource> : PagingFilter, IHasDefault<PagingFilter<TSource>>
{
	public static PagingFilter<TSource> NoPaging { get; } = new(page: 0);
	public static PagingFilter<TSource> Default { get; } = new(page: 0, size: null);
	
	/// <summary>
	/// Get paging without page limit.
	/// </summary>
	private PagingFilter(int page)
		: base(page, null)
	{
	}

	protected PagingFilter(int page, int? size) 
		: base(page, size)
	{
	}

	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public IQueryable<TSource> ApplyPaging(IQueryable<TSource> source)
	{
		if (this.Offset > 0)
			source = source.Skip(this.Offset);
		
		if (this.Size is not null)
			source = source.Take(this.Size.Value);

		return source;
	}
}

/// <inheritdoc cref="IPagingFilter{TSource}"/>
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