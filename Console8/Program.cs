using SGS.OAD.HrInfo;

namespace Console8
{
    internal class Program
    {
        static void Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 可注入連線字串或指定伺服器與資料庫(二擇一)，或者直接使用目前預設值
            var helper = HrInfoHelper.Create()
                //.WithConnectionString("your private connection string")
                //.WithDataBase("server name", "db name")
                .Build();

            Employee? emp;

            // 透過 AD 帳號取得員工資料
            emp = helper.GetByAdId("Nancy-Tw_Hu");
            PrintData(emp);

            // 透過員工編號取得員工資料
            emp = helper.GetByEmpId("T18038");
            PrintData(emp);

            // 僅取員工編號
            Console.WriteLine($"Get EmpId (staff code) only: {helper.GetEmpId("brian_li")}");
        }

        static void PrintData(object data)
        {
            foreach (var property in data.GetType().GetProperties())
            {
                Console.WriteLine($"{property.Name}: {property.GetValue(data)}");
            }
            Console.WriteLine($"{Environment.NewLine}");
        }
    }
}
