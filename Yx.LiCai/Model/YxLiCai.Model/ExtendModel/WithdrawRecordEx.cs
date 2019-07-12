using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 提现记录
    /// </summary>
    public class WithdrawRecordEx
    {
        public long OrderInfoID { get; set; }
        public string LoginName { get; set; }
        public decimal WithdrawMoney { get; set; }
        public decimal FinalWithdrawMoney { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
