using System;
using System.Text;
using YxLiCai.Tools.Enums;
using YxLiCai.Dao.M_Users;
using YxLiCai.Model.UserRaise;
using YxLiCai.Tools.Util;
using YxLiCai.Dao.User;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using System.Data;
using YxLiCai.Dao.Order;
using YxLiCai.Model.User;
namespace YxLiCai.Dao.M_Raise
{
    /// <summary>
    /// 用户投资相关功能
    /// </summary>
    public class UserRaiseDao
    {

        /// <summary>
        /// 用户投资产品--余额投资
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="raiseMoney">投资金额</param>        
        /// <param name="productId">产品id</param>
        /// <param name="addRates">加息券id列表:</param>
        /// <param name="addRate">加息额度</param>
        /// <param name="type">类型（1:Q1月，2:Q3,3:Q6，4:Q12）</param>
        /// <returns></returns>
        public UserRaiseProductEnmu UserBlanceRaiseProduct(long uid, decimal raiseMoney, int productId, string addRates, int type)
        {
            Product.ProductDao productDao = new Product.ProductDao();
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            UserInfoDao userInfoDao = new UserInfoDao();
            OrderInfoDao orderInfoDao=new OrderInfoDao();
            //先判断产品是否已经投满
            if (!productDao.ProductHasDone(productId))
            {
                decimal rate = productDao.ProductRate(productId);
                //获取用户余额
                decimal blance = userMoneyDao.GetUserBlance(uid);
                if (blance >= raiseMoney)
                {
                    decimal addRate = 0;
                    if (!string.IsNullOrEmpty(addRates))
                    {
                        string[] addrs = addRates.Split(':');
                        if (addrs.Length > 0)
                        {
                            foreach (var i in addrs)
                            {
                                int rid = YxLiCai.Tools.Util.ParseHelper.ToInt(i);
                                addRate += userInfoDao.GetRateCoupon(uid, rid);
                            }
                        }
                        if (addRate > YxLiCai.Tools.Const.SystemConst.AddRateMax)
                        {
                            return UserRaiseProductEnmu.AddRateMax;
                        }
                    }  

                    //插入买入记录表
                    UserRaiseProductModel model = new UserRaiseProductModel();
                    model.interest_rate_coupons = addRates;
                    model.interest_rate_added = addRate;
                    model.interest_rate = rate + addRate;
                    model.product_id = productId;
                    model.purchase_amount_balance = raiseMoney;
                    model.purchase_amount_bank = 0;
                    model.purchase_amount = raiseMoney;
                    model.create_time = DateTime.Now;
                    model.e_time = DateTime.Now.AddDays(YxLiCai.Tools.Const.SystemConst.GetProductCategory(type).Duration);
                    model.status = 1;
                    model.op_status =0;
                    model.product_type = type;
                    model.records_summary_id = 0;
                    model.user_id = uid;
                    model.remark = "";
                    model.version = 0;
                    model.third_order_id = Helper.generateOrderCode(uid, productId);
                    if (productDao.UserRaise_ProductAdd(model))
                    {

                        //增加用户资金流水表
                        UserAmountRecModel userAmountRecModel = new UserAmountRecModel();
                        userAmountRecModel.Prodtype = type;
                        userAmountRecModel.amount = raiseMoney;
                        userAmountRecModel.c_time = DateTime.Now;
                        userAmountRecModel.creator_id = 0;
                        userAmountRecModel.remark = "余额购买";
                        userAmountRecModel.user_id = uid;
                        userAmountRecModel.type = 0;//购买充值
                        userAmountRecModel.version = 0;
                        userMoneyDao.AddUserAmountRecModel(userAmountRecModel);
                        //更新账户余额，投资本金 ,月季年子账号金额
                        userMoneyDao.UpdateUserCount(uid, raiseMoney);
                        if (type == 1)
                        {
                            userMoneyDao.UpdateUserCountMonth(uid, raiseMoney);
                        }
                        else if (type == 2)
                        {
                            userMoneyDao.UpdateUserCountSeason(uid, raiseMoney);
                        }
                        else if (type == 3)
                        {
                            userMoneyDao.UpdateUserCountSeason(uid, raiseMoney);
                        }
                        else if (type == 4)
                        {
                            userMoneyDao.UpdateUserCountYear(uid, raiseMoney);
                        }
                        //更新加息券状态
                        if (!string.IsNullOrEmpty(addRates))
                        {
                            string[] addrs = addRates.Split(':');
                            foreach (var i in addrs)
                            {
                                int rid = YxLiCai.Tools.Util.ParseHelper.ToInt(i);
                                userInfoDao.UpdateRateCoupon(uid, rid);
                            }  
                        }

                        int userid = YxLiCai.Tools.Util.ParseHelper.ToInt(uid);
                        //更新产品金额
                        productDao.UpdateProductBalance(productId, raiseMoney,
                            orderInfoDao.IsBuyUserByPidAndUserid(productId, userid) ? 0 : 1);

                        //如果可投金额小于0，则产品下架
                        if (productDao.ProductAvailableAmount(productId) <= 100)
                        {
                            productDao.UpdateProductStatus(productId);
                        }
                        return UserRaiseProductEnmu.Success;
                    }
                    else
                    {
                        return UserRaiseProductEnmu.Failed;
                    }  
                }
                else//余额不足
                {
                    return UserRaiseProductEnmu.NotEnoughBlance;
                } 
            }
            else
            {
                return UserRaiseProductEnmu.ProductHasDone;
            }
        }

