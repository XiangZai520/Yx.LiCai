using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.Order;
using YxLiCai.Model.Project;

namespace YxLiCai.Dao.Order
{
    /// <summary>
    /// 订单项目关联数据层
    /// </summary>
    public class OrderProjectDao
    { 
        /// <summary>
        ///获取订单资金流向
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public List<ProjectModel> GetListOrderProduct(int order_id, long user_id)
        {
            string strSql = @"SELECT d.borrower,d.loan_period,TRUNCATE((c.amount/b.amount)*amount_real,2) amount,TRUNCATE(a.amount_real,2) amount_real
                                FROM user_ord_info a
                                INNER JOIN pdt_ord_info b ON a.pdt_ord_id=b.id
                                INNER JOIN pdt_ord_pjt c ON b.id=c.pdt_ord_id
                                INNER JOIN pjt d ON c.pjt_id=d.id
                                WHERE a.user_id=?user_id AND a.id=?ord_id";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?user_id", MySqlDbType.Int64,20),
                    new MySqlParameter("?ord_id", MySqlDbType.Int32,11)
			};
            parameters[0].Value = user_id;
            parameters[1].Value = order_id;
            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单资金流向", parameters).Tables[0];
            List<ProjectModel> list = new List<ProjectModel>();
            if (dt.Rows.Count > 0)
            {
                decimal amount_real = 0;
                decimal amount_real_add = 0;
                decimal amount_temp = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    amount_real = Convert.ToDecimal(dt.Rows[i]["amount_real"]);
                    var entity = new ProjectModel();
                    entity.Borrower = dt.Rows[i]["borrower"].ToString();
                    entity.LoanPeriod = Convert.ToInt32(dt.Rows[i]["loan_period"]);

                    amount_temp = Convert.ToDecimal(dt.Rows[i]["amount"]);

                    if (i != (dt.Rows.Count - 1))
                    {
                        entity.Amount = amount_temp;
                        amount_real_add = amount_real_add + amount_temp;
                    }
                    else
                    {
                        entity.Amount = amount_real - amount_real_add;
                    }
                    list.Add(entity);
                }
            }
            return list;
        } 
    }
}
