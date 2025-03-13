#if NET472 || NET48
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

namespace SGS.OAD.HrInfo;

/// <summary>
/// HrInfoHelper 類別，用於處理 HR 資料的輔助類別。
/// </summary>
public class HrInfoHelper
{
    private string? _serverName = "APSE-IDB029";
    private string? _databaseName = "HR_DataSharing";
    private string? _connectionString;

    private HrInfoHelper() { }

    /// <summary>
    /// 建立 HrInfoHelper 的建構器。
    /// </summary>
    /// <returns>返回 Builder 物件。</returns>
    public static Builder Create()
    {
        return new Builder();
    }

    /// <summary>
    /// HrInfoHelper 的建構器類別。
    /// </summary>
    public class Builder
    {
        private readonly HrInfoHelper instance = new();

        /// <summary>
        /// 設定連接字串。
        /// </summary>
        /// <param name="connectionString">連接字串。</param>
        /// <returns>返回 Builder 物件。</returns>
        public Builder WithConnectionString(string connectionString)
        {
            instance._connectionString = connectionString;
            return this;
        }

        /// <summary>
        /// 設定資料庫伺服器名稱和資料庫名稱。
        /// </summary>
        /// <param name="serverName">伺服器名稱。</param>
        /// <param name="databseName">資料庫名稱。</param>
        /// <returns>返回 Builder 物件。</returns>
        public Builder WithDataBase(string serverName, string databseName)
        {
            instance._serverName = serverName;
            instance._databaseName = databseName;
            return this;
        }

        /// <summary>
        /// 建立 HrInfoHelper 物件。
        /// </summary>
        /// <returns>返回 HrInfoHelper 物件。</returns>
        public HrInfoHelper Build()
        {
            if (!string.IsNullOrEmpty(instance._connectionString))
                return instance;

            if (string.IsNullOrEmpty(instance._serverName) || string.IsNullOrEmpty(instance._databaseName))
                throw new InvalidOperationException("ServerName and DatabaseName must be provided.");
            instance._connectionString = Util.GetConnectionString(instance._serverName, instance._databaseName);
            return instance;
        }

        /// <summary>
        /// 非同步建立 HrInfoHelper 物件。
        /// </summary>
        /// <param name="cancellationToken">取消操作的標記。</param>
        /// <returns>返回 HrInfoHelper 物件。</returns>
        public async Task<HrInfoHelper> BuildAsync(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(instance._connectionString))
                return instance;

            if (string.IsNullOrEmpty(instance._serverName) || string.IsNullOrEmpty(instance._databaseName))
                throw new InvalidOperationException("ServerName and DatabaseName must be provided.");

            instance._connectionString = await Util.GetConnectionStringAsync(instance._serverName, instance._databaseName, cancellationToken: cancellationToken);

