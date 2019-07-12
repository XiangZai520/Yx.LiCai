using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model
{ 
    /// <summary>
    /// 系统操作日志类
    /// </summary>
    public class SysActionLogModel
    {
        public int id { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string adminName { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public int adminId { get; set; }
        /// <summary>
        /// 操作类型 (0系统1项目2产品3放款4还款5提现6赎回7活动8会员',
        /// </summary>
        public int OpType { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 备注 备注(记录谁在什么时间把什么由什么变成了什么) 
        /// </summary>
        public string Remark { get; set; }
    }
}
