using System.Collections;

namespace Server.Core.Models;

public class ListResponse<T>
{
    public IDictionary<int, List<T>> Items { get; set; }
    public ListResponse(IDictionary<int, List<T>> items)
    {
        Items = items;
    }
}