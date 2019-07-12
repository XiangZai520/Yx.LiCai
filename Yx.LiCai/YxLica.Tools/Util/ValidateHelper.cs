using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YxLiCai.Tools.Util
{
    public class ValidateHelper
    {
        /// <summary>
        /// 校验一个字符串是否为手机号码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public static bool IsPhoneNum(string phone)
        {
            Regex regex = new Regex(@"^0?(13[0-9]|15[0-9]|18[0-9]|14[0-9]|17[0-9])[0-9]{8}$", RegexOptions.IgnoreCase);
            return regex.Match(phone).Success;
        }

        /// <summary>
        /// 校验一个字符串是否为身份证
        /// </summary>
        /// <param name="iD">身份证</param>
        /// <returns></returns>
        public static bool IsIDNum(string iD)
        {
            Regex regex = new Regex(@"(^\d{18}$)|(^\d{15}$)|(^\d{17}(\d|X|x)$)", RegexOptions.IgnoreCase);
            return regex.Match(iD).Success;
        }

        /// <summary>
        /// 是否是登录密码
        /// </summary>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        public static bool IsPwd(string pwd) 
        {
            Regex regex = new Regex(@"^[0-9a-zA-Z_/]{6,16}$", RegexOptions.IgnoreCase);
            return regex.Match(pwd).Success;
        }

        /// <summary>
        /// 是否是交易密码
        /// </summary>
        /// <param name="fetchPwd">交易密码</param>
        /// <returns></returns>
        public static bool IsFetchPwd(string fetchPwd) 
        {
            Regex regex = new Regex(@"^[0-9]{6}$",RegexOptions.IgnoreCase);
            return regex.Match(fetchPwd).Success;
        }

        /// <summary>
        /// 是否是验证码
        /// </summary>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        public static bool IsCaptcha(string captcha) 
        {
            Regex regex = new Regex(@"^[0-9]{6}$", RegexOptions.IgnoreCase);
            return regex.Match(captcha).Success;
        }		


    }
}
