using System;
using YxLiCai.DataHelper.Config;
using YxLiCai.DataHelper.Provider;
using YxLiCai.DataHelper.IDataHelper;
namespace YxLiCai.DataHelper.DbProviderFactory
{
    /// <summary>
    /// 基础数据操作类
    /// </summary>
    public sealed class DbProvider
    {

        #region 变量
        /// <summary>
        /// 数据操作类
        /// </summary>
        private static IDbProvider dbProvider = null;
        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();
        #endregion

        #region 数据操作类实例
        /// <summary>
        /// 数据操作类实例
        /// </summary>
        public static IDbProvider Instance
        {
            get
            {
                if (dbProvider == null)
                {
                    lock (lockHelper)
                    {
                        if (dbProvider == null)
                        {
                            try
                            {
                                dbProvider = (IDbProvider)Activator.CreateInstance(Type.GetType("DataHelper.Provider." + BaseConfig.BaseType + "ProcedureProvider"));
                            }
                            catch
                            {
                                throw new Exception("请检查web.config节点数据库类型DataType是否正确");
                            }
                        }
                    }
                }
                return dbProvider;
            }
        }
        #endregion

    }
}
