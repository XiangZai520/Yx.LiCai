using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Tools;
using YxLiCai.Dao.Account;
using YxLiCai.Tools.Enums;
using YxLiCai.Model.Authority;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Service.Account
{
    public class AdminAuthenticationService
    {
        private static string cookieName = YxLiCai.Tools.Const.SystemConst.userInfoCookieName;

        public void SignIn(string data)
        {
            CookieHelper.WriteCookie(cookieName, data, DateTime.Now.AddDays(7));
            CookieHelper.WriteCookie("userinfo", data, DateTime.Now.AddDays(7));
        }

        public void SignOut()
        {
            CookieHelper.RemoveCookie(cookieName);
            CookieHelper.RemoveCookie("menulist");
        }
        AccountDao accountDao = new AccountDao();
        /// <summary>
        /// 用户登录
        /// danny-20150324
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultInfo<UserLoginResults> ValidateUser(string userName, string password)
        {
            var result = new ResultInfo<UserLoginResults> { Message = "", Result = false, Data = UserLoginResults.Failed };
            try
            {
                result.Data = accountDao.ValidateUser(userName, password);
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 获取个人账号信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<AccountModel> GetAccountByName(string name)
        {
            var result = new ResultInfo<AccountModel> { Message = "", Result = false, Data = null };
            try
            {
                result.Data = accountDao.GetAccountInfoByLoginName(name);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 获取个人账号信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<AccountModel> GetAccountInfoByID(int id)
        {
            var result = new ResultInfo<AccountModel> { Message = "", Result = false, Data = null };
            try
            {
                result.Data = accountDao.GetAccountInfoByID(id);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 是否存在该账号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<bool> HasAccountInfo(string name)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = accountDao.HasAccountInfo(name);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<bool> AddAccountInfo(AccountModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = accountDao.AddAccountInfo(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }


        /// <summary>
        /// 修改账户密码
        /// 平扬-20150526 
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateAccountPwd(int aid, string pwd)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = accountDao.UpdateAccountPwd(aid, pwd);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 根据账户id获取用户所有菜单权限 2015.6.24
        /// </summary>
        /// <param name="AccountID">用户账户id</param>
        /// <returns></returns>
        public ResultInfo<List<AccountMenuEx>> GetAccountMenuByAccountID(int AccountID)
        {
            ResultInfo<List<AccountMenuEx>> result = new ResultInfo<List<AccountMenuEx>>();
            result.Result = false;
            try
            {
                result.Data = accountDao.GetAccountMenuByAccountID(AccountID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = null;
            }
            return result;
        }

    }
}
