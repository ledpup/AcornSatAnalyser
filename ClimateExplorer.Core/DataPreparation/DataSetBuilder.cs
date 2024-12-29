﻿namespace ClimateExplorer.Core.DataPreparation;

using ClimateExplorer.Core.DataPreparation.Model;
using ClimateExplorer.Core.Model;
using System.Diagnostics;
using static ClimateExplorer.Core.Enums;

public class DataSetBuilder
{
    public async Task<BuildDataSetResult> BuildDataSet(PostDataSetsRequestBody request)
    {
        ValidateRequest(request);

        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Reads raw data (from one or multiple sources) & derive a series from it as per the request
        var series = await SeriesProvider.GetSeriesDataPointsForRequest(request.SeriesDerivationType, request.SeriesSpecifications!);

        if (series.DataRecords != null && series.DataRecords.All(x => x.Value == null))
        {
            throw new Exception("All data points in the series are null. Check the raw input file");
        }

        Console.WriteLine("GetSeriesDataPointsForRequest completed in " + sw.Elapsed);

        if (request.MinimumDataResolution != null && series.DataResolution < request.MinimumDataResolution)
        {
            throw new Exception($"The data resolution of this series is {series.DataResolution}. A minimum data resolution thresold of {request.MinimumDataResolution} is required for this type of aggregation.");
        }

        // Run the rest of the pipeline (this is a separate method for testability)
        var dataPoints = BuildDataSetFromDataPoints(series.DataRecords!, series.DataResolution, request);

        if (dataPoints.All(x => x.Value == null))
        {
            throw new Exception("All data points are null. There was insufficient data for adequate aggregation.");
        }

        return
            new BuildDataSetResult
            {
                DataPoints = dataPoints,
                RawDataPoints = request.IncludeRawDataPoints ? series.DataRecords : null,
                UnitOfMeasure = series.UnitOfMeasure,
            };
    }

    public ChartableDataPoint[] BuildDataSetFromDataPoints(DataRecord[] dataPoints, DataResolution dataResolution, PostDataSetsRequestBody request)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Apply specified transformation (if any) to each data point in the series
        var transformedDataPoints = SeriesTransformer.ApplySeriesTransformation(dataPoints, request.SeriesTransformation);

        Console.WriteLine("ApplySeriesTransformation completed in " + sw.Elapsed);
        sw.Restart();

        // Filter data at series level
        var filteredDataPoints = SeriesFilterer.ApplySeriesFilters(transformedDataPoints, request.FilterToSouthernHemisphereTemperateSeason, request.FilterToTropicalSeason, request.FilterToYear, request.FilterToYearsAfterAndIncluding, request.FilterToYearsBefore);

        Console.WriteLine("ApplySeriesFilters completed in " + sw.Elapsed);
        sw.Restart();

        // When BinningRule is ByYearAndDay, we can drop-out of the data pipeline process here.
        // No aggregation is required because we're just returning the data at the original resolution (i.e., daily)
        if (request.BinningRule == BinGranularities.ByYearAndDay)
        {
            return ConvertDataPointsToChartableDataPoints(filteredDataPoints);
        }

        // Assign to Bins, Buckets and Cups
        var rawBins = Binner.ApplyBinningRules(filteredDataPoints, request.BinningRule, request.CupSize, dataResolution);

        Console.WriteLine("ApplyBinningRules completed in " + sw.Elapsed);
        sw.Restart();

        // Flag bins that have a bucket containing a cup with insufficient data
        var filteredRawBins =
            BinRejector.ApplyBinRejectionRules(
                rawBins,
                request.RequiredCupDataProportion,
                request.RequiredBucketDataProportion,
                request.RequiredBinDataProportion);

        Console.WriteLine("ApplyBinRejectionRules completed in " + sw.Elapsed);
        sw.Restart();

        // Calculate aggregates for each bin
        var aggregatedBins = BinAggregator.AggregateBins(filteredRawBins, request.BinAggregationFunction, request.BucketAggregationFunction, request.CupAggregationFunction, request.SeriesTransformation);

        // Calculate final value based on bin aggregates
        var finalBins = FinalBinValueCalculator.CalculateFinalBinValues(aggregatedBins, request.Anomaly);

        Console.WriteLine("AggregateBins completed in " + sw.Elapsed);
        sw.Restart();

        return
            finalBins
            .Select(
                x =>
                new ChartableDataPoint
                {
                    BinId = x.Identifier!.Id,
                    Label = x.Identifier.Label,
                    Value = x.Value,
                })
            .ToArray();
    }

    public void ValidateRequest(PostDataSetsRequestBody request)
    {
        if (request.SeriesSpecifications == null)
        {
            throw new ArgumentNullException(nameof(request.SeriesSpecifications));
        }
    }

    private static ChartableDataPoint[] ConvertDataPointsToChartableDataPoints(DataRecord[] filteredDataPoints)
    {
        return filteredDataPoints
            .Select(x => (new YearAndDayBinIdentifier(x.Year, x.Month!.Value, x.Day!.Value), x.Value))
            .Select(
            x =>
            new ChartableDataPoint
            {
                BinId = x.Item1.Id,
                Label = x.Item1.Label,
                Value = x.Item2 == null ? null : x.Item2.Value,
            })
        .ToArray();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "Rule conflict")]
    public class BuildDataSetResult
    {
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public ChartableDataPoint[]? DataPoints { get; set; }
        public DataRecord[]? RawDataPoints { get; set; }
    }
}
