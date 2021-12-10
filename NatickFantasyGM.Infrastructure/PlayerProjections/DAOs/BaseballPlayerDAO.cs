using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatickFantasyGM.Infrastructure.PlayerProjections.DAOs;

internal class BaseballPlayerDAO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<BattingStatsDAO> BattingProjections { get; set; } = new List<BattingStatsDAO>();
    public List<PitchingStatsDAO> PitchingProjections { get; set; } = new List<PitchingStatsDAO>();
}
