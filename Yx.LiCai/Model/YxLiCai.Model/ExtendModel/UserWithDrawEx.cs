using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 用户提现类
    /// </summary>
    public class UserWithDrawEx
    {
        public int id { get; set; }
        public string login_name { get; set; }
        public string real_name { get; set; }
        public string bank_name { get; set; }
        public string bank_card { get; set; }
        public string bank_phone { get; set; }
        public decimal amount { get; set; }
        public decimal amount_balance { get; set; }
        public decimal amount_principal { get; set; }
        public DateTime c_time { get; set; }
        public string auditor_name { get; set; }
        public DateTime audit_time { get; set; }
        public string loan_name { get; set; }
        public DateTime loan_time { get; set; }
        public int status { get; set; }
        public int user_id { get; set; }
        public string remark { get; set; }
        //账户余额及冻结金额
        public decimal account_amount_blance { get; set; }
        public decimal account_amount_blance_fz { get; set; }
    }
}
