using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.LoanManager
{
    /// <summary>
    /// 放款管理数据处理类
    /// </summary>
    public class LoanManagerDao
    {
        #region 待放款
        /// <summary>
        /// 获取待放款列表
        /// </summary>
        /// <param name="mName">融资方姓名</param>
        /// <param name="LoanPeriod">项目周期</param>
        /// <param name="LowMoney"></param>
        /// <param name="UpMoney"></param>
        /// status : 状态： 0未处理；3.已放款；5.失败
        /// <param name="Scount">起始记录</param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetLoanList(string mName, int LoanPeriod, decimal LowMoney, decimal UpMoney,
            int Scount, int PageSize)
        {
            mName = "%" + mName.Trim() + "%";
            string strsql = @"
                            SELECT  a.id,a.c_time AS ctime,a.pjt_id AS pjid,a.fer_account_id AS account_id,a.amount AS amount_expect,
	                                b.name AS projectname,b.amount AS Amount,b.loan_period AS LoanPeriod,b.amount_lent,
	                                d.name AS fina_name,d.bank_name,d.bank_card,d.myreal_name,d.phone
                            FROM fer_account_item a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_info d ON a.fer_id=d.id 
                            WHERE a.`amount` > 0 
                            AND (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)
                            Order By a.c_time DESC
                            LIMIT ?Scount,?PageSize;

                            SELECT  COUNT(1)
                            FROM fer_account_item a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_info d ON a.fer_id=d.id 
                            WHERE a.`amount` > 0 
                            AND (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)";

            MySqlParameter[] parameters = {
					new MySqlParameter("?mName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?LAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?UAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?Scount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = mName;
            parameters[1].Value = LoanPeriod;
            parameters[2].Value = LowMoney;
            parameters[3].Value = UpMoney;
            parameters[4].Value = Scount;
            parameters[5].Value = PageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strsql, "获取待放款列表", parameters);
        }
        /// <summary>
        /// 获取所有放款记录
        /// 不分页，导出Excel
        /// </summary>
        /// <param name="PJname">融资方名称</param>
        /// <param name="LoanPeriod">项目期限</param>
        /// <param name="LowMoney">项目金额</param>
        /// <param name="UpMoney">项目金额</param>
        /// <param name="status">状态：0未处理，3放款成功；5失败</param>
        /// <returns></returns>
        public DataSet GetAllLoanList(string mName, int LoanPeriod, decimal LowMoney, decimal UpMoney, int status)
        {
            mName = "%" + mName.Trim() + "%";
            string strsql = @"  
                            SELECT  a.id,a.c_time AS ctime,a.pjt_id AS pjid,a.fer_account_id AS account_id,a.amount AS amount_expect,
	                                b.name AS projectname,b.amount AS Amount,b.loan_period AS LoanPeriod,b.amount_lent,
	                                d.name AS fina_name,d.bank_name,d.bank_card,d.myreal_name,d.phone
                            FROM fer_account_item a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_info d ON a.fer_id=d.id 
                            WHERE (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)";

            MySqlParameter[] parameters = {
					new MySqlParameter("?mName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?LAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?UAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?Status", MySqlDbType.Int32)
            };
            parameters[0].Value = mName;
            parameters[1].Value = LoanPeriod;
            parameters[2].Value = LowMoney;
            parameters[3].Value = UpMoney;
            parameters[4].Value = status;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strsql, "根据查询条件获取所有放款记录", parameters);
        }
        /// <summary>
        /// 全部待放款记录
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllLoanList()
        {
            return GetAllLoanList("", -1, -1, -1, 0);
        }
        /// <summary>
        /// 插入放款失败记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pjid"></param>
        /// <param name="amount_lent"></param>
        /// <param name="adminid"></param>
        /// <param name="status"></param>
        /// <param name="BankNum"></param>
        /// <returns></returns>
        public bool LoanFailed(int pjid, decimal amount, int adminid, string remark = "")
        {
            string strsql = @"INSERT INTO pjt_loan_fail
                            (pjt_id,creator_id,c_time,amount,STATUS,remark)
                            VALUES
                            (?pjid,?AdminID,?CTime,?Amount,?STATUS,?Remark) ";

            MySqlParameter[] parameters = {
                    new MySqlParameter("?pjid", MySqlDbType.Int32,11),
                    new MySqlParameter("?AdminID", MySqlDbType.Int32),
                    new MySqlParameter("?CTime", MySqlDbType.DateTime),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal),
                    new MySqlParameter("?STATUS", MySqlDbType.Int16),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = pjid;
            parameters[1].Value = adminid;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = amount;
            parameters[4].Value = 0;
            parameters[5].Value = "财务放款 " + remark;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strsql, "插入放款失败记录", parameters);
        }
        /// <summary>
        /// 放款成功
        /// </summary>
        public bool LoanSucceeded(int pjid, decimal amount, int adminid, string bankCard, int fer_account_id, 
            int recordID = -1, string remark = "")
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            #region 1.更新项目放款金额
            var sql1 = @"UPDATE pjt SET amount_lent=amount_lent+?amount_lent WHERE id=?pjid;";
            MySqlParameter[] param1 = {
                    new MySqlParameter("?pjid", MySqlDbType.Int32,11),
                    new MySqlParameter("?amount_lent", MySqlDbType.Decimal)};
            param1[0].Value = pjid;
            param1[1].Value = amount;
            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            #region 2.插入放款记录
            var sql2 = @"   INSERT INTO pjt_loan_log(pjt_id,c_time,amount,creator_id,bank_card_num,remark)
                            VALUES(?pjid,?LogTime,?amount,?adminid,?BankNum,?Remark);";
            MySqlParameter[] param2 = {
                    new MySqlParameter("?pjid", MySqlDbType.Int32,11),
                    new MySqlParameter("?LogTime", MySqlDbType.DateTime),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?adminid", MySqlDbType.Int32),
                    new MySqlParameter("?BankNum", MySqlDbType.VarChar, 30),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar, 100)};
            param2[0].Value = pjid;
            param2[1].Value = DateTime.Now;
            param2[2].Value = amount;
            param2[3].Value = adminid;
            param2[4].Value = bankCard;
            param2[5].Value = remark;
            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            #region 3.更新fer_account_item
            var sql3 = @"UPDATE fer_account_item SET 
	                        m_time=?MTime,amount=amount-?Amount,remark=?Remark
	                        WHERE pjt_id=?pjid;";
            MySqlParameter[] param3 = {
                    new MySqlParameter("?pjid", MySqlDbType.Int32,11),
                    new MySqlParameter("?MTime", MySqlDbType.DateTime),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar, 100)};
            param3[0].Value = pjid;
            param3[1].Value = DateTime.Now;
            param3[2].Value = amount;
            param3[3].Value = remark;
            sqlArray.Add(sql3);
            paramArray.Add(param3);
            #endregion

            #region 4.更新融资方账户
            var sql4 = @"UPDATE fer_account SET 
	                        m_time=?MTime,amount=amount-?Amount,amount_user=amount_user-?Amount,remark=?Remark
                        WHERE id=?MerchantID;";
            MySqlParameter[] param4 = {
                    new MySqlParameter("?MTime", MySqlDbType.DateTime),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar, 100),
                    new MySqlParameter("?MerchantID", MySqlDbType.Int32)};
            param4[0].Value = DateTime.Now;
            param4[1].Value = amount;
            param4[2].Value = remark;
            param4[3].Value = fer_account_id;
            sqlArray.Add(sql4);
            paramArray.Add(param4);
            #endregion

            #region 5.插入融资方账户日志 fer_account_log
            var sql5 = @"INSERT INTO fer_account_log
                                (c_time,creator_id,fer_account_id,TYPE,amount_before,amount_after,change_amount,account_source_id,pjt_id,remark)
                            VALUES
                                (?CTime,?AdminID,?FerAccountID,?Type,
                                (SELECT amount+?Amount FROM fer_account WHERE id=?FerAccountID),
                                (SELECT amount FROM fer_account WHERE id=?FerAccountID),
                                ?Amount,?SourceID,?pjid,?Remark);";

            MySqlParameter[] param5 = {
					new MySqlParameter("?CTime", MySqlDbType.DateTime),
                    new MySqlParameter("?AdminID", MySqlDbType.Int32,11),
                    new MySqlParameter("?FerAccountID", MySqlDbType.Int32),
                    new MySqlParameter("?Type", MySqlDbType.Int16),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal),
                    new MySqlParameter("?SourceID", MySqlDbType.Int16),
                    new MySqlParameter("?pjid", MySqlDbType.Int32),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar,100)
			};
            param5[0].Value = DateTime.Now;
            param5[1].Value = adminid;
            param5[2].Value = fer_account_id;
            param5[3].Value = 0;
            param5[4].Value = amount;
            param5[5].Value = 20;
            param5[6].Value = pjid;
            param5[7].Value = "财务放款 " + remark;
            sqlArray.Add(sql5);
            paramArray.Add(param5);
            #endregion

            #region 6.重新放款成功，更新放款失败记录状态
            if (recordID > 0)
            {
                var sql6 = @"UPDATE pjt_loan_fail SET STATUS=1 
                            WHERE id=?ID AND pjt_id=?ProjectID;";
                MySqlParameter[] param6 = {
                    new MySqlParameter("?ID", MySqlDbType.Int32),
                    new MySqlParameter("?ProjectID", MySqlDbType.Int32)};
                param6[0].Value = recordID;
                param6[1].Value = pjid;

                sqlArray.Add(sql6);
                paramArray.Add(param6);
            }
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region 放款失败
        /// <summary>
        /// 获取放款失败列表
        /// </summary>
        /// <param name="mName">融资方姓名</param>
        /// <param name="LoanPeriod">项目周期</param>
        /// <param name="LowMoney"></param>
        /// <param name="UpMoney"></param>
        /// status : 状态： 0未处理；3.已放款；5.失败
        /// <param name="Scount">起始记录</param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetLoanFailedList(string mName, int LoanPeriod, decimal LowMoney, decimal UpMoney,
            DateTime? sDate, DateTime? eDate, int Scount, int PageSize)
        {
            mName = "%" + mName.Trim() + "%";
            string strsql = @"
                            SELECT a.id,a.c_time AS ctime,a.pjt_id AS pjid,b.name AS projectname,b.amount AS Amount,
                                b.loan_period AS LoanPeriod,b.amount_lent,a.amount AS amount_expect,c.id AS account_id,
                                d.name AS fina_name,d.bank_name,d.bank_card,d.myreal_name,d.phone,
                                a.`creator_id` AS adminID,a.`c_time` AS loanTime,a.remark AS Remark
                            FROM pjt_loan_fail a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_account c ON b.fer_account_id = c.id
                            INNER JOIN fer_info d ON c.fer_id=d.id   
                            WHERE a.status=0
                            AND (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)
                            AND (?SDate IS NULL OR a.c_time>=?SDate) AND (?EDate IS NULL OR a.c_time<=?EDate)
                            Order By a.c_time DESC
                            LIMIT ?Scount,?PageSize;

                            SELECT  COUNT(1)
                            FROM pjt_loan_fail a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_account c ON b.fer_account_id = c.id
                            INNER JOIN fer_info d ON c.fer_id=d.id
                            WHERE a.status=0	  
                            AND (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)
                            AND (?SDate IS NULL OR a.c_time>=?SDate) AND (?EDate IS NULL OR a.c_time<=?EDate);";

            MySqlParameter[] parameters = {
					new MySqlParameter("?mName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?LAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?UAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?Scount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?SDate", MySqlDbType.DateTime),
                    new MySqlParameter("?EDate", MySqlDbType.DateTime)
            };
            parameters[0].Value = mName;
            parameters[1].Value = LoanPeriod;
            parameters[2].Value = LowMoney;
            parameters[3].Value = UpMoney;
            parameters[4].Value = Scount;
            parameters[5].Value = PageSize;
            parameters[6].Value = sDate;
            parameters[7].Value = eDate;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strsql, "获取放款失败列表", parameters);
        }
        #endregion

        #region 放款记录
        /// <summary>
        /// 获取放款记录
        /// </summary>
        /// <param name="mName">融资方姓名</param>
        /// <param name="LoanPeriod">项目周期</param>
        /// <param name="LowMoney"></param>
        /// <param name="UpMoney"></param>
        /// status : 状态： 0未处理；3.已放款；5.失败
        /// <param name="Scount">起始记录</param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataSet GetLoanLog(string mName, int LoanPeriod, decimal LowMoney, decimal UpMoney,
            DateTime? sDate, DateTime? eDate, int Scount, int PageSize)
        {
            mName = "%" + mName.Trim() + "%";
            string strsql = @"
                            SELECT  a.id,a.pjt_id AS pjid,a.c_time AS ctime,a.amount AS amount_expect,a.creator_id AS adminID,a.remark AS Remark,
	                            b.name AS projectname,b.amount AS Amount,b.loan_period AS LoanPeriod,
	                            c.id AS m_id,d.name AS fina_name,d.myreal_name,d.phone,d.bank_name,d.bank_card
                            FROM pjt_loan_log a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_account c ON b.fer_account_id = c.id
                            INNER JOIN fer_info d ON c.fer_id=d.id
                            WHERE (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)
                            AND (?SDate IS NULL OR a.c_time>=?SDate) AND (?EDate IS NULL OR a.c_time <= ?EDate)
                            Order By a.c_time DESC
                            LIMIT ?Scount,?PageSize;

                            SELECT COUNT(1) 
                            FROM pjt_loan_log a
                            INNER JOIN pjt b ON a.pjt_id=b.id
                            INNER JOIN fer_account c ON b.fer_account_id = c.id
                            INNER JOIN fer_info d ON c.fer_id=d.id
                            WHERE (?mName='%%' OR d.name LIKE ?mName) 
                            AND (?LoanPeriod=-1 OR b.loan_period=?LoanPeriod) 
                            AND (?LAmount=-1 OR b.amount>=?LAmount) AND (?UAmount=-1 OR b.amount<=?UAmount)
                            AND (?SDate IS NULL OR a.c_time>=?SDate) AND (?EDate IS NULL OR a.c_time <= ?EDate)";

            MySqlParameter[] parameters = {
					new MySqlParameter("?mName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?LAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?UAmount", MySqlDbType.Decimal),
                    new MySqlParameter("?Scount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?SDate", MySqlDbType.DateTime),
                    new MySqlParameter("?EDate", MySqlDbType.DateTime),
            };
            parameters[0].Value = mName;
            parameters[1].Value = LoanPeriod;
            parameters[2].Value = LowMoney;
            parameters[3].Value = UpMoney;
            parameters[4].Value = Scount;
            parameters[5].Value = PageSize;
            parameters[6].Value = sDate;
            parameters[7].Value = eDate;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strsql, "获取待放款列表", parameters);
        }
        #endregion
    }
}
