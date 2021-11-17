﻿namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

public class SimpleStat : Stat
{
    public override double Value { get; }

    public SimpleStat(StatIdentifier identifier, double value)
    {
        StatIdentifier = identifier;
        Value = value;
    }

    public SimpleStat NewValue(double value)
    {
        return new SimpleStat(StatIdentifier, value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StatIdentifier;
        yield return Value;
    }
}
