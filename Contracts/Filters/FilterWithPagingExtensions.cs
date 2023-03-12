using System.Linq.Expressions;
using CodeChops.Contracts.Paging;

namespace CodeChops.Contracts.Filters;

public static class FilterWithPagingExtensions
{
	/// <summary>
	/// Applies paging to the queryable source with the provided <paramref name="orderBy"/>. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> FilterWithPaging<TSource, TKey>(this IQueryable<TSource> source, IFilterWithPaging<TSource> filter, Expression<Func<TSource, TKey>> orderBy)
	{
		source = filter.ApplyFilter(source);
		source = filter.ApplyPaging(source, orderBy);

		return source;
	}
	
	/// <summary>
	/// Applies paging to the queryable source based on the ID. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> FilterWithPaging<TSource, TId>(this IQueryable<TSource> source, IFilterWithPaging<TSource> filter)
		where TSource : IHasId<TId>
		where TId : IId<TId>
	{
		source = filter.ApplyFilter(source);
		source = filter.ApplyPaging(source, s => s.Id);

		return source;
	}
}