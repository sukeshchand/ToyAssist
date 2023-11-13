﻿using System.Collections.Generic;
namespace ToyAssist.Web.TypeExtensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
           => self.Select((item, index) => (item, index));

    }
}
