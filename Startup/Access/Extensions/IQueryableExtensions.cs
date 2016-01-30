using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Security.Claims;
using System.Data.Entity;

namespace Access.Extensions
{
   public static  class IQueryableExtensions
    {
        #region  Orderby Extensions
        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> collection, string key, string direction)
        {
            LambdaExpression sortLambda = BuildLambda<T>(key);

            if (direction.ToUpper() == "ASC")
                return collection.OrderBy((Func<T, object>)sortLambda.Compile());
            else
                return collection.OrderByDescending((Func<T, object>)sortLambda.Compile());
        }

        private static LambdaExpression BuildLambda<T>(string key)
        {
            ParameterExpression TParameterExpression = Expression.Parameter(typeof(T), "p");
            LambdaExpression sortLambda = Expression.Lambda(Expression.Convert(Expression.Property(TParameterExpression, key), typeof(object)), TParameterExpression);
            return sortLambda;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static IQueryable<Tmodel> CustomOrderby<Tmodel>(this IQueryable<Tmodel> query, FilterOptionModel filter) where Tmodel : BaseModel
        {
            if (string.IsNullOrEmpty(filter.OrderByProperty)) return query.OrderBy(o => o.Id);

            return filter.IsDesc
                ? query.OrderByDescending(filter.OrderByProperty)
                : query.OrderBy(filter.OrderByProperty);
        }
        #endregion


        #region Center Logic extensions

        public static Task<int?>GetCenterIdAsync(this AccessContext Context)
        {
            var userId = ClaimsPrincipal.Current.UserId();

            return Context.Users.Where(u => u.Id == userId).Select(u => u.CenterId).FirstOrDefaultAsync();
        }

        #endregion
    }
}
