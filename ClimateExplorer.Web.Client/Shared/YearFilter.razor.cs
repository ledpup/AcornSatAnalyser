﻿using Blazorise;
using ClimateExplorer.Core.DataPreparation;
using Microsoft.AspNetCore.Components;
using static ClimateExplorer.Core.Enums;

namespace ClimateExplorer.Web.Client.Shared;

public partial class YearFilter
{
    [Parameter] public ChartStartYears? ChartStartYear { get; set; }
    [Parameter] public List<short>? DatasetYears { get; set; }
    [Parameter] public List<short>? SelectedYears { get; set; }
    [Parameter] public string? SelectedStartYear { get; set; }
    [Parameter] public string? SelectedEndYear { get; set; }
    [Parameter] public BinGranularities SelectedBinGranularity { get; set; }

    [Parameter] public EventCallback<ChartStartYears?> OnDynamicStartYearChanged { get; set; }
    [Parameter] public EventCallback<string?> OnStartYearTextChanged { get; set; }
    [Parameter] public EventCallback<string> OnEndYearTextChanged { get; set; }
    [Parameter] public EventCallback<bool?> OnShowRangeSliderChanged { get; set; }
    [Parameter] public bool? ShowRangeSlider { get; set; }

    public string? SelectedStartYearInternal { get; set; }
    public string? SelectedEndYearInternal { get; set; }

    Modal? filterModal;

    public List<string> SelectedYearsText = new();

    public string PopupText { get; set; } = @"<p>This dialog allows you to change the start and end years for the chart. For example, if you want to see the change in temperature/rainfall for the 20th century, you could set the end year to 2000.</p>
<p><strong>Range slider</strong>: allows you to graphically increase and decrease the start and end year; or, by moving the slider within the extents, it allows you to change both start and end years at the same time, changing the range of years on the chart. For example, you could filter to only 30 years of data, between 1910 and 1940, then move the slider to another set of 30 years, between 1940 and 1970.</p>
<p><strong>Dynamically set the start year of the chart to be the most recent start year across all the datasets on the chart</strong>: when this is checked, the start year for the chart will be the last start year found across the datasets. For example, Canberra's temperature records start in 1914. Canberra's rainfall records start in 1924. With the option checked, the chart will start in 1924 because that's the latest start year. The start year for the chart will dynamically adjust to whatever datasets are selected for viewing. This option is checked by default.<p>";

    private Modal? InfoModal;
    private Task ShowYearFilteringInfo()
    {
        if (!string.IsNullOrWhiteSpace(PopupText))
        {
            return InfoModal!.Show();
        }
        return Task.CompletedTask;
    }


    public async Task Show()
    {
        await filterModal!.Show();
    }

    public async Task Hide()
    {
        await filterModal!.Hide();
    }


    protected override void OnParametersSet()
    {
        SelectedStartYearInternal = SelectedStartYear;
        SelectedEndYearInternal = SelectedEndYear;

        SelectedYearsText = new();

        base.OnParametersSet();
    }

    void ValidateYear(ValidatorEventArgs e)
    {
        var year = Convert.ToString(e.Value);

        e.Status =
            string.IsNullOrEmpty(year)
            ? ValidationStatus.None
            :
                year.Length == 4
                ? ValidationStatus.Success
                : ValidationStatus.Error;
    }

    async Task OnStartYearTextChangedInternal(string? text)
    {
        if (SelectedStartYearInternal == text)
        {
            return;
        }

        SelectedStartYearInternal = text;
        if (text?.Length == 0 || text?.Length == 4)
        {
            await DynamicStartYearChanged(null);
            await OnStartYearTextChanged.InvokeAsync(text);
        }
    }

    async Task OnEndYearTextChangedInternal(string text)
    {
        SelectedEndYearInternal = text;

        if (text.Length == 0 || text.Length == 4)
        {
            await OnEndYearTextChanged.InvokeAsync(text);
        }
    }

    async Task DynamicStartYearChanged(ChartStartYears? value)
    {
        ChartStartYear = value;
        if (ChartStartYear != null)
        {
            SelectedStartYearInternal = null;
            await OnStartYearTextChanged.InvokeAsync(SelectedStartYearInternal);
        }
        await OnDynamicStartYearChanged.InvokeAsync(value);
    }

    async Task ShowRangeSliderChanged(bool? value)
    {
        ShowRangeSlider = value;
        await OnShowRangeSliderChanged.InvokeAsync(value);
    }
}
