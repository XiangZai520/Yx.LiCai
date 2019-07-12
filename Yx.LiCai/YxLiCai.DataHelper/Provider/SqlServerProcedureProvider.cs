using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using YxLiCai.DataHelper.IDataHelper;
using YxLiCai.DataHelper.DbFactory;
using YxLiCai.DataHelper.Parameters;
namespace YxLiCai.DataHelper.Provider
{
    /// <summary>
    /// 存储过程操作类
    /// </summary>
    internal class SqlServerProcedureProvider :BaseDbHelper, IDbProvider
    {
        
        #region 执行存储过程或SQL语句返回DbDataReader
        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(
            string Sql,
            Hashtable hashtable
            )
        {
            return dbHelper.ExecuteReader(Sql, CommandType.StoredProcedure, SqlServerParameter.CreateProcedureParameter(Sql, hashtable));
        }
        #endregion

        #region 执行存储过程或SQL语句
        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        public bool ExecuteNonQuery(
            string Sql,
            Hashtable hashtable
            )
        {
            return dbHelper.ExecuteNonQuery(Sql, CommandType.StoredProcedure, SqlServerParameter.CreateProcedureParameter(Sql, hashtable));
        }
        #endregion

        #region 执行存储过程或SQL语句返回第一行第一列
        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        public object ExecuteScalar(
            string Sql,
            Hashtable hashtable
            )
        {
            return dbHelper.ExecuteScalar(Sql, CommandType.StoredProcedure, SqlServerParameter.CreateProcedureParameter(Sql, hashtable));
        }
        #endregion

        #region 执行存储过程或SQL语句返回DataSet
        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(
            string Sql,
            Hashtable hashtable
            )
        {
            return dbHelper.ExecuteDataSet(Sql, CommandType.StoredProcedure, SqlServerParameter.CreateProcedureParameter(Sql, hashtable));
        }
        #endregion

    }
}
