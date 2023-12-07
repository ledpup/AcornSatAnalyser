# Climate Explorer

[Climate Explorer](https://www.climateexplorer.net/) is a website to help people understand climate change. It's focussed on trying to provide a simple and approachable interface for people to explore the changes to climate in their region. This github site is the digital repository for everything used to bring [the website](https://www.climateexplorer.net/) together.

The data is sourced from:

- [NOAA](https://www.noaa.gov/) United States of America's **National Oceanic and Atmospheric Administration**, a scientific and regulatory agency within the United States Department of Commerce
- [NCEI](https://www.ncei.noaa.gov/) United States of America's **National Centers for Environmental Information** (NCEI), is a U.S. government agency that manages one of the world's largest archives of atmospheric, coastal, geophysical, and oceanic data. It is an office of NOAA, which operates under the U.S. Department of Commerce
- [BOM](http://www.bom.gov.au/) Australia's **Bureau of Meteorology**
- [NIWA](https://niwa.co.nz/) New Zealand's **National Institute of Water and Atmospheric Research** (maintains the [7-stations](https://niwa.co.nz/seven-stations) and [11-station](https://niwa.co.nz/our-science/climate/information-and-resources/nz-temp-record/temperature-trends-from-raw-data) series)
- [NSIDC](https://nsidc.org/home) United States of America's **National Snow & Ice Data Center**
- [Met Office](https://www.metoffice.gov.uk/) United Kingdom's national weather service
- [Royal Observatory of Belgium](https://www.astro.oma.be/en/) The Royal Observatory of Belgium (ROB) is a scientific research institution. The main activities are Reference Systems and Planetology, Seismology and Gravimetry, Astronomy and Astrophysics and Solar Physics and Space Weather.

## Glossary
- [GHCNm](https://www.ncei.noaa.gov/products/land-based-station/global-historical-climatology-network-monthly) Global Historical Climatology Network monthly provides monthly climate summaries from thousands of weather stations around the world
- [ACORN-SAT](http://www.bom.gov.au/climate/data/acorn-sat/): Australian Climate Observations Reference Network – Surface Air Temperature
- [RAIA](http://www.bom.gov.au/climate/data/acorn-sat/#tabs=Data-and-networks): Remote Australian Islands and Antarctica (a monthly dataset that is a smaller part of ACORN-SAT)
- [HadCET](https://www.metoffice.gov.uk/hadobs/hadcet/index.html): The Hadley Centre Central England Temperature (HadCET) dataset is the longest instrumental record of temperature in the world
- [7-stations](https://niwa.co.nz/seven-stations) and [11-station](https://niwa.co.nz/our-science/climate/information-and-resources/nz-temp-record/temperature-trends-from-raw-data) series are [NIWA](https://niwa.co.nz/)'s selection of stations for climate research 

## Technical
- Built in [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/) using
  - .NET 8
  - C#
  - Blazor
  - Minimal Web API
- Below are descriptions of the main projects:
  - Web: Blazor server-side website that displays the data to the user. This is a wrapper project, most of the Blazor files are in Web.Client.
  - Web.Client: Blazor Web Assembly version of the website. This will download to the browser and the browser will switch to using this after its downloaded.
  - WebApi: Web API that gets and processes the data that Visualiser uses
  - Core: shared files between Visualiser, Analyser and WebApi
  - UnitTests: tests for various sub-systems
- Additional libraries used
  - https://github.com/Megabit/Blazorise
  - https://github.com/DP-projects/DPBlazorMap
  - https://github.com/AeonLucid/GeoCoordinate.NetStandard1
  - https://github.com/arivera12/BlazorCurrentDevice
  - https://www.nuget.org/packages/DBSCAN/ ([source](https://github.com/viceroypenguin/Dbscan))
  - https://www.nuget.org/packages/Blazored.LocalStorage/ ([source](https://github.com/Blazored/LocalStorage))

## How to use

- Download the github repo. 
- Open in Visual Studio 2022. 
- Set your start-up projects to be Web and WebApi and run. Two websites should start-up; the user interface and the web API.
