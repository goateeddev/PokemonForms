using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> function)
        {
            return enumerable.Select(item => function(item)).ToList();
        }

        public static List<TResult> ToList<TSource, TResult>(this List<TSource> enumerable, Func<TSource, TResult> function)
        {
            return enumerable?.Select(item => function(item)).ToList();
        }
    }
}