using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.Exceptions;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using System.Linq;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;

public class Evaluation : BaseEntity<Guid>
{
    public int PlayerId { get; }
    public List<WeightedProjection> ProjectionWeights { get; private set; }

    public Projection Projection { get; private set; }

    public bool IsMissingProjectionSources { get; private set; }

    private List<ProjectionSource> _missingSources = new List<ProjectionSource>();
    public IReadOnlyList<ProjectionSource> MissingSources { get { return _missingSources.AsReadOnly(); } }

    private List<ThirdPartyProjection> _allThirdPartyProjections = new List<ThirdPartyProjection>();

    public Evaluation(Guid id, int playerId, List<WeightedProjection> weightedProjections, List<ThirdPartyProjection> allThirdPartyProjections)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        PlayerId = Guard.Against.NegativeOrZero(playerId, nameof(PlayerId));
        ProjectionWeights = weightedProjections;

        _allThirdPartyProjections = Guard.Against.NullOrEmpty(allThirdPartyProjections, nameof(_allThirdPartyProjections)).ToList();

        GenerateProjection();
    }

    private void GenerateProjection()
    {
        if (ProjectionWeights.Sum(weights => weights.Weight) != 100)
            throw new ProjectionWeightException("The projection weights for a player must equal 100.", nameof(ProjectionWeights));

        var projection = _allThirdPartyProjections.First();
        var includedWeightedProjections = ProjectionWeights.Where(w => _allThirdPartyProjections.Select(x => x.Id).Contains(w.ProjectionId.GetValueOrDefault()));

        MarkMissingProjectionSources(ProjectionWeights, includedWeightedProjections);

        var stats = InitializeProjectionSimpleStats(projection);
        GenerateSimpleStats(stats, includedWeightedProjections);
        GenerateRatios(projection, stats);

        var simpleStats = stats.Select(stat => Stat.Simplify(stat)).ToList();

        Projection = new Projection(simpleStats);
    }

    private void MarkMissingProjectionSources(List<WeightedProjection> projectionWeights, IEnumerable<WeightedProjection> includedWeightedProjections)
    {
        _missingSources.Clear();

        var missingProjections = projectionWeights.Where(w => !includedWeightedProjections.Contains(w));

        if (missingProjections.Count() == 0)
        {
            IsMissingProjectionSources = false;
            return;
        }

        IsMissingProjectionSources = true;
        _missingSources.AddRange(missingProjections.Select(projection => projection.ProjectionSource));
    }

    private static List<Stat> InitializeProjectionSimpleStats(ThirdPartyProjection projection)
    {
        var stats = new List<Stat>();

        foreach (var projStat in projection.SimpleStats)
        {
            stats.Add(new SimpleStat(projStat.StatName, projStat.Type, 0));
        }

        return stats;
    }
    private void GenerateSimpleStats(List<Stat> stats, IEnumerable<WeightedProjection> includedWeightedProjections)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            var stat = stats[i];

            double valueSum = 0;
            double includedWeightTotal = 0;
            foreach (var weightedProjection in includedWeightedProjections)
            {
                var projectionHasStat = _allThirdPartyProjections
                    .First(t => t.Id == weightedProjection.ProjectionId.GetValueOrDefault())
                    .TryGetStat(stat.StatName, out Stat projectionStat);

                if (projectionHasStat)
                {
                    var projectionValue = projectionStat.Value;

                    valueSum += projectionValue * weightedProjection.Weight;
                    includedWeightTotal += weightedProjection.Weight;
                }
            }

            var averageValue = valueSum / includedWeightTotal;
            stats[i] = new SimpleStat(stat.StatName, stat.Type, averageValue);
        }
    }
    private static void GenerateRatios(ThirdPartyProjection projection, List<Stat> stats)
    {
        foreach (var ratio in projection.Ratios)
        {
            stats.Add(new Ratio(ratio.StatName, ratio.Type, ratio.Formula, stats.Where(x => x.Type == ratio.Type)));
        }
    }
    

    //Owner projections are optional.
    //Owner projections have a lot of unique settings and composition (Probably need to be their own entity rather than just a regular projection.
}
