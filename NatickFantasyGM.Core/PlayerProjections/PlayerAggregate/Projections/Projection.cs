using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;

public class Projection : BaseEntity<Guid>
{
    public int ProjectionSourceId { get; }

    private List<Stat> _stats = new List<Stat>();
    public IEnumerable<Stat> Stats => _stats.AsReadOnly();

    public IEnumerable<SimpleStat> SimpleStats => _stats.OfType<SimpleStat>().ToList().AsReadOnly();

    public IEnumerable<Ratio> Ratios => _stats.OfType<Ratio>().ToList().AsReadOnly();

    public Projection(Guid id, int sourceId, List<KeyValuePair<StatIdentifier, double>> simpleStats, List<KeyValuePair<StatIdentifier, string>> ratios)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        ProjectionSourceId = Guard.Against.NegativeOrZero(sourceId, nameof(ProjectionSourceId));
        _stats = BuildStats(simpleStats, ratios);
    }

    private List<Stat> BuildStats(List<KeyValuePair<StatIdentifier, double>> simpleStats, List<KeyValuePair<StatIdentifier, string>> ratios)
    {
        Guard.Against.NullOrEmpty(simpleStats, nameof(simpleStats));
        Guard.Against.Null(ratios, nameof(ratios));

        var stats = new List<Stat>();

        foreach (var simpleStat in simpleStats)
        {
            stats.Add(new SimpleStat(simpleStat.Key, simpleStat.Value));
        }

        foreach (var ratio in ratios)
        {
            stats.Add(new Ratio(ratio.Key, ratio.Value, stats));
        }

        return stats;
    }

    public void UpdateSimpleStat(SimpleStat stat)
    {
        var currentStat = _stats.FirstOrDefault(s => s.StatIdentifier == stat.StatIdentifier && s is SimpleStat);

        if(currentStat != null)
        {
            _stats.Replace(currentStat, stat);
        }
    }
}
