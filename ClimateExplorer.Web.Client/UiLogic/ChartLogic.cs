﻿namespace ClimateExplorer.Web.UiLogic;

using ClimateExplorer.Core;
using ClimateExplorer.Web.UiModel;
using ClimateExplorer.Core.DataPreparation;
using ClimateExplorer.Core.Model;
using static ClimateExplorer.Core.Enums;
using ClimateExplorer.Web.Client.Shared;

public static class ChartLogic
{
    public static string BuildChartTitle(List<SeriesWithData> chartSeriesWithData)
    {
        if (chartSeriesWithData.Count == 1)
        {
            return chartSeriesWithData.Single().ChartSeries!.FriendlyTitle;
        }

        var locationNames =
            chartSeriesWithData
            .SelectMany(x => x.ChartSeries!.SourceSeriesSpecifications!)
            .Select(x => x.LocationName)
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        if (locationNames.Length > 0)
        {
            return string.Join(", ", locationNames);
        }

        return "Climate data";
    }

    //public static LineChartDataset<double?> GetLineChartDataset(
    //   string label,
    //   List<double?> values,
    //   ChartColor chartColor,
    //   UnitOfMeasure unitOfMeasure,
    //   SeriesTransformations seriesTransformations,
    //   bool renderSmallPoints,
    //   SeriesAggregationOptions seriesAggregationOptions)
    //{
    //    var count = values.Count;
    //    var colour = new List<string>();
    //    for (var i = 0; i < count; i++)
    //    {
    //        colour.Add(chartColor);
    //    }

    //    var lineChartDataset =
    //           new LineChartDataset<double?>
    //           {
    //               Label = label,
    //               Data = values,
    //               BackgroundColor = colour,
    //               BorderColor = colour,
    //               Fill = false,
    //               PointRadius = renderSmallPoints ? 0.1f : 1,
    //               ShowLine = true,
    //               PointBorderColor = colour,
    //               PointHoverBackgroundColor = colour,
    //               BorderDash = new List<int> { },

    //               // Tension = 0.1f,
    //               YAxisID = GetYAxisId(seriesTransformations, unitOfMeasure, seriesAggregationOptions),
    //           };

    //    return lineChartDataset;
    //}

    //public static BarChartDataset<double?> GetBarChartDataset(
    //   string label,
    //   List<double?> values,
    //   UnitOfMeasure unitOfMeasure,
    //   bool? absoluteValues,
    //   bool redPositive,
    //   SeriesTransformations seriesTransformations,
    //   SeriesAggregationOptions seriesAggregationOptions)
    //{
    //    var colour = GetBarChartColourSet(values, seriesTransformations == SeriesTransformations.IsFrosty ? false : redPositive);

    //    return
    //           new BarChartDataset<double?>
    //           {
    //               Label = label,
    //               Data = values.Select(x => absoluteValues.GetValueOrDefault() && x.HasValue ? Math.Abs(x.Value) : x).ToList(),
    //               BorderColor = colour,
    //               BackgroundColor = colour,
    //               YAxisID = GetYAxisId(seriesTransformations, unitOfMeasure, seriesAggregationOptions),
    //           };
    //}

    //public static ChartDataset<double?> GetChartDataset(
    //   string label,
    //   List<double?> values,
    //   UnitOfMeasure unitOfMeasure,
    //   ChartType chartType,
    //   ChartColor? chartColour = null,
    //   bool? absoluteValues = false,
    //   bool redPositive = true,
    //   SeriesTransformations seriesTransformations = SeriesTransformations.Identity,
    //   SeriesAggregationOptions seriesAggregationOptions = SeriesAggregationOptions.Mean,
    //   bool renderSmallPoints = false)
    //{
    //    switch (chartType)
    //    {
    //        case ChartType.Line:
    //            if (!chartColour.HasValue)
    //            {
    //                throw new NullReferenceException(nameof(chartColour));
    //            }

