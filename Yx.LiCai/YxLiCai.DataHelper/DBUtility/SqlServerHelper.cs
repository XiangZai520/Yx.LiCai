using System;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using YxLiCai.DataHelper.Common;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace YxLiCai.DataHelper.DBUtility
{
    /// <summary>
    /// 轻量级SQLSERVER数据库操作类
    /// </summary>
    public class SqlServerHelper : DbHelper
    {
        #region 变量
        /// <summary>
        /// 其他链接字符串
        /// </summary>
        private string ConnectionString = null;
        private string NotesString1 = "--";
        private string NotesString2 = ";--";
        /// <summary>
        /// 链接字符串缓存
        /// </summary>
        private static Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlServerHelper()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region 创建执行命令对象
        /// <summary>
        /// 创建执行命令对象
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">SQL语句参数</param>
        /// <returns></returns>
        protected override DbCommand CreateCommand(
            string Sql, 
            CommandType ComType, 
            DbParameter[] Prams
            )
        {
            DbConnection Cn = new SqlConnection(ConnectionStringHelper.GetConnectionString(ConnectionString));
            DbCommand Cmd = Cn.CreateCommand();
            Cmd.CommandType = ComType;
            Cmd.CommandText = Sql;
            Cmd.CommandTimeout = 120;
            if (Prams != null)
            {
                Cmd.Parameters.AddRange(Prams);
            }
            DbParameter p =new  System.Data.SqlClient.SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null);
            Cmd.Parameters.Add(p);
            return Cmd;
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
        protected override DbParameter MakeParam(
            string PramName,
            DbType dbType,
            int Size,
            object Value,
            ParameterDirection Direction
        )
        {
            DbParameter dbParameter = new SqlParameter();
            dbParameter.ParameterName = PramName;
            dbParameter.DbType = dbType;
            if (Size == 16
                ||Size==-1
                )
            {
                if (Value is byte[])
                {
                    dbParameter.Size = ((byte[])Value).Length;
                }
                else
                {
                    dbParameter.Size = Value.ToString().Length;
                }
            }
            else
            {
                dbParameter.Size = Size;
            }
            dbParameter.Direction = Direction;
            if (Value != null && Direction == ParameterDirection.Input)
            {
                dbParameter.Value = Value;
            }
            return dbParameter;
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="PramName">参数名称</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">数据大小</param>
        /// <param name="Value">参数值</param>
        /// <param name="Direction">参类型</param>
        /// <returns></returns>
        public override DbParameter[] MakeInParams(ref string sql,
            string PramName,
            DbType dbType,
            int Size,
            object Value
        )
        {
            string[] idArray = Value.ToString().Split(',');
            
            StringBuilder idParameter = new StringBuilder();
            DbParameter[] paras =new DbParameter[idArray.Length];
           
            for (int i = 0; i < idArray.Length; i++)
            {
                paras[i] = base.MakeInParam( PramName + i, dbType, Size, int.Parse(idArray[i]).ToString());
                idParameter.Append( PramName + i + ",");
            }

            sql = sql.Replace(PramName, idParameter.ToString().TrimEnd(','));

            return paras;
        }
        #endregion

        #region 执行存储过程或SQL语句返回DbDataReader
        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="notes">注释文本</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override DbDataReader ExecuteReader(string Sql, string notes,  params DbParameter[] Prams)
        {
            if (Sql.Trim().EndsWith(";")){Sql = Sql + NotesString1 + notes;}
            else { Sql = Sql + NotesString2 + notes; }
            return  ExecuteReader(Sql, CommandType.Text,Prams);
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override DbDataReader ExecuteReader(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            )
        {
            DbCommand Cmd = CreateCommand(Sql, ComType, Prams);
            Cmd.Connection.Open();
            DbDataReader reader = (DbDataReader)Cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //<<<---Added By dongtaizhao On 2009-09-28
            Cmd.Parameters.Clear();
            //--->>>Added By dongtaizhao On 2009-09-28

            if (TraceHelper.IsTrace() == true)
            {
                HttpContext.Current.Trace.IsEnabled = true;
                HttpContext.Current.Trace.Write("MS-SQL", TraceHelper.BuildParameters(Prams) + " " + Sql);
            }

            return reader;
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回DbDataReader
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(
            string Sql,
            CommandType ComType
            )
        {
            return ExecuteReader(Sql, ComType, null);
        }
        #endregion

        #region 执行存储过程或SQL语句

        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="notes">注释文本</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override bool ExecuteNonQuery(string Sql, string notes,  params DbParameter[] Prams)
        {
            if (Sql.Trim().EndsWith(";")) { Sql = Sql + NotesString1 + notes; }
            else { Sql = Sql + NotesString2 + notes; }
            return ExecuteNonQuery(Sql, CommandType.Text, Prams);
        }

        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="notes">注释文本</param>
        /// <param name="reslut">执行结果</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override bool ExecuteNonQuery(string Sql, string notes,out int reslut, params DbParameter[] Prams)
        {
            if (Sql.Trim().EndsWith(";")) { Sql = Sql + NotesString1 + notes; }
            else { Sql = Sql + NotesString2 + notes; }
            reslut = 0;
            return ExecuteNonQuery(Sql,out reslut, Prams);
        }

        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override bool ExecuteNonQuery(string Sql, CommandType ComType, params DbParameter[] Prams)
        {
            DbCommand Cmd = CreateCommand(Sql, ComType, Prams);
            Cmd.Connection.Open();
            int i =Cmd.ExecuteNonQuery();
            Cmd.Connection.Close();
            //<<<---Added By dongtaizhao On 2009-09-28
            Cmd.Parameters.Clear();
            //--->>>Added By dongtaizhao On 2009-09-28

            if (TraceHelper.IsTrace() == true)
            {
                HttpContext.Current.Trace.IsEnabled = true;
                HttpContext.Current.Trace.Write("MS-SQL", TraceHelper.BuildParameters(Prams) + " " + Sql);
            }

            return i > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="ComType"></param>
        /// <param name="reslut">默认值是0，注意避开使用</param>
        /// <param name="Prams"></param>
        /// <returns></returns>
        public override bool ExecuteNonQuery(string Sql, CommandType ComType, out int reslut, params DbParameter[] Prams)
        {
            reslut = 0;
            DbCommand Cmd = CreateCommand(Sql, ComType, Prams);
            Cmd.Connection.Open();
            Cmd.ExecuteNonQuery();
            Cmd.Connection.Close();
            reslut = int.Parse(Cmd.Parameters["ReturnValue"].Value.ToString());
            //<<<---Added By dongtaizhao On 2009-09-28
            Cmd.Parameters.Clear();
            //--->>>Added By dongtaizhao On 2009-09-28

            if (TraceHelper.IsTrace() == true)
            {
                HttpContext.Current.Trace.IsEnabled = true;
                HttpContext.Current.Trace.Write("MS-SQL", TraceHelper.BuildParameters(Prams) + " " + Sql);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="result">默认值是0，注意避开使用</param>
        /// <param name="Prams"></param>
        /// <returns></returns>
        public override bool ExecuteNonQuery(string Sql, out int result, params DbParameter[] Prams)
        {
            return ExecuteNonQuery(Sql, CommandType.Text, out result, Prams);
        }


        /// <summary>
        /// 执行存储过程或SQL语句
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(
            string Sql,
            CommandType ComType
            )
        {
            return ExecuteNonQuery(Sql, ComType, null);
        }
        #endregion

        #region 执行存储过程或SQL语句返回第一行第一列

        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="Prams">注释文本</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override object ExecuteScalar(string Sql, string notes,  params DbParameter[] Prams)
        {
            if (Sql.Trim().EndsWith(";")) { Sql = Sql + NotesString1 + notes; }
            else { Sql = Sql + NotesString2 + notes; }
            return ExecuteScalar(Sql, CommandType.Text, Prams);
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override object ExecuteScalar(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            )
        {
            DbCommand Cmd = CreateCommand(Sql, ComType, Prams);

            Cmd.Connection.Open();
            //<<<---Modified By dongtaizhao on 2009-09-28
            //return Cmd.ExecuteScalar();
            object objectValue = Cmd.ExecuteScalar();
            Cmd.Parameters.Clear();
            Cmd.Connection.Close();

            if (TraceHelper.IsTrace() == true)
            {
                HttpContext.Current.Trace.IsEnabled = true;
                HttpContext.Current.Trace.Write("MS-SQL", TraceHelper.BuildParameters(Prams) + " " + Sql);
            }

            return objectValue;
            //--->>>Modified By dongtaizhao on 2009-09-28
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回第一行第一列
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <returns></returns>
        public object ExecuteScalar(
            string Sql,
            CommandType ComType
            )
        {
            return ExecuteScalar(Sql, ComType, null);
        }
        #endregion

        #region 执行存储过程或SQL语句返回DataSet
        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="notes">注释文本</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(string Sql, string notes,  params DbParameter[] Prams)
        {
            if (Sql.Trim().EndsWith(";")) { Sql = Sql + NotesString1 + notes; }
            else { Sql = Sql + NotesString2 + notes; }
            return ExecuteDataSet(Sql, CommandType.Text, Prams);
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <param name="Prams">参数</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSet(
            string Sql,
            CommandType ComType,
            params DbParameter[] Prams
            )
        {
            DbDataAdapter dap = new SqlDataAdapter();
            DataSet dat = new DataSet();
                dap.SelectCommand = CreateCommand(Sql, ComType, Prams);
                dap.Fill(dat);
                //<<<---Added By dongtaizhao by 2009-09-28
                dap.SelectCommand.Parameters.Clear();
                //--->>>Added By dongtaizhao by 2009-09-28

            //判断是否显示跟踪信息
            bool ShowTrace = TraceHelper.IsTrace();
            if (ShowTrace == true)
            {
                HttpContext.Current.Trace.IsEnabled = true;
                HttpContext.Current.Trace.Write("MS-SQL", TraceHelper.BuildParameters(Prams) + " " + Sql);
            }

            return dat;
        }

        /// <summary>
        /// 执行存储过程或SQL语句返回DataSet
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <param name="ComType">语句类型,存储过程或SQL语句</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(
            string Sql,
            CommandType ComType
            )
        {
            return ExecuteDataSet(Sql, ComType, null);
        }
        #endregion

        #region 执行一组SQL语句并且启动事务
        /// <summary>
        /// 执行一组SQL语句并且启动事务
        /// </summary>
        /// <param name="Sql">要执行的SQL语句或存储过程名称</param>
        /// <returns></returns>
        public override bool ExecuteTransSql(
            string Sql
            )
        {
            DbCommand Cmd = CreateCommand(Sql, CommandType.Text, null);
            DbTransaction Trans = null;
                Trans = Cmd.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                Cmd.Transaction = Trans;
                Cmd.Connection.Open();
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                return true;
        }
        public override bool ExecutMultiSQLwithTransaction(List<string> p_SqlArray, List<DbParameter[]> p_ParameterArray)
        {
            //SQL语句数量与参数数组数量不符
            if (p_SqlArray.Count != p_ParameterArray.Count) return false;
            DbTransaction Trans = null;
            DbCommand Cmd = null;
            try
            {
                //Trans = new SqlConnection(ConnectionStringHelper.GetConnectionString(ConnectionString)).BeginTransaction(IsolationLevel.ReadCommitted);
                for (int i = 0; i < p_SqlArray.Count; i++)
                {
                    Cmd = CreateCommand(p_SqlArray[i], CommandType.Text, p_ParameterArray[i]);
                    if (Trans == null)
                    {
                        Cmd.Connection.Open();
                        Trans = Cmd.Connection.BeginTransaction();
                    }
                    Cmd.Transaction = Trans;
                    Cmd.Connection = Trans.Connection;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                return true;
            }
            catch
            {
                Trans.Rollback();
                return false;
            }
            finally
            {
                Cmd.Connection.Close();
            }

        }
        #endregion
        
    }
}
