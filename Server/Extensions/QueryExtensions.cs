using Server.Core.Models;

namespace Server.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> ApplyAuthFiltering<T>(this IQueryable<T> self, string username)
        where T: IAuthFilterable
    {
        return self.Where(item => item.User.Username == username);
    }
    
    public static IQueryable<IGrouping<int, T>> ApplySubjectGrouping<T>(this IQueryable<T> self)
        where T: Subject
    {
        return self.OrderBy(s => s.Count).GroupBy(s => s.WeekDay);
    }
}