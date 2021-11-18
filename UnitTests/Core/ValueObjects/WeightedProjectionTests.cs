using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Core.ValueObjects;

public class WeightedProjectionTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(100.000001)]
    [InlineData(-0.000001)]
    public void Constructor_WeightOutside100PercentRange_ThrowsArgumentException(double weight)
    {
        void Action() => new WeightedProjection(Guid.NewGuid(), weight);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(99)]
    [InlineData(99.999999)]
    [InlineData(0.000001)]
    public void Constructor_WeightInside100PercentRange_NoException(double weight)
    {
        var weightedProjection = new WeightedProjection(Guid.NewGuid(), weight);

        Assert.Equal(weight, weightedProjection.Weight);
    }
}
