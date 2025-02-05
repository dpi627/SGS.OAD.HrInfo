using SGS.OAD.HrInfo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Console48
{
    internal class Program
    {
        static async Task Main()
        {
            Test();
            await TestAsync();
        }

        static void Test()
        {
            var helper = HrInfoHelper.Create().Build();
            Console.WriteLine($"EmpId: {helper.GetEmpId("brian_li")}");
        }

        static async Task TestAsync(CancellationToken cancellationToken = default)
        {
            var helper = await HrInfoHelper.Create().BuildAsync(cancellationToken);
            Console.WriteLine($"EmpId: {await helper.GetEmpIdAsync("brian_li")}");
        }
    }
}
