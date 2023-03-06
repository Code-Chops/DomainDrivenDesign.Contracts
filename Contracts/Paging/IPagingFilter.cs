namespace CodeChops.Contracts.Paging;

/// <summary>
/// For filtering out a page of a collection. 
/// </summary>
public interface IPagingFilter<TSource>
{
	public static abstract int DefaultPageSize { get; }
	
	IQueryable<TSource> ApplyPaging(IQueryable<TSource> shows);
}