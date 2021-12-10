namespace NatickFantasyGM.Infrastructure.PlayerProjections.DAOs;

internal class PitchingStatsDAO
{
    public int StatProviderId { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

    public int GamesStarted { get; set; }

    public int Games { get; set; }

    public int Saves { get; set; }

    public double InningsPitched { get; set; }

    public int HitsAllowed { get; set; }

    public int EarnedRuns { get; set; }

    public int HomeRunsAllowed { get; set; }

    public int StrikeOuts { get; set; }

    public int Walks { get; set; }

    public double WAR { get; set; }

}
