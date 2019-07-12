using System;
using System.Data;
using System.Collections.Generic;

using YxLiCai.DataHelper;
using YxLiCai.Model.Charge;
using MySql.Data.MySqlClient;


namespace YxLiCai.Dao.ChargeManager
{
    /// <summary>
    /// 充值提醒数据处理类
    /// add by lxm 2015年7月2日
    /// </summary>
    public class ChargeDAO
    {
        /// <summary>
        /// 获取充值提醒列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetChargeReminderList(string name, int uType, decimal amount_min, decimal amount_max)
        {
            name = "%" + name.Trim() + "%";
            //type: 0 = 融资方；1 = 保理；2=平台（活动）
            var strSql = @"
                        SELECT TB.*, 10000 AS 'alert' FROM 
                        (
	                        SELECT A.fer_id AS 'id', A.amount_repay - A.amount_repay_fz AS 'balance', A.interest_payable AS 'amount',
	                            B.myreal_name AS 'name', B.phone, 0 AS 'type'
                            FROM fer_account A INNER JOIN fer_info B
                            ON A.fer_id = B.id
                            AND (A.interest_payable > A.amount_repay - A.amount_repay_fz OR A.amount_repay - A.amount_repay_fz < 10000)
                            UNION
                            SELECT user_id AS 'id', CASE WHEN amount_blance>0 THEN amount_blance ELSE 0 END  AS 'balance', 
                            CASE WHEN amount_blance>=0 THEN 0 ELSE amount_blance END 'amount',
	                            real_name AS 'name', phone, 1 AS 'type'
                            FROM user_account 
                            WHERE user_type=1 
                            AND amount_blance < 10000
                            UNION
                            SELECT id, CASE WHEN amount>0 THEN amount ELSE 0 END  AS 'balance', CASE WHEN amount>=0 THEN 0 ELSE amount END 'amount',
	                            NAME, NULL AS 'phone', 2 AS 'type'
                            FROM plat_account
                            WHERE TYPE=2
                            AND amount < 10000
                        )TB
                        WHERE (?NAME='%%' OR TB.name LIKE ?NAME)
                        AND (?TYPE=-1 OR TB.type = ?TYPE)
                        AND (?Amount_Min=-1 OR TB.amount >= ?Amount_Min)
                        AND (?Amount_Max=-1 OR TB.amount <= ?Amount_Max)";

            MySqlParameter[] parameters = {
					new MySqlParameter("?NAME", MySqlDbType.VarChar),
                    new MySqlParameter("?TYPE", MySqlDbType.Int32),
                    new MySqlParameter("?Amount_Min", MySqlDbType.Decimal),
                    new MySqlParameter("?Amount_Max", MySqlDbType.Decimal)
            };
            parameters[0].Value = name;
            parameters[1].Value = uType;
            parameters[2].Value = amount_min;
            parameters[3].Value = amount_max;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取充值提醒列表", parameters);
        }
        /// <summary>
        /// 转为model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public ChargeModel DataToModel(DataRow row)
        {
            if (row == null)
                return null;

            var model = new ChargeModel();

            #region match
            if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
            {
                model.id = Convert.ToInt32(row["id"].ToString());                
            }
            if (row.Table.Columns.Contains("balance") && row["balance"] != null && row["balance"].ToString() != "")
            {
                model.Balance = Convert.ToDecimal(row["balance"].ToString());
            }
            if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
            {
                model.Amount = Convert.ToDecimal(row["amount"].ToString());
            }
            if (row.Table.Columns.Contains("name") && row["name"] != null)
            {
                model.Name = row["name"].ToString();
            }
            if (row.Table.Columns.Contains("phone") && row["phone"] != null)
            {
                model.Phone = row["phone"].ToString();
            }
            if (row.Table.Columns.Contains("type") && row["type"] != null && row["type"].ToString() != "")
            {
                int type = Convert.ToInt32(row["type"].ToString());
                switch (type)
                {
                    case 0: model.Type = "融资方账户"; break;
                    case 1: model.Type = "保理账户"; break;
                    case 2: model.Type = "活动账户"; break;
                    default: model.Type = "未定义"; break;
                }
            }
            if (row.Table.Columns.Contains("alert") && row["alert"] != null && row["alert"].ToString() != "")
            {
                model.AlertValue = Convert.ToDecimal(row["alert"].ToString());
            }
            #endregion

            return model;
        }
    }
}
