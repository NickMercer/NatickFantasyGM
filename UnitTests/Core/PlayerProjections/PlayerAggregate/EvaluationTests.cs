using NatickFantasyGM.Core.PlayerProjections.Exceptions;
using System.Linq;
using Xunit;

namespace UnitTests.Core.PlayerProjections.PlayerAggregate;

public class EvaluationTests
{
    [Fact]
    public void Constructor_ValidProjectionsAndWeights_GeneratesCorrectProjection()
    {
        var evaluation = PlayerAggregateBuilder.GetValidEvaluation(50, 50);

        Assert.Equal("25", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "H").ToValueString());
        Assert.Equal("105", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "AB").ToValueString());
        Assert.Equal("4.500", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "HR").ToValueString());

        Assert.Equal(".238", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "AVG").ToValueString());
        Assert.Equal(".180", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "HRP").ToValueString());
    }

    [Fact]
    public void Constructor_ValidUnevenWeights_GeneratesCorrectProjection()
    {
        var evaluation = PlayerAggregateBuilder.GetValidEvaluation(75, 25);

        Assert.Equal("17.500", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "H").ToValueString());
        Assert.Equal("102.500", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "AB").ToValueString());
        Assert.Equal("3.750", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "HR").ToValueString());

        Assert.Equal(".171", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "AVG").ToValueString());
        Assert.Equal(".214", evaluation.Projection.Stats.FirstOrDefault(s => s.StatName.Abbreviation == "HRP").ToValueString());
    }

    [Fact]
    public void Constructor_InvalidWeights_ThrowsWeightException()
    {
        void Action() => PlayerAggregateBuilder.GetValidEvaluation(100, 1);

        Assert.Throws<ProjectionWeightException>(Action);
    }
}
