using Natick.SharedKernel;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

public abstract class Stat : ValueObject<Stat>
{
    public StatIdentifier StatIdentifier { get; protected set; }

    public abstract double Value { get; }

    public override string ToString()
    {
        return $"{StatIdentifier.Abbreviation} - {ToValueString()}";
    }

    public virtual string ToValueString()
    {
        var valueString = Value % 1 == 0
            ? Value.ToString()
            : Value.ToString("#.000");

        return valueString;
    }
}
