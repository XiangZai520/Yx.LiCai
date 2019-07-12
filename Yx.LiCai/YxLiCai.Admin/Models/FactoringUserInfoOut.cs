using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.Admin.Models
{
    /// <summary>
    /// 保理用户中心outModel
    /// </summary>
    public class FactoringUserInfoOut
    {
        public long user_id { get; set; }
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string BankCardNo { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal KeYongMoney { get; set; }
        public decimal ZhaiQuanMoney { get; set; }
        public decimal QianKuanMoney { get; set; }
        public string Today { get; set; }
    }
}