using CodeChops.Contracts.Paging;

namespace CodeChops.Contracts.Filters;

public interface IFilterWithPaging<TSource> : IFilter<TSource>, IPagingFilter<TSource>
{
}