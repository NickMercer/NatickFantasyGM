using Ardalis.GuardClauses;
using NatickFantasyGM.Core.PlayerProjections.Exceptions;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;

namespace Ardalis.GuardClauses;

public static class ProjectionGuardExtensions
{
    public static void ProjectionWeightInvalidSum(this IGuardClause guardClause, IEnumerable<double> weights, string parameterName)
    {
        if(weights.Sum() != 100)
        {
            throw new ProjectionWeightException("Projection weights do not sum to 100 percent", parameterName);
        }
    }

    public static void InvalidThirdPartyProjectionCount(this IGuardClause guardClause, int projectionCount, string parameterName)
    {
        var thirdPartyProjections = ProjectionSource.ThirdPartySources;
        if (projectionCount != thirdPartyProjections.Count())
        {
            throw new ProjectionCountException($"Projection has an invalid amount of third party projections. Expected: {thirdPartyProjections.Count()}, Actual: {projectionCount}.", parameterName);
        }
    }
}
