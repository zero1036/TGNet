using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
//using MongoDB.Bson;
//using MongoDB.Driver

namespace TG.Example
{
    public class ExpressLearn
    {
        public void Start()
        {
            //使用LambdaExpression构建表达式树
            Expression<Func<int, int, int, int>> expr = (x, y, z) => (x + y) / z;
            Console.WriteLine(expr.Compile()(1, 2, 3));
            //使用LambdaExpression构建可执行的代码
            Func<int, int, int, int> fun = (x, y, z) => (x + y) / z;
            Console.WriteLine(fun(1, 2, 3));
            //动态构建表达式树
            ParameterExpression pe1 = Expression.Parameter(typeof(int), "x");
            ParameterExpression pe2 = Expression.Parameter(typeof(int), "y");
            ParameterExpression pe3 = Expression.Parameter(typeof(int), "z");
            var body = Expression.Divide(Expression.Add(pe1, pe2), pe3);
            var w = Expression.Lambda<Func<int, int, int, int>>(body, new ParameterExpression[] { pe1, pe2, pe3 });
            Console.WriteLine(w.Compile()(1, 2, 3));
            List<Entity> list = new List<Entity> { new Entity { Id1 = 1 }, new Entity { Id1 = 2 }, new Entity { Id1 = 3 } };
            var d = list.AsQueryable().WhereIn(o => o.Id1, new int[] { 1, 2 });
            d.ToList().ForEach(o =>
            {
                Console.WriteLine(o.Id1);
            });
            Console.ReadKey();
        }


        public void sdfs()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "y");
            var body = Expression.Add(x, y);
            var add = Expression.Lambda<Func<int, int, int>>(
                  body, x, y).Compile();
        }
    }


    public class Entity
    {
        //public ObjectId Id;
        public int Id1;
        public string Name { get; set; }
    }
    public static class cc
    {
        public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> query, Expression<Func<T, TValue>> obj, IEnumerable<TValue> values)
        {
            return query.Where(BuildContainsExpression(obj, values));
        }
        private static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector)
            {
                throw new ArgumentNullException("valueSelector");
            }
            if (null == values)
            {
                throw new ArgumentNullException("values");
            }
            var p = valueSelector.Parameters.Single();
            if (!values.Any()) return e => false;
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate(Expression.Or);
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
    }
}
