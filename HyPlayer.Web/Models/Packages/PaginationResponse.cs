namespace HyPlayer.Web.Models.Packages;

public class PaginationResponse<T>
{
    public T[] Data { get; set; } = [];
    public int Total { get; set; }
}