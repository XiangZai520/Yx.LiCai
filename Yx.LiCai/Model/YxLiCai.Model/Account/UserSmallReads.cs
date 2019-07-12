/*
 * 用户Reads存储信息实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
namespace YxLiCai.Model.Account
{
    /// <summary>
    /// 用户Reads存储信息
    /// </summary>
    [Serializable]
    public class UserSmallReads
    {
        /// <summary>
        /// 账户ID(主键)
        /// </summary>
        public int AccountID { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string MyRealName { get; set; }
        /// <summary>
        /// 我的身份证
        /// </summary>
        public string MyRealCard { get; set; }
        /// <summary>
        /// 是否实名认证
        /// </summary>
        public int IsRealCard { get; set; }
        /// <summary>
        /// 是否绑定银行卡
        /// </summary>
        public int IsBindBank { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal MyBalance { get; set; }
        /// <summary>
        /// 卡号前6位
        /// </summary>
        public string First_num { get; set; }
        /// <summary>
        /// 卡号后54位
        /// </summary>
        public string Last_num { get; set; }
        /// <summary>
        /// 银行预留手机号
        /// </summary>
        public string Bank_Phone { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string SPassword { get; set; }
    }
}
