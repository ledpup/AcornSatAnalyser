using ClimateExplorer.Core.DataPreparation;
using ClimateExplorer.Core.ViewModel;
using ClimateExplorer.Visualiser.UiModel;
using static ClimateExplorer.Core.Enums;

namespace ClimateExplorer.Visualiser.Pages;
public partial class RegionalAndGlobal : ChartablePage
{
    public RegionalAndGlobal()
    {
        pageName = "regionalandglobal";
    }

    void IDisposable.Dispose()
    {
        Dispose();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Locations = (await DataService!.GetLocations()).ToList();
    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateUiStateBasedOnQueryString(false);

        await AddDefaultChart();
    }

    private async Task AddDefaultChart()
    {
        if (chartView == null || chartView.ChartSeriesList == null || chartView.ChartSeriesList.Any())
        {
            return;
        }

        var co2 = DataSetDefinitionViewModel.GetDataSetDefinitionAndMeasurement(DataSetDefinitions!, null, DataType.CO2, null, throwIfNoMatch: true);

        chartView!.ChartSeriesList!.Add(
            new ChartSeriesDefinition()
            {
                SeriesDerivationType = SeriesDerivationTypes.ReturnSingleSeries,
                SourceSeriesSpecifications = SourceSeriesSpecification.BuildArray(null, co2!),
                Aggregation = SeriesAggregationOptions.Maximum,
                BinGranularity = BinGranularities.ByYear,
                SecondaryCalculation = SecondaryCalculationOptions.AnnualChange,
                Smoothing = SeriesSmoothingOptions.MovingAverage,
                SmoothingWindow = 10,
                Value = SeriesValueOptions.Value,
                Year = null,
                DisplayStyle = SeriesDisplayStyle.Line,
                RequestedColour = UiLogic.Colours.Brown,
            }
        );

        await BuildDataSets();
    }

    string GetPageTitle()
    {
        string title = $"ClimateExplorer";

        Logger!.LogInformation("GetPageTitle() returning '" + title + "' NavigateTo");

        return title;
    }

    protected override async Task UpdateComponents()
    {
        await Task.CompletedTask;
    }
}
