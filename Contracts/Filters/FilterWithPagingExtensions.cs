using CodeChops.Contracts.Paging;

namespace CodeChops.Contracts.Filters;

public static class FilterWithPagingExtensions
{
	/// <summary>
	/// Applies paging to the queryable source. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> FilterWithPaging<TSource>(this IQueryable<TSource> source, IFilterWithPaging<TSource> filter)
	{
		source = filter.ApplyFilter(source);
		source = filter.ApplyPaging(source);

		return source;
	}
}