using System.Linq.Expressions;
using System.Reflection;

namespace ReactChat.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, object filterObject)
        {
            if (filterObject == null)
                return query;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression combinedExpression = null;

            foreach (PropertyInfo property in filterObject.GetType().GetProperties())
            {
                var value = property.GetValue(filterObject);
                if (value == null)
                    continue;

                var propertyExpression = Expression.Property(parameter, property.Name);
                var constantExpression = Expression.Constant(value);

                // Handling different data types

                Expression? filterExpression = null;

                if (property.PropertyType == typeof(string))
                {
                    MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    filterExpression = Expression.Call(propertyExpression, containsMethod, constantExpression);
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    var convertedValue = Expression.Convert(constantExpression, property.PropertyType);
                    filterExpression = Expression.Equal(propertyExpression, convertedValue);
                }
                else
                {
                    filterExpression = Expression.Equal(propertyExpression, constantExpression);
                }

                combinedExpression = combinedExpression == null
                    ? filterExpression
                    : Expression.AndAlso(combinedExpression, filterExpression);
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

    }
}
