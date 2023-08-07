using Server.Core.Models;

namespace Server.Extensions;

public class StudyDay
{
    public int WeekDay { get; set; }
    public IQueryable<Subject> Subjects { get; set; }
}