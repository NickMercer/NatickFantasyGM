using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Core.PlayerProjections.PlayerAggregate;

public class ThirdPartyProjectionTests
{
    [Fact]
    public void Constructor_InvalidId_ThrowsException()
    {
        void Action() => new ThirdPartyProjection(default(Guid), ProjectionSource.ZiPS, new List<KeyValuePair<StatName, double>>(), new List<KeyValuePair<StatName, string>>());

        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidSimpleStats_ThrowsException()
    {
        void Action() => new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, null, new List<KeyValuePair<StatName, string>>());

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidRatios_ThrowsException()
    {
        var simpleStats = new List<KeyValuePair<StatName, double>>
        {
            new KeyValuePair<StatName, double>(new StatName("Test", "T"), 1)
        };

        void Action() => new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, null);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void UpdateStat_StatExists_StatsUpdated()
    {
        var simpleStats = new List<KeyValuePair<StatName, double>>
        {
            new KeyValuePair<StatName, double>(new StatName("Test", "T"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test2", "T2"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test3", "T3"), 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, new List<KeyValuePair<StatName, string>>());
        var statToUpdate = new SimpleStat(new StatName("Test3", "T3"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatName == new StatName("Test3", "T3"));

        Assert.Equal(12, newStat?.Value);
    }

    [Fact]
    public void UpdateStat_StatDoesNotExist_NoUpdate()
    {
        var simpleStats = new List<KeyValuePair<StatName, double>>
        {
            new KeyValuePair<StatName, double>(new StatName("Test", "T"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test2", "T2"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test3", "T3"), 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, new List<KeyValuePair<StatName, string>>());
        var statToUpdate = new SimpleStat(new StatName("Test4", "T4"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatName == new StatName("Test4", "T4"));

        Assert.Null(newStat);
    }

    [Fact]
    public void UpdateStat_StatIsRatioIngredient_RatioUpdated()
    {
        var simpleStats = new List<KeyValuePair<StatName, double>>
        {
            new KeyValuePair<StatName, double>(new StatName("Test", "T"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test2", "T2"), 1),
            new KeyValuePair<StatName, double>(new StatName("Test3", "T3"), 1),
        };

        var ratioStats = new List<KeyValuePair<StatName, string>>
        {
            new KeyValuePair<StatName, string>(new StatName("TestRatio", "TR"), "T * T2")
        };

        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, ratioStats);

        Assert.Equal(1, projection.Ratios.FirstOrDefault(r => r.StatName == new StatName("TestRatio", "TR"))?.Value);

        var statToUpdate = new SimpleStat(new StatName("Test2", "T2"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var updatedRatio = projection.Ratios.FirstOrDefault(r => r.StatName == new StatName("TestRatio", "TR"));

        Assert.Equal(12, updatedRatio?.Value);
    }
}
