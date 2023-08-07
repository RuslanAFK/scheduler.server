using Server.Extensions;

namespace Server.Controllers.Resources;

public class StudyWeekResource
{
    public ICollection<StudyDay> Days { get; set; }
}