namespace RachaStats.Application.Common.Pagination;

public class PaginatedResponse<T>
{
    public List<T> Itens { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItens { get; set; }
    public int TotalCount { get; set; }
}