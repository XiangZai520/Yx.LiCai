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
    /// 用户月账户
    /// </summary>
    public class UserCountMonthDao
    {
        /// <summary>
        /// 获取用户月资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public decimal GetMyMoney(long Id)
        {
            decimal MyMoney = 0;
            string strSql = @"SELECT  TRUNCATE(amount_invest,2) as amount FROM user_count_month WHERE user_id=?Id LIMIT 0,1";
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
    }
}
