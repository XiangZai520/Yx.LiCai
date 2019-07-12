using System;
using YxLiCai.Dao.M_Raise;
using YxLiCai.Tools.Enums;
using YxLiCai.Dao.User;
using YxLiCai.Tools;
using YxLiCai.Model.UserRaise;
using YxLiCai.Model.User;
namespace YxLiCai.Server.User
{
    /// <summary>
    /// 用户投资服务
    /// </summary>
    public class UserRaiseService
    {
        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();

        UserRaiseDao userRaiseDao = new UserRaiseDao();
        UserInfoDao userInfoDao = new UserInfoDao();
        /// <summary>
        /// 用户投资产品--余额投资
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="raiseMoney"></param>
        /// <param name="rate">产品利率</param>
        /// <param name="productId"></param>
        /// <param name="addRates">1;2;3;</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UserRaiseProductEnmu UserBlanceRaiseProduct(long uid, decimal raiseMoney, int productId, string addRates, int type)
        {
            try
            {
                if (raiseMoney < 100)
                {
                    return UserRaiseProductEnmu.Not100;
                }
                return userRaiseDao.UserBlanceRaiseProduct(uid, raiseMoney, productId, addRates, type);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return UserRaiseProductEnmu.Failed;
            } 
        }

        /// <summary>
        /// 用户投资产品--银行卡余额投资
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="raiseMoney"></param>
        /// <param name="rate">产品利率</param>
        /// <param name="productId"></param>
        /// <param name="addRates">1;2;3;</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UserRaiseProductEnmu UserBankRaiseProduct(long uid, decimal raiseMoney, int productId, string addRates, int type,string orderId)
        {
            try
            {
                if (raiseMoney < 100)
                {
                    orderId = "";
                    return UserRaiseProductEnmu.Not100;
                }
                return userRaiseDao.UserRaiseProduct(uid, raiseMoney, productId, addRates, type, orderId);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return UserRaiseProductEnmu.Failed;
            }
        }


        /// <summary>
        /// 插入资金记录表
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool AddUserAmountRecModel(UserAmountRecModel userAmountRecModel)
        { 
            try
            {
                return new YxLiCai.Dao.M_Users.UsersMoneyDao().AddUserAmountRecModel(userAmountRecModel);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return false;
            }
        }


        /// <summary>
        /// 易宝回调方法，更新状态为失败
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool UpdateUserRaiseProductStatus(string orderid)
        {
            lock (lockHelper)
            {
                try
                {
                    return userRaiseDao.UpdateUserRaiseProductStatus(orderid);
                }
                catch (Exception ex)
                {
                    LogHelper.LogWriterFromFilter(ex);
                    return false;
                }
            }
        }


        /// <summary>
        /// 易宝回调方法，更新状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool UserRaiseProductStatus(string orderid)
        {  
             lock (lockHelper)
             {   
                try
                {
                    return userRaiseDao.UserRaiseProductStatus(orderid);
                }
                catch(Exception ex)
                {
                    LogHelper.LogWriterFromFilter(ex);
                    return false;
                }
            }
        }

        public UserRaiseProductModel GetUserRaiseProductModel(string order_id) {
            try {
                return userRaiseDao.GetUserRaiseProductModel(order_id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return null;
            }
        }

    }
}
