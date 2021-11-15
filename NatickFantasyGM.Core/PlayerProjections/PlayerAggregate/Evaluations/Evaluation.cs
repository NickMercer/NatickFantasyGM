using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.Enums;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Statistics;

namespace NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;

public class Evaluation : BaseEntity<Guid>
{
    public int PlayerId { get; }

    private List<Stat> _stats = new List<Stat>();

    public IEnumerable<Stat> Stats => _stats.AsReadOnly();
    public IEnumerable<SimpleStat> SimpleStats => _stats.OfType<SimpleStat>().ToList().AsReadOnly();
    public IEnumerable<Ratio> Ratios => _stats.OfType<Ratio>().ToList().AsReadOnly();


    private List<ProjectionSourceEnum> _includedProjectionSources = new List<ProjectionSourceEnum>();
    public IEnumerable<ProjectionSourceEnum> IncludedProjectionSources => _includedProjectionSources.AsReadOnly();

    private Dictionary<ProjectionSourceEnum, double> _weights = new Dictionary<ProjectionSourceEnum, double>();
    public IReadOnlyDictionary<ProjectionSourceEnum, double> Weights => _weights;

    private List<ThirdPartyProjection> _thirdPartyProjections = new List<ThirdPartyProjection>();

    public Evaluation(Guid id, int playerId, Dictionary<ProjectionSourceEnum, double> includedProjectionWeights, List<ThirdPartyProjection> allThirdPartyProjections)
    {
        Id = Guard.Against.Default(id, nameof(Id));
        PlayerId = Guard.Against.NegativeOrZero(playerId, nameof(PlayerId));
        
        _weights = Guard.Against.NullOrEmpty(includedProjectionWeights, nameof(_weights)).ToDictionary(x => x.Key, x => x.Value);
        Guard.Against.ProjectionWeightInvalidSum(_weights.Values, nameof(_weights));

        _thirdPartyProjections = Guard.Against.NullOrEmpty(allThirdPartyProjections, nameof(_thirdPartyProjections)).ToList();
        Guard.Against.InvalidThirdPartyProjectionCount(_thirdPartyProjections.Count, nameof(_thirdPartyProjections));
    }

    //Owner projections are optional.
    //Owner projections have a lot of unique settings and composition (Probably need to be their own entity rather than just a regular projection.
}
