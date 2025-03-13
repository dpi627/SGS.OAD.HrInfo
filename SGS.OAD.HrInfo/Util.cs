using SGS.OAD.DB;

namespace SGS.OAD.HrInfo;

internal class Util
{
    /// <summary>
    /// 獲取連接字串。
    /// </summary>
    /// <param name="server">伺服器名稱。</param>
    /// <param name="database">資料庫名稱。</param>
    /// <param name="appName">應用程式名稱，預設為 "SGS.OAD.HrInfo"。</param>
    /// <returns>連接字串。</returns>
    public static string GetConnectionString(string server, string database, string appName = "SGS.OAD.HrInfo")
    {
        var dbInfo = DbInfoBuilder.Init()
            .SetServer(server)
            .SetDatabase(database)
            .SetAppName(appName)
            .Build();
        return dbInfo.ConnectionString;
    }

    /// <summary>
    /// 非同步獲取連接字串。
    /// </summary>
    /// <param name="server">伺服器名稱。</param>
    /// <param name="database">資料庫名稱。</param>
    /// <param name="appName">應用程式名稱，預設為 "SGS.OAD.HrInfo"。</param>
    /// <param name="cancellationToken">取消操作的標記。</param>
    /// <returns>連接字串的非同步任務。</returns>
    public static async Task<string> GetConnectionStringAsync(string server, string database, string appName = "SGS.OAD.HrInfo", CancellationToken cancellationToken = default)
    {
        return await DbInfoBuilder.Init()
            .SetServer(server)
            .SetDatabase(database)
            .SetAppName(appName)
            .BuildAsync(cancellationToken)
            .ContinueWith(t => t.Result.ConnectionString);
    }

    /// <summary>
    /// 獲取日期時間。
    /// </summary>
    /// <param name="val">日期時間的物件。</param>
    /// <returns>如果值有效，則返回日期時間；否則返回 null。</returns>
    public static DateTime? GetDateTime(object val)
    {
        if (val != DBNull.Value && Convert.ToInt32(val) != 0)
        {
            return DateTime.ParseExact(val.ToString(), "yyyyMMdd", null);
        }
        return null;
    }

    /// <summary>
    /// 獲取結束日期時間。
    /// </summary>
    /// <param name="val">日期時間的物件。</param>
    /// <returns>如果值有效，則返回結束日期時間；否則返回 null。</returns>
    public static DateTime? GetQuiteDate(object val)
    {
        return GetDateTime(val)?.AddDays(1).AddSeconds(-1);
    }
}
