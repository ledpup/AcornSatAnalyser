﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcornSat.Core
{
    public static class AverageFunctions
    {
        public static float Median(this IEnumerable<float> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            var data = values.OrderBy(n => n).ToArray();
            if (data.Length == 0)
                throw new InvalidOperationException();
            if (data.Length % 2 == 0)
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0f;
            return data[data.Length / 2];
        }

        public static float Mode(this IEnumerable<float> values)
        {
            var mode = values.GroupBy(n => n)
                                .OrderByDescending(g => g.Count())
                                .Select(g => g.Key).FirstOrDefault();
            return mode;
        }
    }
}
