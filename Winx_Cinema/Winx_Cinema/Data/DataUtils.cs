using System.Linq.Expressions;
using System.Reflection;

namespace Winx_Cinema.Data
{
    public static class DataUtils
    {
        private const string EntityParameter = "e";
        private const char DescendSymbol = '-';
        private const char SplitSymbol = ',';

        public static IQueryable<T> Search<T>(this IQueryable<T> entities, string? search, List<string> properties)
        {
            if (string.IsNullOrWhiteSpace(search))
                return entities;

            search = search.ToLower().Trim();

            // Building expresion
            var param = Expression.Parameter(typeof(T), EntityParameter);
            var constant = Expression.Constant(search);

            Expression? body = null;
            foreach (var propertyName in properties)
            {
                var member = Expression.Property(param, propertyName);
                var next = Expression.Call(member, nameof(string.ToLower), Type.EmptyTypes);
                next = Expression.Call(next, nameof(string.Contains), Type.EmptyTypes, constant);
                body = body == null ? next : Expression.OrElse(body, next);
            }

            if (body == null)
                throw new ArgumentException("No search base expressions were passed");

            var exp = Expression.Lambda<Func<T, bool>>(body, param);

            // Search for entities with similar string properties
            return entities.Where(exp);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> entities, string[] sortBy, Dictionary<string, Expression<Func<T, object>>> expressions)
        {
            IOrderedQueryable<T> sortedEntities = null!;

            bool wasSorted = false;
            sortBy = sortBy.Distinct().ToArray(); // remove duplicates
            foreach (var sortField in sortBy)
            {
                if (string.IsNullOrWhiteSpace(sortField))
                    continue;

                string sortCriteria = sortField.Trim();

                // Get sorting order
                bool ascending = true;
                if (sortCriteria.StartsWith(DescendSymbol))
                {
                    sortCriteria = sortCriteria.TrimStart(DescendSymbol);
                    ascending = false;
                }

                // Get sorting criteria
                if (!expressions.TryGetValue(sortCriteria,
                    out Expression<Func<T, object>>? sortKey))
                    continue;

                // Check if was sorted
                if (!wasSorted)
                {
                    wasSorted = true;
                    // Apply primary sort
                    if (ascending)
                    {
                        sortedEntities = entities.OrderBy(sortKey);
                        continue;
                    }
                    sortedEntities = entities.OrderByDescending(sortKey);
                    continue;
                }

                // Apply secondary sort
                if (ascending)
                {
                    sortedEntities = sortedEntities.ThenBy(sortKey);
                    continue;
                }
                sortedEntities = sortedEntities.ThenByDescending(sortKey);
            }

            if (wasSorted)
                return sortedEntities;
            return entities;
        }

        public static IQueryable<T> FilterIn<T>(this IQueryable<T> entities, string? filter, string property)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return entities;

            // Building expresion
            string[] values = filter.ToLower().Trim().Split(SplitSymbol);

            var param = Expression.Parameter(typeof(T), EntityParameter);
            var constant = Expression.Constant(values);

            var member = Expression.Property(param, property);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var body = Expression.Call(member, nameof(string.ToLower), Type.EmptyTypes);
            body = Expression.Call(typeof(Enumerable), nameof(string.Contains), [propertyType], constant, body);
            var exp = Expression.Lambda<Func<T, bool>>(body, param);

            // Filter for entities with properties which contain any
            return entities.Where(exp);
        }
    }
}
