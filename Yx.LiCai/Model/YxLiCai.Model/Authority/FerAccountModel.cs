using System;

namespace YxLiCai.Model.Authority
{
    /// <summary>
    /// 融资方用户账户实体
    /// </summary>
    public class FerAccountModel
    {
        /// <summary>
        /// 融资方ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 融资方账户ID
        /// </summary>
        public int Fer_account { get; set; }
     
    }
}
