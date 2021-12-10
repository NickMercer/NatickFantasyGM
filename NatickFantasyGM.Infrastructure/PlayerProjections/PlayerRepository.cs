using NatickFantasyGM.Core.PlayerProjections.Aggregates;
using NatickFantasyGM.Core.PlayerProjections.Interfaces;
using NatickFantasyGM.Core.PlayerProjections.Specifications.Core;
using NatickFantasyGM.Infrastructure.PlayerProjections.DAOs;
using Dapper;
using Microsoft.Data.Sqlite;
using NatickFantasyGM.Core.ValueObjects;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Evaluations;
using NatickFantasyGM.Core.PlayerProjections.ValueObjects;
using NatickFantasyGM.Core.PlayerProjections.PlayerAggregate.Projections;
using NatickFantasyGM.Core.PlayerProjections;

namespace NatickFantasyGM.Infrastructure.PlayerProjections;

public class PlayerRepository : IPlayerRepository
{
    private static List<RatioDAO> _ratios = new List<RatioDAO>();
    private static List<Player> _players = new List<Player>();

    private static bool _initialized = false;

    public PlayerRepository()
    {
        if(_initialized == false)
        {
            Initialize();
            _initialized = true;
        }
    }

    private void Initialize()
    {
        _ratios = LoadDefaultRatiosFromDB();
        _players = LoadAllPlayersFromDB();
    }

    private List<RatioDAO> LoadDefaultRatiosFromDB()
    {
        using (var conn = new SqliteConnection(DatabaseCredentials.PlayerStatsDBConnectionString))
        {
            var commandString = "SELECT * FROM RelativeStatFormulas";

            var ratioDAOs = conn.Query<RatioDAO>(commandString);

            return ratioDAOs.ToList();
        }
    }

    private List<Player> LoadAllPlayersFromDB()
    {
        using (var conn = new SqliteConnection(DatabaseCredentials.PlayerStatsDBConnectionString))
        {
            var commandString = "SELECT players.*, batting.*, pitching.* " +
                                "FROM BaseballPlayers players " +
                                "LEFT JOIN BaseballPlayers_BattingStats batting ON players.Id = batting.PlayerId " +
                                "LEFT JOIN BaseballPlayers_PitchingStats pitching ON players.Id = pitching.PlayerId";

            var playerLookup = new Dictionary<int, BaseballPlayerDAO>();
            var playerDAOs = conn.Query<BaseballPlayerDAO, BattingStatsDAO, PitchingStatsDAO, BaseballPlayerDAO>(commandString, (player, batting, pitching) =>
            {
                BaseballPlayerDAO playerEntry;
                
                if(!playerLookup.TryGetValue(player.Id, out playerEntry))
                {
                    playerEntry = player;
                    playerEntry.BattingProjections = new List<BattingStatsDAO>();
                    playerEntry.PitchingProjections = new List<PitchingStatsDAO>();
                    playerLookup.Add(player.Id, playerEntry);
                }

                playerEntry.BattingProjections.Add(batting);
                playerEntry.PitchingProjections.Add(pitching);

                return playerEntry;
            }, splitOn: "Id, Id").Distinct().ToList();

            var players = new List<Player>();
            foreach (var playerDAO in playerDAOs)
            {
                var player = CreatePlayerFromDAO(playerDAO);
                players.Add(player);
            }

            return players;
        }
    }

    private Player CreatePlayerFromDAO(BaseballPlayerDAO playerDAO)
    {
        var allThirdPartyProjections = GenerateThirdPartyProjections(playerDAO);

        var weightedProjections = WeightProjections(allThirdPartyProjections);

        var evaluation = new Evaluation(Guid.NewGuid(), playerDAO.Id, weightedProjections, allThirdPartyProjections);

        var name = new FullName(playerDAO.FirstName, playerDAO.LastName);
        var player = new Player(playerDAO.Id, name, evaluation);

        return player;
    }

    private static List<WeightedProjection> WeightProjections(List<ThirdPartyProjection> allThirdPartyProjections)
    {
        //TODO: Replace this with weighting based on owner preferences.
        var weightedProjections = new List<WeightedProjection>();
        var zips = allThirdPartyProjections.FirstOrDefault(x => x.ProjectionSource == ProjectionSource.ZiPS);
        var steamer = allThirdPartyProjections.FirstOrDefault(x => x.ProjectionSource == ProjectionSource.Steamer);

        if (zips != null)
        {
            weightedProjections.Add(new WeightedProjection(zips.Id, 50));
        }

        if (steamer != null)
        {
            weightedProjections.Add(new WeightedProjection(steamer.Id, 50));
        }

        return weightedProjections;
    }

