﻿using System;
using System.Globalization;

namespace AcornSat.WebApi.Model.DataSetBuilder
{
    public static class DateHelpers
    {
        public static string GetShortMonthName(short monthNumber)
        {
            return new DateTime(2000, monthNumber, 1).ToString("MMM", CultureInfo.InvariantCulture);
        }

        public static DateOnly GetLastDayInMonth(short year, short month)
        {
            // Strategy: start with the 28th day of the specified month. Keep adding days until we trip over into the next month
            // (which may also be the next year).
            DateOnly d = new DateOnly(year, month, 1);

            do
            {
                d = d.AddDays(1);
            } while (d.Month == month);

            return d;
        }
    }
}
