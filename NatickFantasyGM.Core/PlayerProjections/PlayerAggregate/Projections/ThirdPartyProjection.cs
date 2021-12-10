using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;

public class ThirdPartyProjection : BaseEntity<Guid>
{
    public ProjectionSource ProjectionSource { get; }

    private List<Stat> _stats = new List<Stat>();

    public IEnumerable<SimpleStat> SimpleStats => _stats.OfType<SimpleStat>().ToList().AsReadOnly();
    public IEnumerable<Ratio> Ratios => _stats.OfType<Ratio>().ToList().AsReadOnly();

    public ThirdPartyProjection(Guid id, ProjectionSource source, List<Tuple<StatName, StatType, double>> simpleStats, List<Tuple<StatName, StatType, string>> ratios)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        ProjectionSource = Guard.Against.Default(source, nameof(ProjectionSource));
        _stats = BuildStats(simpleStats, ratios);
    }

    private List<Stat> BuildStats(List<Tuple<StatName, StatType, double>> simpleStats, List<Tuple<StatName, StatType, string>> ratios)
    {
        Guard.Against.NullOrEmpty(simpleStats, nameof(simpleStats));
        Guard.Against.Null(ratios, nameof(ratios));

        var stats = new List<Stat>();

        foreach (var simpleStat in simpleStats)
        {
            stats.Add(new SimpleStat(simpleStat.Item1, simpleStat.Item2, simpleStat.Item3));
        }

        foreach (var ratio in ratios)
        {
            stats.Add(new Ratio(ratio.Item1, ratio.Item2, ratio.Item3, stats.Where(x => x.Type == ratio.Item2)));
        }

        return stats;
    }

    public void UpdateSimpleStat(SimpleStat stat)
    {
        var currentStat = _stats.FirstOrDefault(s => s.StatName == stat.StatName && s is SimpleStat);

        if(currentStat != null)
        {
            _stats.Replace(currentStat, stat);
        }
    }

    public Stat GetStat(StatName statName)
    {
        return _stats.First(x => x.StatName == statName);
    }
}
