namespace Server.Controllers.Resources;

public class GetSubjectResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int WeekDay { get; set; }
    public int Week { get; set; }
    public int Count { get; set; }
    public string? Professor { get; set; }
    public string? Url { get; set; }
    public string? Address { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Type { get; set; }
}