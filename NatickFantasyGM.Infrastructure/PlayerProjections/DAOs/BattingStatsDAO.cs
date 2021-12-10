namespace NatickFantasyGM.Infrastructure.PlayerProjections.DAOs;

internal class BattingStatsDAO
{
    public int StatProviderId { get; set; }

    public int Games { get; set; }

    public int PlateAppearances { get; set; }

    public int AtBats { get; set; }

    public int Hits { get; set; }

    public int Doubles { get; set; }

    public int Triples { get; set; }

    public int HomeRuns { get; set; }

    public int Runs { get; set; }

    public int RBIs { get; set; }

    public int Walks { get; set; }

    public int StrikeOuts { get; set; }

    public int HitByPitches { get; set; }

    public int StolenBases { get; set; }

    public int CaughtStealing { get; set; }

    public int SacrificeFlies { get; set; }

    public int SacrificeBunts { get; set; }

    public double WAR { get; set; }
}