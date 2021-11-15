using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Core.PlayerProjections.PlayerAggregate.Statistics;

public class SimpleStatTests
{
    [Theory]
    [InlineData(12, "12")]
    [InlineData(-1, "-1")]
    [InlineData(0, "0")]
    public void ToValueString_IntegerValue_ReturnsInteger(double value, string valueString)
    {
        var stat = new SimpleStat(new StatIdentifier("Test", "T"), value);

        var result = stat.ToValueString();

        Assert.Equal(valueString, result);
    }

    [Theory]
    [InlineData(12.3, "12.300")]
    [InlineData(0.333, ".333")]
    [InlineData(0.5, ".500")]
    [InlineData(-1.234567, "-1.235")]
    public void ToValueString_DecimalValue_ReturnsThreeDigitDecimal(double value, string valueString)
    {
        var stat = new SimpleStat(new StatIdentifier("Test", "T"), value);

        var result = stat.ToValueString();

        Assert.Equal(valueString, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidName_ThrowsArgumentException(string name)
    {
        void Action() => new SimpleStat(new StatIdentifier(name, "A"), 12);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        void Action() => new SimpleStat(new StatIdentifier("Test", abbreviation), 12);

        Assert.ThrowsAny<ArgumentException>(Action);
    }
}
