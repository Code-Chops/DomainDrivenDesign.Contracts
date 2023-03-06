using CodeChops.Contracts.Paging;

namespace CodeChops.Contracts.Filters;

public static class FilterExtensions
{
	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source, IFilter<TSource> filter)
	{
		return filter.ApplyFilter(source);
	}
}