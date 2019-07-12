using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Account
{
    /// <summary>
    /// 登录后存储用户登录信息实体类
    /// </summary>
    public class SimpleFerUserInfo
    {
        /// <summary>
        /// 融资方ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 融资方登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 融资方公司名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 融资方登录名字
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 融资方账户ID
        /// </summary>
        public int Fer_account { get; set; }
    } 
}
