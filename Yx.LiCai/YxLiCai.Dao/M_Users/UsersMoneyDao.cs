using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.DataHelper;
using YxLiCai.Model.User;
using YxLiCai.Model.UserRaise;
namespace YxLiCai.Dao.M_Users
{
    /// <summary>
    /// 用户资产数据访问操作类
    /// 平扬 2015.5.29
    /// </summary>
    public class UsersMoneyDao
    {

        /// <summary>
        /// 获取用户可提现金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>TRUNCATE(4545.1366,2);
        public decimal GetUserWithdrawals(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (TRUNCATE(UC.amount_blance,2)-TRUNCATE(UC.amount_blance_fz,2)+(TRUNCATE(M.principal,2)-TRUNCATE(M.principal_fz,2))) from user_account UC inner join user_count_month M ON M.user_id=UC.user_id where UC.user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户可提现金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取用户可提现本金
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserPrincipal(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TRUNCATE(M.principal,2)-TRUNCATE(M.principal_fz,2) as Principal from user_count_month M where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户可提现本金", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0m;
        }

        /// <summary>
        /// 获取用户盈利金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserInterest(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TRUNCATE(amount_blance,2)-TRUNCATE(amount_blance_fz,2) from user_account where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户可提现本金", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0m;
        }

        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserBlance(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TRUNCATE(amount_blance,2)-TRUNCATE(amount_blance_fz,2) from user_account where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户余额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取用户资产
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserCountModel GetUserCount(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select user_id,principal,amount_invest,interest_yesterday,interest_all,amount_blance_fz,amount_blance,interest_paid,interest_added,t_interest");
            strSql.Append(" from user_account ");
            strSql.Append(" where user_id=?id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64)
			};
            parameters[0].Value = userId;
            UserCountModel model = new UserCountModel();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取用户资产", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UserCountModel DataRowToModel(DataRow row)
        {

            UserCountModel model = new UserCountModel();
            if (row != null)
            {
                if (row["user_id"] != null && row["user_id"].ToString() != "")
                {
                    model.UserId = long.Parse(row["user_id"].ToString());
                }
                if (row["amount_invest"] != null && row["amount_invest"].ToString() != "")
                {
                    model.RaiseMoney = decimal.Parse(row["amount_invest"].ToString());
                }
                if (row["principal"] != null && row["principal"].ToString() != "")
                {
                    model.principal = decimal.Parse(row["principal"].ToString());
                } 
                 //冻结本金
                model.principal_fz = GetUserLockMoney(model.UserId);
                
                if (row["interest_yesterday"] != null && row["interest_yesterday"].ToString() != "")
                {
                    model.YesterdayInterest = decimal.Parse(row["interest_yesterday"].ToString());
                }
                if (row["interest_all"] != null && row["interest_all"].ToString() != "")
                {
                    model.AllInterest = decimal.Parse(row["interest_all"].ToString());
                }
                if (row["amount_blance_fz"] != null && row["amount_blance_fz"].ToString() != "")
                {
                    model.LockMoney = decimal.Parse(row["amount_blance_fz"].ToString());
                }
                if (row["amount_blance"] != null && row["amount_blance"].ToString() != "")
                {
                    model.MyBlance = decimal.Parse(row["amount_blance"].ToString());
                }
                if (row["interest_paid"] != null && row["interest_paid"].ToString() != "")
                {
                    model.HaveInterest = decimal.Parse(row["interest_paid"].ToString());
                }
                if (row["interest_added"] != null && row["interest_added"].ToString() != "")
                {
                    model.NotInterest = decimal.Parse(row["interest_added"].ToString());
                }
                if (row["t_interest"] != null && row["t_interest"].ToString() != "")
                {
                    model.Tinterest = decimal.Parse(row["t_interest"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 银行卡购买-更新账户投资相关金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        public bool UpdateRaiseUserCount(long userId, decimal RaiseMoney)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_account set amount=amount+?RaiseMoney, amount_invest=amount_invest+?RaiseMoney,amount_blance=amount_blance+?RaiseMoney,amount_blance_fz=amount_blance_fz+?RaiseMoney where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.UInt64,11),
					new MySqlParameter("?RaiseMoney", MySqlDbType.Decimal,18)};
            parameters[0].Value = userId;
            parameters[1].Value = RaiseMoney;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "银行卡购买-更新月账户投资相关金额", parameters);
        }

        /// <summary>
        /// 更新账户投资相关金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateUserCount(long userId, decimal blance)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_account set amount_invest=amount_invest+?RaiseMoney, amount_blance_fz=amount_blance_fz+?RaiseMoney  where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.UInt64,11),
					new MySqlParameter("?RaiseMoney", MySqlDbType.Decimal,18)};
            parameters[0].Value = userId;
            parameters[1].Value = blance;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新月账户投资相关金额", parameters);
        }

        /// <summary>
        /// 更新月账户投资相关金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateUserCountMonth(long userId, decimal blance)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_count_month set ");
            strSql.Append("amount_invest=amount_invest+?RaiseMoney");
            strSql.Append(" where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.UInt64,11),
					new MySqlParameter("?RaiseMoney", MySqlDbType.Decimal,18)};
            parameters[0].Value = userId;
            parameters[1].Value = blance;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新月账户投资相关金额", parameters);
        }

