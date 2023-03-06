namespace CodeChops.Contracts.Paging;

public static class PagingFilterExtensions
{
	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> ApplyPaging<TSource>(this IQueryable<TSource> source, PagingFilter filter)
	{
		if (filter.Offset > 0)
			source = source.Skip(filter.Offset);
		
		if (filter.Size is not null)
			source = source.Take(filter.Size.Value);

		return source;
	}
}