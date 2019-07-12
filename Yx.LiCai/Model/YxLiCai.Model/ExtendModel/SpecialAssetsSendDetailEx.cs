using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 特殊资产发放明细
    /// </summary>
    public class SpecialAssetsSendDetailEx
    {
        public long user_id { get; set; }
        public string name { get; set; }
        public decimal amount { get; set; }
        public int type { get; set; }
        public decimal rate { get; set; }
        public int enable_day { get; set; }
        public DateTime c_time { get; set; }
        public DateTime e_time { get; set; }
    }
}
