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
    public class UserAddInterestDao
    {

        /// <summary>
        /// 增加一条用户加息券数据 by张浩然 2015年6月30日
        /// </summary>
        public bool AddUserAddInterest(UserAddInterestModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_add_interest(");
            strSql.Append("user_id,act_id,interest_id,interest_rate,enable_day,c_time,use_time,e_time,use_status,invest_id,version,remark,creator_id,m_time)");
            strSql.Append(" values (");
            strSql.Append("@user_id,@act_id,@interest_id,@interest_rate,@enable_day,@c_time,@use_time,@e_time,@use_status,@invest_id,@version,@remark,@creator_id,@m_time)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@user_id", MySqlDbType.Int32,20),
					new MySqlParameter("@act_id", MySqlDbType.Int32,4),
					new MySqlParameter("@interest_id", MySqlDbType.Int32,4),
					new MySqlParameter("@interest_rate", MySqlDbType.Decimal,20),
					new MySqlParameter("@enable_day", MySqlDbType.Int32,4),
					new MySqlParameter("@c_time", MySqlDbType.DateTime),
					new MySqlParameter("@use_time", MySqlDbType.DateTime),
					new MySqlParameter("@e_time", MySqlDbType.DateTime),
					new MySqlParameter("@use_status", MySqlDbType.Int32,4),
					new MySqlParameter("@invest_id", MySqlDbType.Int32,4),
					new MySqlParameter("@version", MySqlDbType.Int32,4),
					new MySqlParameter("@remark", MySqlDbType.VarChar,100),
					new MySqlParameter("@creator_id", MySqlDbType.Int32,4),
					new MySqlParameter("@m_time", MySqlDbType.DateTime)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.act_id;
            parameters[2].Value = model.interest_id;
            parameters[3].Value = model.interest_rate;
            parameters[4].Value = model.enable_day;
            parameters[5].Value = model.c_time;
            parameters[6].Value = model.use_time;
            parameters[7].Value = model.e_time;
            parameters[8].Value = model.use_status;
            parameters[9].Value = model.invest_id;
            parameters[10].Value = model.version;
            parameters[11].Value = model.remark;
            parameters[12].Value = model.creator_id;
            parameters[13].Value = model.m_time;

            //object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "新增一条用户加息券数据 by张浩然 2015年6月30日", parameters);
            //if (i != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "新增一条用户加息券数据 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 根据邀请人编号和活动编号获取邀请人参与加息券活动的次数 by 张浩然 2015-7-8
        /// </summary>
        /// <param name="userId">邀请人编号</param>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public int GetUserAddInterestCountByInvitedUserIdAndActId(long userId, int actId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select count(id) ");
            strSql.Append(" from user_add_interest ");
            strSql.Append(" where user_id=?userId and act_id=?actId ");

            MySqlParameter[] parameters = {					
                    new MySqlParameter("?userId", MySqlDbType.Int64,20),
                    new MySqlParameter("?actId", MySqlDbType.Int32,11)
            };

            parameters[0].Value = userId;
            parameters[1].Value = actId;

            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据邀请人编号和活动编号获取邀请人参与加息券活动的次数 by 张浩然 2015-7-8", parameters);
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
