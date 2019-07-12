using System;
using System.Collections;
using System.Configuration;
namespace YxLiCai.DataHelper.Common
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    internal class ConnectionStringHelper
    {

        #region 变量
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static readonly string ConnectionString = null;
        /// <summary>
        /// 链接字符串缓存
        /// </summary>
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
        #endregion

        #region 静态构造函数
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ConnectionStringHelper()
        {
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                hashtable["ConnectionString"] = ConnectionString;
            }
        }
        #endregion

        #region 获取数据库连接字符串
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="connectionString">数据库连接字符串名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionString)
        {
            if (connectionString == null)
            {
                connectionString = "AutoHomeConnectionString";
            }
            if (hashtable[connectionString] == null)
            {
                hashtable[connectionString] = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            }
            return hashtable[connectionString].ToString();
        }
        #endregion

    }
}