    //            return GetLineChartDataset(label, values, chartColour.Value, unitOfMeasure, seriesTransformations, renderSmallPoints, seriesAggregationOptions);
    //        case ChartType.Bar:
    //            return GetBarChartDataset(label, values, unitOfMeasure, absoluteValues, redPositive, seriesTransformations, seriesAggregationOptions);
    //    }

    //    throw new NotImplementedException($"ChartType {chartType}");
    //}

    //public static ChartTrendlineData CreateTrendline(int datasetIndex, ChartColor colour)
    //{
    //    return
    //        new ChartTrendlineData
    //        {
    //            DatasetIndex = datasetIndex,
    //            Width = 3,
    //            Color = colour,
    //        };
    //}

    public static string GetXAxisLabel(BinGranularities binGranularity)
    {
        return binGranularity switch
        {
            BinGranularities.ByYear => "Year",
            BinGranularities.ByYearAndMonth => "Month and year",
            BinGranularities.ByYearAndWeek => "Week and year",
            BinGranularities.ByYearAndDay => "Date",
            BinGranularities.ByMonthOnly => "Month of the year",
            BinGranularities.BySouthernHemisphereTemperateSeasonOnly => "Southern hemisphere temperate season",
            BinGranularities.BySouthernHemisphereTropicalSeasonOnly => "Southern hemisphere tropical season",
            _ => throw new NotImplementedException($"BinGranularity {binGranularity}"),
        };
    }

    public static ChartSeries AddDataSetToChart(
        SeriesWithData cs,
        DataSet dataSet,
        string label,
        string htmlColourCode,
        bool absoluteValues = false,
        bool redPositive = true,
        bool renderSmallPoints = false)
    {
        //var colour = ChartColor.FromHtmlColorCode(htmlColourCode);

        cs.ChartSeries!.Colour = htmlColourCode;

        //var chartType =
        //    chartSeries.ChartSeries.DisplayStyle == SeriesDisplayStyle.Line
        //    ? ChartType.Line
        //    : ChartType.Bar;

        //var chartDataset = GetChartDataset(label, values, dataSet.MeasurementDefinition!.UnitOfMeasure, chartType, colour, absoluteValues, redPositive, chartSeries.ChartSeries.SeriesTransformation, chartSeries.ChartSeries.Aggregation, renderSmallPoints);

        // await chart.AddDataSet(chartDataset);

        return new ChartSeries
        {
            Name = label,
            ChartDataItems = dataSet.DataRecords.Select(x => new ChartDataItem(x.Label!, (decimal?)x.Value)).ToList(),
            Colour = htmlColourCode,
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "Rules conflict")]
    public static Tuple<BinIdentifier, BinIdentifier> GetBinRangeToPlotForGaplessRange(
        IEnumerable<DataSet> preProcessedDataSets,
        bool chartAllData,
        string selectedStartYear,
        string selectedEndYear)
    {
        // Parse the start and end years, if any, specified by the user
        var userStartYear = string.IsNullOrEmpty(selectedStartYear) ? null : (short?)short.Parse(selectedStartYear);
        var userEndYear = string.IsNullOrEmpty(selectedEndYear) ? null : (short?)short.Parse(selectedEndYear);

        // Analyse the data we want to plot, to find the first & last bin we have a value for, for each data set
        var firstBinInEachDataSet =
            preProcessedDataSets
            .Select(x => x.GetFirstDataRecordWithValueInDataSet())
            .Select(x => (BinIdentifierForGaplessBin)x.GetBinIdentifier()!);

        var lastBinInEachDataSet =
            preProcessedDataSets
            .Select(x => x.GetLastDataRecordWithValueInDataSet())
            .Select(x => (BinIdentifierForGaplessBin)x.GetBinIdentifier()!);

        var firstBinAcrossAllDataSets = firstBinInEachDataSet.Min();
        var lastFirstBinAcrossAllDataSets = firstBinInEachDataSet.Max();
        var lastBinAcrossAllDataSets = lastBinInEachDataSet.Max();

        BinIdentifierForGaplessBin? startBin = null;

        if (chartAllData)
        {
            startBin = firstBinAcrossAllDataSets!;
        }
        else if (userStartYear.HasValue)
        {
            if (userStartYear.Value > firstBinAcrossAllDataSets!.FirstDayInBin.Year)
            {
                if (firstBinAcrossAllDataSets is YearBinIdentifier)
                {
                    startBin = new YearBinIdentifier(userStartYear.Value);
                }

                if (firstBinAcrossAllDataSets is YearAndMonthBinIdentifier)
                {
                    startBin = new YearAndMonthBinIdentifier(userStartYear.Value, 1);
                }

                if (firstBinAcrossAllDataSets is YearAndWeekBinIdentifier)
                {
                    startBin = new YearAndWeekBinIdentifier(userStartYear.Value, 1);
                }

                if (firstBinAcrossAllDataSets is YearAndDayBinIdentifier)
                {
                    startBin = new YearAndDayBinIdentifier(userStartYear.Value, 1, 1);
                }
            }
        }
        else
        {
            startBin = lastFirstBinAcrossAllDataSets!;
        }

        var endBin = lastBinAcrossAllDataSets;

        if (userEndYear != null)
        {
            if (userEndYear.Value < endBin!.FirstDayInBin.Year)
            {
                if (endBin is YearBinIdentifier)
                {
                    endBin = new YearBinIdentifier(userEndYear.Value);
                }

                if (endBin is YearAndMonthBinIdentifier)
                {
                    endBin = new YearAndMonthBinIdentifier(userEndYear.Value, 1);
                }

                if (endBin is YearAndWeekBinIdentifier)
                {
                    endBin = new YearAndWeekBinIdentifier(userEndYear.Value, 1);
                }

                if (endBin is YearAndDayBinIdentifier)
                {
                    endBin = new YearAndDayBinIdentifier(userEndYear.Value, 1, 1);
                }
            }
        }

        return new Tuple<BinIdentifier, BinIdentifier>(startBin!, endBin!);
    }

