using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.Refund;

namespace YxLiCai.Dao.Refund
{
    /// <summary>
    /// 财务还款操作数据处理类
    /// </summary>
    public class RefundDAO
    {
        /// <summary>
        /// 转为还款model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public RefundModelExtend DataToRefundModel(DataRow row)
        {
            var model = new RefundModelExtend();

            #region match
            if (row != null)
            {
                if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = long.Parse(row["id"].ToString());
                }
                if (row.Table.Columns.Contains("merchantID") && row["merchantID"] != null && row["merchantID"].ToString() != "")
                {
                    model.merchantID = int.Parse(row["merchantID"].ToString());
                }
                if (row.Table.Columns.Contains("pjid") && row["pjid"] != null && row["pjid"].ToString() != "")
                {
                    model.pjid = int.Parse(row["pjid"].ToString());
                }
                if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row.Table.Columns.Contains("ctime") && row["ctime"] != null && row["ctime"].ToString() != "")
                {
                   model.ctime = DateTime.Parse(row["ctime"].ToString());
                }
                if (row.Table.Columns.Contains("rtime") && row["rtime"] != null && row["rtime"].ToString() != "")
                {
                    model.rtime = DateTime.Parse(row["rtime"].ToString());
                }
                if (row.Table.Columns.Contains("ProjectName") && row["ProjectName"] != null && row["ProjectName"].ToString() != "")
                {
                    model.ProjectName = row["ProjectName"].ToString();
                }
                if (row.Table.Columns.Contains("ProjectAmount") && row["ProjectAmount"] != null && row["ProjectAmount"].ToString() != "")
                {
                    model.ProjectAmount = decimal.Parse(row["ProjectAmount"].ToString());
                }
                if (row.Table.Columns.Contains("pjt_id") && row["pjt_id"] != null && row["pjt_id"].ToString() != "")
                {
                    model.pjt_id = Convert.ToInt32(row["pjt_id"].ToString());
                }
                if (row.Table.Columns.Contains("Balance") && row["Balance"] != null && row["Balance"].ToString() != "")
                {
                    model.MerchantBalance = decimal.Parse(row["Balance"].ToString());
                }
                if (row.Table.Columns.Contains("financier_name") && row["financier_name"] != null && row["financier_name"].ToString() != "")
                {
                    model.MerchantName = row["financier_name"].ToString();
                }
                if (row.Table.Columns.Contains("repay_amount") && row["repay_amount"] != null && row["repay_amount"].ToString() != "")
                {
                    model.RepayAmount = decimal.Parse(row["repay_amount"].ToString());
                }
                if (row.Table.Columns.Contains("type") && row["type"] != null && row["type"].ToString() != "")
                {
                    model.Type = Convert.ToInt32(row["type"].ToString()) == 1 ? "本金" : "利息";
                }
                if (row.Table.Columns.Contains("interest_paid") && row["interest_paid"] != null && row["interest_paid"].ToString() != "")
                {
                    model.interest_paid = decimal.Parse(row["interest_paid"].ToString());
                }
                if (row.Table.Columns.Contains("LoanPeriod") && row["LoanPeriod"] != null && row["LoanPeriod"].ToString() != "")
                {
                    model.LoanPeriod = Convert.ToInt32(row["LoanPeriod"].ToString());
                }
                if (row.Table.Columns.Contains("status") && row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = Convert.ToInt32(row["status"].ToString());
                }
            }
            #endregion

