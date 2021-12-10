using Ardalis.GuardClauses;
using NatickFantasyGM.Core.PlayerProjections.Services.StatFormulas;

namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class Ratio : Stat
{
    public string Formula { get; }

    private IEnumerable<Stat> _statCollection;

    public override double Value 
    { 
        get
        {
            return new FormulaParser().Calculate(StatName.Abbreviation, Formula, _statCollection);
        }
    }

    public static SimpleStat Simplify(Ratio stat)
    {
        return new SimpleStat(stat.StatName, stat.Type, stat.Value);
    }

    public Ratio(StatName identifier, StatType type, string formula, IEnumerable<Stat> statCollection)
    {
        StatName = identifier;
        Type = type;

        Formula = Guard.Against.NullOrWhiteSpace(formula, nameof(formula));
        _statCollection = Guard.Against.NullOrEmpty(statCollection, nameof(statCollection));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StatName;
        yield return Type;
        yield return Formula;
        yield return Value;
    }
}
