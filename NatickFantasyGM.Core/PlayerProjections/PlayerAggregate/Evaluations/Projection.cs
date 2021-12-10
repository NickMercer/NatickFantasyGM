using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;

public class Projection : ValueObject<Projection>
{
    public IReadOnlyCollection<SimpleStat> Stats { get; }

    public Projection(IEnumerable<SimpleStat> simpleStats, IEnumerable<Ratio> ratios = null)
    {
        var statList = Guard.Against.NullOrEmpty(simpleStats, nameof(Stats)).ToList();

        if(ratios != null)
        {
            statList.AddRange(ratios.Select(ratio => new SimpleStat(ratio.StatName, ratio.Type, ratio.Value)));
        }

        Stats = statList.AsReadOnly();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Stats;
    }
}
