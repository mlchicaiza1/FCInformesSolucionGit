using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MvcJqGrid.Enums;
using MvcJqGrid.Extensions;

namespace FCInformesSolucion.Helpers
{
    public static class CollectionUtils
    {
        //public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate, params object[] values)
        //{
        //    return (IQueryable<T>)Where((IQueryable)source, predicate, values);
        //}

        //public static IQueryable Where(this IQueryable source, string predicate, params object[] values)
        //{
        //    if (source == null) throw new ArgumentNullException("source");
        //    if (predicate == null) throw new ArgumentNullException("predicate");
        //    LambdaExpression lambda = DynamicExpression.Lambda(source.ElementType, typeof(bool), predicate, values);
        //    return source.Provider.CreateQuery(
        //        Expression.Call(
        //            typeof(Queryable), "Where",
        //            new Type[] { source.ElementType },
        //            source.Expression, Expression.Quote(lambda)));
        //}

        public static bool IsNullOrEmpty<T>(IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNullOrEmpty(IEnumerable source)
        {
            return source == null || !source.GetEnumerator().MoveNext();
        }



        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, SortOrder sort) where TEntity : class
        {
            IQueryable<TEntity> returnValue = null;
            if (propertyName.IsNullOrWhiteSpace())
            {
                propertyName = "Id";
            }
            var landa = GetExpression<TEntity>(propertyName );
            string methodName = String.Empty;
            switch (sort)
            {
                case SortOrder.Desc:
                    // returnValue = source.OrderByDescending();
                    methodName = "OrderByDescending";
                    break;
                case SortOrder.Asc:
                    methodName = "OrderBy";
                    // returnValue = source.OrderBy(expression);
                    break;
            }
            return (IOrderedQueryable<TEntity>)typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), landa.ReturnType)
                .Invoke(null, new object[] { source, landa });

            //  return returnValue; 
        }

        public static LambdaExpression GetExpression<TEntity>(string prop)
        {
            var param = Expression.Parameter(typeof(TEntity), "p");
            var parts = prop.Split('.');
            var type = typeof(TEntity);
            //  Expression expr = null;
            var parent = parts.Aggregate<string, Expression>(param, Expression.Property);

            foreach (string p in parts)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(p);
                // expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, parent, param);
            return lambda;

            // return Expression.Lambda(parent, param);
        }

        public static IQueryable<TEntity> SortAndPage<TEntity>(this IQueryable<TEntity> list, string sidx, SortOrder sort, int page, int rows) where TEntity : class
        {

            list = OrderBy(list, sidx, sort);
            list = list.Skip((page - 1) * rows).Take(rows);
            return list;
        }
        public static IEnumerable<TEntity> Distinct<TEntity>(this IQueryable<TEntity> list, Func<TEntity, object> func) where TEntity : class
         {
             return list.GroupBy(func).Select(g => g.First());
         }
        public static IEnumerable<TEntity> Distinct<TEntity>(this IEnumerable<TEntity> list, Func<TEntity, object> func) where TEntity : class
        {
            return list.GroupBy(func).Select(g => g.First());
        }

        public static IEnumerable<SelectListItem> GetListWithSelection(this IEnumerable<SelectListItem> list,
            string value)
        {
            return list.Select(i => new SelectListItem
            {
                Text = i.Text,
                Value = i.Value,
                Selected = i.Value == value
            });
        }
    }

   
}