        /// <summary>
        /// 用户投资产品--银行卡投资
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="raiseMoney">投资金额</param>        
        /// <param name="productId">产品id</param>
        /// <param name="addRates">加息券id列表</param>
        /// <param name="type">类型（1:Q1月，2:Q3,3:Q6，4:Q12）</param>
        /// <returns></returns>
        public UserRaiseProductEnmu UserRaiseProduct(long uid, decimal raiseMoney,int productId, string addRates, int type,string orderId)
        {            
            Product.ProductDao productDao = new Product.ProductDao();
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            UserInfoDao userInfoDao = new UserInfoDao();
            //先判断产品是否已经投满
            if (!productDao.ProductHasDone(productId))
            {
                decimal rate = productDao.ProductRate(productId);
                decimal addRate = 0;
                if (!string.IsNullOrEmpty(addRates))
                {
                    string[] addrs = addRates.Split(':');
                    foreach (var i in addrs)
                    {
                        int rid = YxLiCai.Tools.Util.ParseHelper.ToInt(i);
                        addRate += userInfoDao.GetRateCoupon(uid, rid);
                    }
                    if (addRate > YxLiCai.Tools.Const.SystemConst.AddRateMax)
                    {
                        return UserRaiseProductEnmu.AddRateMax;
                    }
                }  
                UserRaiseProductModel model = new UserRaiseProductModel();
                model.interest_rate_coupons = addRates;
                model.interest_rate_added = addRate;
                model.interest_rate = rate + addRate;
                model.product_id = productId;
                model.purchase_amount_balance = 0;
                model.purchase_amount_bank = raiseMoney;
                model.purchase_amount = raiseMoney;
                model.create_time = DateTime.Now;
                model.e_time = DateTime.Now.AddDays(YxLiCai.Tools.Const.SystemConst.GetProductCategory(type).Duration);
                model.status = 0;//默认未付款
                model.op_status = 0;
                model.product_type = type;
                model.user_id = uid;
                model.records_summary_id = 0;
                model.version = 0;
                model.third_order_id = orderId;
                if (productDao.UserRaise_ProductAdd(model))
                { 
                    return UserRaiseProductEnmu.Success;
                }
                else
                {
                    return UserRaiseProductEnmu.Failed;
                }
 
            }
            else
            {
                return UserRaiseProductEnmu.ProductHasDone;
            } 
        }

