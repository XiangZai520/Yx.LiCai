/*
 * 用户Reads存储信息实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
namespace YxLiCai.Model.User
{
    /// <summary>
    /// 用户Reads存储信息
    /// </summary>
    public class UserSmallReads
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        ///// <summary>
        ///// 登录密码
        ///// </summary>
        //public string Password { get; set; }
        /// <summary>
        /// 用户账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string MyRealName { get; set; }
        /// <summary>
        /// 我的邀请码
        /// </summary>
        public string MyCode { get; set; }
        /// <summary>
        /// 我的身份证
        /// </summary>
        public string MyRealCard { get; set; }
        /// <summary>
        /// 是否实名认证
        /// </summary>
        public int? IsRealCard { get; set; }
        /// <summary>
        /// 是否绑定银行卡
        /// </summary>
        public int IsBindBank { get; set; }
        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal MyBalance { get; set; }
        /// <summary>
        /// 是否设置交易密码
        /// </summary>
        public int IsJiaoYIPW { get; set; }
    }
}