    private static List<ThirdPartyProjection> GenerateThirdPartyProjections(BaseballPlayerDAO playerDAO)
    {
        var allThirdPartyProjections = new List<ThirdPartyProjection>();

        foreach (var source in ProjectionSource.ThirdPartySources)
        {
            var battingDAO = playerDAO.BattingProjections.FirstOrDefault(x => x.StatProviderId == source.SourceId);
            var pitchingDAO = playerDAO.PitchingProjections.FirstOrDefault(x => x.StatProviderId == source.SourceId);

            var simpleStats = new List<Tuple<StatName, StatType, double>>();
            var ratios = new List<Tuple<StatName, StatType, string>>();

            if (battingDAO != null)
            {
                var simpleOffensives = new List<Tuple<StatName, StatType, double>>
                {
                    new Tuple<StatName, StatType, double>(new StatName("Games", "G"), StatType.Offensive, battingDAO.Games),
                    new Tuple<StatName, StatType, double>(new StatName("Plate Appearances", "PA"), StatType.Offensive, battingDAO.PlateAppearances),
                    new Tuple<StatName, StatType, double>(new StatName("At Bats", "AB"), StatType.Offensive, battingDAO.AtBats),
                    new Tuple<StatName, StatType, double>(new StatName("Hits", "H"), StatType.Offensive, battingDAO.Hits),
                    new Tuple<StatName, StatType, double>(new StatName("Doubles", "2B"), StatType.Offensive, battingDAO.Doubles),
                    new Tuple<StatName, StatType, double>(new StatName("Triples", "3B"), StatType.Offensive, battingDAO.Triples),
                    new Tuple<StatName, StatType, double>(new StatName("Home Runs", "HR"), StatType.Offensive, battingDAO.HomeRuns),
                    new Tuple<StatName, StatType, double>(new StatName("Runs", "R"), StatType.Offensive, battingDAO.Runs),
                    new Tuple<StatName, StatType, double>(new StatName("Runs Batted In", "RBI"), StatType.Offensive, battingDAO.RBIs),
                    new Tuple<StatName, StatType, double>(new StatName("Walks", "BB"), StatType.Offensive, battingDAO.Walks),
                    new Tuple<StatName, StatType, double>(new StatName("Strike Outs", "SO"), StatType.Offensive, battingDAO.StrikeOuts),
                    new Tuple<StatName, StatType, double>(new StatName("Hit By Pitches", "HBP"), StatType.Offensive, battingDAO.HitByPitches),
                    new Tuple<StatName, StatType, double>(new StatName("Stolen Bases", "SB"), StatType.Offensive, battingDAO.StolenBases),
                    new Tuple<StatName, StatType, double>(new StatName("Caught Stealing", "CS"), StatType.Offensive, battingDAO.CaughtStealing),
                    new Tuple<StatName, StatType, double>(new StatName("Sacrifice Flies", "SF"), StatType.Offensive, battingDAO.SacrificeFlies),
                    new Tuple<StatName, StatType, double>(new StatName("Sacrifice Bunts", "SB"), StatType.Offensive, battingDAO.SacrificeBunts),
                    new Tuple<StatName, StatType, double>(new StatName("Wins Above Replacement", "WAR"), StatType.Offensive, battingDAO.WAR)
                };
                simpleStats.AddRange(simpleOffensives);

                foreach (var ratioDAO in _ratios.Where(x => x.StatType == StatType.Offensive))
                {
                    ratios.Add(new Tuple<StatName, StatType, string>(new StatName(ratioDAO.Name, ratioDAO.Abbreviation), ratioDAO.StatType, ratioDAO.Formula));
                }
            }

            if (pitchingDAO != null)
            {
                var simplePitches = new List<Tuple<StatName, StatType, double>>
                {
                    new Tuple<StatName, StatType, double>(new StatName("Wins", "W"), StatType.Pitching, pitchingDAO.Wins),
                    new Tuple<StatName, StatType, double>(new StatName("Losses", "L"), StatType.Pitching, pitchingDAO.Losses),
                    new Tuple<StatName, StatType, double>(new StatName("Games Started", "GS"), StatType.Pitching, pitchingDAO.GamesStarted),
                    new Tuple<StatName, StatType, double>(new StatName("Games", "G"), StatType.Pitching, pitchingDAO.Games),
                    new Tuple<StatName, StatType, double>(new StatName("Saves", "S"), StatType.Pitching, pitchingDAO.Saves),
                    new Tuple<StatName, StatType, double>(new StatName("Innings Pitched", "IP"), StatType.Pitching, pitchingDAO.InningsPitched),
                    new Tuple<StatName, StatType, double>(new StatName("Hits Allowed", "HA"), StatType.Pitching, pitchingDAO.HitsAllowed),
                    new Tuple<StatName, StatType, double>(new StatName("Earned Runs", "ER"), StatType.Pitching, pitchingDAO.EarnedRuns),
                    new Tuple<StatName, StatType, double>(new StatName("Home Runs Allowed", "HRA"), StatType.Pitching, pitchingDAO.HomeRunsAllowed),
                    new Tuple<StatName, StatType, double>(new StatName("Strike Outs", "K"), StatType.Pitching, pitchingDAO.StrikeOuts),
                    new Tuple<StatName, StatType, double>(new StatName("Walks", "BB"), StatType.Pitching, pitchingDAO.Walks),
                    new Tuple<StatName, StatType, double>(new StatName("Wins Above Replacement", "WAR"), StatType.Pitching, pitchingDAO.WAR)
                };
                simpleStats.AddRange(simplePitches);

                foreach (var ratioDAO in _ratios.Where(x => x.StatType == StatType.Pitching))
                {
                    ratios.Add(new Tuple<StatName, StatType, string>(new StatName(ratioDAO.Name, ratioDAO.Abbreviation), ratioDAO.StatType, ratioDAO.Formula));
                }
            }

            allThirdPartyProjections.Add(new ThirdPartyProjection(Guid.NewGuid(), source, simpleStats, ratios));
        }

        return allThirdPartyProjections;
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
