using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.User;
using YxLiCai.Tools.Util;

namespace YxLiCai.Dao.User
{
    /// <summary>
    /// 用户邀请 by张浩然 2015年6月30日
    /// </summary>
    public class UserInvitationDao
    {
        /// <summary>
        /// 新增一条用户邀请数据 by张浩然 2015年6月30日
        /// </summary>
        public bool AddUserInvitation(UserInvitationModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_invitation(");
            strSql.Append("user_id,invited_user_id,c_time,m_time,creator_id,version,remark,act_id,invited_login_name)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?invited_user_id,?c_time,?m_time,?creator_id,?version,?remark,?act_id,?invited_login_name)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?invited_user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,4),
					new MySqlParameter("?version", MySqlDbType.Int32,4),
					new MySqlParameter("?remark", MySqlDbType.VarChar,100),
					new MySqlParameter("?act_id", MySqlDbType.Int32,11),
                    new MySqlParameter("?invited_login_name",MySqlDbType.VarChar,100)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.invited_user_id;
            parameters[2].Value = model.c_time;
            parameters[3].Value = model.m_time;
            parameters[4].Value = model.creator_id;
            parameters[5].Value = model.version;
            parameters[6].Value = model.remark;
            parameters[7].Value = model.act_id;
            parameters[8].Value = model.invited_login_name;
           
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "新增一条用户邀请数据 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 获取后5条邀请记录（前台） by张浩然 2015-7-1
        /// </summary>
        /// <returns></returns>
        public List<UserInvitationModel> GetUserInvitationList(long userid) 
        { 
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select invited_login_name,c_time ");
            strSql.Append(" from user_invitation where user_id=?user_id");
            strSql.Append(" order by c_time desc limit 0,10 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20)};
            parameters[0].Value = userid;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取后5条邀请记录（前台） by张浩然 2015-7-1", parameters);
            List<UserInvitationModel> list = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<UserInvitationModel>();
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    UserInvitationModel model = new UserInvitationModel();

                    model.invited_login_name = dr["invited_login_name"].ToString();
                    model.c_time = YxLiCai.Tools.Util.ParseHelper.ToDatetime(dr["c_time"]);

                    list.Add(model);
                }
            }
            return list;
        }

    }
}
