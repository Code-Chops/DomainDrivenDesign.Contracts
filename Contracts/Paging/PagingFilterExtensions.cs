using System.Linq.Expressions;

namespace CodeChops.Contracts.Paging;

public static class PagingFilterExtensions
{
	/// <summary>
	/// Applies paging to the queryable source with the provided <paramref name="orderBy"/>. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> Paging<TSource, TKey>(this IQueryable<TSource> source, IPagingFilter<TSource> filter, Expression<Func<TSource, TKey>> orderBy)
	{
		return filter.ApplyPaging(source, orderBy);
	}
	
	/// <summary>
	/// Applies paging to the queryable source based on the ID. <see cref="PagingFilter.Page"/> is 0-based.
	/// </summary>
	public static IQueryable<TSource> Paging<TSource, TId>(this IQueryable<TSource> source, IPagingFilter<TSource> filter)
		where TSource : IHasId<TId>
		where TId : IId<TId>
	{
		return filter.ApplyPaging(source, s => s.Id);
	}
}