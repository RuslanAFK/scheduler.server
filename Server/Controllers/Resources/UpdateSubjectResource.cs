using System.ComponentModel.DataAnnotations;

namespace Server.Controllers.Resources;

public class UpdateSubjectResource
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required, Range(0, 6)]
    public int WeekDay { get; set; }
    [Required, Range(0, 2)]
    public int Week { get; set; }
    [Required, Range(1, 8)]
    public int Count { get; set; }
    public string? Professor { get; set; }
    [Url]
    public string? Url { get; set; }
    public string? Address { get; set; }
    [Required, Range(0, 23)]
    public int Hours { get; set; }
    [Required, Range(0, 59)]
    public int Minutes { get; set; }
    [Required, Range(0, 2)]
    public int Type { get; set; }
}