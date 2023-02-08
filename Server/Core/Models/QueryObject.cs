namespace Server.Core.Models;

public class QueryObject
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? Search { get; set; }
}