        /// <summary>
        /// 更新季账户投资相关金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateUserCountSeason(long userId, decimal blance)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_count_season set ");
            strSql.Append("amount_invest=amount_invest+?RaiseMoney");
            strSql.Append(" where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.UInt64,11),
					new MySqlParameter("?RaiseMoney", MySqlDbType.Decimal,18)};
            parameters[0].Value = userId;
            parameters[1].Value = blance;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新季账户投资相关金额", parameters);
        }


        /// <summary>
        /// 更新年账户投资相关金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blance"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateUserCountYear(long userId, decimal blance)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_count_year set ");
            strSql.Append("amount_invest=amount_invest+?RaiseMoney");
            strSql.Append(" where user_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.UInt64,11),
					new MySqlParameter("?RaiseMoney", MySqlDbType.Decimal,18)};
            parameters[0].Value = userId;
            parameters[1].Value = blance;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新年账户投资相关金额", parameters);
        }



        /// <summary>
        /// 用户申请提现
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="money">申请提现金额</param>
        /// <returns></returns>
        public bool UserWithdrawCash(long userId, decimal money, string l_name, string r_name, string phone, string bankname, string bkcard)
        {
            var blance = GetUserBlance(userId); //用户余额
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_withdraw(");
            strSql.Append("user_id,l_name,r_name,u_phone,bk_name,bk_card,c_time,amount,amount_balance,amount_principal,status,op_status,rec_summary_id,rfd_balance_id,rfd_principal_id,remark)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?l_name,?r_name,?u_phone,?bk_name,?bk_card,?c_time,?amount,?amount_balance,?amount_principal,?status,?op_status,?rec_summary_id,?rfd_balance_id,?rfd_principal_id,?remark)");
            strSql.Append(";update user_account set amount_blance_fz=amount_blance_fz+?amount_balance where user_id=?user_id");
            strSql.Append(";update user_count_month set principal_fz=principal_fz+?amount_principal where user_id=?user_id");
            strSql.Append("; insert into user_amount_rec(user_id,pdt_type,type,amount,c_time,creator_id,version,remark)");
            strSql.Append(" values (?user_id,1,1,?amount,?c_time,0,0,?remark)");

            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?user_id", MySqlDbType.UInt64,20),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_balance", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_principal", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,4),
					new MySqlParameter("?op_status", MySqlDbType.Int16,4),
					new MySqlParameter("?auditor_id", MySqlDbType.Int32,11),
					new MySqlParameter("?audit_time", MySqlDbType.DateTime),
					new MySqlParameter("?rec_summary_id", MySqlDbType.UInt64,20),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
                    new MySqlParameter("?l_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?r_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?u_phone", MySqlDbType.VarChar,50),
					new MySqlParameter("?bk_name", MySqlDbType.VarChar,250),
					new MySqlParameter("?bk_card", MySqlDbType.VarChar,50),
                    new MySqlParameter("?rfd_balance_id", MySqlDbType.VarChar,50),
                    new MySqlParameter("?rfd_principal_id", MySqlDbType.VarChar,50),};

            parameters[0].Value = DateTime.Now;
            parameters[1].Value = userId;
            parameters[2].Value = money;
            parameters[3].Value = money > blance ? blance : money;
            parameters[4].Value = money > blance ? money - blance : 0;
            parameters[5].Value = 0;
            parameters[6].Value = 0;//(0:未处理1:待处理2:处理成功3:处理失败)
            parameters[7].Value = 0;
            parameters[8].Value = DateTime.Now;
            parameters[9].Value = 0;
            parameters[10].Value = "";
            parameters[11].Value = l_name;
            parameters[12].Value = r_name;
            parameters[13].Value = phone;
            parameters[14].Value = bankname;
            parameters[15].Value = bkcard;
            parameters[16].Value = 0;
            parameters[17].Value = 0;
            List<string> listsql = new List<string>();
            listsql.Add(strSql.ToString());
            List<DbParameter[]> par = new List<DbParameter[]>();
            par.Add(parameters);
            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(listsql, par);
        }

        /// <summary>
        /// 用户申请赎回
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="money">申请提现金额</param>
        /// <param name="raiseProductId">用户投资产品id</param>
        /// <returns></returns>
        public bool UserRedemptionCash(long userId, decimal money, long raiseProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_redemption(");
            strSql.Append("c_time,user_id,ord_id,stauts,op_status,amount,auditor_id,audit_time,remark,version)");
            strSql.Append(" values (");
            strSql.Append("?create_time,?user_id,?order_id,?stauts,?sys_status,?amount,?auditor_id,?audit_time,?remark,?version)");
            strSql.Append("; insert into user_amount_rec(user_id,type,amount,c_time,creator_id,version,remark)");
            strSql.Append(" values (?user_id,2,?amount,?create_time,0,0,?remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?create_time", MySqlDbType.DateTime),
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?order_id", MySqlDbType.Int64,20),
					new MySqlParameter("?stauts", MySqlDbType.Int16,4),
					new MySqlParameter("?sys_status", MySqlDbType.Int16,4),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?auditor_id", MySqlDbType.Int32,11),
					new MySqlParameter("?audit_time", MySqlDbType.DateTime),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?version", MySqlDbType.Int32,11)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = userId;
            parameters[2].Value = raiseProductId;
            parameters[3].Value = 0;
            parameters[4].Value = 0;
            parameters[5].Value = money;
            parameters[6].Value = 0;
            parameters[7].Value = DateTime.Now;
            parameters[8].Value = "";
            parameters[9].Value = 1;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "插入用户申请赎回表", parameters);
        }


        /// <summary>
        /// 获取用户特享金列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">当前页面</param>
        /// <param name="pagesize">页码</param>
        /// <param name="count">总记录</param>
        /// <returns></returns>
        public List<UserSpecialAssetsModel> GetUserSpecialAssets(long uid, int page, int pagesize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select amount,c_time,u_time,rate,rate_increase,rate_added,TRUNCATE(interest_added,2) as interest_added,interest_paid,invited_user_name from  user_special_assets where  user_id=?UserId and use_status=1 order by u_time desc  LIMIT ?startpage,?pagesize;");
            strSql.Append("select count(*) from  user_special_assets where user_id=?UserId and use_status=1;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?UserId", MySqlDbType.Int32,11),
                    new MySqlParameter("?startpage", MySqlDbType.Int32,11),
                    new MySqlParameter("?pagesize", MySqlDbType.Int32,11)};
            parameters[0].Value = uid;
            parameters[1].Value = page;
            parameters[2].Value = pagesize;
            count = 0;
            List<UserSpecialAssetsModel> list = new List<UserSpecialAssetsModel>();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取用户特享金列表", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    UserSpecialAssetsModel mod = new UserSpecialAssetsModel();
                    mod.amount = Tools.Util.ParseHelper.ToDecimal(row["amount"].ToString());
                    mod.c_time = Tools.Util.ParseHelper.ToDatetime(row["c_time"].ToString());
                    mod.u_time = Tools.Util.ParseHelper.ToDatetime(row["u_time"].ToString());
                    mod.rate = Tools.Util.ParseHelper.ToDecimal(row["rate"].ToString());
                    mod.rate_increase = Tools.Util.ParseHelper.ToDecimal(row["rate_increase"].ToString());
                    mod.rate_added = Tools.Util.ParseHelper.ToDecimal(row["rate_added"].ToString());
                    mod.interest_added = Tools.Util.ParseHelper.ToDecimal(row["interest_added"].ToString());
                    mod.interest_paid = Tools.Util.ParseHelper.ToDecimal(row["interest_paid"].ToString());
                    mod.invited_user_name = row["invited_user_name"].ToString();
                    list.Add(mod);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                count = Tools.Util.ParseHelper.ToInt(ds.Tables[1].Rows[0][0].ToString());
            }
            return list;
        }

        /// <summary>
        /// 获取用户特享金列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">当前页面</param>
        /// <param name="pagesize">页码</param>
        /// <param name="count">总记录</param>
        /// <returns></returns>
        public List<UserSpecialAssetsModel> GetUserAllSpecialAssets(long uid, int page, int pagesize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select amount,c_time,u_time,rate,rate_increase,rate_added,TRUNCATE(interest_added,2) as interest_added,interest_paid,invited_user_name,invited_user_id from  user_special_assets where  user_id=?UserId  order by c_time desc  LIMIT ?startpage,?pagesize;");
            strSql.Append("select count(*) from  user_special_assets where user_id=?UserId");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?UserId", MySqlDbType.Int32,11),
                    new MySqlParameter("?startpage", MySqlDbType.Int32,11),
                    new MySqlParameter("?pagesize", MySqlDbType.Int32,11)};
            parameters[0].Value = uid;
            parameters[1].Value = page;
            parameters[2].Value = pagesize;
            count = 0;
            List<UserSpecialAssetsModel> list = new List<UserSpecialAssetsModel>();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取用户特享金列表", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    UserSpecialAssetsModel mod = new UserSpecialAssetsModel();
                    mod.amount = Tools.Util.ParseHelper.ToDecimal(row["amount"].ToString());
                    mod.c_time = Tools.Util.ParseHelper.ToDatetime(row["c_time"].ToString());
                    if (row["u_time"] != DBNull.Value)
                    {
                        mod.u_time = Tools.Util.ParseHelper.ToDatetime(row["u_time"].ToString());
                    }
                    mod.rate = Tools.Util.ParseHelper.ToDecimal(row["rate"].ToString());
                    mod.rate_increase = Tools.Util.ParseHelper.ToDecimal(row["rate_increase"].ToString());
                    mod.rate_added = Tools.Util.ParseHelper.ToDecimal(row["rate_added"].ToString());
                    mod.interest_added = Tools.Util.ParseHelper.ToDecimal(row["interest_added"].ToString());
                    mod.interest_paid = Tools.Util.ParseHelper.ToDecimal(row["interest_paid"].ToString());
                    mod.invited_user_name = row["invited_user_name"].ToString();
                    if (row["invited_user_id"] != DBNull.Value)
                    {
                        mod.invited_user_id = Tools.Util.ParseHelper.ToInt(row["invited_user_id"].ToString());
                    }
                    list.Add(mod);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                count = Tools.Util.ParseHelper.ToInt(ds.Tables[1].Rows[0][0].ToString());
            }
            return list;
        }


        /// <summary>
        /// 获取用户 冻结本金
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserLockMoney(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  m.principal_fz+s.principal_fz+y.principal_fz  FROM user_count_month m inner join user_count_season s on s.user_id=m.user_id inner join user_count_year y on y.user_id=s.user_id  WHERE m.user_id=?user_id;"); 
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户冻结本金", parameters);
            if (obj != null)
            {
                return decimal.Parse(obj.ToString()); 
            } 
            return 0;
        } 



        /// <summary>
        /// 获取用户 在投中的资产
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserCountModel GetUserRaiseMoney(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  principal-principal_fz as amount_invest FROM user_count_month  WHERE user_id=?user_id;");
            strSql.Append("SELECT  principal-principal_fz AS season_amount FROM user_count_season  WHERE user_id=?user_id;");
            strSql.Append("SELECT  principal-principal_fz AS year_amount FROM  user_count_year WHERE user_id=?user_id;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取用户特享金总金额", parameters);
            UserCountModel mod = new UserCountModel();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)//月月赢在投资产
            {
                mod.MyMoney = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)//季季享在投资产
            {
                mod.MyBlance = decimal.Parse(ds.Tables[1].Rows[0][0].ToString());
            }
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)//年年丰在投资产
            {
                mod.LockMoney = decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
            }
            return mod;
        }

        /// <summary>
        /// 获取用户特享金总金额(在投中）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserSpecialAssets(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(amount) FROM  user_special_assets  WHERE  user_id=?user_id AND use_status=1");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户特享金总金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }



        /// <summary>
        /// 获取用户特享金总金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserAllSpecialAssets(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(amount)  FROM  user_special_assets  WHERE   user_id=?user_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户特享金总金额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }


        /// <summary>
        /// 获取用户加息券总额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserAllAddInterest(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(interest_rate) as interest_rate FROM user_add_interest WHERE  user_id=?user_id; ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户加息券总额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }

        /// <summary>
        /// 获取用户加息券总额(可用的)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserAddInterest(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(interest_rate) as interest_rate FROM user_add_interest WHERE  user_id=?user_id AND use_status=0 AND e_time > NOW(); ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户加息券总额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }



        /// <summary>
        /// 获取用户加息券可用张数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserAddInterestCount(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(*) as interest_rate FROM user_add_interest WHERE  user_id=?user_id AND use_status=0 AND e_time > NOW();");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户加息券张数", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToInt(obj);
            }
            return 0;
        }
        /// <summary>
        /// 获取用户加息券总张数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserAllAddInterestCount(long userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(*) as interest_rate FROM user_add_interest WHERE  user_id=?user_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户加息券张数", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToInt(obj);
            }
            return 0;
        }

        /// <summary>
        /// 增加金额记录日志
        /// </summary>
        public bool AddUserAmountRecModel(UserAmountRecModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_amount_rec(");
            strSql.Append("user_id,pdt_type,type,amount,c_time,creator_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?prod_type,?type,?amount,?c_time,?creator_id,?version,?remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?prod_type", MySqlDbType.Int16,4),
                    new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.Prodtype;
            parameters[2].Value = model.type;
            parameters[3].Value = model.amount;
            parameters[4].Value = model.c_time;
            parameters[5].Value = model.creator_id;
            parameters[6].Value = model.version;
            parameters[7].Value = model.remark;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "增加金额记录日志", parameters);
        }


        #region 获取用户资金流水记录列表

        /// <summary>
        /// 获取用户资金流水记录列表
        /// </summary>
        /// <param name="UserID">用户</param>
        /// <param name="Prodtype">产品类型（1月、2三个月、3六个月、4年）</param>
        /// <param name="SCount">当前开始记录</param>
        /// <param name="ECout">结束记录</param>
        /// <returns></returns>
        public List<UserAmountRecModel> GetListUserAmountRecModel(long UserID, int Prodtype, int SCount, int ECout, out int record)
        {
            List<UserAmountRecModel> list = new List<UserAmountRecModel>();
            string strSql = @"SELECT   id, 
                                        amount, 
                                        c_time, 
                                        type,pdt_type 
                                FROM     user_amount_rec 
                                WHERE    user_id=?userid  and pdt_type=?prod_type
                                ORDER BY c_time DESC limit ?scount,?ecout;
                                SELECT count(*)  FROM user_amount_rec WHERE user_id=?userid and pdt_type=?prod_type";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,11),   
                    new MySqlParameter("?prod_type", MySqlDbType.Int32),  
                    new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                    new MySqlParameter("?ECout", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserID;
            parameters[1].Value = Prodtype;
            parameters[2].Value = SCount;
            parameters[3].Value = ECout;
            record = 0;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取资金流水记录", parameters);
            UserAmountRecModel item;
            DataTable dt1 = ds.Tables[0];
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    item = new UserAmountRecModel();
                    item.id = Convert.ToInt32(dr["id"]);
                    item.amount = Convert.ToDecimal(dr["amount"]);
                    item.c_time = Convert.ToDateTime(dr["c_time"]);
                    item.type = Tools.Util.ParseHelper.ToInt(dr["type"]);
                    item.Prodtype = Convert.ToInt32(dr["pdt_type"]);
                    list.Add(item);
                }
            }
            DataTable dt2 = ds.Tables[1];
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                record = Convert.ToInt32(dt2.Rows[0][0].ToString());
            }
            return list;
        }

        #endregion
    }
}
