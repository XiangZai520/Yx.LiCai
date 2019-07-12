using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;
namespace YxLiCai.DataHelper.IDataHelper
{
    /// <summary>
    /// 数据访问接口
    /// </summary>
    public interface IDbProvider
    {

        #region 执行存储过程或SQL语句返回DbDataReader
        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        DbDataReader ExecuteReader(
           string Sql,
           Hashtable hashtable
           );
        #endregion

        #region 执行存储过程或SQL语句
        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        bool ExecuteNonQuery(
           string Sql,
           Hashtable hashtable
           );
        #endregion

        #region 执行存储过程或SQL语句返回第一行第一列
        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        object ExecuteScalar(
            string Sql,
            Hashtable hashtable
            );
        #endregion

        #region 执行存储过程或SQL语句返回DataSet
        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="hashtable">参数值</param>
        /// <returns></returns>
        DataSet ExecuteDataSet(
            string Sql,
            Hashtable hashtable
            );
        #endregion

    }
}