        /// <summary>
        /// 获取用户银行卡投资产品记录
        /// </summary>
        public UserRaiseProductModel GetUserRaiseProductModel(string order_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,c_time,user_id,purchase_amount,purchase_amount_balance,purchase_amount_bank,status,interest_rate,interest_rate_add,pdt_id,pdt_type,interest_rate_coupons,third_ord_id,records_summary_id,ord_id,version,remark from user_invest_rec ");
            strSql.Append(" where third_ord_id=?third_order_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?third_order_id", MySqlDbType.VarChar)
			};
            parameters[0].Value = order_id;

            var ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取用户银行卡投资产品记录", parameters).Tables[0];

            UserRaiseProductModel mode = new UserRaiseProductModel(); 
            if (ds.Rows.Count > 0)
            {
                return DataRowToModel(ds.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 更新用户投资产品记录表状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUserRaiseProductStatus(string third_ord_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_invest_rec set status=4  where third_ord_id=?third_ord_id and status=0;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?third_ord_id", MySqlDbType.VarChar,100)};
            parameters[0].Value = third_ord_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新用户投资产品记录表状态", parameters);
        }

        /// <summary>
        /// 更新用户投资产品记录表状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUserRaiseProductStatus(long id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_invest_rec set status=1,op_status=0  where id=?id and status=0;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.UInt64,11)};
            parameters[0].Value = id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新用户投资产品记录表状态", parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private UserRaiseProductModel DataRowToModel(DataRow row)
        {
            UserRaiseProductModel model = new UserRaiseProductModel();
           if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["c_time"]!=null && row["c_time"].ToString()!="")
				{
					model.create_time=DateTime.Parse(row["c_time"].ToString());
				}
				if(row["user_id"]!=null && row["user_id"].ToString()!="")
				{
					model.user_id=int.Parse(row["user_id"].ToString());
				}
				if(row["purchase_amount"]!=null && row["purchase_amount"].ToString()!="")
				{
					model.purchase_amount=decimal.Parse(row["purchase_amount"].ToString());
				}
				if(row["purchase_amount_balance"]!=null && row["purchase_amount_balance"].ToString()!="")
				{
					model.purchase_amount_balance=decimal.Parse(row["purchase_amount_balance"].ToString());
				}
				if(row["purchase_amount_bank"]!=null && row["purchase_amount_bank"].ToString()!="")
				{
					model.purchase_amount_bank=decimal.Parse(row["purchase_amount_bank"].ToString());
				}
				if(row["status"]!=null && row["status"].ToString()!="")
				{
					model.status=int.Parse(row["status"].ToString());
				}
				if(row["interest_rate"]!=null && row["interest_rate"].ToString()!="")
				{
					model.interest_rate=decimal.Parse(row["interest_rate"].ToString());
				}
				if(row["interest_rate_add"]!=null && row["interest_rate_add"].ToString()!="")
				{
					model.interest_rate_added=decimal.Parse(row["interest_rate_add"].ToString());
				}
				if(row["pdt_id"]!=null && row["pdt_id"].ToString()!="")
				{
					model.product_id=int.Parse(row["pdt_id"].ToString());
				}
				if(row["pdt_type"]!=null && row["pdt_type"].ToString()!="")
				{
					model.product_type=int.Parse(row["pdt_type"].ToString());
				}
				if(row["interest_rate_coupons"]!=null)
				{
					model.interest_rate_coupons=row["interest_rate_coupons"].ToString();
				}
				if(row["third_ord_id"]!=null)
				{
					model.third_order_id=row["third_ord_id"].ToString();
				}
				if(row["records_summary_id"]!=null && row["records_summary_id"].ToString()!="")
				{
					model.records_summary_id=int.Parse(row["records_summary_id"].ToString());
				} 
				if(row["version"]!=null && row["version"].ToString()!="")
				{
					model.version=int.Parse(row["version"].ToString());
				}
				if(row["remark"]!=null)
				{
					model.remark=row["remark"].ToString();
				}
            }
            return model;
        }

            
        /// <summary>
        /// 银行卡投资-回调方法-第三方支付返回调用
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        public bool UserRaiseProductStatus(string orderId)
        {            
            object lockHelper=new object();
            Product.ProductDao productDao = new Product.ProductDao();
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            UserInfoDao userInfoDao = new UserInfoDao();
            OrderInfoDao orderInfoDao = new OrderInfoDao();
          
            UserRaiseProductModel model = GetUserRaiseProductModel(orderId);
            if (model.status == 1) return false;
            long uid = model.user_id;
            decimal raiseMoney = model.purchase_amount; 
            int type = model.product_type;
            UpdateUserRaiseProductStatus(model.id);
          
            //更新加息券状态
            if (!string.IsNullOrEmpty(model.interest_rate_coupons))
            {
                string[] addrs = model.interest_rate_coupons.Split(':');
                foreach (var i in addrs)
                {
                    int rid = YxLiCai.Tools.Util.ParseHelper.ToInt(i);
                    userInfoDao.UpdateRateCoupon(model.user_id, rid);
                }
            }
  
            //插入用户帐户充值记录表
            YxLiCai.Model.User.UserAccountLogModel addUserAccountLogModel = new YxLiCai.Model.User.UserAccountLogModel();
            addUserAccountLogModel.account_source_id = 1;
            addUserAccountLogModel.c_time = DateTime.Now;
            addUserAccountLogModel.from_id = 0;
            addUserAccountLogModel.version = 0;
            addUserAccountLogModel.user_id = uid;
            addUserAccountLogModel.type = 1;
            addUserAccountLogModel.creator_id = 0;
            addUserAccountLogModel.before_amount = getUserBlance(uid);
            addUserAccountLogModel.change_amount = raiseMoney;
            addUserAccountLogModel.after_amount = addUserAccountLogModel.before_amount + raiseMoney;
            addUserAccountLogModel.remark = "银行卡购买充值";
            AddUserAccountLogModel(addUserAccountLogModel);

            //增加用户资金流水表
            UserAmountRecModel userAmountRecModel = new UserAmountRecModel();
            userAmountRecModel.Prodtype = type;
            userAmountRecModel.amount = raiseMoney;
            userAmountRecModel.c_time = DateTime.Now;
            userAmountRecModel.creator_id = 0;
            userAmountRecModel.remark = "银行卡购买充值";
            userAmountRecModel.user_id = uid;
            userAmountRecModel.type = 0;//购买充值
            userAmountRecModel.version = 0;
            userMoneyDao.AddUserAmountRecModel(userAmountRecModel);

            //更新账户余额，投资本金 ,月季年子账号金额
            userMoneyDao.UpdateRaiseUserCount(uid, raiseMoney);
            if (type == 1)
            {
                userMoneyDao.UpdateUserCountMonth(uid, raiseMoney);
            }
            else if (type == 2)
            {
                userMoneyDao.UpdateUserCountSeason(uid, raiseMoney);
            }
            else if (type == 3)
            {
                userMoneyDao.UpdateUserCountSeason(uid, raiseMoney);
            }
            else if (type == 4)
            {
                userMoneyDao.UpdateUserCountYear(uid, raiseMoney);
            }
            UserBlanceItemModel mod = new UserBlanceItemModel();
            mod.user_id = uid;
            mod.amount = raiseMoney;
            mod.create_time = DateTime.Now;
            mod.type = 1;
            mod.remark = "银行卡购买"+raiseMoney;
            if(HasUserBlanceItem(mod))
            {
                updateUserBlanceItem(mod);
            }
            else
            {
                AddUserBlanceItem(mod);
            }
           
            //更新产品金额

            int userid = YxLiCai.Tools.Util.ParseHelper.ToInt(uid);
            //更新产品金额
            productDao.UpdateProductBalance(model.product_id, model.purchase_amount,
                orderInfoDao.IsBuyUserByPidAndUserid(model.product_id, userid) ? 0 : 1);

            // productDao.UpdateProductBalance(model.product_id, model.purchase_amount);
            //如果可投金额小于0，则产品下架
            if (productDao.ProductAvailableAmount(model.product_id) <=100)
            {
                productDao.UpdateProductStatus(model.product_id);
            }
            return true;
        }

