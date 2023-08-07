using Server.Core.Models;

namespace Server.Extensions;

public static partial class QueryExtensions
{
    public static IQueryable<T> ApplyAuthFiltering<T>(this IQueryable<T> self, string username)
        where T: IAuthFilterable
    {
        return self.Where(item => item.User.Username == username);
    }
    
    public static IQueryable<KeyValuePair<int, List<T>>> ApplySubjectGrouping<T>(this IQueryable<T> self)
        where T: Subject
    {
        return self.OrderBy(s => s.Count).GroupBy(s => s.WeekDay).Select(group =>
            new KeyValuePair<int, List<T>>(group.Key, group.ToList()));
    }

    public static StudyWeek ToNormalized<T>(this IQueryable<IGrouping<int, T>> self)
        where T : Subject
    {
        var studyWeek = new StudyWeek();
        foreach (var group in self)
        {
            var studyDay = new StudyDay
            {
                WeekDay = group.Key,
                Subjects = group.AsQueryable()
            };
            studyWeek.Days.Add(studyDay);
        }
        return studyWeek;
    }
}