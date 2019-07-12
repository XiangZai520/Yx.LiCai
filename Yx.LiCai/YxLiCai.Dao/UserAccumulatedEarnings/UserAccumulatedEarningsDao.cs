/*
 * 各个账户累计收益数据层
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using YxLiCai.DataHelper;
using YxLiCai.Model.UserAccumulatedEarnings;

namespace YxLiCai.Dao.UserAccumulatedEarnings
{
    /// <summary>
    /// 用户账户累计收益数据访问层
    /// </summary>
    public class UserAccumulatedEarningsDao
    {

        #region 获取用户各个账户累计收益金额
        /// <summary>
        /// 获取用户月账户信息（累计收益总和）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserMonth(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(TRUNCATE(earnings_amount,2)) AS earningsamount FROM user_income_month where user_id=?user_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户月账户利息金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        /// <summary>
        /// 获取用户季账户信息（累计收益总和）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserSeason(long userId, string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(TRUNCATE(earnings_amount,2)) AS earningsamount FROM user_income_season where user_id=?user_id and type=?type ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
                    new MySqlParameter("?type", MySqlDbType.Int32,4)};
            parameters[0].Value = userId;
            parameters[1].Value = type;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户季账户利息金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        /// <summary>
        /// 获取用户季账户信息（累计收益总和）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserYear(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(TRUNCATE(earnings_amount,2)) AS earningsamount FROM user_income_year where user_id=?user_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户季账户利息金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        #endregion


        #region 获取各个账户的记录数
        /// <summary>
        /// 月账户总数量
        /// </summary>
        /// <param name="UserId"></param>   
        /// <returns></returns>
        public int GetTotalCount_month(long UserId)
        {
            int TotalCount = 0;
            string strSql = @"SELECT COUNT(1) FROM user_income_month WHERE   user_id=?Id  ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11)                   
			};
            parameters[0].Value = UserId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取月账户总数", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                }

            }
            return TotalCount;
        }
        /// <summary>
        /// 季账户总数量
        /// </summary>
        /// <param name="UserId"></param>   
        /// <returns></returns>
        public int GetTotalCount_season(long UserId, int ProductType)
        {
            int tepe = 0;
            if (ProductType == 3)
            {
                tepe = 1;
            }
            int TotalCount = 0;
            string strSql = @"SELECT COUNT(1) FROM user_income_season  WHERE   user_id=?Id and type=" + tepe;
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取季度账户总数", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                }

            }
            return TotalCount;
        }
        /// <summary>
        /// 年账户总数量
        /// </summary>
        /// <param name="UserId"></param>   
        /// <returns></returns>
        public int GetTotalCount_year(long UserId)
        {
            int TotalCount = 0;
            string strSql = @"SELECT COUNT(1) FROM user_income_year  WHERE   user_id=?Id ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取年度账户总数", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                }

            }
            return TotalCount;
        }
        #endregion


        #region 获取各个账户下的数据
        /// <summary>
        /// 获取月账户
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<UserCountMonth_AccumulatedEarnings_Model> GetUserCountMonth_AccumulatedEarnings_Model(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select id,user_id,type,earnings_amount earningsamount,c_time create_time from user_income_month   ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" Order By create_time Desc ");
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取月账户");
            List<UserCountMonth_AccumulatedEarnings_Model> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<UserCountMonth_AccumulatedEarnings_Model>();
                UserCountMonth_AccumulatedEarnings_Model mod = new UserCountMonth_AccumulatedEarnings_Model();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToMonthModel(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取季账户
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<UserCountSeason_AccumulatedEarnings_Model> GetUserCountSeason_AccumulatedEarnings_Model(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select id,user_id,type,earnings_amount earningsamount,c_time create_time from user_income_season   ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" Order By create_time Desc ");
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取季账户");
            List<UserCountSeason_AccumulatedEarnings_Model> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<UserCountSeason_AccumulatedEarnings_Model>();
                UserCountSeason_AccumulatedEarnings_Model mod = new UserCountSeason_AccumulatedEarnings_Model();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    mod = DataRowToSeasonModel(row);
                    list.Add(mod);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取年账户
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<UserCountYear_AccumulatedEarnings_Model> GetUserCountYear_AccumulatedEarnings_Model(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select id,user_id,earnings_amount earningsamount,c_time create_time from user_income_year   ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" Order By create_time Desc ");
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取年账户");
            List<UserCountYear_AccumulatedEarnings_Model> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<UserCountYear_AccumulatedEarnings_Model>();
                UserCountYear_AccumulatedEarnings_Model mod = new UserCountYear_AccumulatedEarnings_Model();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    mod = DataRowToYearModel(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion



        #region 获取各个账户下的分页操作
        /// <summary>
        /// 获取月账户的分页数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SCount"></param>
        /// <param name="ECout"></param>
        /// <returns></returns>
        public List<UserCountMonth_AccumulatedEarnings_Model> GetListOrderByCreateTimeDesc_month(long UserID, int SCount, int ECout)
        {
            List<UserCountMonth_AccumulatedEarnings_Model> list = new List<UserCountMonth_AccumulatedEarnings_Model>();
            string strSql = @"SELECT id,earnings_amount earningsamount,c_time create_time,type FROM user_income_month WHERE user_id=?UserID
ORDER BY create_time DESC LIMIT ?SCount,?ECout";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,11),                   
                    new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                    new MySqlParameter("?ECout", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserID;
            parameters[1].Value = SCount;
            parameters[2].Value = ECout;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取月账户分页数据列表", parameters);
            UserCountMonth_AccumulatedEarnings_Model item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new UserCountMonth_AccumulatedEarnings_Model();

                    item.id = Convert.ToInt32(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"]);
                    item.earningsamount = Convert.ToDecimal(dr["earningsamount"]);
                    item.type = YxLiCai.Tools.Util.ParseHelper.ToInt(dr["type"]);
                    item.create_time = item.create_time.AddDays(-1);
                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取季账户的分页数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SCount"></param>
        /// <param name="ECout"></param>
        /// <returns></returns>
        public List<UserCountSeason_AccumulatedEarnings_Model> GetListOrderByCreateTimeDesc_season(long UserID, int ProductType, int SCount, int ECout)
        {
            int type = 0;
            if (ProductType == 3)
            {
                type = 1;
            }
            List<UserCountSeason_AccumulatedEarnings_Model> list = new List<UserCountSeason_AccumulatedEarnings_Model>();
            string strSql = @"SELECT id,earnings_amount earningsamount,c_time create_time,type FROM user_income_season WHERE user_id=?UserID and type=" + type + " ORDER BY create_time DESC LIMIT ?SCount,?ECout";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,11),                   
                    new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                    new MySqlParameter("?ECout", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserID;          
            parameters[1].Value = SCount;
            parameters[2].Value = ECout;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取月账户分页数据列表", parameters);
            UserCountSeason_AccumulatedEarnings_Model item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new UserCountSeason_AccumulatedEarnings_Model();

                    item.id = Convert.ToInt32(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"]);
                    item.earningsamount = Convert.ToDecimal(dr["earningsamount"]);
                    item.type = YxLiCai.Tools.Util.ParseHelper.ToInt(dr["type"]);
                    item.create_time = item.create_time.AddDays(-1);
                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取年账户的分页数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="SCount"></param>
        /// <param name="ECout"></param>
        /// <returns></returns>
        public List< UserCountYear_AccumulatedEarnings_Model> GetListOrderByCreateTimeDesc_year(long UserID,int SCount, int ECout)
        {
            List<UserCountYear_AccumulatedEarnings_Model> list = new List<UserCountYear_AccumulatedEarnings_Model>();
            string strSql = @"SELECT id,earnings_amount earningsamount,c_time create_time FROM user_income_year WHERE user_id=?UserID  ORDER BY create_time DESC LIMIT ?SCount,?ECout";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,11),                   
                    new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                    new MySqlParameter("?ECout", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserID;
            parameters[1].Value = SCount;
            parameters[2].Value = ECout;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取月账户分页数据列表", parameters);
            UserCountYear_AccumulatedEarnings_Model item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new UserCountYear_AccumulatedEarnings_Model();

                    item.id = Convert.ToInt32(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"]);
                    item.earningsamount = Convert.ToDecimal(dr["earningsamount"]);
                    item.create_time = item.create_time.AddDays(-1);
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion



        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select id,user_id,type,earnings_amount earningsamount,c_time create_time from user_income_month   ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" Order By create_time Desc ");

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString());
        }


        #region 对象转换
        /// <summary>
        /// 得到一个对象实体（月账户）
        /// </summary>
        public UserCountMonth_AccumulatedEarnings_Model DataRowToMonthModel(DataRow row)
        {
            UserCountMonth_AccumulatedEarnings_Model model = new UserCountMonth_AccumulatedEarnings_Model();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.user_id = long.Parse(row["user_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
                if (row["earningsamount"] != null && row["earningsamount"].ToString() != "")
                {
                    model.earningsamount = decimal.Parse(row["earningsamount"].ToString());
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
            }
            return model;
        }
        /// 得到一个对象实体(季账户)
        /// </summary>
        public UserCountSeason_AccumulatedEarnings_Model DataRowToSeasonModel(DataRow row)
        {
            UserCountSeason_AccumulatedEarnings_Model model = new UserCountSeason_AccumulatedEarnings_Model();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.user_id = long.Parse(row["user_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
                if (row["earningsamount"] != null && row["earningsamount"].ToString() != "")
                {
                    model.earningsamount = decimal.Parse(row["earningsamount"].ToString());
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 得到一个对象实体（年账户）
        /// </summary>
        public UserCountYear_AccumulatedEarnings_Model DataRowToYearModel(DataRow row)
        {
            UserCountYear_AccumulatedEarnings_Model model = new UserCountYear_AccumulatedEarnings_Model();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.user_id = long.Parse(row["user_id"].ToString());
                }
                if (row["earningsamount"] != null && row["earningsamount"].ToString() != "")
                {
                    model.earningsamount = decimal.Parse(row["earningsamount"].ToString());
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
            }
            return model;
        }
        #endregion


    }
}
