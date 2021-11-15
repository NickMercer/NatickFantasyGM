using Ardalis.GuardClauses;
using NatickFantasyGM.Core.PlayerProjections.Services.StatFormulas;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

public class Ratio : Stat
{
    private string _formula;
    private IEnumerable<Stat> _statCollection;

    public override double Value 
    { 
        get
        {
            return new FormulaParser().Calculate(StatIdentifier.Abbreviation, _formula, _statCollection);
        }
    }

    public Ratio(StatIdentifier identifier, string formula, IEnumerable<Stat> statCollection)
    {
        StatIdentifier = identifier;

        _formula = Guard.Against.NullOrWhiteSpace(formula, nameof(formula));
        _statCollection = Guard.Against.NullOrEmpty(statCollection, nameof(statCollection));
    }
}
