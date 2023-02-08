using Server.Core.Models;

namespace Server.Extensions;

public static class QueryExtensions
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPage = 1000;
    private const int MaxPageSize = 100;

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> self, QueryObject queryObject, 
        int defaultPageSize=DefaultPageSize)
    {
        var page = queryObject.Page ?? DefaultPage;
        var pageSize = queryObject.PageSize ?? defaultPageSize;
        page = page is > 0 and < MaxPage ? page : DefaultPage;
        pageSize = pageSize is > 0 and  < MaxPageSize ? pageSize : defaultPageSize;
        
        return self.Skip((page - 1) * pageSize).Take(pageSize);
    }
    
    public static IQueryable<T> ApplySearching<T>(this IQueryable<T> self, QueryObject queryObject)
        where T: ISearchable
    {
        if (queryObject.Search == null)
            return self;
        var search = queryObject.Search;
        return self.Where(item => item.Name.ToLower().Contains(search.ToLower()));
    }
    
    public static IQueryable<T> ApplyAuthFiltering<T>(this IQueryable<T> self, string username)
        where T: IAuthFilterable
    {
        return self.Where(item => item.User.Username == username);
    }
}