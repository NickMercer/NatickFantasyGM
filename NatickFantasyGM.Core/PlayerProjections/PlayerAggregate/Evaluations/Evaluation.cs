using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.Exceptions;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;

public class Evaluation : BaseEntity<Guid>
{
    public int PlayerId { get; }
    public List<WeightedProjection> ProjectionWeights { get; private set; }

    public Projection Projection { get; private set; }

    private List<ThirdPartyProjection> _allThirdPartyProjections = new List<ThirdPartyProjection>();

    public Evaluation(Guid id, int playerId, List<WeightedProjection> weightedProjections, List<ThirdPartyProjection> allThirdPartyProjections)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        PlayerId = Guard.Against.NegativeOrZero(playerId, nameof(PlayerId));
        ProjectionWeights = weightedProjections;

        _allThirdPartyProjections = Guard.Against.NullOrEmpty(allThirdPartyProjections, nameof(_allThirdPartyProjections)).ToList();
        Guard.Against.InvalidThirdPartyProjectionCount(_allThirdPartyProjections.Count, nameof(_allThirdPartyProjections));

        GenerateProjection();
    }

    private void GenerateProjection()
    {
        if(ProjectionWeights.Sum(weights => weights.Weight) != 100)
            throw new ProjectionWeightException("The projection weights for a player must equal 100.", nameof(ProjectionWeights));

        //Sum up all the simple stats and average them.
        var projection = _allThirdPartyProjections.First();
        var stats = InitializeProjectionSimpleStats(projection);
        
        for (int i = 0; i < stats.Count; i++)
        {
            var stat = stats[i];

            double valueSum = 0;
            foreach (var weightedProjection in ProjectionWeights)
            {
                var projectionValue = _allThirdPartyProjections
                    .First(t => t.Id == weightedProjection.ProjectionId)
                    .GetStat(stat.StatName)
                    .Value;
                
                valueSum += projectionValue * weightedProjection.Weight;
            }

            var averageValue = valueSum / 100;
            stats[i] = new SimpleStat(stat.StatName, averageValue); 
        }

        //Then calculate all the ratios and turn them into simple stats.
        foreach (var ratio in projection.Ratios)
        {
            stats.Add(new Ratio(ratio.StatName, ratio.Formula, stats));
        }

        var simpleStats = stats.Select(stat => Stat.Simplify(stat)).ToList();

        Projection = new Projection(simpleStats);
    }

    private static List<Stat> InitializeProjectionSimpleStats(ThirdPartyProjection projection)
    {
        var stats = new List<Stat>();

        foreach (var projStat in projection.SimpleStats)
        {
            stats.Add(new SimpleStat(projStat.StatName, 0));
        }

        return stats;
    }

    //Owner projections are optional.
    //Owner projections have a lot of unique settings and composition (Probably need to be their own entity rather than just a regular projection.
}
