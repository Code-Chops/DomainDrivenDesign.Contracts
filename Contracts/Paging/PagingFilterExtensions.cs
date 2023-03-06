namespace CodeChops.Contracts.Paging;

public static class PagingFilterExtensions
{
	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> ApplyPaging<TSource>(this IQueryable<TSource> source, PagingFilter filter)
	{
		return source
			.Skip(filter.Page * filter.Size)
			.Take(filter.Size);
	}
}