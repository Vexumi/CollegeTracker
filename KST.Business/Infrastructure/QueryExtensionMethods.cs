using System.Linq.Expressions;

namespace KST.Business.Infrastructure;

public static class QueryExtensionMethods
{
    public static IQueryable<TSource> WhereIfElse<TSource>(
        this IQueryable<TSource> source, 
        bool condition, 
        Func<TSource, bool> predicateIf, 
        Func<TSource, bool> predicateElse)
    {
        if (condition) return source.Where(predicateIf).AsQueryable();
        return source.Where(predicateElse).AsQueryable();
    }
    
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        if (condition) return query.Where(predicate);
        return query;
    }
}