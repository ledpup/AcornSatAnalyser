﻿using ClimateExplorer.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClimateExplorer.UnitTests;

[TestClass]
public class LocationTests
{
    [TestMethod]
    [DataRow("Portoviejo", "Ecuador", "Portoviejo, Ecuador", "Portoviejo, Ecuador", "Portoviejo, Ecuador")]
    [DataRow("Puyo", "Ecuador", "Puyo, Ecuador", "Puyo, Ecuador", "Puyo, Ecuador")]
    [DataRow("Pichilingue", "Ecuador", "Pichilingue, Ecuador", "Pichilingue, Ecuador", "Pichilingue, Ecuador")]
    [DataRow("Pisco Intl", "Peru", "Pisco Intl, Peru", "Pisco Intl, Peru", "Pisco Intl, Peru")]
    [DataRow("Jorge Chavez Intl", "Peru", "Jorge Chavez Intl", "Jorge Chavez Intl, Peru", "Jorge Chavez Intl, Peru")]
    [DataRow("Canberra", "Australia", "Canberra, Australia", "Canberra, Australia", "Canberra, Australia")]
    [DataRow("Cali Alfonso Bonill", "Colombia", "Cali Alfonso Bonill", "Cali Alfonso Bonill, Colombia", "Cali Alfonso Bonill, Colombia")]

    //"Westermarkelsdorf Fehmarn, Germany"
    [DataRow("Akron Washington Co Ap", "United States of America", "Akron Washington ...", "Akron Washington Co Ap, United States of America", "Akron Washington Co Ap, United States of America")]

    [DataRow("Le Lamentin", "Martinique [France]", "Le Lamentin", "Le Lamentin, Martinique", "Le Lamentin, Martinique [France]")]
    [DataRow("Coloso", "Puerto Rico [United States of America]", "Coloso, Puerto Rico", "Coloso, Puerto Rico", "Coloso, Puerto Rico [United States of America]")]
    [DataRow("Hato", "Netherlands Antilles [Netherlands]", "Hato, Netherlands...", "Hato, Netherlands Antilles", "Hato, Netherlands Antilles [Netherlands]")]
    [DataRow("Bird Island", "South Georgia and the South Sandwich Islands [United Kingdom]", "Bird Island", "Bird Island, South Georgia and the South Sandwich Islands", "Bird Island, South Georgia and the South Sandwich Islands [United Kingdom]")]
    [DataRow("Gibraltar", "Gibraltar [United Kingdom]", "Gibraltar, Gibraltar", "Gibraltar, Gibraltar", "Gibraltar, Gibraltar [United Kingdom]")]

    // Angermunde, Germany

    public void LocationName(string name, string country, string title, string shorterTitle, string fullTitle)
    {
        var location = new Location
        {
            Id = Guid.Empty,
            Name = name,
            Country = country,
            CountryCode = "NA",
            Coordinates = new Coordinates(),
        };
        Assert.AreEqual(name, location.Name);
        Assert.AreEqual(country, location.Country);
        Assert.AreEqual(title, location.Title);
        Assert.AreEqual(shorterTitle, location.ShorterTitle);
        Assert.AreEqual(fullTitle, location.FullTitle);
    }
}
