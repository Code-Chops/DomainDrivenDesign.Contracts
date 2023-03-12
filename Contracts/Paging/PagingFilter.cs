using System.Linq.Expressions;

namespace CodeChops.Contracts.Paging;

/// <inheritdoc cref="CodeChops.Contracts.Paging.PagingFilter" />
public record PagingFilter<TSource> : PagingFilter, IPagingFilter<TSource>
{
	/// <param name="size">A page size of NULL means no page size limit.</param>
	public PagingFilter(int page, int? size) 
		: base(page, size)
	{
	}

	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public IOrderedQueryable<TSource> ApplyPaging<TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> orderBy)
	{
		if (this.Offset > 0)
			source = source.Skip(this.Offset);
		
		if (this.Size is not null)
			source = source.Take(this.Size.Value);

		return source.OrderBy(orderBy);
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