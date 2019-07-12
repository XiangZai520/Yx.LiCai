using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// 登录后存储用户登录信息实体类
    /// </summary>
    [Serializable]
    public class SmallUserInfo
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
        /// 是否设置交易密码
        /// </summary>
        public int IsJiaoYIPW { get; set; }

        /// <summary>
        ///银行卡名称 
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银行卡尾号
        /// </summary>
        public string LastBankNum { get; set; }


        /// <summary>
        /// 银行卡Code
        /// </summary>
        public string BankCode { get; set; }

    }
}
