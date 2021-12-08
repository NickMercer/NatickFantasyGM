using NatickFantasyGM.Core.PlayerProjections.Aggregates;
using NatickFantasyGM.Core.PlayerProjections.Interfaces;
using NatickFantasyGM.Core.PlayerProjections.Specifications.Core;
using System.Linq;

namespace NatickFantasyGM.Infrastructure.PlayerProjections;

public class PlayerRepository : IPlayerRepository
{
    private static List<Player> _players = new List<Player>();
    private static bool _initialized = false;

    public PlayerRepository()
    {
        if(_initialized == false)
        {
            InitializePlayers();
        }
    }

    public void InitializePlayers()
    {
        _players = LoadAllPlayersFromDB();
        _initialized = true;
    }

    private List<Player> LoadAllPlayersFromDB()
    {

    }

    public IReadOnlyList<Player> GetAll()
    {
        return _players.AsReadOnly();
    }

    public IReadOnlyList<Player> GetBySpec(Specification<Player> specification)
    {
        return _players
            .Where(x => specification.IsSatisfiedBy(x))
            .ToList()
            .AsReadOnly();
    }
}
