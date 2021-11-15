using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;
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
        void Action() => new ThirdPartyProjection(default(Guid), 1, new List<KeyValuePair<StatIdentifier, double>>(), new List<KeyValuePair<StatIdentifier, string>>());

        Assert.Throws<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidSourceId_ThrowsException(int sourceId)
    {
        void Action() => new ThirdPartyProjection(Guid.NewGuid(), sourceId, new List<KeyValuePair<StatIdentifier, double>>(), new List<KeyValuePair<StatIdentifier, string>>());

        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidSimpleStats_ThrowsException()
    {
        void Action() => new ThirdPartyProjection(Guid.NewGuid(), 1, null, new List<KeyValuePair<StatIdentifier, string>>());

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidRatios_ThrowsException()
    {
        var simpleStats = new List<KeyValuePair<StatIdentifier, double>>
        {
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test", "T"), 1)
        };

        void Action() => new ThirdPartyProjection(Guid.NewGuid(), 1, simpleStats, null);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void UpdateStat_StatExists_StatsUpdated()
    {
        var simpleStats = new List<KeyValuePair<StatIdentifier, double>>
        {
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test", "T"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test2", "T2"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test3", "T3"), 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), 1, simpleStats, new List<KeyValuePair<StatIdentifier, string>>());
        var statToUpdate = new SimpleStat(new StatIdentifier("Test3", "T3"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatIdentifier == new StatIdentifier("Test3", "T3"));

        Assert.Equal(12, newStat?.Value);
    }

    [Fact]
    public void UpdateStat_StatDoesNotExist_NoUpdate()
    {
        var simpleStats = new List<KeyValuePair<StatIdentifier, double>>
        {
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test", "T"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test2", "T2"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test3", "T3"), 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), 1, simpleStats, new List<KeyValuePair<StatIdentifier, string>>());
        var statToUpdate = new SimpleStat(new StatIdentifier("Test4", "T4"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatIdentifier == new StatIdentifier("Test4", "T4"));

        Assert.Null(newStat);
    }

    [Fact]
    public void UpdateStat_StatIsRatioIngredient_RatioUpdated()
    {
        var simpleStats = new List<KeyValuePair<StatIdentifier, double>>
        {
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test", "T"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test2", "T2"), 1),
            new KeyValuePair<StatIdentifier, double>(new StatIdentifier("Test3", "T3"), 1),
        };

        var ratioStats = new List<KeyValuePair<StatIdentifier, string>>
        {
            new KeyValuePair<StatIdentifier, string>(new StatIdentifier("TestRatio", "TR"), "T * T2")
        };

        var projection = new ThirdPartyProjection(Guid.NewGuid(), 1, simpleStats, ratioStats);

        Assert.Equal(1, projection.Ratios.FirstOrDefault(r => r.StatIdentifier == new StatIdentifier("TestRatio", "TR"))?.Value);

        var statToUpdate = new SimpleStat(new StatIdentifier("Test2", "T2"), 12);

        projection.UpdateSimpleStat(statToUpdate);
        var updatedRatio = projection.Ratios.FirstOrDefault(r => r.StatIdentifier == new StatIdentifier("TestRatio", "TR"));

        Assert.Equal(12, updatedRatio?.Value);
    }
}
