using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.Enums
{
    public enum UserLoginResults
    {
        /// <summary>
        /// 成功
        /// </summary>
        Successful = 1,
        /// <summary>
        /// 账号不存在
        /// </summary>
        UserNotExist = 2,
        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// 账号被锁定
        /// </summary>
        AccountClosed = 4,
        /// <summary>
        /// 系统错误
        /// </summary>
        Failed = 5
    }
}
