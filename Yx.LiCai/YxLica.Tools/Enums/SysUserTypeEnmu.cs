using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Tools.Expand;

namespace YxLiCai.Tools.Enums
{
    public enum SysUserTypeEnmu
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        SimUser = 0,
        /// <summary>
        /// 企业用户
        /// </summary>
        BusinessUser = 1,
        /// <summary>
        /// e租宝账户
        /// </summary>
        FinancialPlanner = 2,
        /// <summary>
        /// e租宝账户
        /// </summary>
        Ezb = 3
    }

    /// <summary>
    /// 用户投资产品状态
    /// </summary>
    public enum UserRaiseProductEnmu
    { 
        /// <summary>
        /// 成功
        /// </summary>
        [DisplayText("成功")]
        Success=0,
        /// <summary>
        /// 产品已经完标
        /// </summary>
        [DisplayText("产品已经完标")]
        ProductHasDone=1,
        /// <summary>
        /// 余额不足
        /// </summary> 
        [DisplayText("余额不足")]
        NotEnoughBlance = 2,
        /// <summary>
        /// 超过加息券最大限额
        /// </summary>
         [DisplayText("超过加息券最大限额")]
        AddRateMax=3,
        /// <summary>
        /// 起投金额小于100
        /// </summary>
         [DisplayText("起投金额小于100")]
        Not100=4,
        /// <summary>
        /// 失败
        /// </summary> 
         [DisplayText("支付失败，请重试")]
         Failed =5
    }
    
}
