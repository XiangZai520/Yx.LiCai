using System;
using System.Data.Common;
using System.Collections;
using System.Xml;
using System.Web;
using System.Data.SqlClient;
using YxLiCai.DataHelper.DataType;
using YxLiCai.DataHelper.Config;
namespace YxLiCai.DataHelper.Parameters
{
    /// <summary>
    /// SqlServer存储过程参数
    /// </summary>
    internal class SqlServerParameter:BaseDbHelper
    {

        #region 变量
        /// <summary>
        /// 缓存存储过程参数
        /// </summary>
        private static Hashtable ParmCache = Hashtable.Synchronized(new Hashtable());
        #endregion

        #region 创建存储过程参数
        /// <summary>
        /// 创建存储过程参数
        /// </summary>
        /// <param name="ProcName">存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        public static DbParameter[] CreateProcedureParameter(
            string ProcName,
            Hashtable hashtable
            )
        {
            if (hashtable == null)
            {
                return null;
            }
            DbParameter[] Parameter = GetCachedParameters(ProcName);
            if (Parameter == null)
            {
                XmlDocument xmlDocument = BaseConfig.ProcConfig;
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("StoredProcedures/StoredProcedure[@name='" + ProcName + "']/Parameters/Parameter");
                if (xmlNodeList.Count > 0)
                {
                    Parameter = new SqlParameter[xmlNodeList.Count];
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        Parameter[i] = dbHelper.MakeInParam(xmlNodeList[i].Attributes["name"].Value, DbTypeHelper.GetSqlServerType(xmlNodeList[i].Attributes["dbtype"].Value), Int32.Parse(xmlNodeList[i].Attributes["size"].Value), hashtable[xmlNodeList[i].Attributes["name"].Value]);
                    }
                    CacheParameters(ProcName, Parameter);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                for (int i = 0; i < Parameter.Length; i++)
                {
                    Parameter[i].Value = hashtable[Parameter[i].ParameterName];
                }
            }
            return Parameter;
        }
        #endregion

        #region 缓存
        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        /// <param name="CommandParameters">缓存对象</param>
        private static void CacheParameters(string CacheKey,
            DbParameter[] CommandParameters
            )
        {
            DbParameter[] ClonedParms = new SqlParameter[CommandParameters.Length];
            for (int i = 0; i < ClonedParms.Length; i++)
            {
                ClonedParms[i] = ((ICloneable)CommandParameters[i]).Clone() as DbParameter;
                ClonedParms[i].Value = DBNull.Value;
            }
            ParmCache[CacheKey] = ClonedParms;
        }
        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        /// <returns></returns>
        private static DbParameter[] GetCachedParameters(string CacheKey)
        {
            DbParameter[] CachedParms = ParmCache[CacheKey] as DbParameter[];
            if (CachedParms == null)
            {
                return null;
            }
            DbParameter[] ClonedParms = new SqlParameter[CachedParms.Length];
            for (int i = 0; i < ClonedParms.Length; i++)
            {
                ClonedParms[i] = ((ICloneable)CachedParms[i]).Clone() as DbParameter;
            }
            return ClonedParms;
        }
        #endregion

        #region 清空缓存参数
        /// <summary>
        /// 清空缓存参数
        /// </summary>
        public static void ClearCachePrameters()
        {
            ParmCache.Clear();
        }
        #endregion
    }
}
