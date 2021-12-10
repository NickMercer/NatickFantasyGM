using NatickFantasyGM.Core.PlayerProjections;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System;
using System.Collections.Generic;

namespace UnitTests.Core.PlayerProjections;

internal class PlayerAggregateBuilder
{
    public static int TestPlayerId = 1;
    public static Guid TestEvalGuid => new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

    public static Guid TestZiPSGuid => new Guid(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);
    public static Guid TestSteamerGuid => new Guid(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3);

    public static Evaluation GetValidEvaluation(double zipsWeight, double steamerWeight)
    {
        var allThirdPartyProjections = GetValidThirdPartyProjections(); 
        var weightedProjections = GetValidWeightedProjections(zipsWeight, steamerWeight);

        return new Evaluation(TestEvalGuid, TestPlayerId, weightedProjections, allThirdPartyProjections);
    }

    public static List<WeightedProjection> GetValidWeightedProjections(double zipsWeight, double steamerWeight)
    {
        var weightedProjections = new List<WeightedProjection>
        {
            new WeightedProjection(TestZiPSGuid, zipsWeight),
            new WeightedProjection(TestSteamerGuid, steamerWeight)
        };

        return weightedProjections;
    }

    public static List<ThirdPartyProjection> GetValidThirdPartyProjections()
    {
        var projections = new List<ThirdPartyProjection>
        {
            new ThirdPartyProjection(TestZiPSGuid,
                ProjectionSource.ZiPS,
                new List<Tuple<StatName, StatType, double>>
                {
                    new Tuple<StatName, StatType, double>(new StatName("Hits", "H"), StatType.Offensive, 10),
                    new Tuple<StatName, StatType, double>(new StatName("At Bats", "AB"), StatType.Offensive, 100),
                    new Tuple<StatName, StatType, double>(new StatName("Home Runs", "HR"), StatType.Offensive, 3)
                },
                new List<Tuple<StatName, StatType, string>>
                {
                    new Tuple<StatName, StatType, string>(new StatName("Batting Average", "AVG"), StatType.Offensive, "H / AB"),
                    new Tuple<StatName, StatType, string>(new StatName("Home Run Percentage", "HRP"), StatType.Offensive, "HR / H")
                }
            ),
            new ThirdPartyProjection(TestSteamerGuid,
                ProjectionSource.Steamer,
                new List<Tuple<StatName, StatType, double>>
                {
                    new Tuple<StatName, StatType, double>(new StatName("Hits", "H"), StatType.Offensive, 40),
                    new Tuple<StatName, StatType, double>(new StatName("At Bats", "AB"), StatType.Offensive, 110),
                    new Tuple<StatName, StatType, double>(new StatName("Home Runs", "HR"), StatType.Offensive, 6)
                },
                new List<Tuple<StatName, StatType, string>>
                {
                    new Tuple<StatName, StatType, string>(new StatName("Batting Average", "AVG"), StatType.Offensive, "H / AB"),
                    new Tuple<StatName, StatType, string>(new StatName("Home Run Percentage", "HRP"), StatType.Offensive, "HR / H")
                }
            ),
        };

        return projections;
    }
}
