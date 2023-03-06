namespace CodeChops.Contracts.Paging;

/// <summary>
/// For filtering out a page of a collection. 
/// </summary>
public interface IPagingFilter<TSource>
{
	IQueryable<TSource> ApplyPaging(IQueryable<TSource> shows);
}