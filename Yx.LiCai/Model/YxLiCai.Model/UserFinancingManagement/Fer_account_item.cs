using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方利息查看表
    /// </summary>
    public class Fer_account_item
    {
        public Fer_account_item()
        { }
        #region Model
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 融资人id
        /// </summary>
        public int fer_id { get; set; }
        /// <summary>
        /// 融资人账户id
        /// </summary>
        public int fer_account_id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public int pft_id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime c_time { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public int creator_id { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public int m_time { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 应付利息
        /// </summary>
        public decimal interest_payable { get; set; }
        /// <summary>
        /// 已付利息
        /// </summary>
        public decimal interest_paid { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Pname { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal Pmoney { get; set; }
        /// <summary>
        /// 还款期限
        /// </summary>
        public int LoanPeriod { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int PID { get; set; }
        #endregion Model
    }
}
