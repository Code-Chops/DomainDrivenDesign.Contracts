namespace CodeChops.Contracts.Filters;

/// <summary>
/// For filtering out <typeparamref name="TSource"/> elements in a collection. 
/// </summary>
public interface IFilter<TSource>
{
	IQueryable<TSource> ApplyFilter(IQueryable<TSource> shows);
}