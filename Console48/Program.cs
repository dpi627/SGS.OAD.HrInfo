using SGS.OAD.HrInfo;
using System;

namespace Console48
{
    internal class Program
    {
        static void Main()
        {
            var helper = HrInfoHelper.Create().Build();
            Console.WriteLine($"EmpId: {helper.GetEmpId("brian_li")}");
        }
    }
}
