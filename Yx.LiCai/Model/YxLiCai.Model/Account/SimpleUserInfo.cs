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
    public class SimpleUserInfoModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public int GroupId { get; set; }
        public int RoleId { get; set; }
    } 
}
