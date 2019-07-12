/*
 * 用户账户操作务逻辑层
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Text;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Tools;
using YxLiCai.Dao.M_Users;
using YxLiCai.Model.UserRaise;
using System.Collections.Generic;

namespace YxLiCai.Server.User
{
    public class UserInfoService
    {
        YxLiCai.Dao.User.UserInfoDao dao = new YxLiCai.Dao.User.UserInfoDao();
        private static string ThisType = "YxLiCai.Server.User.UserInfoService";
        private readonly string msgCodeExpires = System.Configuration.ConfigurationManager.AppSettings["MsgCodeExpiresMinutes"];
        #region 用户注册初始化信息记录

        #endregion

        #region 添加用户
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<int> AddUser(UserInfoModel model)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.AddUser(model);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 修改用户的信息
        /// <summary>
        /// 修改用户的信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUser(UserInfoModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUser(model);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 冻结解封用户
        /// <summary>
        /// 冻结用户
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateFrozenUser(int UserID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateFrozenUser(UserID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 解封用户
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateThawUser(int UserID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateThawUser(UserID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 身份证验证
        /// <summary>
        /// 修改用户的认证状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="MyRealCard">用户身份证</param>
        /// <returns>true：身份验证设置成功 false：身份证设置失败</returns>
        public ResultInfo<bool> UpdateIsRealCard(Int64 UserID, string MyRealCard, string Username)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateIsRealCard(UserID, MyRealCard, Username);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 更新用户帐户信息 by张浩然 2015-7-3
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserAccount(Int64 UserID, string MyRealName)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserAccount(UserID, MyRealName);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;

        }

        #endregion

        #region 删除用户信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ResultInfo<bool> DeleteUser(int id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.DeleteUser(id);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 绑定银行卡
        /// <summary>
        /// 获取用户银行卡
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<UserBankCardModel> GetBankCard(long uid)
        {
            ResultInfo<UserBankCardModel> result = new ResultInfo<UserBankCardModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetBankCard(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public ResultInfo<bool> BoundBankCard(UserBankCardModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.BoundBankCard(model);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region Select操作
        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public ResultInfo<UserInfoModel> GetUserModel(long id)
        {
            ResultInfo<UserInfoModel> result = new ResultInfo<UserInfoModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetUserModel(id);
                result.Result = true;

            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 得到一个对象实体（通过用户手机号）
        /// </summary>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public ResultInfo<UserInfoModel> GetUserModelByPhone(string Phone)
        {
            ResultInfo<UserInfoModel> result = new ResultInfo<UserInfoModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetUserModelByPhone(Phone);
                result.Result = true;

            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据邀请码获得用户信息 by张浩然 2015-7-30
        /// </summary>
        /// <param name="MyCode">邀请码</param>
        /// <returns></returns>
        public ResultInfo<UserInfoModel> GetUserModelByMyCode(string MyCode)
        {
            ResultInfo<UserInfoModel> result = new ResultInfo<UserInfoModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetUserModelByMyCode(MyCode);
                result.Result = true;

            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion


        #region 检查用户是否是注册用户
        /// <summary>
        /// 判断用户账户是否已经注册
        /// </summary>
        /// <param name="Account">用户手机</param>       
        /// <returns></returns>
        public ResultInfo<bool> IsRepeatUser(string Account)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsRepeatUser(Account);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        public ResultInfo<bool> IsBlackUser(string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsBlackUser(Phone);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据身份证判断是否已注册
        /// </summary>
        /// <param name="realCard">身份证</param>
        /// <returns></returns>
        public ResultInfo<bool> IsRepeatUserByRealCard(string realCard)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsRepeatUserByRealCard(realCard);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        #endregion

        #region 用户注册初始化
        /// <summary>
        /// 用户首次注册初始化数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ResultInfo<bool> AddUserInitialization(long UserID, string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.AddUserInitialization(UserID, Phone);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion


        #region 检查用户输入密码（登录、支付）是否正确

        /// <summary>
        /// 更新用户登录时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUserLoginTimes(long id)
        {
            try
            {
                return dao.updateUserLoginTimes(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return false;
        }

        /// <summary>
        /// 判断登录密码是否正确
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultInfo<bool> IsTruePassWord(string Phone, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTruePassWord(Phone, password);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 判断支付密码是否正确
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultInfo<bool> IsTrueSallpassword(string Phone, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTrueSallpassword(Phone, password);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        #endregion

        #region  检查用户验证码是否发送过
        /// <summary>
        /// 检查用户验证码是否发送过
        /// </summary>
        /// <param name="Phone">用户手机</param>
        /// <returns></returns>
        public ResultInfo<bool> IsSendCode(string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsSendCode(Phone);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 检查手机是否已经认证
        /// <summary>
        /// 检查手机是否已经认证
        /// </summary>
        /// <param name="Phone">用户手机</param>
        /// <returns></returns>
        public ResultInfo<bool> IsTruePhone(string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTruePhone(Phone);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 查询用户绑定的的银行卡

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public ResultInfo<string> GetUserBankID(long UserID)
        {
            ResultInfo<string> result = new ResultInfo<string>();
            result.Result = false;
            result.Data = "";
            try
            {
                result.Data = dao.GetUserBankID(UserID);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion

        #region 检查用户是否绑定银行卡


        /// <summary>
        /// 检查用户是否绑定银行卡
        /// </summary>
        /// <param name="UserId">用户ID</param>     
        /// <returns></returns>
        public ResultInfo<bool> IsTrueBindBankCard(long UserId)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTrueBindBankCard(UserId);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 检查用户交易密码是否正确
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultInfo<bool> IsTrueSallPassWord(long uid, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTrueSallPassWord(uid, password);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 账号可提现金额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserWithdrawals(long uid)
        {
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0m;
            try
            {
                result.Data = userMoneyDao.GetUserWithdrawals(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取用户可提现本金
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserPrincipal(long uid)
        {
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0m;
            try
            {
                result.Data = userMoneyDao.GetUserPrincipal(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取用户盈利金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserInterest(long uid)
        {
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0m;
            try
            {
                result.Data = userMoneyDao.GetUserInterest(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }


        #region 登录密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserPassWord(long ID, string PassWord)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserPassWord(ID, PassWord);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 忘记登录密码
        /// </summary>
        /// <param name="Phone">用户手机号</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        public ResultInfo<bool> ForgetUserPassWord(string Phone, string PassWord)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.ForgetUserPassWord(Phone, PassWord);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion
        #region 支付密码

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="id">用户ＩＤ</param>
        /// <param name="Password">支付密码</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserSallpassword(long id, string Password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserSallpassword(id, Password);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 账号余额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetMyBalance(long uid)
        {
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0m;
            try
            {
                result.Data = YxLiCai.Tools.Const.SystemConst.MoenyConvert(userMoneyDao.GetUserBlance(uid));
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }



        /// <summary>
        /// 用户资产
        /// </summary>
        /// <param name="Phone">用户手机</param>
        /// <returns></returns>
        public ResultInfo<UserCountModel> GetUserCount(long UserId)
        {
            UsersMoneyDao userMoneyDao = new UsersMoneyDao();
            ResultInfo<UserCountModel> result = new ResultInfo<UserCountModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = userMoneyDao.GetUserCount(UserId);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 判断交易密码是否与登录密码相同 By张浩然 2015-2-27 10：47：00
        /// </summary>
        /// <param name="fetchPwd">交易密码</param>
        /// <returns></returns>
        public ResultInfo<bool> CheckFetchPwdAndPwd(string fetchPwd, long id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.CheckFetchPwdAndPwd(fetchPwd, id);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 2015年7月7日 by张浩然 判断登录密码是否与交易密码相同
        /// </summary>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public ResultInfo<bool> CheckPwdAndFetchPwd(string Pwd, long id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.CheckPwdAndFetchPwd(Pwd, id);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 2015年7月7日 by张浩然 判断登录密码是否与交易密码相同
        /// </summary>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public ResultInfo<bool> CheckPwdAndFetchPwdByPhone(string Pwd, string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.CheckPwdAndFetchPwdByPhone(Pwd, Phone);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取用户特享金列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">当前页面</param>
        /// <param name="pagesize">页码</param>
        /// <param name="count">总记录</param>
        /// <returns></returns>
        public ResultInfo<List<UserSpecialAssetsModel>> GetUserSpecialAssets(long uid, int page, int pagesize, ref int count)
        {
            ResultInfo<List<UserSpecialAssetsModel>> result = new ResultInfo<List<UserSpecialAssetsModel>>();
            result.Result = false;
            result.Data = null;
            try
            {
                int SCount = (page - 1) * pagesize;
                YxLiCai.Dao.M_Users.UsersMoneyDao dao = new UsersMoneyDao();
                result.Data = dao.GetUserSpecialAssets(uid, SCount, pagesize, ref count);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取用户总特享金列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">当前页面</param>
        /// <param name="pagesize">页码</param>
        /// <param name="count">总记录</param>
        /// <returns></returns>
        public ResultInfo<List<UserSpecialAssetsModel>> GetUserAllSpecialAssets(long uid, int page, int pagesize, ref int count)
        {
            ResultInfo<List<UserSpecialAssetsModel>> result = new ResultInfo<List<UserSpecialAssetsModel>>();
            result.Result = false;
            result.Data = null;
            try
            {
                int SCount = (page - 1) * pagesize;
                YxLiCai.Dao.M_Users.UsersMoneyDao dao = new UsersMoneyDao();
                result.Data = dao.GetUserAllSpecialAssets(uid, SCount, pagesize, ref count);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取用户加息券
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public List<UserRateCouponModel> GetRateCoupons(long uid)
        {
            List<UserRateCouponModel> result = null;
            try
            {
                result = dao.GetRateCoupons(uid);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }


        /// <summary>
        /// 获取加息券总额度(可使用额度)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetAllRateCoupon(long uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.GetAllRateCoupon(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取加息券总额度
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserAddInterest(long uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = new YxLiCai.Dao.M_Users.UsersMoneyDao().GetUserAddInterest(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取加息券可使用张数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<int> GetUserAddInterestCount(long uid)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = new YxLiCai.Dao.M_Users.UsersMoneyDao().GetUserAddInterestCount(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }


        /// <summary>
        /// 获取用户 在投中的资产
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<UserCountModel> GetUserRaiseMoney(long uid)
        {
            ResultInfo<UserCountModel> result = new ResultInfo<UserCountModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                YxLiCai.Dao.M_Users.UsersMoneyDao dao = new UsersMoneyDao();
                result.Data = dao.GetUserRaiseMoney(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }


        /// <summary>
        /// 获取加息券总张数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<int> GetUserAllAddInterestCount(long uid)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = new YxLiCai.Dao.M_Users.UsersMoneyDao().GetUserAllAddInterestCount(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }


        /// <summary>
        /// 获取用户特享金总额(在投中的)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserSpecialAssets(long uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                YxLiCai.Dao.M_Users.UsersMoneyDao dao = new UsersMoneyDao();
                result.Data = dao.GetUserSpecialAssets(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取用户特享金总额
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserAllSpecialAssets(long uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                YxLiCai.Dao.M_Users.UsersMoneyDao dao = new UsersMoneyDao();
                result.Data = dao.GetUserAllSpecialAssets(uid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取用户资金流水记录列表(分页获取)
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="Prodtype">产品类型（1月、2三个月、3六个月、4年）</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">页面条数</param>
        /// <returns></returns>
        public ResultInfo<List<UserAmountRecModel>> GetListUserAmountRecModel(long UserID, int Prodtype, int CurrentPage, int PageSize, out int record)
        {
            record = 0;
            ResultInfo<List<UserAmountRecModel>> result = new ResultInfo<List<UserAmountRecModel>>
            {
                Result = false,
                Data = null
            };
            try
            {
                int SCount = (CurrentPage - 1) * PageSize;
                result.Data = new UsersMoneyDao().GetListUserAmountRecModel(UserID, Prodtype, SCount, PageSize, out record);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #region MyRegion

        /// <summary>
        /// 插入短信发送记录
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        public static ResultInfo<bool> AddUserSendMesg(string Phone, string content)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = Dao.User.UserInfoDao.AddUserSendMesg(Phone, content);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        #endregion
    }
}