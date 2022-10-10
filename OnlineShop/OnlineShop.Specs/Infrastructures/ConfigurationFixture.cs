﻿using Microsoft.Extensions.Configuration;
using Xunit;

namespace OnlineShop.Specs.Infrastructures;

public class ConfigurationFixture
{
    public ConfigurationFixture()
    {
        Value = GetSettings();
    }

    public TestSettings Value { get; }

    private TestSettings GetSettings()
    {
        var settings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, false)
            .AddEnvironmentVariables()
            .AddCommandLine(Environment.GetCommandLineArgs())
            .Build();

        var testSettings = new TestSettings();
        settings.Bind(testSettings);
        return testSettings;
    }
}

public class TestSettings
{
    public string DbConnectionString { get; set; }
}

[CollectionDefinition(
    nameof(ConfigurationFixture),
    DisableParallelization = false)]
public class
    ConfigurationCollectionFixture : ICollectionFixture<
        ConfigurationFixture>
{
}