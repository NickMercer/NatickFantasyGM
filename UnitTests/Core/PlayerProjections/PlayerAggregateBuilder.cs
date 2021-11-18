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
                new List<KeyValuePair<StatName, double>>
                {
                    new KeyValuePair<StatName, double>(new StatName("Hits", "H"), 10),
                    new KeyValuePair<StatName, double>(new StatName("At Bats", "AB"), 100),
                    new KeyValuePair<StatName, double>(new StatName("Home Runs", "HR"), 3)
                },
                new List<KeyValuePair<StatName, string>>
                {
                    new KeyValuePair<StatName, string>(new StatName("Batting Average", "AVG"), "H / AB"),
                    new KeyValuePair<StatName, string>(new StatName("Home Run Percentage", "HRP"), "HR / H")
                }
            ),
            new ThirdPartyProjection(TestSteamerGuid,
                ProjectionSource.Steamer,
                new List<KeyValuePair<StatName, double>>
                {
                    new KeyValuePair<StatName, double>(new StatName("Hits", "H"), 40),
                    new KeyValuePair<StatName, double>(new StatName("At Bats", "AB"), 110),
                    new KeyValuePair<StatName, double>(new StatName("Home Runs", "HR"), 6)
                },
                new List<KeyValuePair<StatName, string>>
                {
                    new KeyValuePair<StatName, string>(new StatName("Batting Average", "AVG"), "H / AB"),
                    new KeyValuePair<StatName, string>(new StatName("Home Run Percentage", "HRP"), "HR / H")
                }
            ),
        };

        return projections;
    }
}
