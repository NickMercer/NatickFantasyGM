using Natick.SharedKernel;

namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public abstract class Stat : ValueObject<Stat>
{
    public StatName StatName { get; protected set; }

    public StatType Type { get; protected set; }

    public abstract double Value { get; }

    public override string ToString()
    {
        return $"{StatName.Abbreviation} - {ToValueString()}";
    }

    public static SimpleStat Simplify(Stat stat)
    {
        return new SimpleStat(stat.StatName, stat.Type, stat.Value);
    }

    public virtual string ToValueString()
    {
        var valueString = Value % 1 == 0
            ? Value.ToString()
            : Value.ToString("#.000");

        return valueString;
    }
}
