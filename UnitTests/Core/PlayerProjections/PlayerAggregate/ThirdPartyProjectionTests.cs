using NatickFantasyGM.Core.PlayerProjections;
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
        void Action() => new ThirdPartyProjection(default(Guid), ProjectionSource.ZiPS, new List<Tuple<StatName, StatType, double>>(), new List<Tuple<StatName, StatType, string>>());

        Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidSimpleStats_ThrowsException()
    {
        void Action() => new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, null, new List<Tuple<StatName, StatType, string>>());

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_InvalidRatios_ThrowsException()
    {
        var simpleStats = new List<Tuple<StatName, StatType, double>>
        {
            new Tuple<StatName, StatType, double>(new StatName("Test", "T"), StatType.Offensive, 1)
        };

        void Action() => new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, null);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void UpdateStat_StatExists_StatsUpdated()
    {
        var simpleStats = new List<Tuple<StatName, StatType, double>>
        {
            new Tuple<StatName, StatType, double>(new StatName("Test", "T"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test2", "T2"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test3", "T3"), StatType.Offensive, 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, new List<Tuple<StatName, StatType, string>>());
        var statToUpdate = new SimpleStat(new StatName("Test3", "T3"), StatType.Offensive, 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatName == new StatName("Test3", "T3"));

        Assert.Equal(12, newStat?.Value);
    }

    [Fact]
    public void UpdateStat_StatDoesNotExist_NoUpdate()
    {
        var simpleStats = new List<Tuple<StatName, StatType, double>>
        {
            new Tuple<StatName, StatType, double>(new StatName("Test", "T"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test2", "T2"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test3", "T3"), StatType.Offensive, 1),
        };
        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, new List<Tuple<StatName, StatType, string>>());
        var statToUpdate = new SimpleStat(new StatName("Test4", "T4"), StatType.Offensive, 12);

        projection.UpdateSimpleStat(statToUpdate);
        var newStat = projection.SimpleStats.FirstOrDefault(s => s.StatName == new StatName("Test4", "T4"));

        Assert.Null(newStat);
    }

    [Fact]
    public void UpdateStat_StatIsRatioIngredient_RatioUpdated()
    {
        var simpleStats = new List<Tuple<StatName, StatType, double>>
        {
            new Tuple<StatName, StatType, double>(new StatName("Test", "T"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test2", "T2"), StatType.Offensive, 1),
            new Tuple<StatName, StatType, double>(new StatName("Test3", "T3"), StatType.Offensive, 1),
        };

        var ratioStats = new List<Tuple<StatName, StatType, string>>
        {
            new Tuple<StatName, StatType, string>(new StatName("TestRatio", "TR"), StatType.Offensive, "T * T2")
        };

        var projection = new ThirdPartyProjection(Guid.NewGuid(), ProjectionSource.ZiPS, simpleStats, ratioStats);

        Assert.Equal(1, projection.Ratios.FirstOrDefault(r => r.StatName == new StatName("TestRatio", "TR"))?.Value);

        var statToUpdate = new SimpleStat(new StatName("Test2", "T2"), StatType.Offensive, 12);

        projection.UpdateSimpleStat(statToUpdate);
        var updatedRatio = projection.Ratios.FirstOrDefault(r => r.StatName == new StatName("TestRatio", "TR"));

        Assert.Equal(12, updatedRatio?.Value);
    }
}
