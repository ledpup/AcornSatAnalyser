﻿using ClimateExplorer.Data.Bom;

var httpClient = new HttpClient();
var userAgent = "Mozilla /5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36";
var acceptLanguage = "en-US,en;q=0.9,es;q=0.8";
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(acceptLanguage);

AcornSatDownloader.DownloadAndExtractData("acorn_sat_v2.5.0_daily_tmean");
AcornSatDownloader.DownloadAndExtractData("acorn_sat_v2.5.0_daily_tmax");
AcornSatDownloader.DownloadAndExtractData("acorn_sat_v2.5.0_daily_tmin");

var stations = await BomLocationsAndStationsMapper.BuildAcornSatLocationsFromReferenceMetaDataAsync(Guid.Parse("E5EEA4D6-5FD5-49AB-BF85-144A8921111E"), "_Australia_unadjusted");
await BomDataDownloader.GetDataForEachStation(httpClient, stations);
await BomLocationsAndStationsMapper.BuildAcornSatAdjustedDataFileMappingAsync(Guid.Parse("b13afcaf-cdbc-4267-9def-9629c8066321"), @"Output\DataFileMapping\DataFileMapping_Australia_unadjusted.json", "_Australia_adjusted");