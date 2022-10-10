﻿using System.Linq.Expressions;
using OnlineShop.Infrastructure;
using OnlineShop.Services.Infrastructure;

namespace OnlineShop.Specs;

public static class Runner
{
    public static void RunScenario(
        params Expression<Action<object>>[] steps)
    {
        var textContext = new { };
        steps.Select(_ => _.Compile()).ForEach(_ => _.Invoke(textContext));
    }
}

public class Feature : Attribute
{
    public Feature(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
    public string InOrderTo { get; set; }
    public string AsA { get; set; }
    public string IWantTo { get; set; }
}

public class Scenario : Attribute
{
    public Scenario(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
}

public class Story : Attribute
{
    public string Title { get; set; }
    public string InOrderTo { get; set; }
    public string AsA { get; set; }
    public string IWantTo { get; set; }
}

public class Given : Attribute
{
    public Given(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}

public class When : Attribute
{
    public When(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}

public class Then : Attribute
{
    public Then(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}

[AttributeUsage(
    AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true)]
public class And : Attribute
{
    public And(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
}