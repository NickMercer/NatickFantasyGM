using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;

public class Projection : BaseEntity<Guid>
{
    public int ProjectionSourceId { get; }

    private List<Stat> _stats = new List<Stat>();
    public IEnumerable<Stat> Stats => _stats.AsReadOnly();

    public Projection(Guid id, int sourceId, List<KeyValuePair<StatIdentifier, double>> simpleStats, List<KeyValuePair<StatIdentifier, string>> ratios)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        ProjectionSourceId = Guard.Against.NegativeOrZero(sourceId, nameof(ProjectionSourceId));
        _stats = BuildStats(simpleStats, ratios);
    }

    private List<Stat> BuildStats(List<KeyValuePair<StatIdentifier, double>> simpleStats, List<KeyValuePair<StatIdentifier, string>> ratios)
    {
        Guard.Against.NullOrEmpty(simpleStats, nameof(simpleStats));

        var stats = new List<Stat>();

        foreach (var simpleStat in simpleStats)
        {
            stats.Add(new SimpleStat(simpleStat.Key, simpleStat.Value));
        }

        foreach (var ratio in ratios)
        {
            stats.Add(new Ratio(ratio.Key, ratio.Value, _stats));
        }

        return stats;
    }
}
