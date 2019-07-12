using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using System.Data.Common;

namespace YxLiCai.Dao.Redemption
{
    /// <summary>
    /// 赎回数据层
    /// </summary>
    public class DedemptionDao
    {
        /// <summary>
        /// 用户赎回
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderinfoids"></param>
        /// <returns></returns>
        public bool UserDedemption(long Userid, List<long> orderinfoids)
        {
            decimal rate = YxLiCai.Tools.Const.SystemConst.RedemptionRate;

            string idStr = "";
            foreach (int id in orderinfoids)
            {
                idStr = idStr + (idStr == "" ? "" : ",") + id.ToString();
            }
            if (idStr == "")
            {
                return false;
            }
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //更新user_account
            string strSql = @"UPDATE user_account a
                        INNER JOIN (
                        SELECT user_id,SUM(amount) amount 
                        FROM user_ord_info
                        WHERE user_id=?Userid AND id IN (" + idStr + @") AND id NOT IN (SELECT ord_id FROM user_redemption)
                        GROUP BY user_id) b ON a.user_id=b.user_id
                        SET a.amount_invest=a.amount_invest-b.amount;";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                            new MySqlParameter("?Userid", MySqlDbType.Int64,20)
			};
            parameters[0].Value = Userid;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //更新user_count_season user_count_year
            strSql = @"UPDATE user_count_season a
                        INNER JOIN (
                        SELECT user_id,SUM(amount) amount 
                        FROM user_ord_info
                        WHERE user_id=?Userid AND id IN (" + idStr + @") AND pdt_type IN (2,3) AND id NOT IN (SELECT ord_id FROM user_redemption)
                        GROUP BY user_id) b ON a.user_id=b.user_id
                        SET a.principal_fz=a.principal_fz+b.amount,a.amount_invest=a.amount_invest-b.amount;
                        UPDATE user_count_year a
                        INNER JOIN (
                        SELECT user_id,SUM(amount) amount 
                        FROM user_ord_info
                        WHERE user_id=?Userid AND id IN (" + idStr + @") AND pdt_type IN (4) AND id NOT IN (SELECT ord_id FROM user_redemption)
                        GROUP BY user_id) b ON a.user_id=b.user_id
                        SET a.principal_fz=a.principal_fz+b.amount,a.amount_invest=a.amount_invest-b.amount;";

            parameters = new MySqlParameter[]
            {
                            new MySqlParameter("?Userid", MySqlDbType.Int64,20)
			};
            parameters[0].Value = Userid;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //插入赎回表
            strSql = @"INSERT INTO user_redemption(c_time,user_id,ord_id,status,op_status,amount,principal,interest,penalty_rate,version)
                                SELECT NOW(),user_id,id,0,0,(amount),amount,0,?redemptionRate,0
                                FROM user_ord_info
                                WHERE id IN (" + idStr + @") AND user_id=?Userid AND id NOT IN (SELECT ord_id FROM user_redemption);";

            parameters = new MySqlParameter[]{
                            new MySqlParameter("?Userid", MySqlDbType.Int64,11),
                            new MySqlParameter("?redemptionRate", MySqlDbType.Decimal,11)
                    };
            parameters[0].Value = Userid;
            parameters[1].Value = rate;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            //插入user_amount_rec
            strSql = @"INSERT INTO user_amount_rec (user_id,pdt_type,TYPE, amount,c_time) 
                        SELECT user_id,pdt_type,2,amount,?c_time
                        FROM user_ord_info
                        WHERE id IN (" + idStr + @") AND user_id=?Userid;";

            parameters = new MySqlParameter[]
            {
                            new MySqlParameter("?Userid", MySqlDbType.Int64,20),
                            new MySqlParameter("?c_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = Userid;
            parameters[1].Value = DateTime.Now;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            //更新order_info
            strSql = @"UPDATE user_ord_info SET atone_status=1  WHERE user_id=?userID AND ord_status=1 AND atone_status=0 AND id IN (" + idStr + ");";

            parameters = new MySqlParameter[]
            {
                            new MySqlParameter("?Userid", MySqlDbType.Int64,20)
			};
            parameters[0].Value = Userid;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
    }
}
