using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.TransactionRecords
{
    /// <summary>
    /// 用户提现记录表
    /// </summary>
    public class User_withdrawModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public string Money { get; set; }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime c_time { get; set; }
    }
}
