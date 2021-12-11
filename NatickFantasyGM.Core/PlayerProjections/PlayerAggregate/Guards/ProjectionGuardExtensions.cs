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
}
