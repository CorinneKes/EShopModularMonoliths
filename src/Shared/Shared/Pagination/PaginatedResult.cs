namespace Shared.Pagination;

public class PaginatedResult<TEntity>
    (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    public int PageIndex { get;  } = pageIndex;
    public int PageSize { get; } = pageSize;

    public long Count { get; } = count; // total number of entries in the dataset

    public IEnumerable<TEntity> Data { get; } = data;
}