        /// <summary>
        /// 增加一条数据 AddUserAccountLogModel
        /// </summary>
        public bool AddUserAccountLogModel(YxLiCai.Model.User.UserAccountLogModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_account_log(");
            strSql.Append("c_time,creator_id,m_time,user_id,type,before_amount,after_amount,change_amount,account_source_id,from_id,user_ord_id,user_rfd_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?c_time,?creator_id,?m_time,?user_id,?type,?before_amount,?after_amount,?change_amount,?account_source_id,?from_id,?user_ord_id,?user_rfd_id,?version,?remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?user_id", MySqlDbType.Int32,11),
					new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?before_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?after_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?change_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?account_source_id", MySqlDbType.Int16,4),
					new MySqlParameter("?from_id", MySqlDbType.Int32,11),
					new MySqlParameter("?user_ord_id", MySqlDbType.Int32,11),
					new MySqlParameter("?user_rfd_id", MySqlDbType.Int32,11),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            parameters[0].Value = model.c_time;
            parameters[1].Value = model.creator_id;
            parameters[2].Value = model.m_time;
            parameters[3].Value = model.user_id;
            parameters[4].Value = model.type;
            parameters[5].Value = model.before_amount;
            parameters[6].Value = model.after_amount;
            parameters[7].Value = model.change_amount;
            parameters[8].Value = model.account_source_id;
            parameters[9].Value = model.from_id;
            //parameters[10].Value = model.pjt_id;
            parameters[10].Value = model.user_ord_id;
            parameters[11].Value = model.user_rfd_id;
            parameters[12].Value = model.version;
            parameters[13].Value = model.remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "插入用户帐户日志表", parameters);
        }

