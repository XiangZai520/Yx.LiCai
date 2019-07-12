using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.DataHelper;
using YxLiCai.Model.TransactionRecords;

namespace YxLiCai.Dao.TransactionRecords
{
    /// <summary>
    /// 交易记录
    /// </summary>
    public class TransactionRecordsDAO
    {

        #region 购买记录
        /// <summary>
        /// 购买记录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="InvestmentProducts">产品类型</param>
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet GetBuyLog(string UserName, int InvestmentProducts, DateTime time_min, DateTime time_max,
            int startIndex, int pageSize)
        {
            if (time_max != DateTime.MinValue)
            {
                time_max = new DateTime(time_max.Year, time_max.Month, time_max.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT phone,user_id,oid.interest_rate_coupon,oid.interest_rate,TRUNCATE(oid.amount,2) amount,oid.id,oid.c_time,oid.pdt_type
                                FROM user_ord_info oid 
                                INNER JOIN user_info u ON u.id=oid.user_id
                                WHERE (oid.ord_status=2 OR oid.ord_status=1) AND (?UserName=-1 OR u.phone = ?UserName)
                                AND (?InvestmentProducts=-1 OR oid.pdt_type=?InvestmentProducts)");
            string sqlWhere = " ";
            if (time_min != DateTime.MinValue)
            {
                sqlWhere += " AND  oid.`c_time` >=?time_min ";
            }
            if (time_max != DateTime.MinValue)
            {
                sqlWhere += " AND  oid.`c_time`<=?time_max ";
            }
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                sqlWhere = " AND oid.`c_time`>=?time_min AND oid.`c_time`<=?time_max ";
            }
            strSql.Append(" " + sqlWhere + " ");
            strSql.Append("ORDER BY oid.c_time DESC");
            strSql.Append(@" LIMIT ?StartIndex,?PageSize;");
            strSql.Append(@"SELECT COUNT(1)  FROM user_ord_info oid 
                                INNER JOIN user_info u ON u.id=oid.user_id
                                WHERE  (oid.ord_status=2 OR oid.ord_status=1) AND (?UserName=-1 OR u.phone = ?UserName)
                                AND (?InvestmentProducts=-1 OR oid.pdt_type=?InvestmentProducts)");
            strSql.Append(" " + sqlWhere + " ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,50),
                    new MySqlParameter("?InvestmentProducts", MySqlDbType.Int32),
                    new MySqlParameter("?time_min", MySqlDbType.DateTime),
                    new MySqlParameter("?time_max", MySqlDbType.DateTime),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = UserName;
            parameters[1].Value = InvestmentProducts;
            parameters[2].Value = time_min;
            parameters[3].Value = time_max;
            parameters[4].Value = startIndex;
            parameters[5].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "购买记录", parameters);
        }
        #endregion

        #region 购买记录转换实体
        /// <summary>
        /// 转为购买model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Ord_infoModel DataToRefundModel(DataRow row)
        {
            var model = new Ord_infoModel();
            #region match
            if (row != null)
            {

                if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                    model.Order = row["id"].ToString();
                }
                if (row.Table.Columns.Contains("c_time") && row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row.Table.Columns.Contains("user_id") && row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(row["user_id"].ToString());
                }
                if (row.Table.Columns.Contains("phone") && row["phone"] != null && row["phone"].ToString() != "")
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row.Table.Columns.Contains("interest_rate_coupon") && row["interest_rate_coupon"] != null && row["interest_rate_coupon"].ToString() != "")
                {
                    model.InterestRate = row["interest_rate_coupon"].ToString();
                }
                if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["amount"].ToString());
                }
                if (row.Table.Columns.Contains("pdt_type") && row["pdt_type"] != null && row["pdt_type"].ToString() != "")
                {
                    model.ProductName = int.Parse(row["pdt_type"].ToString());
                }
                if (row.Table.Columns.Contains("interest_rate") && row["interest_rate"] != null && row["interest_rate"].ToString() != "")
                {
                    model.YerRate = row["interest_rate"].ToString();
                }
            }
            #endregion

