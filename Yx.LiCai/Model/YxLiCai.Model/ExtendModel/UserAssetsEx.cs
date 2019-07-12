using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 自有资产
    /// </summary>
    public class UserAssetsEx
    {
        public long id { get; set; }
        public DateTime create_time { get; set; }
        public decimal interest_rate { get; set; }
        //public decimal CurrentRate { get; set; }
        public decimal interest_rate_added { get; set; }
        public decimal order_investment { get; set; }
        public decimal interest_added { get; set; }
        public DateTime expiration_time { get; set; }
    }
}
