#nullable disable

namespace SGS.OAD.HrInfo
{
    /// <summary>
    /// 員工類別
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 員工代碼
        /// </summary>
        public string StfCode { get; set; }

        /// <summary>
        /// 員工部門
        /// </summary>
        public string StfDept { get; set; }

        /// <summary>
        /// 公司代碼
        /// </summary>
        public string Cmp { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CmpName { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 單位代碼
        /// </summary>
        public string Unitcd { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        public string Unitname { get; set; }

        /// <summary>
        /// 區域名稱
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 區域代碼
        /// </summary>
        public string AreaNo { get; set; }

        /// <summary>
        /// 員工中文姓名
        /// </summary>
        public string StfCname { get; set; }

        /// <summary>
        /// 員工英文姓名
        /// </summary>
        public string StfEname { get; set; }

        /// <summary>
        /// 入職日期
        /// </summary>
        public DateTime? StfCome { get; set; }

        /// <summary>
        /// 離職日期
        /// </summary>
        public DateTime? QuitDate { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 成本類型
        /// </summary>
        public string CostType { get; set; }

        /// <summary>
        /// 職稱名稱
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// 職稱中文名稱
        /// </summary>
        public string TitleNameC { get; set; }

        /// <summary>
        /// 工作類型
        /// </summary>
        public string WorkType { get; set; }

        /// <summary>
        /// 性別類型
        /// </summary>
        public string SexType { get; set; }

        /// <summary>
        /// AD帳號
        /// </summary>
        public string ADAccount { get; set; }

        /// <summary>
        /// 事業單位
        /// </summary>
        public string BusinessUnit { get; set; }

        /// <summary>
        /// 工作地點代碼
        /// </summary>
        public string WorkPlaceCode { get; set; }

        /// <summary>
        /// 工作地點名稱
        /// </summary>
        public string WorkPlaceName { get; set; }

        /// <summary>
        /// 上司員工編號
        /// </summary>
        public string SuperEmpNo { get; set; }

        /// <summary>
        /// 上司中文姓名
        /// </summary>
        public string SuperEmpCName { get; set; }

        /// <summary>
        /// 上司姓名
        /// </summary>
        public string SuperEmpName { get; set; }

        /// <summary>
        /// 上司電子郵件
        /// </summary>
        public string SuperEmpEmail { get; set; }

        /// <summary>
        /// 工作狀態
        /// </summary>
        public string JobStatus { get; set; }

        /// <summary>
        /// 工作狀態描述
        /// </summary>
        public string JobStatusText { get; set; }
    }
}
