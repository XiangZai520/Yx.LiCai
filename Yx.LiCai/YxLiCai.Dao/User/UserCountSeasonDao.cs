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
    /// 用户季账户
    /// </summary>
    public class UserCountSeasonDao
    {
        /// <summary>
        /// 获取用户季账户资产
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public decimal GetMyMoney(long Id)
        {
            decimal MyMoney = 0;
            string strSql = @"SELECT  amount_invest as amount FROM user_count_season WHERE user_id=?Id LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11)
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
        /// 更新赎回账户
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="LockMoney">冻结金额</param>
        public void AddLockMoney(long UserId, decimal LockMoney, decimal order_investment)
        {
            string strSql = @"UPDATE user_count_season SET principal_fz=principal_fz+?order_investment,
                                     amount_invest = amount_invest - ?order_investment WHERE user_id=?UserId;
                                    UPDATE user_account SET amount_invest=amount_invest-?order_investment WHERE user_id=?UserId;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11),
                    new MySqlParameter("?LockMoney", MySqlDbType.Decimal),
                     new MySqlParameter("?order_investment", MySqlDbType.Decimal)
			};
            parameters[0].Value = UserId;
            parameters[1].Value = LockMoney;
            parameters[2].Value = order_investment;
            DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "增加冻结金额", parameters);
        }
    }
}
