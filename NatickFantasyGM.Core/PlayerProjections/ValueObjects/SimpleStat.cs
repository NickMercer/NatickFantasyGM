namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class SimpleStat : Stat
{
    public override double Value { get; }

    public SimpleStat(StatName identifier, StatType type, double value)
    {
        StatName = identifier;
        Type = type;
        Value = value;
    }

    public static SimpleStat AddValue(SimpleStat stat, double value)
    {
        return new SimpleStat(stat.StatName, stat.Type, stat.Value + value);
    }

    public SimpleStat NewValue(double value)
    {
        return new SimpleStat(StatName, Type, value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StatName;
        yield return Type;
        yield return Value;
    }
}
