namespace Server.Controllers.Resources;

public class ListResponseResource<T>
{
    public IDictionary<int, List<T>> Items { get; set; }
}