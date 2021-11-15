using Ardalis.GuardClauses;
using NatickFantasyGM.Core.PlayerProjections.Enums;
using NatickFantasyGM.Core.PlayerProjections.Exceptions;

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
        var thirdPartyProjections = ProjectionSourceEnum.List.Except(new List<ProjectionSourceEnum> { ProjectionSourceEnum.Owner });
        if (projectionCount != thirdPartyProjections.Count())
        {
            throw new ProjectionCountException($"Projection has an invalid amount of third party projections. Expected: {thirdPartyProjections.Count()}, Actual: {projectionCount}.", parameterName);
        }
    }
}
