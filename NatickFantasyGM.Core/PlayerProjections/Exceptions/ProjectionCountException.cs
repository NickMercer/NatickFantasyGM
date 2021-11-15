namespace NatickFantasyGM.Core.PlayerProjections.Exceptions;

internal class ProjectionCountException : ArgumentException
{
    public ProjectionCountException(string message, string paramName) : base(message, paramName)
    {
    }
}
