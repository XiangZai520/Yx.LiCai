using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 用户账户资产
    /// </summary>
    public class UserAccountEx
    {
        public long user_id { get; set; }
        public decimal amount { get; set; }
        public decimal amount_invest { get; set; }
        public decimal amount_blance { get; set; }
        public decimal amount_blance_fz { get; set; }
    }
}
