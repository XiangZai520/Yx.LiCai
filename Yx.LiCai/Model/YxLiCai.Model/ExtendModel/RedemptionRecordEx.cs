using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 赎回记录
    /// </summary>
    public class RedemptionRecordEx
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderInfoID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 赎回金额
        /// </summary>
        public decimal RedemptionMoney { get; set; }
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal WeiYueJin { get; set; }
        /// <summary>
        /// 实到金额
        /// </summary>
        public decimal FinalRedemptionMoney { get; set; }
        /// <summary>
        /// 赎回时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
