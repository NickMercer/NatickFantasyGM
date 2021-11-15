namespace NatickFantasyGM.Core.PlayerProjections.Exceptions;

public class ProjectionWeightException : ArgumentException
{
    public ProjectionWeightException(string message, string paramName) : base(message, paramName)
    {
    }
}
