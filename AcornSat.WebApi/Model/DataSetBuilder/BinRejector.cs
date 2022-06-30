﻿using System;
using System.Linq;

namespace AcornSat.WebApi.Model.DataSetBuilder
{
    public static class BinRejector
    {
        public static RawBin[] ApplyBinRejectionRules(RawBin[] bins, float requiredDataProportion)
        {
            return
                bins
                .Where(x => !BinContainsAnyCupWithInsufficientData(x, requiredDataProportion))
                .ToArray();
        }

        static bool BinContainsAnyCupWithInsufficientData(RawBin bin, float requiredDataProportion)
        {
            foreach (var bucket in bin.Buckets)
            {
                foreach (var cup in bucket.Cups)
                {
                    var daysInCup = (int)((cup.LastDayInCup.ToDateTime(new TimeOnly()) - cup.FirstDayInCup.ToDateTime(new TimeOnly())).TotalDays) + 1;

                    var dataPointsInCup = cup.DataPoints.Count(x => x.Value.HasValue);

                    var proportionOfPointsInCup = (float)dataPointsInCup / daysInCup;

                    if (proportionOfPointsInCup < requiredDataProportion)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
