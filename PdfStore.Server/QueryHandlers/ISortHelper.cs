namespace DocumentStore.Application.QueryHandlers
{
    public interface ISortHelper<in T>
    {
        string CreateSortQuery(string orderByQueryString);
    }
}