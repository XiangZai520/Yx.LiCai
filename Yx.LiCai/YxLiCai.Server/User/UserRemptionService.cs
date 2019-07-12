using System;
using YxLiCai.Dao.M_Users;
using YxLiCai.Tools;

namespace YxLiCai.Server.User
{
    /// <summary>
    /// 用户提现业务类-平扬
    /// </summary>
    public class UserRemptionService
    {
        UsersMoneyDao userMoneyDao = new UsersMoneyDao();
        
        /// <summary>
        /// 用户申请提现
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="money">提现金额</param>
        /// <param name="l_name">登录名</param>
        /// <param name="r_name">真实姓名</param>
        /// <param name="phone">手机号</param>
        /// <param name="bankname">银行卡</param>
        /// <param name="bkcard">卡号</param>
        /// <returns></returns>
        public bool UserWithdrawCash(long userId, decimal money, string l_name, string r_name, string phone, string bankname, string bkcard)
        { 
            try
            {
                var balance = userMoneyDao.GetUserWithdrawals(userId);
                if (money > balance)
                {
                    return false;
                }
                if (money < 0.01m)
                {
                    return false;
                }
                return userMoneyDao.UserWithdrawCash(userId, money, l_name, r_name, phone, bankname, bkcard);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return false;
            }
        }
    }
}
