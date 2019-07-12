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
    /// 用户特享金 by张浩然 2015年6月30日
    /// </summary>
    public class UserSpecialAssetsDao
    {
        /// <summary>
        /// 新增一条用户特享金数据 by张浩然 2015年6月30日
        /// </summary>
        public bool AddUserSpecialAssets(UserSpecialAssetsModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_special_assets(");
            strSql.Append("user_id,act_id,special_id,amount,c_time,u_time,e_time,enable_day,use_status,rate,rate_increase,rate_added,interest_added,interest_paid,m_time,creator_id,version,remark,invited_user_id,invited_user_name)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?act_id,?special_id,?amount,?c_time,?u_time,?e_time,?enable_day,?use_status,?rate,?rate_increase,?rate_added,?interest_added,?interest_paid,?m_time,?creator_id,?version,?remark,?invited_user_id,?invited_user_name)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int32,20),
					new MySqlParameter("?act_id", MySqlDbType.Int32,4),
					new MySqlParameter("?special_id", MySqlDbType.Int32,4),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?u_time", MySqlDbType.DateTime),
					new MySqlParameter("?e_time", MySqlDbType.DateTime),
					new MySqlParameter("?enable_day", MySqlDbType.Int32,4),
					new MySqlParameter("?use_status", MySqlDbType.Int32,4),
					new MySqlParameter("?rate", MySqlDbType.Decimal,20),
					new MySqlParameter("?rate_increase", MySqlDbType.Decimal,20),
					new MySqlParameter("?rate_added", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_added", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_paid", MySqlDbType.Decimal,20),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,4),
					new MySqlParameter("?version", MySqlDbType.Int32,4),
					new MySqlParameter("?remark", MySqlDbType.VarChar,100),
                    new MySqlParameter("?invited_user_id", MySqlDbType.Int64,20),
                    new MySqlParameter("?invited_user_name", MySqlDbType.VarChar,250)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.act_id;
            parameters[2].Value = model.special_id;
            parameters[3].Value = model.amount;
            parameters[4].Value = model.c_time;
            parameters[5].Value = model.u_time;
            parameters[6].Value = model.e_time;
            parameters[7].Value = model.enable_day;
            parameters[8].Value = model.use_status;
            parameters[9].Value = model.rate;
            parameters[10].Value = model.rate_increase;
            parameters[11].Value = model.rate_added;
            parameters[12].Value = model.interest_added;
            parameters[13].Value = model.interest_paid;
            parameters[14].Value = model.m_time;
            parameters[15].Value = model.creator_id;
            parameters[16].Value = model.version;
            parameters[17].Value = model.remark;
            parameters[18].Value = model.invited_user_id;
            parameters[19].Value = model.invited_user_name; 
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "新增一条用户特享金数据 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 根据邀请人编号和活动编号获取邀请人参与特享金活动的次数 by 张浩然 2015-7-8
        /// </summary>
        /// <param name="userId">邀请人编号</param>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public int GetUserSpecialAssetsCountByInvitedUserIdAndActId(long userId, int actId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select count(id) ");
            strSql.Append(" from user_special_assets ");
            strSql.Append(" where user_id=?userId and act_id=?actId ");

            MySqlParameter[] parameters = {					
                    new MySqlParameter("?userId", MySqlDbType.Int64,20),
                    new MySqlParameter("?actId", MySqlDbType.Int32,11)
            };

            parameters[0].Value = userId;
            parameters[1].Value = actId;

            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据邀请人编号和活动编号获取邀请人参与特享金活动的次数 by 张浩然 2015-7-8", parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return YxLiCai.Tools.Util.ParseHelper.ToInt(obj);
            }
        }

    }
}
