using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Extensions
{
   public static  class IQueryableExtensions
    {
        #region  Orderby Extensions
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


         
    }
}
