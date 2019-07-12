using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 加息券发放明细
    /// </summary>
    public class InterestCouponSendDetailEx
    {
        public long user_id { get; set; }
        public string name { get; set; }
        public decimal interest_rate { get; set; }
        public DateTime c_time { get; set; }
        public DateTime e_time { get; set; }
        public int use_status { get; set; }
        public int pdt_type { get; set; }
        public DateTime use_date { get; set; }
        public int enable_day { get; set; }

    }
}
