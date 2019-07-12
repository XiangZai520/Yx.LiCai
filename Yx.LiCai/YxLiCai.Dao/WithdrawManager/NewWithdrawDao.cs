using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.WithdrawManager
{
    /// <summary>
    /// 提现数据类
    /// </summary>
    public class NewWithdrawDao
    {
        #region 用户提现
        /// <summary>
        /// 获取用户/保理提现数据
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">姓名</param>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <param name="status">状态(0未审核1审核通过2已提现3审核未通过4支付失败)</param>
        /// <param name="user_type">0用户1保理</param>
        /// <param name="scount"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public DataSet GetUserWithDrawList(string LoginName, string MyRealName, DateTime s_ctime, DateTime e_ctime, int status, int user_type, int scount, int pagesize)
        {
            LoginName = "%" + LoginName.Trim() + "%";
            MyRealName = "%" + MyRealName.Trim() + "%";

            string strSql = @"SELECT b.login_name,b.real_name,c.bank_name,c.bank_card,c.bank_phone,a.amount,a.amount_balance,a.amount_principal,a.c_time,a.auditor_name,a.audit_time,a.loan_name,a.loan_time,a.status,a.id
                FROM user_withdraw a
                INNER JOIN `user_info` b ON a.`user_id`=b.id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE b.user_type=?user_type AND a.op_status=2 AND (?LoginName='%%' or b.login_name like ?LoginName) AND (?MyRealName='%%' or b.real_name like ?MyRealName) AND (?s_ctime='1900-01-01' or a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' or a.c_time<?e_ctime) AND (?status=-1 or a.status=?status)
                ORDER BY a.c_time DESC
                limit ?scount,?pagesize;
                SELECT COUNT(1)
                FROM user_withdraw a
                INNER JOIN `user_info` b ON a.`user_id`=b.id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE b.user_type=?user_type AND a.op_status=2 AND (?LoginName='%%' or b.login_name like ?LoginName) AND (?MyRealName='%%' or b.real_name like ?MyRealName) AND (?s_ctime='1900-01-01' or a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' or a.c_time<?e_ctime) AND (?status=-1 or a.status=?status) ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?LoginName", MySqlDbType.VarChar),
                    new MySqlParameter("?MyRealName", MySqlDbType.VarChar),
                    new MySqlParameter("?user_type", MySqlDbType.Int32),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?scount", MySqlDbType.Int32),
                    new MySqlParameter("?pagesize", MySqlDbType.Int32)
			};
            parameters[0].Value = LoginName;
            parameters[1].Value = MyRealName;
            parameters[2].Value = user_type;
            parameters[3].Value = s_ctime;
            parameters[4].Value = e_ctime;
            parameters[5].Value = status;
            parameters[6].Value = scount;
            parameters[7].Value = pagesize;
            DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取用户/保理提现数据", parameters);


            return ds;
        }
        /// <summary>
        /// 更新提现状态(审核)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateWithdrawStatus(int id, int status, int auditor_id, DateTime audit_time, string auditor_name, string remark)
        {
            string strSql = @"UPDATE user_withdraw SET status=?status,auditor_id=?auditor_id,audit_time=?audit_time,auditor_name=?auditor_name,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?auditor_id", MySqlDbType.Int32),
                    new MySqlParameter("?audit_time", MySqlDbType.DateTime),
                    new MySqlParameter("?auditor_name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = auditor_id;
            parameters[3].Value = audit_time;
            parameters[4].Value = auditor_name;
            parameters[5].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新提现状态(审核)", parameters);
        }
        /// <summary>
        /// 更新提现状态(放款)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateWithdrawLoanStatus(int id, int status, int loan_id, DateTime loan_time, string loan_name, string remark)
        {
            string strSql = @"UPDATE user_withdraw SET status=?status,loan_id=?loan_id,loan_time=?loan_time,loan_name=?loan_name,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?loan_id", MySqlDbType.Int32),
                    new MySqlParameter("?loan_time", MySqlDbType.DateTime),
                    new MySqlParameter("?loan_name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = loan_id;
            parameters[3].Value = loan_time;
            parameters[4].Value = loan_name;
            parameters[5].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新提现状态(放款)", parameters);
        }
        /// <summary>
        /// 放款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public bool WithdrawLoan(int id, int loan_id, string loan_name, DateTime loan_time)
        {
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //更新为已提现
            string strSql = @"UPDATE user_withdraw SET status=?status,loan_id=?loan_id,loan_time=?loan_time,loan_name=?loan_name WHERE id=?id;";

            DbParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?loan_id", MySqlDbType.Int32),
                    new MySqlParameter("?loan_time", MySqlDbType.DateTime),
                    new MySqlParameter("?loan_name", MySqlDbType.VarChar,20)
			};
            parameters[0].Value = id;
            parameters[1].Value = 2;
            parameters[2].Value = loan_id;
            parameters[3].Value = loan_time;
            parameters[4].Value = loan_name;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加账户变动日志
            strSql = @"INSERT INTO user_account_log (c_time,user_id,TYPE,before_amount,after_amount,change_amount,account_source_id,from_id)
                        SELECT ?c_time,a.user_id,0,b.amount,b.amount-a.amount,a.amount,17,a.id
                        FROM user_withdraw a
                        INNER JOIN user_account b ON a.user_id=b.user_id
                        WHERE a.id=?id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = id;
            parameters[1].Value = DateTime.Now;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            //更新用户总账户
            strSql = @"UPDATE user_account a
                    INNER JOIN user_withdraw b ON a.user_id=b.user_id
                    SET a.amount_blance=a.amount_blance-b.amount,a.amount_blance_fz=a.amount_blance_fz-b.amount,a.amount=a.amount-b.amount
                    WHERE b.id=?id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //更新余额构成
            strSql = @"UPDATE user_balance_item a
                        INNER JOIN user_withdraw b ON a.user_id=b.user_id
                        SET a.amount=a.amount-b.amount,a.amount_fz=a.amount_fz-b.amount
                        WHERE b.id=?id AND a.balance_type=6;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);


            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
        /// <summary>
        /// 插入提现申请表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="from_id"></param>
        /// <param name="c_time"></param>
        /// <param name="creator_id"></param>
        /// <param name="mer_ord_id"></param>
        /// <param name="identityid"></param>
        /// <param name="identitytype"></param>
        /// <param name="card"></param>
        /// <param name="card_top"></param>
        /// <param name="card_last"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool InsertWithdrawApply(int type, int from_id, DateTime c_time, int creator_id, string mer_ord_id, string identityid, int identitytype, string card, string card_top, string card_last, decimal amount)
        {
            string strSql = @"INSERT INTO withdraw_apply (type,from_id,c_time,creator_id,mer_ord_id,identityid,identitytype,card,card_top,card_last,amount) 
                                VALUES(?type,?from_id,?c_time,?creator_id,?mer_ord_id,?identityid,?identitytype,?card,?card_top,?card_last,?amount);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?from_id", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32),
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,100),
                    new MySqlParameter("?identityid", MySqlDbType.VarChar,50),
                    new MySqlParameter("?identitytype", MySqlDbType.Int32),
                    new MySqlParameter("?card", MySqlDbType.VarChar,50),
                    new MySqlParameter("?card_top", MySqlDbType.VarChar,6),
                    new MySqlParameter("?card_last", MySqlDbType.VarChar,4),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),

			};
            parameters[0].Value = type;
            parameters[1].Value = from_id;
            parameters[2].Value = c_time;
            parameters[3].Value = creator_id;
            parameters[4].Value = mer_ord_id;
            parameters[5].Value = identityid;
            parameters[6].Value = identitytype;
            parameters[7].Value = card;
            parameters[8].Value = card_top;
            parameters[9].Value = card_last;
            parameters[10].Value = amount;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "插入提现申请表", parameters);
        }
        /// <summary>
        /// 更新提现申请为已成功
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <param name="thir_ord_id"></param>
        /// <param name="status"></param>
        /// <param name="m_time"></param>
        /// <returns></returns>
        public bool UpdateWithdrawApplyStatus(string mer_ord_id, string thir_ord_id, int status, DateTime m_time)
        {
            string strSql = @"UPDATE withdraw_apply SET thir_ord_id=?thir_ord_id,status=?status,m_time=?m_time WHERE mer_ord_id=?mer_ord_id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,50),
                    new MySqlParameter("?thir_ord_id", MySqlDbType.VarChar,50),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = mer_ord_id;
            parameters[1].Value = thir_ord_id;
            parameters[2].Value = status;
            parameters[3].Value = m_time;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新提现状态", parameters);
        }
        /// <summary>
        /// 更新提现申请错误备注
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <param name="thir_ord_id"></param>
        /// <param name="status"></param>
        /// <param name="m_time"></param>
        /// <returns></returns>
        public bool UpdateWithdrawApplyErrorRemark(string mer_ord_id, string Remark, DateTime m_time)
        {
            string strSql = @"UPDATE withdraw_apply SET Remark=?Remark,m_time=?m_time WHERE mer_ord_id=?mer_ord_id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,50),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar,1000),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = mer_ord_id;
            parameters[1].Value = Remark;
            parameters[2].Value = m_time;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新提现申请错误备注", parameters);
        }
        /// <summary>
        /// 获取用户提现的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetUserWithDrawInfoByID(int id)
        {
            string strSql = @"SELECT a.user_id,c.bank_card,a.amount,a.status,a.remark,d.amount_blance,d.amount_blance_fz
                FROM user_withdraw a
                INNER JOIN user_account d ON a.user_id=d.user_id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE a.id=?id;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;
            DataTable dt = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取用户提现的用户信息", parameters).Tables[0];

            return dt;
        }
        /// <summary>
        /// 根据状态获取所有提现id
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAllUserWithDraw(int status, int user_type)
        {
            string strSql = @"SELECT a.id
                FROM user_withdraw a
                INNER JOIN `user_account` b ON a.`user_id`=b.user_id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE b.user_type=?user_type AND a.op_status=2 AND a.status=?status;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?user_type", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32)
			};
            parameters[0].Value = user_type;
            parameters[1].Value = status;
            DataTable dt = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "根据状态获取所有提现id", parameters).Tables[0];
            return dt;
        }
        #endregion
        #region 用户赎回
        /// <summary>
        /// 获取用户/保理提现数据
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">姓名</param>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <param name="status">状态(0未审核1审核通过2已提现3审核未通过4支付失败)</param>
        /// <param name="user_type">0用户1保理</param>
        /// <param name="scount"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public DataSet GetUserRedemptionList(string LoginName, string MyRealName, DateTime s_ctime, DateTime e_ctime, int status, int scount, int pagesize)
        {
            LoginName = "%" + LoginName.Trim() + "%";
            MyRealName = "%" + MyRealName.Trim() + "%";

            string strSql = @"SELECT b.login_name,b.real_name,c.bank_name,c.bank_card,c.bank_phone,a.amount,CASE WHEN d.amount_violate IS NULL OR d.amount_violate=0 THEN a.amount*penalty_rate ELSE d.amount_violate END AS amount_penalty,a.c_time,a.auditor_name,a.audit_time,a.loan_name,a.loan_time,a.status,a.id
                FROM user_redemption a
                INNER JOIN `user_info` b ON a.`user_id`=b.id
                LEFT JOIN user_ord_info d ON a.ord_id=d.id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE a.op_status=2 AND (?LoginName='%%' or b.login_name like ?LoginName) AND (?MyRealName='%%' or b.real_name like ?MyRealName) AND (?s_ctime='1900-01-01' or a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' or a.c_time<?e_ctime) AND (?status=-1 or a.status=?status)
                ORDER BY a.c_time DESC
                limit ?scount,?pagesize;
                SELECT COUNT(1)
                FROM user_redemption a
                INNER JOIN `user_info` b ON a.`user_id`=b.id
                LEFT JOIN user_ord_info d ON a.ord_id=d.id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE a.op_status=2 AND (?LoginName='%%' or b.login_name like ?LoginName) AND (?MyRealName='%%' or b.real_name like ?MyRealName) AND (?s_ctime='1900-01-01' or a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' or a.c_time<?e_ctime) AND (?status=-1 or a.status=?status) ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?LoginName", MySqlDbType.VarChar),
                    new MySqlParameter("?MyRealName", MySqlDbType.VarChar),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?scount", MySqlDbType.Int32),
                    new MySqlParameter("?pagesize", MySqlDbType.Int32)
			};
            parameters[0].Value = LoginName;
            parameters[1].Value = MyRealName;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = status;
            parameters[5].Value = scount;
            parameters[6].Value = pagesize;
            DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取用户/保理提现数据", parameters);


            return ds;
        }
        /// <summary>
        /// 获取用户赎回的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetUserRedemptionInfoByID(int id)
        {
            string strSql = @"SELECT a.user_id,c.bank_card,a.amount,a.status,CASE WHEN d.amount_violate IS NULL OR d.amount_violate=0 THEN a.amount*penalty_rate ELSE d.amount_violate END AS amount_penalty,a.remark,e.amount_blance,e.amount_blance_fz
                FROM user_redemption a
                INNER JOIN user_account e ON a.user_id=e.user_id
                LEFT JOIN user_ord_info d ON a.ord_id=d.id
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE a.id=?id;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;
            DataTable dt = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取用户赎回的用户信息", parameters).Tables[0];

            return dt;
        }
        /// <summary>
        /// 更新赎回状态(审核)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateRedemptionStatus(int id, int status, int auditor_id, DateTime audit_time, string auditor_name, string remark)
        {
            string strSql = @"UPDATE user_redemption SET status=?status,auditor_id=?auditor_id,audit_time=?audit_time,auditor_name=?auditor_name,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?auditor_id", MySqlDbType.Int32),
                    new MySqlParameter("?audit_time", MySqlDbType.DateTime),
                    new MySqlParameter("?auditor_name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = auditor_id;
            parameters[3].Value = audit_time;
            parameters[4].Value = auditor_name;
            parameters[5].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新赎回状态(审核)", parameters);
        }
        /// <summary>
        /// 更新赎回状态(放款)
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateRedemptionLoanStatus(int id, int status, int loan_id, DateTime loan_time, string loan_name, string remark)
        {
            string strSql = @"UPDATE user_redemption SET status=?status,loan_id=?loan_id,loan_time=?loan_time,loan_name=?loan_name,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?loan_id", MySqlDbType.Int32),
                    new MySqlParameter("?loan_time", MySqlDbType.DateTime),
                    new MySqlParameter("?loan_name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = loan_id;
            parameters[3].Value = loan_time;
            parameters[4].Value = loan_name;
            parameters[5].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新赎回状态(放款)", parameters);
        }
        /// <summary>
        /// 放款（赎回）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public bool RedemptionLoan(int id, int loan_id, string loan_name, DateTime loan_time,decimal amount)
        {
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //更新为已提现
            string strSql = @"UPDATE user_redemption SET status=?status,loan_id=?loan_id,loan_time=?loan_time,loan_name=?loan_name WHERE id=?id;";

            DbParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?loan_id", MySqlDbType.Int32),
                    new MySqlParameter("?loan_time", MySqlDbType.DateTime),
                    new MySqlParameter("?loan_name", MySqlDbType.VarChar,20)
			};
            parameters[0].Value = id;
            parameters[1].Value = 2;
            parameters[2].Value = loan_id;
            parameters[3].Value = loan_time;
            parameters[4].Value = loan_name;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加账户变动日志
            strSql = @"INSERT INTO user_account_log (c_time,user_id,TYPE,before_amount,after_amount,change_amount,account_source_id,from_id)
                        SELECT ?c_time,a.user_id,0,b.amount,b.amount-?amount,?amount,18,a.id
                        FROM user_redemption a
                        INNER JOIN user_account b ON a.user_id=b.user_id
                        WHERE a.id=?id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?amount", MySqlDbType.Decimal)
			};
            parameters[0].Value = id;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = amount;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            //更新用户总账户
            strSql = @"UPDATE user_account a
                    INNER JOIN user_redemption b ON a.user_id=b.user_id
                    SET a.amount_blance=a.amount_blance-?amount,a.amount_blance_fz=a.amount_blance_fz-?amount,a.amount=a.amount-?amount
                    WHERE b.id=?id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?amount", MySqlDbType.Decimal)
			};
            parameters[0].Value = id;
            parameters[1].Value = amount;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //更新余额构成
            strSql = @"UPDATE user_balance_item a
                        INNER JOIN user_redemption b ON a.user_id=b.user_id
                        SET a.amount=a.amount-?amount,a.amount_fz=a.amount_fz-?amount
                        WHERE b.id=?id AND a.balance_type=6;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?amount", MySqlDbType.Decimal)
			};
            parameters[0].Value = id;
            parameters[1].Value = amount;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //更新ord_info atone_status=2
            strSql = @"UPDATE user_ord_info a
                        INNER JOIN user_redemption b ON a.id=b.ord_id
                        SET a.atone_status=2
                        WHERE b.id=?id;";

            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
        /// <summary>
        /// 根据状态获取所有赎回id
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAllUserRedemption(int status)
        {
            string strSql = @"SELECT a.id
                FROM user_redemption a
                
                LEFT JOIN `user_bank_card` c ON a.user_id=c.user_id AND c.status=1
                WHERE a.op_status=2 AND a.status=?status;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?status", MySqlDbType.Int32)
			};
            parameters[0].Value = status;
            DataTable dt = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "根据状态获取所有赎回id", parameters).Tables[0];
            return dt;
        }
        #endregion
    }
}
