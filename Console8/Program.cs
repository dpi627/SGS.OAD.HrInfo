using SGS.OAD.HrInfo;

namespace Console8
{
    internal class Program
    {
        static async Task Main()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 測試同步
            Test();
            // 測試非同步
            await TestAsync();
        }

        static void Test()
        {
            HrInfoHelper? helper = HrInfoHelper.Create().Build();

            Employee? emp;
            // 透過 AD 帳號取得員工資料
            emp = helper.GetByAdId("Nancy-Tw_Hu");
            PrintData(emp);
            // 透過員工編號取得員工資料
            emp = helper.GetByEmpId("T18038");
            PrintData(emp);
            // 僅取員工編號
            string empId = helper.GetEmpId("brian_li");
            Console.WriteLine($"EmpId: {empId}");
        }

        static async Task TestAsync()
        {
            HrInfoHelper? helper = await HrInfoHelper.Create().BuildAsync();

            Employee? emp;
            // 透過 AD 帳號取得員工資料
            emp = await helper.GetByAdIdAsync("Nancy-Tw_Hu");
            PrintData(emp);
            // 透過員工編號取得員工資料
            emp = await helper.GetByEmpIdAsync("T18038");
            PrintData(emp);
            // 僅取員工編號
            string empId = await helper.GetEmpIdAsync("brian_li");
            Console.WriteLine($"EmpId: {empId}");
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
