using System.Reflection;

namespace NatickFantasyGM.Infrastructure;

internal static class DatabaseCredentials
{
    private static string PlayerStatsDBPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"NGMPlayerStatsDB.db");

    public static string PlayerStatsDBConnectionString => $"Data Source={PlayerStatsDBPath};";
}
