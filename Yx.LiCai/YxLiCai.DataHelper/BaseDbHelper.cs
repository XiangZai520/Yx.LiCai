using System;
using System.Collections;
using YxLiCai.DataHelper.Common;
using YxLiCai.DataHelper.IDataHelper;
using YxLiCai.DataHelper.DbFactory;
using YxLiCai.DataHelper.DBUtility;
using YxLiCai.DataHelper.DbProviderFactory;
namespace YxLiCai.DataHelper
{
    /// <summary>
    /// 轻量级数据层调用接口
    /// </summary>
    public abstract class BaseDbHelper
    {
        /// <summary>
        /// XML配置文件方法
        /// </summary>
        protected static readonly IDbProvider dbProvider = DbProvider.Instance;
        /// <summary>
        /// 参数方法
        /// </summary>
        protected static readonly DbHelper dbHelper = DataAccess.Instance;
        /// <summary>
        /// 数据实例缓存
        /// </summary>
        private static Hashtable DbHelperCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 自定义字符串实例
        /// </summary>
        /// <param name="ConnectionString">连接字符串名称</param>
        /// <returns></returns>
        protected DbHelper BuildDbHelper(string ConnectionString)
        {
            if (DbHelperCache[ConnectionString] == null)
            {
                DbHelperCache[ConnectionString] = (DbHelper)new SqlServerHelper(ConnectionString);
            }
            return DbHelperCache[ConnectionString] as DbHelper;
        }
    }
}
