using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;

namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class WeightedProjection : ValueObject<WeightedProjection>
{
    public Maybe<Guid> ProjectionId { get; }

    public ProjectionSource ProjectionSource { get; }

    public double Weight { get; }

    public WeightedProjection(ProjectionSource source, double weight, Guid projectionId = default(Guid))
    {
        ProjectionId = new Maybe<Guid>(projectionId);
        ProjectionSource = source;
        Weight = Guard.Against.OutOfRange(weight, nameof(Weight), 0, 100);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProjectionId;
        yield return ProjectionSource;
        yield return Weight;
    }
}
