using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using YxLiCai.DataHelper.IDataHelper;
using YxLiCai.DataHelper.DBUtility;
using System.Collections.Generic;
using System.Web;

namespace YxLiCai.DataHelper.Common
{
    /// <summary>
    /// 轻量级数据层
    /// </summary>
    public abstract class DbHelper : IDbHelper, ICommand
    {

        #region 创建执行命令对象
        /// <summary>
        /// 创建执行命令对象
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">SQL语句参数</param>
        /// <returns></returns>
        protected abstract DbCommand CreateCommand(
            string Sql,
            CommandType ComType,
            DbParameter[] Prams
            );
        #endregion

        #region 创建输入参数
        /// <summary>
        /// 创建输入参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        ///<param name="dbType">数据类型</param>
        ///<param name="Size">数据大小</param>
        /// <param name="Value">参数值</param>
        /// <returns></returns>
        public DbParameter MakeInParam(
            string ParamName,
            DbType dbType,
            int Size,
            object Value
            )
        {
            return MakeParam(ParamName, dbType, Size, Value, ParameterDirection.Input);
        }
        
        #endregion

        #region 创建参数
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="PramName">参数名称</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">数据大小</param>
        /// <param name="Value">参数值</param>
        /// <param name="Direction">参类型</param>
        /// <returns></returns>
        protected abstract DbParameter MakeParam(
            string PramName,
            DbType dbType,
            int Size,
            object Value,
            ParameterDirection Direction
        );

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="PramName">参数名称</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">数据大小</param>
        /// <param name="Value">参数值</param>
        /// <param name="Direction">参类型</param>
        /// <returns></returns>
        public abstract DbParameter[] MakeInParams(ref string sql,
            string PramName,
            DbType dbType,
            int Size,
            object Value
        );
        
        #endregion

        #region 传入返回值参数
        /// <summary>
        /// 传入返回值参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        ///<param name="dbType">数据类型</param>
        ///<param name="Size">数据大小</param>
        /// <returns></returns>
        public DbParameter MakeOutParam(
            string ParamName,
            DbType dbType,
            int Size
            )
        {
            return MakeParam(ParamName, dbType, Size, null, ParameterDirection.Output);
        }
        #endregion

        #region 执行存储过程或SQL语句返回DbDataReader
        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract DbDataReader ExecuteReader(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );

        public abstract DbDataReader ExecuteReader(string Sql,string notes,params DbParameter[] Prams);

        /// <summary>
        /// 执行SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句</param>
        /// <param name="Prams">参数值</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(
            string Sql,
            params object[] Prams
            )
        {
            string SQL = string.Format(Sql, this.ReplacePrams(Prams));
            return ExecuteReader(SQL, CommandType.Text, null);
        }
        #endregion

        #region 执行存储过程或SQL语句
        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract bool ExecuteNonQuery(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );

        public abstract bool ExecuteNonQuery(string Sql,CommandType ComType,out int result, params DbParameter[] Prams);

        public abstract bool ExecuteNonQuery(string Sql,string notes, params DbParameter[] Prams);


        public abstract bool ExecuteNonQuery(string Sql, string notes,out int result, params DbParameter[] Prams);

        public abstract bool ExecuteNonQuery(string Sql, out int result, params DbParameter[] Prams);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL值语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(
            string Sql,
            params object[] Prams
            )
        {
            string SQL = string.Format(Sql, this.ReplacePrams(Prams));
            return ExecuteNonQuery(SQL, CommandType.Text, null);
        }
        #endregion

        #region 执行存储过程或SQL语句返回第一行第一列
        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );

        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="notes">SQL语句注释</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(
            string Sql,
            string notes,params 
            DbParameter[] Prams
            );

        /// <summary>
        /// 执行SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句</param>
        /// <param name="Prams">参数值</param>
        /// <returns></returns>
        public object ExecuteScalar(
            string Sql,
            params object[] Prams
            )
        {
            string SQL = string.Format(Sql, this.ReplacePrams(Prams));
            return ExecuteScalar(SQL, CommandType.Text, null);
        }
        #endregion

        #region 执行存储过程或SQL语句返回DataSet
        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataSet(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            );

        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="notes">SQL语句注释</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataSet(
            string Sql,
            string notes,
            params DbParameter[] Prams
            );

        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句</param>
        /// <param name="Prams">参数值</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(
            String Sql,
            params object[] Prams
            )
        {
            string SQL = string.Format(Sql, this.ReplacePrams(Prams));
            return ExecuteDataSet(SQL, CommandType.Text, null);
        }

        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(
            String Sql
            )
        {
            return ExecuteDataSet(Sql, CommandType.Text, null);
        }
        #endregion

        #region 执行一组SQL语句并且启动事务
        /// <summary>
        /// 执行一组SQL语句并且启动事务
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <returns></returns>
        public abstract bool ExecuteTransSql(
            string Sql
            );
        /// <summary>
        /// 在同一事务内执行多条SQL语句。（SQL语句数组与参数数组需对应）
        /// </summary>
        /// <param name="p_SqlArray">SQL语句数组</param>
        /// <param name="p_ParameterArray">参数数组</param>
        /// <returns>成功与否</returns>
        public abstract bool ExecutMultiSQLwithTransaction(List<string> p_SqlArray, List<DbParameter[]> p_ParameterArray);
        #endregion

        #region 替换非法字符
        /// <summary>
        /// 替换非法字符
        /// </summary>
        /// <param name="Prams">要替换的字符</param>
        /// <returns>替换后的字符</returns>
        private object[] ReplacePrams(object[] Prams)
        {
            for (int i = 0; i < Prams.Length; i++)
            {
                if (Prams[i] is string)
                {
                    Prams[i] = Prams[i].ToString().Replace("'", "''");
                }
            }
            return Prams;
        }
        #endregion

    }
}
