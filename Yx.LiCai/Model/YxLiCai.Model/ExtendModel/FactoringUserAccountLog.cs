using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 保理账户日志
    /// </summary>
    public class FactoringUserAccountLog
    {
        public int pjt_id { get; set; }
        public string pjt_name { get; set; }
        public decimal pjt_amount { get; set; }
        public decimal change_amount { get; set; }
        public int type { get; set; }
    }
}
