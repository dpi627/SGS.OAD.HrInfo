#if NET472 || NET48
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif

namespace SGS.OAD.HrInfo;

public class HrInfoHelper
{
    private string? _serverName = "APSE-IDB029";
    private string? _databaseName = "HR_DataSharing";
    private string? _connectionString;

    private HrInfoHelper() { }

    public static Builder Create()
    {
        return new Builder();
    }

    public class Builder
    {
        private readonly HrInfoHelper instance = new();
        public Builder WithConnectionString(string connectionString)
        {
            instance._connectionString = connectionString;
            return this;
        }
        public Builder WithDataBase(string serverName, string databseName)
        {
            instance._serverName = serverName;
            instance._databaseName = databseName;
            return this;
        }
        public HrInfoHelper Build()
        {
            return BuildAsync().GetAwaiter().GetResult();
        }
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

    public string GetEmpId(string AdId)
    {
        return GetEmpIdAsync(AdId).GetAwaiter().GetResult();
    }

    public Employee GetByAdId(string adId)
    {
        return GetByAdIdAsync(adId).GetAwaiter().GetResult();
    }

    public Employee GetByEmpId(string empId)
    {
        return GetByEmpIdAsync(empId).GetAwaiter().GetResult();
    }

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

    public async Task<string> GetEmpIdAsync(string adId, CancellationToken cancellationToken = default)
    {
        Employee emp = await GetByAdIdAsync(adId, cancellationToken);
        return emp.StfCode;
    }

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