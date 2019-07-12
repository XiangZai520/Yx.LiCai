using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Model.Order;
using YxLiCai.Tools.Util;

namespace YxLiCai.Dao.Order
{
    /// <summary>
    /// 订单数据类
    /// </summary>
    public class OrderInfoDao
    {
        /// <summary>
        /// 自有资产总数量
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <returns></returns>
        public int GetTotalCount(long Id, int ProductType)
        {
            int TotalCount = 0;
            string strSql = @"SELECT Count(1) 
                            FROM   (SELECT a.c_time create_time, 
                                           a.interest_rate, 
                                           a.interest_rate_added, 
                                           a.amount AS order_investment, 
                                           a.interest_added, 
                                           a.id 
                                    FROM   user_ord_info a
                                    INNER JOIN user_invest_rec b ON a.id=b.ord_id 
                                    WHERE  a.user_id =?id 
                                           AND a.pdt_type =?producttype 
                                           AND a.atone_status = 0 
                                           AND a.ord_status IN ( 1 )  AND b.status=2
                                    UNION ALL 
                                    SELECT c_time create_time, 
                                           interest_rate, 
                                           interest_rate_add, 
                                           purchase_amount, 
                                           0, 
                                           0 
                                    FROM   user_invest_rec 
                                    WHERE  status = 1 
                                           AND user_id =?id 
                                           AND pdt_type =?producttype) AS t1  ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11),
                    new MySqlParameter("?ProductType", MySqlDbType.Int64,11)
			};
            parameters[0].Value = Id;
            parameters[1].Value = ProductType;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单、投资总数", parameters);
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
        /// 自有资产列表
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <param name="SCount">开始计数</param>
        /// <param name="pageIndex">页容量</param>
        /// <returns></returns>
        public List<YxLiCai.Model.ExtendModel.UserAssetsEx> GetListOrderByCreateTimeDesc(long Id, int ProductType, int SCount, int pageSize)
        {
            List<UserAssetsEx> list = new List<UserAssetsEx>();
            string strSql = @"SELECT  expiration_time,create_time,interest_rate,interest_rate_added,order_investment,interest_added,id FROM (
SELECT IFNULL(expiration_time,now()) expiration_time,b.c_time create_time,a.interest_rate,a.interest_rate_coupon interest_rate_added,(a.amount-a.amount_withdraw) AS  order_investment,a.interest_added,a.id
FROM user_ord_info a
INNER JOIN user_invest_rec b ON a.id=b.ord_id 
WHERE a.user_id=?Id AND a.pdt_type=?ProductType AND a.atone_status=0 AND a.ord_status IN (1) AND b.status=2
UNION ALL
SELECT e_time as expiration_time ,c_time create_time,interest_rate,interest_rate_add,purchase_amount,0,0 
FROM user_invest_rec
WHERE   STATUS=1 AND user_id=?Id AND pdt_type=?ProductType
) AS t1 
ORDER BY create_time DESC LIMIT ?SCount,?ECout";
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int64,11),
                    new MySqlParameter("?ProductType", MySqlDbType.Int64,11),
                    new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                    new MySqlParameter("?ECout", MySqlDbType.Int64,11)
			};
            parameters[0].Value = Id;
            parameters[1].Value = ProductType;
            parameters[2].Value = SCount;
            parameters[3].Value = pageSize;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单、投资列表", parameters);
            UserAssetsEx item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new UserAssetsEx();

                    item.id = Convert.ToInt64(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"]);
                    item.order_investment = Convert.ToDecimal(dr["order_investment"]);
                    item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);
                    item.interest_rate_added = Convert.ToDecimal(dr["interest_rate_added"]);
                    item.interest_added = Convert.ToDecimal(dr["interest_added"]);
                    item.expiration_time = Convert.ToDateTime(dr["expiration_time"]);

                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        ///根据orderinfoid 和 userid 获取实体
        /// </summary>
        /// <param name="Id">订单id</param>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public YxLiCai.Model.Order.order_info GetEntityByUserIDAndID(long Id, long UserID)
        {
            order_info entity = null;
            string strSql = @"SELECT id,c_time create_time,user_id,amount AS  order_investment,amount_withdraw order_withdrawal,pdt_id product_id,pdt_type product_type,ord_type order_type,ord_status order_status,rfd_status refund_status
,interest_pay_type,interest_rate,interest_rate_added,interest_added,interest_paid,expiration_time,VERSION,remark,atone_status
FROM user_ord_info WHERE user_id=?UserID AND Id=?Id";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,11),
                    new MySqlParameter("?Id", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserID;
            parameters[1].Value = Id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取用户订单", parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                entity = new order_info();
                entity.id = Convert.ToInt64(dr["id"]);
                entity.create_time = Convert.ToDateTime(dr["create_time"]);
                entity.user_id = Convert.ToInt64(dr["user_id"]);
                entity.order_investment = Convert.ToDecimal(dr["order_investment"]);

                entity.order_withdrawal = Convert.ToDecimal(dr["order_withdrawal"]);
                entity.product_id = Convert.ToInt32(dr["product_id"]);
                entity.product_type = Convert.ToInt32(dr["product_type"]);
                entity.order_type = Convert.ToInt32(dr["order_type"]);

                entity.order_status = Convert.ToInt32(dr["order_status"]);
                entity.refund_status = Convert.ToInt32(dr["refund_status"]);
                entity.interest_pay_type = Convert.ToInt32(dr["interest_pay_type"]);
                entity.interest_rate = Convert.ToDecimal(dr["interest_rate"]);

                entity.interest_rate_added = Convert.ToDecimal(dr["interest_rate_added"]);
                entity.interest_added = Convert.ToDecimal(dr["interest_added"]);
                entity.interest_paid = Convert.ToDecimal(dr["interest_paid"]);
                entity.expiration_time = Convert.ToDateTime(dr["expiration_time"] == DBNull.Value ? "1900-01-01" : dr["expiration_time"]);
                entity.version = Convert.ToInt32(dr["VERSION"] == DBNull.Value ? 0 : dr["VERSION"]);
                entity.remark = Convert.ToString(dr["remark"]);
                entity.atone_status = Convert.ToInt32(dr["atone_status"] == DBNull.Value ? 0 : dr["atone_status"]);

            }

            return entity;
        }
        /// <summary>
        /// 赎回列表
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <param name="SCount">开始计数</param>
        /// <param name="PageSize">页容量</param>
        /// <returns></returns>
        public List<YxLiCai.Model.Order.order_info> GetListOrderByCreateTimeDesc(long Id, List<int> ProductType, int SCount, int PageSize)
        {
            string ProductTypeStr = "";
            foreach (int ProductType_item in ProductType)
            {
                ProductTypeStr = ProductTypeStr + (ProductTypeStr == "" ? "" : ",") + ProductType_item.ToString(); ;
            }
            if (ProductTypeStr == "")
            {
                return null;
            }
            List<order_info> list = new List<order_info>();
            string strSql = @"SELECT a.id,b.c_time create_time,a.user_id,a.amount AS  order_investment,a.amount_withdraw order_withdrawal,a.pdt_id product_id,a.pdt_type product_type,a.ord_type order_type,a.ord_status order_status,a.rfd_status refund_status
        ,a.interest_pay_type,a.interest_rate,a.interest_rate_added,a.interest_added,a.interest_paid,a.expiration_time,a.VERSION,a.remark,a.atone_status
        FROM user_ord_info a INNER JOIN user_invest_rec b ON a.id=b.ord_id WHERE DATE_FORMAT(a.expiration_time,'%Y-%m-%d')>DATE_FORMAT(NOW(),'%Y-%m-%d') AND a.user_id=?Id AND a.ord_status=1  AND a.atone_status=0 AND a.pdt_type in (" + ProductTypeStr + ") ORDER BY create_time DESC LIMIT ?SCount,?ECout";
            MySqlParameter[] parameters = {
                            new MySqlParameter("?Id", MySqlDbType.Int64,11),
                            new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                            new MySqlParameter("?ECout", MySqlDbType.Int64,11)
                    };
            parameters[0].Value = Id;
            parameters[1].Value = SCount;
            parameters[2].Value = PageSize;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单列表", parameters);
            order_info item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new order_info();
                    item.id = Convert.ToInt64(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"] == DBNull.Value ? "1900-01-01" : dr["create_time"]);
                    item.user_id = Convert.ToInt64(dr["user_id"]);
                    item.order_investment = Convert.ToDecimal(dr["order_investment"]);

                    item.order_withdrawal = Convert.ToDecimal(dr["order_withdrawal"]);
                    item.product_id = Convert.ToInt32(dr["product_id"]);
                    item.product_type = Convert.ToInt32(dr["product_type"]);
                    item.order_type = Convert.ToInt32(dr["order_type"]);

                    item.order_status = Convert.ToInt32(dr["order_status"]);
                    item.refund_status = Convert.ToInt32(dr["refund_status"]);
                    item.interest_pay_type = Convert.ToInt32(dr["interest_pay_type"]);
                    item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);

                    item.interest_rate_added = Convert.ToDecimal(dr["interest_rate_added"]);
                    item.interest_added = Convert.ToDecimal(dr["interest_added"]);
                    item.interest_paid = Convert.ToDecimal(dr["interest_paid"]);
                    item.expiration_time = Convert.ToDateTime(dr["expiration_time"] == DBNull.Value ? "1900-01-01" : dr["expiration_time"]);
                    item.version = Convert.ToInt32(dr["VERSION"] == DBNull.Value ? 0 : dr["VERSION"]);
                    item.remark = Convert.ToString(dr["remark"]);
                    item.atone_status = Convert.ToInt32(dr["atone_status"] == DBNull.Value ? 0 : dr["atone_status"]);

                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// 赎回总数量
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <returns></returns>
        public int GetTotalCount(long Id, List<int> ProductType)
        {
            string ProductTypeStr = "";
            foreach (int ProductType_item in ProductType)
            {
                ProductTypeStr = ProductTypeStr + (ProductTypeStr == "" ? "" : ",") + ProductType_item.ToString(); ;
            }
            if (ProductTypeStr == "")
            {
                return 0;
            }
            int TotalCount = 0;
            string strSql = @"SELECT COUNT(1)
        FROM user_ord_info WHERE DATE_FORMAT(expiration_time,'%Y-%m-%d')>DATE_FORMAT(NOW(),'%Y-%m-%d') AND  user_id=?Id AND ord_status=1  AND atone_status=0 AND pdt_type in (" + ProductTypeStr + ") ";
            MySqlParameter[] parameters = {
                            new MySqlParameter("?Id", MySqlDbType.Int64,11)
                    };
            parameters[0].Value = Id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单总数", parameters);
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
        /// 根据userid和 orderinfoid获取列表
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="OrderInfoIDs">订单id</param>
        /// <returns></returns>
        public List<YxLiCai.Model.Order.order_info> GetListOrderByIDSAndUserID(long UserId, List<long> OrderInfoIDs)
        {
            string OrderInfoIDStr = "";
            foreach (long OrderInfoID_item in OrderInfoIDs)
            {
                OrderInfoIDStr = OrderInfoIDStr + (OrderInfoIDStr == "" ? "" : ",") + OrderInfoID_item.ToString(); ;
            }
            if (OrderInfoIDStr == "")
            {
                return null;
            }
            List<order_info> list = new List<order_info>();
            string strSql = @"SELECT id,c_time create_time,user_id,amount AS  order_investment,amount_withdraw order_withdrawal,pdt_id product_id,pdt_type product_type,ord_type order_type,ord_status order_status,rfd_status refund_status
        ,interest_pay_type,interest_rate,interest_rate_added,interest_added,interest_paid,expiration_time,VERSION,remark,atone_status
        FROM user_ord_info WHERE user_id=?UserId AND ord_status=1 AND atone_status=0 AND id in (" + OrderInfoIDStr + ") ";
            MySqlParameter[] parameters = {
                            new MySqlParameter("?UserId", MySqlDbType.Int64,11),
                            new MySqlParameter("?SCount", MySqlDbType.Int64,11),
                            new MySqlParameter("?ECout", MySqlDbType.Int64,11)
                    };
            parameters[0].Value = UserId;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取订单列表", parameters);
            order_info item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new order_info();
                    item.id = Convert.ToInt64(dr["id"]);
                    item.create_time = Convert.ToDateTime(dr["create_time"]);
                    item.user_id = Convert.ToInt64(dr["user_id"]);
                    item.order_investment = Convert.ToDecimal(dr["order_investment"]);

                    item.order_withdrawal = Convert.ToDecimal(dr["order_withdrawal"]);
                    item.product_id = Convert.ToInt32(dr["product_id"]);
                    item.product_type = Convert.ToInt32(dr["product_type"]);
                    item.order_type = Convert.ToInt32(dr["order_type"]);

                    item.order_status = Convert.ToInt32(dr["order_status"]);
                    item.refund_status = Convert.ToInt32(dr["refund_status"]);
                    item.interest_pay_type = Convert.ToInt32(dr["interest_pay_type"]);
                    item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);

                    item.interest_rate_added = Convert.ToDecimal(dr["interest_rate_added"]);
                    item.interest_added = Convert.ToDecimal(dr["interest_added"]);
                    item.interest_paid = Convert.ToDecimal(dr["interest_paid"]);
                    item.expiration_time = Convert.ToDateTime(dr["expiration_time"] == DBNull.Value ? "1900-01-01" : dr["expiration_time"]);
                    item.version = Convert.ToInt32(dr["VERSION"] == DBNull.Value ? 0 : dr["VERSION"]);
                    item.remark = Convert.ToString(dr["remark"]);
                    item.atone_status = Convert.ToInt32(dr["atone_status"] == DBNull.Value ? 0 : dr["atone_status"]);


                    list.Add(item);
                }
            }

            return list;
        }
        /// <summary>
        /// 更新前台赎回状态为申请中
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="ids">订单id</param>
        public void UpdateAtoneStatus(long userID, List<long> ids)
        {
            string idStr = "";
            foreach (int id in ids)
            {
                idStr = idStr + (idStr == "" ? "" : ",") + id.ToString();
            }
            if (idStr == "")
            {
                return;
            }
            string strSql = @"UPDATE user_ord_info SET atone_status=1  WHERE user_id=?userID AND ord_status=1 AND atone_status=0 AND id IN (" + idStr + ")";
            MySqlParameter[] parameters = {
                            new MySqlParameter("?userID", MySqlDbType.Int64,11)
                    };
            parameters[0].Value = userID;

            DataSet ds = DataBaseOperator.MoneyWriteDbHelper.ExecuteDataSet(strSql, "更新前台赎回状态为已申请", parameters);
        }
        /// <summary>
        /// 获取会员投资记录
        /// </summary>
        /// <param name="UserId">会员id</param>
        /// <param name="LoginName">用户名</param>
        /// <returns></returns>
        public List<PurchaseRecordEx> GetMemberPurchaseRecord(long UserId)
        {
            List<PurchaseRecordEx> list = new List<PurchaseRecordEx>();
            string strSql = @"SELECT id,pdt_type product_type,amount as order_investment,interest_rate,c_time create_time,interest_rate_coupon
                                    FROM user_ord_info
                                    WHERE user_id=?UserId
                                    UNION ALL
                                    SELECT 0,pdt_type product_type,purchase_amount,interest_rate,c_time create_time,interest_rate_add
                                    FROM user_invest_rec
                                    WHERE user_id=?UserId AND STATUS=1";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)
			};
            parameters[0].Value = UserId;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取会员投资列表", parameters);
            PurchaseRecordEx item;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    item = new PurchaseRecordEx();

                    item.OrderInfoID = Convert.ToInt64(dr["id"]);
                    item.CreateTime = Convert.ToDateTime(dr["create_time"]);
                    item.LoginName = "";
                    item.YearRate = Convert.ToDecimal(dr["interest_rate"]);
                    item.PurchaseMoney = Convert.ToDecimal(dr["order_investment"]);
                    item.ProductType = Convert.ToInt32(dr["product_type"]);
                    item.InterestRateCoupon = Convert.ToDecimal(dr["interest_rate_coupon"]);

                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 判断用户是否买过此产品 by 张浩然 2015-7-20
        /// </summary>
        /// <param name="pid">产品编号</param>
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public bool IsBuyUserByPidAndUserid(int pid,int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select count(id) from user_ord_info ");
            strSql.Append(" where pdt_id=?pid and user_id=?userid and (ord_status=1 or ord_status=2) ");
            MySqlParameter[] parameters = {					
                    new MySqlParameter("?pid", MySqlDbType.Int32,11),
                    new MySqlParameter("?userid",MySqlDbType.Int32,11)
            };
            parameters[0].Value = pid;
            parameters[1].Value = userid;
            object ob = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "判断用户是否买过此产品 by 张浩然 2015-7-20", parameters);
            if (ob != null)
            {
                if (ParseHelper.ToInt(ob)!=0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }
    }
}
