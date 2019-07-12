using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.TransactionRecords
{
    /// <summary>
    /// 赎回记录表
    /// </summary>
    public class User_redemptionModel
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
        /// 产品名称
        /// </summary>
        public int ProductName { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public string YerRate { get; set; }
        /// <summary>
        /// 加息券
        /// </summary>
        public string InterestRate { get; set; }
        /// <summary>
        /// 赎回金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal LiquidatedMoney { get; set; }
        /// <summary>
        /// 违约金利率
        /// </summary>
        public decimal penalty_rate { get; set; }
        /// <summary>
        /// 赎回本金
        /// </summary>
        public decimal principal { get; set; }
        /// <summary>
        /// 赎回申请时间
        /// </summary>
        public DateTime c_time { get; set; }
        /// <summary>
        /// 用户iD
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal LiquidatedDamages { get; set; }
        /// <summary>
        /// 实到金额
        /// </summary>
        public decimal ActualAmount { get; set; }
    }
}
