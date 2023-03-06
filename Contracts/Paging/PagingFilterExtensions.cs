namespace CodeChops.Contracts.Paging;

public static class PagingFilterExtensions
{
	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, IPagingFilter<TSource> filter)
	{
		return filter.ApplyPaging(source);
	}
}