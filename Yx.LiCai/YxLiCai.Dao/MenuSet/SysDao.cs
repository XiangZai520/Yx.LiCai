using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model;

namespace YxLiCai.Dao.MenuSet
{
    /// <summary>
    /// 系统操作类
    /// </summary>
    public class SysDao
    {
        /// <summary>
        /// 插入系统日志 操作类型(0系统1项目2产品3放款4还款5提现6赎回7活动8会员)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddSysLog(SysActionLogModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sys_action_log(");
            strSql.Append("admin_name,admin_id,op_type,remark,c_time)");
            strSql.Append(" values (");
            strSql.Append("?admin_name,?admin_id,?op_type,?remark,?c_time)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?admin_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?admin_id", MySqlDbType.Int32,11),
					new MySqlParameter("?op_type", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?c_time", MySqlDbType.DateTime)};
            parameters[0].Value = model.adminName;
            parameters[1].Value = model.adminId;
            parameters[2].Value = model.OpType;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.CTime;
            return YxLiCai.DataHelper.DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(strSql.ToString(), "插入系统日志", parameters);
        }

    }
}