        /// <summary>
        /// 更新账户充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateUserBlanceItem(UserBlanceItemModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_balance_item set amount=amount+?amount where user_id=?user_id and balance_type=?balance_type "); 
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?balance_type", MySqlDbType.Int16,4),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.type;
            parameters[2].Value = model.amount;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新账户充值记录", parameters);

        }

       
        /// <summary>
        /// 用户纯余额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public decimal getUserBlance(long uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  amount from user_account where user_id=?user_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20)};
            parameters[0].Value = uid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户余额", parameters);
            if (obj != null)
                return Convert.ToDecimal(obj.ToString());
            return 0m;

        }



        /// <summary>
        /// 是否有账户充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool HasUserBlanceItem(UserBlanceItemModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select 1 from user_balance_item where user_id=?user_id and balance_type=?balance_type ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?balance_type", MySqlDbType.Int16,4)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.type;
            object obj = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "是否有账户充值记录", parameters);
            if (obj != null)
                return true;
            else
                return false;
        }
        
        /// <summary>
        /// 增加一条账户充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserBlanceItem(UserBlanceItemModel model)
        {
            string strSql=@"insert into user_balance_item( c_time,user_id,balance_type,amount,remark)  values ( 
                             ?create_time,?user_id,?balance_type,?amount,?remark)  ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?create_time", MySqlDbType.DateTime),
					new MySqlParameter("?user_id", MySqlDbType.Int64,20),
					new MySqlParameter("?balance_type", MySqlDbType.Int16,4),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = model.user_id;
            parameters[2].Value = model.type;
            parameters[3].Value = model.amount;
            parameters[4].Value = model.remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "增加一条账户充值记录", parameters); 
        }
    }
}
