using YxLiCai.Tools.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YxLiCai.Tools.Util
{
    public abstract class Helper
    {

        /// <summary>
        /// IP
        /// </summary>
        /// <returns>µ±Ç°Ò³Ãæ¿Í»§¶ËµÄIP</returns>
        public static string GetIP()
        {
            string result = String.Empty;
            try
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CIP"];

                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return result;
            }
            catch
            {
                return "127.0.0.1";
            }

        }
       
         
        /// <summary>
        /// 根据userId,productid生成订单号(15位)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string generateOrderCode(long userId, int productId)
        {
            string result = DateTime.Now.ToString("yyMMddHHmmss") + userId.ToString() + productId.ToString() + new Random().Next(100).ToString("D3");
            return result;
        }

        /// <summary>
        /// 根据userId生成订单号(15位)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string generateOrderCode(long userId)
        {
            string result = userId.ToString() + DateTime.Now.ToString("yyMMddHHmmss") + new Random().Next(100).ToString("D3");
            return result;
        }
        /// <summary>
        /// 生成guid
        /// </summary>
        /// <returns></returns>
        public static string Uuid()
        {
            StringBuilder SB = new StringBuilder();
            string[] uuid = Guid.NewGuid().ToString().Split('-');
            foreach (var item in uuid)
            {
                SB.Append(item);
            }
            return SB.ToString();
        }
        public static Random random = new Random(); 
        public static String getRandom(int length)
        { 
            StringBuilder ret = new StringBuilder(); 
            for (int i = 0; i < length; i++)
            { 
                bool isChar = (random.Next(2) % 2 == 0);// 输出字母还是数字 
                if (isChar)
                { // 字符串

                    int choice = random.Next(2) % 2 == 0 ? 65 : 97; // 取得大写字母还是小写字母 
                    ret.Append((char)(choice + random.Next(26))); 
                }
                else
                { // 数字 
                    ret.Append((random.Next(10))); 
                } 
            } 
            return ret.ToString();
        }
        /// <summary>
        /// 获取图片路径+名称
        /// 平扬
        /// 2015年4月9日 09:22:55
        /// </summary>
        /// <returns></returns>
        public static string CreateImageName(string fileExt = ".jpg")
        {
            YxLiCai.Tools.NoSql.RedisCache.RedisCache redis = new NoSql.RedisCache.RedisCache();
            long imgName = redis.Incr(YxLiCai.Tools.Const.RedisCacheKey.ImageIdentity, DateTime.Now.AddHours(1));//为当前图片增加每小时内的唯一标识
            return string.Concat(DateTime.Now.ToString("yyyy/MM/dd/HH/mmss"), imgName, fileExt);
        }
    }
}
