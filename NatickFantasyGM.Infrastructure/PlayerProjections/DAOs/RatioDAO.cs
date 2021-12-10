using NatickFantasyGM.Core.PlayerProjections;

namespace NatickFantasyGM.Infrastructure.PlayerProjections.DAOs;

internal class RatioDAO
{
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public StatType StatType { get; set; }
    public string Formula { get; set; }
}
