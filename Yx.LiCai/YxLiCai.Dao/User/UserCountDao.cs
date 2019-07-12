using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.User
{
    /// <summary>
    /// 用户总账户数据层
    /// </summary>
    public class UserCountDao
    {
        /// <summary>
        /// 获取用户总资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public decimal GetMyMoney(long Id)
        {
            decimal MyMoney = 0m;
            string strSql = @"SELECT  amount_invest FROM user_account WHERE user_id=?Id LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,20)
			};
            parameters[0].Value = Id;

            var result = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql, "获取账户金额", parameters);
            if (result != null)
            {
                MyMoney = Convert.ToDecimal(result);
            }
            return MyMoney;
        }
        /// <summary>
        /// 获取用户总资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public decimal GetAllMyMoney(long Id)
        {
            decimal MyMoney = 0m;
            string strSql = @"SELECT   TRUNCATE(amount_invest+amount_blance,2) FROM user_account WHERE user_id=?Id LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,20)
			};
            parameters[0].Value = Id;

            var result = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql, "获取账户金额", parameters);
            if (result != null)
            {
                MyMoney = Convert.ToDecimal(result);
            }
            return MyMoney;
        }
        /// <summary>
        /// 获取保理userid
        /// </summary>
        /// <param name="Id">用户类型1保理公司</param>
        /// <returns></returns>
        public long GetUserIDByUserType(int user_type = 1)
        {
            long UserID = 0;
            string strSql = @"SELECT  id FROM user_info WHERE user_type=?user_type LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_type", MySqlDbType.Int32)
			};
            parameters[0].Value = user_type;

            var result = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql, "获取保理userid", parameters);
            if (result != null)
            {
                UserID = Convert.ToInt64(result);
            }
            return UserID;
        }
        /// <summary>
        /// 根据用户id获取账户情况
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable GetUserAccountByUserID(long user_id)
        {
            string strSql = @"SELECT  user_id,amount_invest,amount_blance,amount_blance_fz FROM user_account WHERE user_id=?user_id LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int32)
			};
            parameters[0].Value = user_id;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据用户id获取账户情况", parameters).Tables[0];

        }

    }
}
