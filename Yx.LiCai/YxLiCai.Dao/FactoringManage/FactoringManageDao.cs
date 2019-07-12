using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.FactoringManage
{
    /// <summary>
    /// 保理数据层
    /// </summary>
    public class FactoringManageDao
    {
        /// <summary>
        /// 获取用户账户日志
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="s_time"></param>
        /// <param name="e_time"></param>
        /// <returns></returns>
        public DataTable GetUserAccountLog(long user_id, DateTime s_time, DateTime e_time)
        {
            string strSql = @"SELECT  a.pjt_id,b.name,b.amount,a.change_amount,a.type
                              FROM user_account_log a
                              INNER JOIN pjt b ON a.pjt_id=b.id
                              WHERE a.user_id=?user_id AND a.c_time>=?s_time AND a.c_time<?e_time;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?s_time", MySqlDbType.DateTime),
                    new MySqlParameter("?e_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = s_time;
            parameters[2].Value = e_time;
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取用户账户日志", parameters).Tables[0];

        }
        /// <summary>
        /// 获取保理债权列表（默认当前只有一个保理）
        /// </summary>
        /// <returns></returns>
        public DataTable GetInvestmentDetail()
        {
            string strSql = @"SELECT c.pjt_id,d.name pjt_name,c.amount
                                FROM user_account a
                                INNER JOIN ord_info b ON a.user_id=b.user_id
                                INNER JOIN ord_pjt c ON b.id=c.ord_id
                                INNER JOIN pjt d ON c.pjt_id=d.id
                                WHERE a.user_type=1 AND b.ord_status=1 AND b.atone_status=0;";

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取保理债权列表", null).Tables[0];
        }
        /// <summary>
        /// 插入客户申请充值记录（只是申请，未经过第三方通知成功）
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="c_time"></param>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public bool InsertUserRecharge(long user_id, DateTime c_time, int type, decimal amount, string mer_ord_id)
        {
            string strSql = @"INSERT INTO user_recharge(user_id, c_time, TYPE, amount, mer_ord_id)
                                VALUES(?user_id, ?c_time, ?type, ?amount, ?mer_ord_id);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = c_time;
            parameters[2].Value = type;
            parameters[3].Value = amount;
            parameters[4].Value = mer_ord_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "插入客户申请充值记录", parameters);
        }
        /// <summary>
        /// 根据商户编号获取充值记录
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public DataTable GetUserRecharge(string mer_ord_id)
        {
            string strSql = @"SELECT id,c_time,user_id,type,amount,status,mer_ord_id,version,remark,pay_time,third_ord_id,user_poundage,mer_poundage  
FROM user_recharge 
WHERE mer_ord_id=?mer_ord_id LIMIT 0,1;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = mer_ord_id;
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据商户编号获取充值记录", parameters).Tables[0];
        }
        /// <summary>
        /// 更新充值申请为充值成功
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <param name="amount"></param>
        /// <param name="pay_time"></param>
        /// <param name="third_ord_id"></param>
        /// <param name="user_poundage"></param>
        /// <param name="mer_poundage"></param>
        /// <param name="op_type">1插入2更新 user_balance_item</param>
        /// <returns></returns>
        public bool UpdateUserRechargeForRechargeSuccess(int id, long user_id, decimal amount, DateTime pay_time, string third_ord_id, decimal user_poundage, decimal mer_poundage, int op_type)
        {
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //更改充值申请为成功
            string strSql = @"UPDATE user_recharge SET STATUS=1,pay_time=?pay_time,third_ord_id=?third_ord_id,user_poundage=?user_poundage,mer_poundage=?mer_poundage 
                                WHERE id=?id AND user_id=?user_id;";

            DbParameter[] parameters = {
					new MySqlParameter("?pay_time", MySqlDbType.DateTime),
                    new MySqlParameter("?third_ord_id", MySqlDbType.VarChar,100),
                    new MySqlParameter("?user_poundage", MySqlDbType.Decimal),
                    new MySqlParameter("?mer_poundage", MySqlDbType.Decimal),
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?user_id", MySqlDbType.Int64)
			};
            parameters[0].Value = pay_time;
            parameters[1].Value = third_ord_id;
            parameters[2].Value = user_poundage;
            parameters[3].Value = mer_poundage;
            parameters[4].Value = id;
            parameters[5].Value = user_id;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加账户变动日志
            strSql = @"INSERT INTO user_account_log (c_time,user_id,TYPE,before_amount,after_amount,change_amount,account_source_id,from_id)
                        SELECT ?c_time,?user_id,1,amount,(amount+?amount),?amount,13,?id
                        FROM user_account
                        WHERE user_id=?user_id;";

            parameters = new MySqlParameter[]
            {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = user_id;
            parameters[2].Value = amount;
            parameters[3].Value = id;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加余额
            strSql = @"UPDATE user_account SET amount_blance=amount_blance+?amount,amount=amount+?amount WHERE user_id=?user_id;";
            parameters = new MySqlParameter[]
            {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?amount", MySqlDbType.Decimal)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = amount;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //更新 user_balance_item
            if (op_type == 1)
            {
                strSql = @"INSERT INTO user_balance_item (user_id,balance_type,c_time,amount,amount_fz) 
                            VALUES(?user_id,1,?c_time,?amount,0);";
                parameters = new MySqlParameter[]
                    {
					    new MySqlParameter("?user_id", MySqlDbType.Int64),
                        new MySqlParameter("?amount", MySqlDbType.Decimal),
                        new MySqlParameter("?c_time", MySqlDbType.DateTime)
			        };
                parameters[0].Value = user_id;
                parameters[1].Value = amount;
                parameters[2].Value = DateTime.Now;
                strSql_array.Add(strSql);
                parameters_array.Add(parameters);
            }
            else if (op_type == 2)
            {
                strSql = @"UPDATE user_balance_item SET amount=amount+?amount WHERE balance_type=1 AND user_id=?user_id;";
                parameters = new MySqlParameter[]
                    {
					    new MySqlParameter("?user_id", MySqlDbType.Int64),
                        new MySqlParameter("?amount", MySqlDbType.Decimal)
			        };
                parameters[0].Value = user_id;
                parameters[1].Value = amount;

                strSql_array.Add(strSql);
                parameters_array.Add(parameters);
            }

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
        /// <summary>
        /// 根据user_id获取充值记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable GetUserRechargeByUserID(long user_id, DateTime s_ctime, DateTime e_ctime, int status)
        {
            string strSql = @"SELECT id,c_time,user_id,type,amount,status,mer_ord_id,version,remark,pay_time,third_ord_id,user_poundage,mer_poundage  
                                FROM user_recharge 
                                WHERE user_id=?user_id  AND (?status=-1 OR status=?status) AND (?s_ctime='1900-01-01' OR c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR c_time<?e_ctime) ORDER BY c_time DESC;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = status;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据user_id获取充值记录", parameters).Tables[0];
        }
        /// <summary>
        /// 保理新增提现申请
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="WithdrawAmount">申请提现金额</param>
        /// <param name="l_name">登录名</param>
        /// <param name="r_name">真实姓名</param>
        /// <param name="u_phone">手机号</param>
        /// <param name="bk_name">银行名称</param>
        /// <param name="bk_card">银行卡</param>
        /// <returns></returns>
        public bool WithdrawApply(long user_id, decimal WithdrawAmount, string l_name, string r_name, string u_phone, string bk_name, string bk_card)
        {
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //增加提现申请记录
            string strSql = @"INSERT INTO user_withdraw (user_id,l_name,r_name,u_phone,bk_name,bk_card,c_time,amount,amount_balance,amount_principal,STATUS,op_status)
                              VALUES(?user_id,?l_name,?r_name,?u_phone,?bk_name,?bk_card,?c_time,?amount,?amount_balance,?amount_principal,?status,?op_status) ;";

            DbParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?l_name", MySqlDbType.VarChar,50),
                    new MySqlParameter("?r_name", MySqlDbType.VarChar,50),
                    new MySqlParameter("?u_phone", MySqlDbType.VarChar,50),
                    new MySqlParameter("?bk_name", MySqlDbType.VarChar,250),
                    new MySqlParameter("?bk_card", MySqlDbType.VarChar,50),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?amount_balance", MySqlDbType.Decimal),
                    new MySqlParameter("?amount_principal", MySqlDbType.Decimal),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?op_status", MySqlDbType.Int32)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = l_name;
            parameters[2].Value = r_name;
            parameters[3].Value = u_phone;
            parameters[4].Value = bk_name;
            parameters[5].Value = bk_card;
            parameters[6].Value = DateTime.Now;
            parameters[7].Value = WithdrawAmount;
            parameters[8].Value = WithdrawAmount;
            parameters[9].Value = 0;
            parameters[10].Value = 0;
            parameters[11].Value = 0;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加账户冻结金额
            strSql = @"UPDATE user_account SET amount_blance_fz=amount_blance_fz+?amount WHERE user_id=?user_id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?amount", MySqlDbType.Decimal)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = WithdrawAmount;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);


            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
        /// <summary>
        /// 根据user_id获取提现记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable GetUserWithdrawByUserID(long user_id, DateTime s_ctime, DateTime e_ctime, int status)
        {
            string strSql = @"SELECT   id,  user_id,  l_name,  r_name,  u_phone,  bk_name,  bk_card,  c_time,  amount,  amount_balance,  amount_principal,  status,                         op_status,  auditor_id,  audit_time,  rec_summary_id,  rfd_balance_id,  rfd_principal_id,  remark 
                               FROM  user_withdraw WHERE user_id=?user_id
                               AND (?status=-1 OR status=?status) AND (?s_ctime='1900-01-01' OR c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR c_time<?e_ctime) ORDER BY c_time DESC;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = status;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据user_id获取提现记录", parameters).Tables[0];
        }
        /// <summary>
        /// 获取账户构成数量
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="balance_type"></param>
        /// <returns></returns>
        public int GetUserBalanceItemCountByUserIDAndType(long user_id, int balance_type)
        {
            string strSql = @"SELECT COUNT(1) FROM user_balance_item WHERE balance_type=?balance_type AND user_id=?user_id";
            MySqlParameter[] parameters = {
					new MySqlParameter("?balance_type", MySqlDbType.Int32),
                    new MySqlParameter("?user_id", MySqlDbType.Int64)
			};
            parameters[0].Value = balance_type;
            parameters[1].Value = user_id;
            return Convert.ToInt32(DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql, "获取账户构成数量", parameters));
        }
    }
}
