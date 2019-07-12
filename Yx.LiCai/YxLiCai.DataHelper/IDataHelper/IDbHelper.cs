using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
namespace YxLiCai.DataHelper.IDataHelper
{

    /// <summary>
    /// 轻量级数据操作类
    /// </summary>
    public interface IDbHelper
    {

        #region 执行存储过程或SQL语句返回DbDataReader
        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        DbDataReader ExecuteReader(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );
        #endregion

        #region 执行存储过程或SQL语句
        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        bool ExecuteNonQuery(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );
        #endregion

        #region 执行存储过程或SQL语句返回第一行第一列
        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        object ExecuteScalar(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );
        #endregion

        #region 执行存储过程或SQL语句返回DataSet
        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        DataSet ExecuteDataSet(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );
        #endregion

        #region 执行一组SQL语句并且启动事务
        /// <summary>
        /// 执行一组SQL语句并且启动事务
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <returns></returns>
        bool ExecuteTransSql(
            string Sql
            );
        #endregion

    }
}
