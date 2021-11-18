using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Core.PlayerProjections.ValueObjects;

public class RatioTests
{
    [Theory]
    [InlineData(12, "24")]
    [InlineData(-1, "-2")]
    [InlineData(0, "0")]
    public void ToValueString_IntegerValue_ReturnsInteger(double value, string valueString)
    {
        var stat = new Ratio(new StatName("Test", "T"), "A * 2", new List<Stat> 
        {
            new SimpleStat(new StatName("Aaaa", "A"), value)
        });

        var result = stat.ToValueString();

        Assert.Equal(valueString, result);
    }

    [Theory]
    [InlineData(12.3, "4.100")]
    [InlineData(1, ".333")]
    [InlineData(0.5, ".167")]
    [InlineData(-1.234567, "-.412")]
    public void ToValueString_DecimalValue_ReturnsThreeDigitDecimal(double value, string valueString)
    {
        var stat = new Ratio(new StatName("Test", "T"), "A / 3", new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), value)
        });

        var result = stat.ToValueString();

        Assert.Equal(valueString, result);
    }

    [Fact]
    public void ToValueString_ValueNaN_ReturnsNaNString()
    {
        var stats = new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 12)
        };
        var ratio = new Ratio(new StatName("Test", "T"), "1 / 0", stats);
        stats.Add(ratio);

        Assert.Equal(double.NaN, ratio.Value);
        Assert.Equal("NaN", ratio.ToValueString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidName_ThrowsArgumentException(string name)
    {
        void Action() => new Ratio(new StatName(name, "T"), "A / 3", new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 1)
        });

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        void Action() => new Ratio(new StatName("Test", abbreviation), "A / 3", new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 1)
        });

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_InvalidFormula_ThrowsArgumentException(string formula)
    {
        void Action() => new Ratio(new StatName("Test", "T"), formula, new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 1)
        });

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_NullStatCollection_ThrowsArgumentException()
    {
        void Action() => new Ratio(new StatName("Test", "T"), "A / 3", null);

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Constructor_EmptyStatCollection_ThrowsArgumentException()
    {
        void Action() => new Ratio(new StatName("Test", "T"), "A / 3", new List<Stat>());

        Assert.ThrowsAny<ArgumentException>(Action);
    }

    [Fact]
    public void Value_FormulaBasedOnSimpleStats_ReturnsCorrectValue()
    {
        var ratio = new Ratio(new StatName("Test", "T"), "(A / B) + C", new List<Stat>
        {
            new SimpleStat(new StatName("Aaa", "A"), 12),
            new SimpleStat(new StatName("Bbb", "B"), 3),
            new SimpleStat(new StatName("Ccc", "C"), 0.5)
        });

        Assert.Equal(4.5, ratio.Value);
    }

    [Fact]
    public void Value_FormulaBasedOnRatioStats_ReturnsCorrectValue()
    {
        var stats = new List<Stat>
        {
            new SimpleStat(new StatName("Aaa", "A"), 12),
            new SimpleStat(new StatName("Bbb", "B"), 3),
            new SimpleStat(new StatName("Ccc", "C"), 0.5)
        };
        stats.Add(new Ratio(new StatName("Ddd", "D"), "(A / B) + C", stats));
        var ratio = new Ratio(new StatName("Test", "T"), "D + 5", stats);
        stats.Add(ratio);

        Assert.Equal(9.5, ratio.Value);
    }

    [Fact]
    public void Value_FormulaWithLoop_ThrowsInvalidOperationException()
    {
        var stats = new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 12)
        };
        var ratio = new Ratio(new StatName("Test", "T"), "T + 5", stats);
        stats.Add(ratio);

        Assert.Throws<InvalidOperationException>(() => ratio.Value);
    }

    [Fact]
    public void Value_DividingByZero_ReturnsDoubleNaN()
    {
        var stats = new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 12)
        };
        var ratio = new Ratio(new StatName("Test", "T"), "1 / 0", stats);
        stats.Add(ratio);

        Assert.Equal(double.NaN, ratio.Value);
    }

    [Fact]
    public void Value_MissingRequiredStats_ReturnsDoubleNaN()
    {
        var stats = new List<Stat>
        {
            new SimpleStat(new StatName("Aaaa", "A"), 12)
        };
        var ratio = new Ratio(new StatName("Test", "T"), "A / B", stats);
        stats.Add(ratio);

        Assert.Equal(double.NaN, ratio.Value);
    }
}
