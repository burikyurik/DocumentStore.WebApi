namespace DocumentStore.Application.QueryHandlers
{
    // ReSharper disable once UnusedTypeParameter
    public interface ISortHelper<in T>
    {
        string CreateSortQuery(string orderByQueryString);
    }
}