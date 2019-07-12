using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 活动充值记录
    /// </summary>
    public class ActRechargeRecEx
    {
        public DateTime c_time { get; set; }
        public decimal amount { get; set; }
        public int from_id { get; set; }
    }
}
