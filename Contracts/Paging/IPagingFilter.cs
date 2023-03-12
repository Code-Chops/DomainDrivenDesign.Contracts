using System.Linq.Expressions;

namespace CodeChops.Contracts.Paging;

/// <summary>
/// For filtering out a page of a collection. 
/// </summary>
public interface IPagingFilter<TSource>
{
	IOrderedQueryable<TSource> ApplyPaging<TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> orderBy);
}