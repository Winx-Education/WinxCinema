using System.Linq.Expressions;
using System.Reflection;

namespace Winx_Cinema.Data
{
    public static class DataUtils
    {
        private const string EntityParameter = "e";
        private const char DescendSymbol = '-';
        private const char SplitSymbol = ',';
        private const char LessSymbol = '<';
        private const char RangeSymbol = '-';
        private const char GreaterSymbol = '>';

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

        public static IQueryable<T> FilterRange<T, PropertyT>(this IQueryable<T> entities, string? filter, string property) where PropertyT : struct
        {
            if (string.IsNullOrWhiteSpace(filter))
                return entities;

            // Building expresion
            BinaryExpression body;
            var param = Expression.Parameter(typeof(T), EntityParameter);
            // Validate filter values
            try
            {
                body = filter[0] switch
                {
                    LessSymbol or GreaterSymbol => CreateThanExpression<T, PropertyT>(filter, param, property),
                    _ => CreateRangeExpression<T, PropertyT>(filter, param, property),
                };
            }
            catch (Exception)
            {
                return entities;
            }

            var exp = Expression.Lambda<Func<T, bool>>(body, param);

            // Filter for entities with properties within specified range
            return entities.Where(exp);
        }

        private static BinaryExpression CreateThanExpression<T, PropertyT>(string filter, ParameterExpression param, string property) where PropertyT : struct
        {
            bool less = filter[0] == LessSymbol;
            filter = filter.TrimStart(less ? LessSymbol : GreaterSymbol);

            var filterValue = (PropertyT)Convert.ChangeType(filter, typeof(PropertyT));

            // Fullfill PostreSQL UTC DateTime requirement
            if (filterValue is DateTime dateTime)
                filterValue = (PropertyT)(object)DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            var constant = Expression.Constant(filterValue);
            var member = Expression.Property(param, property);

            // Set limit based on first filter symbol
            if (less)
                return Expression.LessThanOrEqual(member, constant);
            return Expression.GreaterThanOrEqual(member, constant);
        }

        private static BinaryExpression CreateRangeExpression<T, PropertyT>(string filter, ParameterExpression param, string property) where PropertyT : struct
        {
            string[] values = filter.Split(RangeSymbol);

            // Validate lenght
            if (values.Length != 2)
                throw new ArgumentException("Invalid number of values were passed");

            PropertyT[] filterValues = [
                (PropertyT)Convert.ChangeType(values[0], typeof(PropertyT)),
                (PropertyT)Convert.ChangeType(values[1], typeof(PropertyT))
            ];

            // Fullfill PostreSQL UTC DateTime requirement
            if (typeof(PropertyT) == typeof(DateTime))
            {
                filterValues[0] = (PropertyT)(object)DateTime.SpecifyKind((DateTime)(object)filterValues[0], DateTimeKind.Utc);
                filterValues[1] = (PropertyT)(object)DateTime.SpecifyKind((DateTime)(object)filterValues[1], DateTimeKind.Utc);
            }

            var member = Expression.Property(param, property);

            // Set lower limit
            var constant = Expression.Constant(filterValues[0]);
            var lowerLimit = Expression.GreaterThanOrEqual(member, constant);

            // Set upper limit
            constant = Expression.Constant(filterValues[1]);
            var upperLimit = Expression.LessThanOrEqual(member, constant);
            return Expression.AndAlso(lowerLimit, upperLimit);
        }
    }
}