            return model;
        }
        #endregion

        #region 赎回记录
        /// <summary>
        /// 赎回记录
        /// </summary>
        /// <param name="UserName">用户名</param>      
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet Getuser_redemptionLog(string UserName, DateTime time_min, DateTime time_max,
            int startIndex, int pageSize)
        {
            if (time_max != DateTime.MinValue)
            {
                time_max = new DateTime(time_max.Year, time_max.Month, time_max.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT phone, 
                                   oid.c_time, 
                                   oid.id, 
                                   oid.ord_id, 
                                   oid.amount, 
                                   oid.principal, 
                                   oid.interest, 
                                   oid.user_id, 
                                   oid.penalty_rate, 
                                   TRUNCATE(oid.amount * penalty_rate,2)          AS LiquidatedDamages, 
                                   TRUNCATE( amount - ( amount * penalty_rate ),2) AS ActualAmount 
                            FROM   user_redemption oid 
                                   INNER JOIN user_info u 
                                           ON u.id = oid.user_id  
                                WHERE oid.op_status=2 and (?UserName=-1 OR u.phone = ?UserName)");
            string sqlWhere = " ";
            if (time_min != DateTime.MinValue)
            {
                sqlWhere += " AND  c_time >=?time_min ";
            }
            if (time_max != DateTime.MinValue)
            {
                sqlWhere += " AND  c_time<=?time_max ";
            }
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                sqlWhere = " AND c_time>=?time_min AND c_time<=?time_max ";
            }
            strSql.Append(" " + sqlWhere + " ");
            strSql.Append("ORDER BY c_time DESC ");
            strSql.Append(@" LIMIT ?StartIndex,?PageSize;");

            strSql.Append(@"SELECT COUNT(1)  FROM user_redemption  oid
                                           INNER JOIN user_info u 
                                                     ON u.id = oid.user_id                          
                                           WHERE  oid.op_status=2 and (?UserName=-1 OR u.phone = ?UserName) ");
            strSql.Append(" " + sqlWhere + " ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,50),                
                    new MySqlParameter("?time_min", MySqlDbType.DateTime),
                    new MySqlParameter("?time_max", MySqlDbType.DateTime),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = UserName;
            parameters[1].Value = time_min;
            parameters[2].Value = time_max;
            parameters[3].Value = startIndex;
            parameters[4].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "赎回记录", parameters);
        }
        #endregion

        #region 赎回记录转换实体
        /// <summary>
        /// 转为赎回model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public User_redemptionModel DataToRefund(DataRow row)
        {
            var model = new User_redemptionModel();
            #region match
            if (row != null)
            {

                if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row.Table.Columns.Contains("ord_id") && row["ord_id"] != null && row["ord_id"].ToString() != "")
                {
                    model.Order = row["ord_id"].ToString();
                }

                if (row.Table.Columns.Contains("c_time") && row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row.Table.Columns.Contains("user_id") && row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.user_id = int.Parse(row["user_id"].ToString());
                }
                if (row.Table.Columns.Contains("Phone") && row["Phone"] != null && row["Phone"].ToString() != "")
                {
                    model.Phone = row["Phone"].ToString();
                }
                if (row.Table.Columns.Contains("principal") && row["principal"] != null && row["principal"].ToString() != "")
                {
                    model.principal = decimal.Parse(row["principal"].ToString());
                }
                if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Amount = Tools.Const.SystemConst.MoenyConvert(decimal.Parse(row["amount"].ToString()));
                }

                if (row.Table.Columns.Contains("penalty_rate") && row["penalty_rate"] != null && row["penalty_rate"].ToString() != "")
                {
                    model.penalty_rate = decimal.Parse(row["penalty_rate"].ToString());
                }
                if (row.Table.Columns.Contains("LiquidatedDamages") && row["LiquidatedDamages"] != null && row["LiquidatedDamages"].ToString() != "")
                {
                    model.LiquidatedDamages = Tools.Const.SystemConst.MoenyConvert(decimal.Parse(row["LiquidatedDamages"].ToString()));
                }
                if (row.Table.Columns.Contains("ActualAmount") && row["ActualAmount"] != null && row["ActualAmount"].ToString() != "")
                {
                    model.ActualAmount = Tools.Const.SystemConst.MoenyConvert(decimal.Parse(row["ActualAmount"].ToString()));
                }


            }
            #endregion

            return model;
        }
        #endregion



        #region 提现记录
        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="UserName">用户名</param>      
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet Getuser_withdrawLog(string UserName, DateTime time_min, DateTime time_max,
            int startIndex, int pageSize)
        {
            if (time_max != DateTime.MinValue)
            {
                time_max = new DateTime(time_max.Year, time_max.Month, time_max.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  id,user_id,l_name, TRUNCATE(amount,2) amount,c_time FROM  user_withdraw  
                                WHERE op_status=2 and (?UserName=-1 OR l_name = ?UserName)");
            string sqlWhere = " ";
            if (time_min != DateTime.MinValue)
            {
                sqlWhere += " AND  c_time >=?time_min ";
            }
            if (time_max != DateTime.MinValue)
            {
                sqlWhere += " AND  c_time<=?time_max ";
            }
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                sqlWhere = " AND c_time>=?time_min AND c_time<=?time_max ";
            }
            strSql.Append(" " + sqlWhere + " ");
            strSql.Append("ORDER BY c_time DESC ");
            strSql.Append(@" LIMIT ?StartIndex,?PageSize;");

            strSql.Append(@"SELECT COUNT(1)  FROM user_withdraw  
                               WHERE op_status=2 and (?UserName=-1 OR l_name = ?UserName)");
            strSql.Append(" " + sqlWhere + " ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.Int64),                
                    new MySqlParameter("?time_min", MySqlDbType.DateTime),
                    new MySqlParameter("?time_max", MySqlDbType.DateTime),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = UserName;
            parameters[1].Value = time_min;
            parameters[2].Value = time_max;
            parameters[3].Value = startIndex;
            parameters[4].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "提现记录", parameters);
        }
        #endregion

        #region 提现记录转换实体
        /// <summary>
        /// 转为提现model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public User_withdrawModel DataToRefundwithdraw(DataRow row)
        {
            var model = new User_withdrawModel();
            #region match
            if (row != null)
            {
                if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = row["id"].ToString();
                }
                if (row.Table.Columns.Contains("ord_id") && row["ord_id"] != null && row["ord_id"].ToString() != "")
                {
                    model.Order = row["ord_id"].ToString();
                }
                if (row.Table.Columns.Contains("c_time") && row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row.Table.Columns.Contains("l_name") && row["l_name"] != null && row["l_name"].ToString() != "")
                {
                    model.Phone = row["l_name"].ToString();

                }
                if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Money = row["amount"].ToString();
                }
            }
            #endregion

            return model;
        }
        #endregion


        #region 辅助方法
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="UserNAme"></param>
        /// <returns></returns>
        public static long GetUserID(string UserNAme)
        {
            string strSql = @"SELECT id FROM user_info WHERE phone=?UserNAme LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserNAme", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = UserNAme;

            var result = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql, "获取USERID", parameters);
            if (result != null)
            {
                return Convert.ToInt64(result);
            }
            return 0;
        }

        /// <summary>
        /// 获取用户手机号
        /// </summary>
        /// <param name="USerID">用户ID</param>
        /// <returns></returns>
        public static string GetUserPhone(long USerID)
        {
            string strSql = @"SELECT phone FROM user_info WHERE id=?USerID LIMIT 0,1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?USerID", MySqlDbType.Int64,11)
			};
            parameters[0].Value = USerID;

            var result = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql, "获取手机号", parameters);
            if (result != null)
            {
                return result.ToString();
            }
            return "";
        }
        #endregion

    }
}
