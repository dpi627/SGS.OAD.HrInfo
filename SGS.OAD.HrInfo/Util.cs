using SGS.OAD.DB;

namespace SGS.OAD.HrInfo;

internal class Util
{
    public static string GetConnectionString(string server, string database, string appName = "SGS.OAD.HrInfo")
    {
        return GetConnectionStringAsync(server, database, appName).GetAwaiter().GetResult();
    }
    public static async Task<string> GetConnectionStringAsync(string server, string database, string appName = "SGS.OAD.HrInfo", CancellationToken cancellationToken = default)
    {
        return await DbInfoBuilder.Init()
            .SetServer(server)
            .SetDatabase(database)
            .SetAppName(appName)
            .BuildAsync(cancellationToken)
            .ContinueWith(t => t.Result.ConnectionString);
    }

    public static DateTime? GetDateTime(object val)
    {
        if (val != DBNull.Value && Convert.ToInt32(val) != 0)
        {
            return DateTime.ParseExact(val.ToString(), "yyyyMMdd", null);
        }
        return null;
    }

    public static DateTime? GetQuiteDate(object val)
    {
        return GetDateTime(val)?.AddDays(1).AddSeconds(-1);
    }
}