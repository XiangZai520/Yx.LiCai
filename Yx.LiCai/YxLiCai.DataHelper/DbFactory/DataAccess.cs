using System;
using YxLiCai.DataHelper.Config;
using YxLiCai.DataHelper.Common;
using YxLiCai.DataHelper.DBUtility;
namespace YxLiCai.DataHelper.DbFactory
{

    /// <summary>
    /// 数据访问助手类
    /// </summary>
    public sealed class DataAccess
    {

        #region 变量
        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();
        /// <summary>
        /// 数据库操作类
        /// </summary>
        private static DbHelper dbHelper = null;
        #endregion

        #region 数据库实例
        /// <summary>
        /// 数据库实例
        /// </summary>
        public static DbHelper Instance
        {
            get
            {
                if (dbHelper == null)
                {
                    lock (lockHelper)
                    {
                        if (dbHelper == null)
                        {
                            try
                            {
                                dbHelper = (DbHelper)Activator.CreateInstance(Type.GetType("DataHelper.DBUtility." + BaseConfig.BaseType + "Helper"));
                            }
                            catch
                            {
                                throw new Exception("请检查web.config节点数据库类型DataType是否正确");
                            }
                        }
                    }
                }
                return dbHelper;
            }
        }
        #endregion

    }
}