    public static string GetYAxisId(SeriesTransformations seriesTransformations, UnitOfMeasure unitOfMeasure, SeriesAggregationOptions seriesAggregationOptions)
    {
        return seriesTransformations switch
        {
            SeriesTransformations.IsFrosty => "daysOfFrost",
            SeriesTransformations.DayOfYearIfFrost => seriesAggregationOptions == SeriesAggregationOptions.Maximum ? "lastDayOfFrost" : "firstDayOfFrost",
            SeriesTransformations.EqualOrAbove25 => "daysEqualOrAbove25",
            SeriesTransformations.EqualOrAbove35 => "daysEqualOrAbove35",
            SeriesTransformations.EqualOrAbove1 => "daysEqualOrAbove1",
            SeriesTransformations.EqualOrAbove1AndLessThan10 => "daysEqualOrAbove1LessThan10",
            SeriesTransformations.EqualOrAbove10 => "daysEqualOrAbove10",
            SeriesTransformations.EqualOrAbove10AndLessThan25 => "daysEqualOrAbove10LessThan25",
            SeriesTransformations.EqualOrAbove25mm => "daysEqualOrAbove25mm",
            _ => unitOfMeasure.ToString().ToLowerFirstChar()
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "Rule conflict")]
    public static List<string> GetBarChartColourSet(List<double?> values, bool redPositive = true)
    {
        var count = values.Count;

        //var red = ChartColor.FromRgba(255, 63, 63, 1f);
        //var blue = ChartColor.FromRgba(63, 63, 255, 1f);

        var colour = new List<string>();
        //for (var i = 0; i < count; i++)
        //{
        //    ChartColor chartColor;
        //    if (values[i].HasValue)
        //    {
        //        var value = values![i]!.Value * (redPositive ? 1f : -1f);
        //        if (value > 0)
        //        {
        //            chartColor = red;
        //        }
        //        else
        //        {
        //            chartColor = blue;
        //        }

        //        colour.Add(chartColor);
        //    }
        //    else
        //    {
        //        colour.Add(null!);
        //    }
        //}

        return colour;
    }
}
