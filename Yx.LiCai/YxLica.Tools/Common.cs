using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLica.Tools
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 获取加星号后的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="firstNum"></param>
        /// <param name="LastNum"></param>
        /// <returns></returns>
        public static string GetHideStr(string str, int firstNum, int LastNum)
        {
            try
            {
                if (str != null && str.Length > LastNum)
                {
                    str = str.Substring(0, firstNum) + "***" + str.Substring(str.Length - LastNum, LastNum);
                }
            }
            catch
            {
            }
            return str;
        }
    }
}