            return model;
        }

        #region 还息
        /// <summary>
        /// 还款 - 利息列表
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet GetRefundInterestList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
            int startIndex, int pageSize)
        {
            merchantName = "%" + merchantName.Trim() + "%";
            string strSql = @"
                    SELECT  A.id,A.fer_account_id AS merchantID,A.pjt_id AS pjid,A.interest_payable AS amount,A.interest_paid,
	                    P.name AS ProjectName, P.Amount AS ProjectAmount,P.loan_period AS LoanPeriod,
	                    F.amount_repay - F.amount_repay_fz AS Balance,
	                    I.`name` AS financier_name
                    FROM fer_account_item A
                    INNER JOIN pjt P ON A.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`id` = A.fer_account_id
                    INNER JOIN fer_info I ON I.`id` = A.`fer_id`
                    where P.rep_status <>3
                    AND (?MerName='%%' OR I.name LIKE ?MerName)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR A.`interest_payable`>=?AmountMin) AND (?AmountMax=-1 OR A.`interest_payable`<=?AmountMax)
                    ORDER BY pjid 
                    LIMIT ?StartIndex,?PageSize;

                    SELECT  COUNT(1)
                    FROM fer_account_item A
                    INNER JOIN pjt P ON A.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`id` = A.fer_account_id
                    INNER JOIN fer_info I ON I.`id` = A.`fer_id`
                    where P.rep_status <>3
                    AND (?MerName='%%' OR I.name LIKE ?MerName)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR A.`interest_payable`>=?AmountMin) AND (?AmountMax=-1 OR A.`interest_payable`<=?AmountMax);";

            MySqlParameter[] parameters = {
					new MySqlParameter("?MerName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?AmountMin", MySqlDbType.Decimal),
                    new MySqlParameter("?AmountMax", MySqlDbType.Decimal),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = merchantName;
            parameters[1].Value = loanPeriod;
            parameters[2].Value = amount_min;
            parameters[3].Value = amount_max;
            parameters[4].Value = startIndex;
            parameters[5].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取待处理利息列表", parameters);
        }
        /// <summary>
        /// 财务还利息操作
        /// </summary>
        /// <param name="recordID">记录id</param>
        /// <param name="merchantID">融资方id</param>
        /// <param name="amount">还款金额</param>
        /// <param name="pjid">项目id</param>
        /// <returns></returns>
        public bool RefundInterest(long recordID, int merchantID, decimal amount, int pjid)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 插入还息记录表 fer_repay_interest
            #region
            var sql1 = @"
                        UPDATE fer_repay_interest
                        SET STATUS=1,rtime=?RefundTime
                        WHERE id=?ID";
            MySqlParameter[] param1 = {
					new MySqlParameter("?RefundTime", MySqlDbType.DateTime),
                    new MySqlParameter("?ID", MySqlDbType.Int32)};
            param1[0].Value = DateTime.Now;
            param1[1].Value = recordID;

            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion
            
            // 2）更新项目表 pjt 已还金额+
            #region
            var sql2 = @"
                        UPDATE pjt
                        SET amount_paid=amount_paid + ?Amount
                        WHERE id=?PID";
            MySqlParameter[] param2 = {
				new MySqlParameter("?Amount", MySqlDbType.Decimal),
                new MySqlParameter("?PID", MySqlDbType.Int64)};
            param2[0].Value = amount;
            param2[1].Value = pjid;

            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            // 3）更新融资方账户表 fer_account 
            #region
            var sql3 = @"
                        UPDATE fer_account
                        SET amount=amount-?Amount,amount_repay=amount_repay-?Amount,amount_repay_fz=amount_repay_fz-?Amount 
                        WHERE fer_id=?MID";
            MySqlParameter[] param3 = {
				new MySqlParameter("?Amount", MySqlDbType.Decimal),
                new MySqlParameter("?MID", MySqlDbType.Int64)};
            param3[0].Value = amount;
            param3[1].Value = merchantID;

            sqlArray.Add(sql3);
            paramArray.Add(param3);
            #endregion

            // 4）插入还款日志表 fer_repay_log
            #region
            var sql4 = @"
                        INSERT INTO fer_repay_log(pjt_id,amount,TYPE,creator_id,c_time,remark,fer_account_id)
                        VALUES(?PID,?Amount,?Type,?MID,?CTime,?Remark,?MID);";
            MySqlParameter[] param4 = {
                new MySqlParameter("?CTime", MySqlDbType.DateTime),
                new MySqlParameter("?MID", MySqlDbType.Int64),
                new MySqlParameter("?Type", MySqlDbType.Int16),
				new MySqlParameter("?Amount", MySqlDbType.Decimal),
                new MySqlParameter("?PID", MySqlDbType.Int64),
                new MySqlParameter("?Remark", MySqlDbType.VarChar)
            };
            param4[0].Value = DateTime.Now;
            param4[1].Value = merchantID;
            param4[2].Value = 1; //类型(0利息，1本金)
            param4[3].Value = amount;
            param4[4].Value = pjid;
            param4[5].Value = "融资方还利息";

            sqlArray.Add(sql4);
            paramArray.Add(param4);
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region 还本金
        /// <summary>
        /// 还款 - 本金列表
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet GetRefundPrincipalList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
            int startIndex, int pageSize)
        {
            merchantName = "%" + merchantName.Trim() + "%";
            string strSql = @"
                    SELECT  R.id,R.`fer_account_id` AS 'merchantID',R.`pjt_id` AS 'pjid',R.`amount`,R.`c_time` AS `ctime`,R.status,	
	                    P.`name` AS `ProjectName`,P.`Amount` AS 'ProjectAmount',P.loan_period AS `LoanPeriod`, P.amount_paid AS repay_amount,
	                    F.amount_repay - F.amount_repay_fz AS 'Balance',
	                    I.`name` AS financier_name
                    FROM fer_repay_principal R
                    INNER JOIN pjt P ON R.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`fer_id` = R.`fer_id`
                    INNER JOIN fer_info I ON I.`id` = R.`fer_id`
                    WHERE R.`status`=0 OR R.`status`=1
                    AND (?MerName='%%' OR I.name LIKE ?MerName)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax)
                    ORDER BY R.`c_time` DESC 
                    LIMIT ?StartIndex,?PageSize;

                    SELECT COUNT(1)
                    FROM fer_repay_principal R
                    INNER JOIN pjt P ON R.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`fer_id` = R.`fer_id`
                    INNER JOIN fer_info I ON I.`id` = R.`fer_id`
                    WHERE R.`status`=0 OR R.`status`=1
                    AND (?MerName='%%' OR I.name LIKE ?MerName)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax)";
            MySqlParameter[] parameters = {
					new MySqlParameter("?MerName", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?AmountMin", MySqlDbType.Decimal),
                    new MySqlParameter("?AmountMax", MySqlDbType.Decimal),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = merchantName;
            parameters[1].Value = loanPeriod;
            parameters[2].Value = amount_min;
            parameters[3].Value = amount_max;
            parameters[4].Value = startIndex;
            parameters[5].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取待处理本金列表", parameters);
        }
        /// <summary>
        /// 财务还本金相关操作
        /// </summary>
        /// <param name="recordID">记录id</param>
        /// <param name="merchantID">融资方id</param>
        /// <param name="amount">还息金额</param>
        /// <param name="pjid">项目id</param>
        /// <param name="adminId">操作人id</param>
        /// <returns></returns>
        public bool PayPrincipal(long recordID, int merchantID, decimal amount, int pjid, int adminId)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 更新还息记录表 fer_repay_principal
            #region
            var sql1 = @"
                        UPDATE fer_repay_principal
                        SET STATUS=1,op_status=1
                        WHERE id=?ID";
            MySqlParameter[] param1 = {
                    new MySqlParameter("?ID", MySqlDbType.Int32)};
            param1[0].Value = recordID;

            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            // 2）更新项目表 pjt 已还金额+
            #region
//            var sql2 = @"
//                        UPDATE pjt
//                        SET amount_paid=amount_paid + ?Amount,rep_status=(CASE WHEN (amount_paid + ?Amount)>=amount THEN 3 ELSE 0 END)
//                        WHERE id=?PID";
//            MySqlParameter[] param2 = {
//                new MySqlParameter("?Amount", MySqlDbType.Decimal),
//                new MySqlParameter("?PID", MySqlDbType.Int64)};
//            param2[0].Value = amount;
//            param2[1].Value = pjid;

//            sqlArray.Add(sql2);
//            paramArray.Add(param2);
            #endregion

            // 3）更新融资方账户表 fer_account 
            #region
//            var sql3 = @"
//                        UPDATE fer_account
//                        SET amount=amount-?Amount,amount_repay=amount_repay-?Amount,amount_repay_fz=amount_repay_fz-?Amount,amount_paid=amount_paid+?Amount
//                        WHERE fer_id=?MID";
//            MySqlParameter[] param3 = {
//                new MySqlParameter("?Amount", MySqlDbType.Decimal),
//                new MySqlParameter("?MID", MySqlDbType.Int64)};
//            param3[0].Value = amount;
//            param3[1].Value = merchantID;

//            sqlArray.Add(sql3);
//            paramArray.Add(param3);
            #endregion

            // 4）插入还款日志表 fer_repay_log
            #region
//            var sql4 = @"
//                        INSERT INTO fer_repay_log(pjt_id,amount,TYPE,creator_id,c_time,remark,fer_account_id)
//                        VALUES(?PID,?Amount,?Type,?AdminID,?CTime,?Remark,?MID);";
//            MySqlParameter[] param4 = {
//                new MySqlParameter("?CTime", MySqlDbType.DateTime),
//                new MySqlParameter("?MID", MySqlDbType.Int64),
//                new MySqlParameter("?Type", MySqlDbType.Int16),
//                new MySqlParameter("?Amount", MySqlDbType.Decimal),
//                new MySqlParameter("?PID", MySqlDbType.Int64),
//                new MySqlParameter("?Remark", MySqlDbType.VarChar),
//                new MySqlParameter("?AdminID", MySqlDbType.Int64)
//            };
//            param4[0].Value = DateTime.Now;
//            param4[1].Value = merchantID;
//            param4[2].Value = 1; //类型(0利息，1本金)
//            param4[3].Value = amount;         
//            param4[4].Value = pjid;
//            param4[5].Value = "融资方还本金";
//            param4[6].Value = adminId;    

//            sqlArray.Add(sql4);
//            paramArray.Add(param4);
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region 还款日志
        /// <summary>
        /// 还款日志 - 本金 + 利息
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet GetRefundLog(string merchantName, int pjt_id, decimal amount_min, decimal amount_max,
            DateTime? sDate, DateTime? eDate, int type, int startIndex, int pageSize)
        {
            merchantName = "%" + merchantName.Trim() + "%";
            string strSql = @"
                    SELECT 	R.id,SUM(R.amount) AS amount,R.c_time AS `ctime`,
	                    P.`name` AS `ProjectName`,P.`Amount` AS 'ProjectAmount',R.pjt_id AS `pjid`, P.amount_paid AS repay_amount,
	                    I.`name` AS financier_name,R.type
                    FROM fer_repay_log R
                    INNER JOIN pjt P ON R.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`id` = R.`fer_account_id`
                    INNER JOIN fer_info I ON I.`id` = F.fer_id
                    WHERE (?MerName='%%' OR I.name LIKE ?MerName)
                    AND (?pjt_id=-1 OR R.pjt_id=?pjt_id) 
                    AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax)
                    AND (?SDate IS NULL OR R.c_time >= ?SDate) AND (?EDate IS NULL OR R.c_time <= ?EDate)
                    AND (?Type=-1 OR R.type=?Type) 
                    GROUP BY R.`source_id`,R.`type`
                    ORDER BY R.c_time DESC
                    LIMIT ?StartIndex,?PageSize;

                    SELECT COUNT(1) FROM 
                    (
                        SELECT SUM(R.`amount`)
                        FROM fer_repay_log R
                        INNER JOIN pjt P ON R.pjt_id=P.`id`
                        INNER JOIN fer_account F ON F.`id` = R.`fer_account_id`
                        INNER JOIN fer_info I ON I.`id` = F.fer_id
                        WHERE (?MerName='%%' OR I.name LIKE ?MerName)
                        AND (?pjt_id=-1 OR R.pjt_id=?pjt_id) 
                        AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax) 
                        AND (?SDate IS NULL OR R.c_time >= ?SDate) AND (?EDate IS NULL OR R.c_time <= ?EDate)
                        AND (?Type=-1 OR R.type=?Type) 
                        GROUP BY R.`source_id`,R.`type`
                    )T";
            MySqlParameter[] parameters = {
					new MySqlParameter("?MerName", MySqlDbType.VarChar),
                    new MySqlParameter("?pjt_id", MySqlDbType.Int32),
                    new MySqlParameter("?AmountMin", MySqlDbType.Decimal),
                    new MySqlParameter("?AmountMax", MySqlDbType.Decimal),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?SDate", MySqlDbType.DateTime),  
                    new MySqlParameter("?EDate", MySqlDbType.DateTime),
                    new MySqlParameter("?Type", MySqlDbType.Int32)
            };
            parameters[0].Value = merchantName;
            parameters[1].Value = pjt_id;
            parameters[2].Value = amount_min;
            parameters[3].Value = amount_max;
            parameters[4].Value = startIndex;
            parameters[5].Value = pageSize;
            parameters[6].Value = sDate;
            parameters[7].Value = eDate;
            parameters[8].Value = type;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取还款记录", parameters);
        }
        #endregion
    }
}
