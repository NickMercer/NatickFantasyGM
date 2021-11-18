using Ardalis.GuardClauses;
using Natick.SharedKernel;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;

namespace NatickFantasyGM.Core.PlayerProjections.ValueObjects;

public class WeightedProjection : ValueObject<WeightedProjection>
{
    public Guid ProjectionId { get; }

    public double Weight { get; }

    public WeightedProjection(Guid projectionId, double weight)
    {
        ProjectionId = projectionId;
        Weight = Guard.Against.OutOfRange(weight, nameof(Weight), 0, 100);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProjectionId;
        yield return Weight;
    }
}
