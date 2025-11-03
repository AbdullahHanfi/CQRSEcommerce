namespace Domain.Shared.Collections;

public class PaginatedList<Entity> : List<Entity> {
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalItems { get; private set; }
    public int PageSize { get; private set; }

    public PaginatedList(List<Entity> items, int count, int pageIndex, int pageSize) {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalItems = count;
        PageSize = pageSize;

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<Entity>> CreateAsync(IQueryable<Entity> source, int pageIndex, int pageSize) {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<Entity>(items, count, pageIndex, pageSize);
    }
}
