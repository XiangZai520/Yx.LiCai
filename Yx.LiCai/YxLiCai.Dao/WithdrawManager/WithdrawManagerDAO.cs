using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.WithdrawManager
{
    /// <summary>
    /// 用户提现、赎回 后台审核管理类
    /// add by lxm
    /// </summary>
    public class WithdrawManagerDAO
    {
        #region 融资方提现
        /// <summary>
        /// 查询融资方提现记录
        /// </summary>
        public DataSet GetMerchantWithdrawList(string lName, int status, DateTime? sDate, DateTime? eDate,
            int startIndex, int pageSize)
        {
            lName = "%" + lName.Trim() + "%";
            var strSql = @" 
                            SELECT  a.id,a.`fer_id`,a.`amount`,a.`c_time`,a.`auditor_id`,a.`audit_time`,a.`operator_id`,a.`pay_time`,a.`status`,a.remark,
	                            b.`l_name`,b.`myreal_name`,b.`bank_name`,b.`bank_card`,b.`phone`
                            FROM fer_withdraw a
                            INNER JOIN fer_info b ON a.`creator_id` = b.`id`
                            WHERE (?STATUS = -1 OR a.`status`=?STATUS)
                            AND (?NAME = '%%' OR b.`l_name` LIKE ?NAME)
                            AND (?SDate IS NULL OR a.`c_time` >= ?SDate) 
                            AND (?EDate IS NULL OR a.`c_time` <= ?EDate)
                            ORDER BY a.`c_time` DESC 
                            LIMIT ?StartIndex, ?PageSize;

                            SELECT  COUNT(1)
                            FROM fer_withdraw a
                            INNER JOIN fer_info b ON a.`creator_id` = b.`id`
                            WHERE (?STATUS = -1 OR a.`status`=?STATUS)
                            AND (?NAME = '%%' OR b.`l_name` LIKE ?NAME)
                            AND (?SDate IS NULL OR a.`c_time` >= ?SDate) 
                            AND (?EDate IS NULL OR a.`c_time` <= ?EDate)";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
					new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?NAME", MySqlDbType.VarChar, 50),
                    new MySqlParameter("?SDate", MySqlDbType.DateTime),  
                    new MySqlParameter("?EDate", MySqlDbType.DateTime),
                    new MySqlParameter("?STATUS", MySqlDbType.Int16)};
            parameters[0].Value = startIndex;
            parameters[1].Value = pageSize;
            parameters[2].Value = lName;
            parameters[3].Value = sDate;
            parameters[4].Value = eDate;
            parameters[5].Value = status;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "查询融资方提现记录", parameters);
        }
        /// <summary>
        /// 查询融资方提现记录
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public DataSet GetMerchantWithdrawList(int status, string idList = "")
        {
            var strSql = new StringBuilder();
            strSql.Append(@" 
                            SELECT  a.id,a.`fer_id`,a.`amount`,a.`c_time`,a.`auditor_id`,a.`audit_time`,a.`operator_id`,a.`pay_time`,a.`status`,a.remark,
                                b.`l_name`,b.`myreal_name`,b.`bank_name`,b.`bank_card`,b.`phone`
                            FROM fer_withdraw a
                            INNER JOIN fer_info b ON a.`fer_id` = b.`id`
                            WHERE a.`status`= ?Status ");

            if (!string.IsNullOrEmpty(idList))
            {
                strSql.Append(" AND a.id IN (" + idList + ");");
            }

            MySqlParameter[] parameters = {
                    new MySqlParameter("?Status", MySqlDbType.Int16)};
            parameters[0].Value = status;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "查询融资方提现记录", parameters);
        }
        /// <summary>
        /// 更新融资方提现记录
        /// </summary>
        public bool UpdateMerchantWithdrawRecord(int status, int auditorID, int recordID, string remark = "")
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE fer_withdraw SET STATUS=?Status,remark=?Remark");
            if (auditorID > 0)
            {
                strSql.Append(@",auditor_id=?AuditorID,audit_time=?AuditTime");
            }
            strSql.Append(@" WHERE id=?ID;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("?Status", MySqlDbType.Int16),
					new MySqlParameter("?AuditorID", MySqlDbType.Int32),
                    new MySqlParameter("?AuditTime",MySqlDbType.DateTime),
                    new MySqlParameter("?Remark",MySqlDbType.VarChar,200),
                    new MySqlParameter("?ID",MySqlDbType.Int32)};
            parameters[0].Value = status;
            parameters[1].Value = auditorID;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = remark;
            parameters[4].Value = recordID;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新赎回申请状态", parameters);
        }
        /// <summary>
        /// 融资方提现 - 财务支付 
        /// </summary>
        public bool PayMerchant(long recordId, int mID, int status, int adminID, decimal amount, string orderID, int fer_account_id, string remark = "")
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            #region 1) 更新融资方提现记录表 fer_withdraw
            var sql1 = @"UPDATE fer_withdraw 
                        SET STATUS=?Status,operator_id=?AdminID,pay_time=?PayTime,remark=?Remark
                        WHERE id=?ID AND fer_id=?MID";
            MySqlParameter[] param1 = {
                    new MySqlParameter("?Status", MySqlDbType.Int16),
					new MySqlParameter("?AdminID", MySqlDbType.Int32),
                    new MySqlParameter("?PayTime",MySqlDbType.DateTime),
                    new MySqlParameter("?OrderID",MySqlDbType.VarChar,50),
                    new MySqlParameter("?ID",MySqlDbType.Int32),
                    new MySqlParameter("?MID",MySqlDbType.Int32),
                    new MySqlParameter("?Remark",MySqlDbType.VarChar, 200)};
            param1[0].Value = status;
            param1[1].Value = adminID;
            param1[2].Value = DateTime.Now;
            param1[3].Value = orderID;
            param1[4].Value = recordId;
            param1[5].Value = mID;
            param1[6].Value = remark;

            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            if (status == 3)
            {
                #region 2)申请提现后改变账户冻结金额
                var sql2 = @" UPDATE fer_account
                        SET amount=amount-?Amount,amount_repay=amount_repay-?Amount,amount_repay_fz=amount_repay_fz-?Amount
                        WHERE id=?MID";

                MySqlParameter[] param2 = {        
                    new MySqlParameter("?MID", MySqlDbType.Int64),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal)};
                param2[0].Value = mID;
                param2[1].Value = amount;

                sqlArray.Add(sql2);
                paramArray.Add(param2);
                #endregion

                #region 3)融资方账户变更日志fer_account_log
                var sql3 = @"INSERT INTO fer_account_log
                                (c_time,creator_id,fer_account_id,TYPE,amount_before,amount_after,change_amount,account_source_id,pjt_id,remark)
                            VALUES
                                (?CTime,?AdminID,?FerAccountID,?Type,
                                (SELECT amount+?Amount FROM fer_account WHERE id=?FerAccountID),
                                (SELECT amount FROM fer_account WHERE id=?FerAccountID),
                                ?Amount,?SourceID,?pjid,?Remark);";

                MySqlParameter[] param3 = {
					new MySqlParameter("?CTime", MySqlDbType.DateTime),
                    new MySqlParameter("?AdminID", MySqlDbType.Int32,11),
                    new MySqlParameter("?FerAccountID", MySqlDbType.Int32),
                    new MySqlParameter("?Type", MySqlDbType.Int16),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal),
                    new MySqlParameter("?SourceID", MySqlDbType.Int16),
                    new MySqlParameter("?pjid", MySqlDbType.Int32),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar,100)};
                param3[0].Value = DateTime.Now;
                param3[1].Value = adminID;
                param3[2].Value = fer_account_id;
                param3[3].Value = 0;
                param3[4].Value = amount;
                param3[5].Value = 20;
                param3[6].Value = -1;
                param3[7].Value = "融资方提现 " + remark;
                sqlArray.Add(sql3);
                paramArray.Add(param3);
                #endregion
            }

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        /// <summary>
        /// 批量通过
        /// </summary>
        public bool PassMultiMerchantWithdraw(string idList, int adminID)
        {
            var strSql = @" UPDATE fer_withdraw SET STATUS=?Status,auditor_id=?AuditorID,audit_time=?AuditTime
                            WHERE id in (" + idList + ") And STATUS=0;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?Status", MySqlDbType.Int16),
					new MySqlParameter("?AuditorID", MySqlDbType.Int32),
                    new MySqlParameter("?AuditTime",MySqlDbType.DateTime)};
            parameters[0].Value = 1;
            parameters[1].Value = adminID;
            parameters[2].Value = DateTime.Now;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "批量审核通过", parameters);
        }
        /// <summary>
        /// 全部审核通过
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public bool PassAllMerchantWithdraw(int adminID)
        {
            var strSql = @" UPDATE fer_withdraw SET STATUS=?Status,auditor_id=?AuditorID,audit_time=?AuditTime
                            WHERE STATUS=0;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?Status", MySqlDbType.Int16),
					new MySqlParameter("?AuditorID", MySqlDbType.Int32),
                    new MySqlParameter("?AuditTime",MySqlDbType.DateTime)};
            parameters[0].Value = 1;
            parameters[1].Value = adminID;
            parameters[2].Value = DateTime.Now;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "全部审核通过", parameters);
        }

        #endregion
    }
}