            return instance;
        }
    }

    /// <summary>
    /// 根據 AD 帳號取得員工編號。
    /// </summary>
    /// <param name="AdId">AD 帳號。</param>
    /// <returns>返回員工編號。</returns>
    public string GetEmpId(string AdId)
    {
        return GetEmpIdAsync(AdId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 根據 AD 帳號取得員工資料。
    /// </summary>
    /// <param name="adId">AD 帳號。</param>
    /// <returns>返回 Employee 物件。</returns>
    public Employee GetByAdId(string adId)
    {
        return GetByAdIdAsync(adId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 根據員工編號取得員工資料。
    /// </summary>
    /// <param name="empId">員工編號。</param>
    /// <returns>返回 Employee 物件。</returns>
    public Employee GetByEmpId(string empId)
    {
        string query = @"
            SELECT TOP 1 *
            FROM Employee (nolock)
            WHERE stf_code = @empId
        ";

        Employee? emp = null;

        using (SqlConnection connection = new(_connectionString))
        {
            try
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@empId", empId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        emp = SetData(reader);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        return emp ?? new Employee();
    }

    /// <summary>
    /// 非同步根據 AD 帳號取得員工資料。
    /// </summary>
    /// <param name="adId">AD 帳號。</param>
    /// <param name="cancellationToken">取消操作的標記。</param>
    /// <returns>返回 Employee 物件。</returns>
    public async Task<Employee> GetByAdIdAsync(string adId, CancellationToken cancellationToken = default)
    {
        string query = @"
            SELECT TOP 1 *
            FROM Employee (nolock)
            WHERE ADAccount = @adAccount AND (QUITDATE = 0 OR QUITDATE >= @today)
        ";

        Employee? emp = null;

        using (SqlConnection connection = new(_connectionString))
        {
            try
            {
                await connection.OpenAsync(cancellationToken);
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@adAccount", adId);
                command.Parameters.AddWithValue("@today", DateTime.Now.ToString("yyyyMMdd"));
                using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    emp = SetData(reader);
                }
            }
            catch
            {
                throw;
            }
        }

        return emp ?? new Employee();
    }

    /// <summary>
    /// 非同步根據員工編號取得員工資料。
    /// </summary>
    /// <param name="empId">員工編號。</param>
    /// <param name="cancellationToken">取消操作的標記。</param>
    /// <returns>返回 Employee 物件。</returns>
    public async Task<Employee> GetByEmpIdAsync(string empId, CancellationToken cancellationToken = default)
    {
        string query = @"
            SELECT TOP 1 *
            FROM Employee (nolock)
            WHERE stf_code = @empId
        ";

        Employee? emp = null;

        using (SqlConnection connection = new(_connectionString))
        {
            try
            {
                await connection.OpenAsync(cancellationToken);
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@empId", empId);
                using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                    {
                        emp = SetData(reader);
                    }
                }
            catch
            {
                throw;
            }
        }

        return emp ?? new Employee();
    }

    /// <summary>
    /// 非同步根據 AD 帳號取得員工編號。
    /// </summary>
    /// <param name="adId">AD 帳號。</param>
    /// <param name="cancellationToken">取消操作的標記。</param>
    /// <returns>返回員工編號。</returns>
    public async Task<string> GetEmpIdAsync(string adId, CancellationToken cancellationToken = default)
    {
        Employee emp = await GetByAdIdAsync(adId, cancellationToken);
        return emp.StfCode;
    }

    /// <summary>
    /// 設定員工資料。
    /// </summary>
    /// <param name="reader">SqlDataReader 物件。</param>
    /// <returns>返回 Employee 物件。</returns>
    private static Employee SetData(SqlDataReader reader)
    {
        return new Employee
        {
            StfCode = reader["stf_code"].ToString(),
            StfDept = reader["stf_dept"].ToString(),
            Cmp = reader["cmp"].ToString(),
            CmpName = reader["cmp_name"].ToString(),
            DeptName = reader["dept_name"].ToString(),
            Unitcd = reader["unitcd"].ToString(),
            Unitname = reader["unitname"].ToString(),
            AreaName = reader["area_name"].ToString(),
            AreaNo = reader["area_no"].ToString(),
            StfCname = reader["stf_cname"].ToString(),
            StfEname = reader["stf_ename"].ToString(),
            StfCome = Util.GetDateTime(reader["stf_come"]),
            QuitDate = Util.GetQuiteDate(reader["QUITDATE"]),
            Email = reader["email"].ToString(),
            CostType = reader["costtype"].ToString(),
            TitleName = reader["TitleName"].ToString(),
            TitleNameC = reader["TitleNameC"].ToString(),
            WorkType = reader["WORKTYPE"].ToString(),
            SexType = reader["SEXTYPE"].ToString(),
            ADAccount = reader["ADAccount"].ToString(),
            BusinessUnit = reader["Business_Unit"].ToString(),
            WorkPlaceCode = reader["WORKPLACE_CODE"].ToString(),
            WorkPlaceName = reader["WORKPLACE_NAME"].ToString(),
            SuperEmpNo = reader["SuperEmpNo"].ToString(),
            SuperEmpCName = reader["SuperEmpCName"].ToString(),
            SuperEmpName = reader["SuperEmpName"].ToString(),
            SuperEmpEmail = reader["SuperEmpEmail"].ToString(),
            JobStatus = reader["JOBSTATUS"].ToString(),
            JobStatusText = reader["JOBSTATUS_Text"].ToString()
        };
    }
}
