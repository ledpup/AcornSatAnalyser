﻿using ClimateExplorer.Core.Model;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using static ClimateExplorer.Core.DataPreparation.SeriesProvider;
using static System.Net.WebRequestMethods;

namespace ClimateExplorer.Visualiser.UiTests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BrowserLocationTests
    {
        IPlaywright _playwright;
        IBrowser _browser;
        IPage _page;

        string _baseApiUrl = "http://localhost:54836";
        string _baseSiteUrl = "http://localhost:5298";
        string _publicSiteUrl = "http://climateexplorer.net";

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            var chromium = _playwright.Chromium;
            _browser = await chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true, });
            var context = await _browser.NewContextAsync( new BrowserNewContextOptions { ViewportSize = new ViewportSize { Width = 1440, Height = 1000} });
            _page = await context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task GoToAllLocationsAndGetAScreenshot()
        {
            var locations = await GetLocationsFromApi();

            var australiaAnomaly = locations.Single(x => x.Id == new Guid("143983a0-240e-447f-8578-8daf2c0a246a"));
            locations.Remove(australiaAnomaly);

            var di = new DirectoryInfo("Assets\\Locations");
            if (!di.Exists)
            {
                di.Create();
            }

            var locationsTemplate = System.IO.File.ReadAllText("locations-template.markdown");

            locationsTemplate = locationsTemplate.Replace("NumberOfLocations", locations.Count.ToString());

            var markdown = new List<string>();

            var firstLocation = true;

            foreach (var location in locations)
            {
                var urlName = location.Name.Replace(" ", "-");
                await _page.GotoAsync($"{_baseSiteUrl}/location/{urlName}");
                Thread.Sleep(firstLocation ? 5000 : 3000);
                await _page.ScreenshotAsync(new()
                {
                    Path = $"Assets\\Locations\\{location.Name}.png",
                });

                markdown.Add($"| [{location.Name}]({_publicSiteUrl}/location/{urlName}) | ![{location.Name}]({{{{site.url}}}}/blog/assets/locations/{location.Name}.png){{: width=\"400\" }} | \r\n");
                firstLocation = false;
            }

            markdown.ForEach(x => locationsTemplate += x);

            System.IO.File.WriteAllText("locations.markdown", locationsTemplate);
        }

        private async Task<List<Location>> GetLocationsFromApi()
        {
            var request = await _playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = _baseApiUrl,
            });

            var response = await request.GetAsync("location");


            Assert.True(response.Ok);
            var jsonResponse = await response.JsonAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var locations = new List<Location>();
            foreach (var locationObj in jsonResponse?.EnumerateArray())
            {
                var location = locationObj.Deserialize<Location>(options);
                locations.Add(location);
            }

            return locations;
        }
    }
}