namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class SimpleStat : Stat
{
    public override double Value { get; }

    public SimpleStat(StatName identifier, double value)
    {
        StatName = identifier;
        Value = value;
    }

    public static SimpleStat AddValue(SimpleStat stat, double value)
    {
        return new SimpleStat(stat.StatName, stat.Value + value);
    }

    public SimpleStat NewValue(double value)
    {
        return new SimpleStat(StatName, value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StatName;
        yield return Value;
    }
}
