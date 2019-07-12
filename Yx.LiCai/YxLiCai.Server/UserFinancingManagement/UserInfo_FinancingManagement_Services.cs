/*
 * 融资方用户账户信息业务逻辑
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using YxLiCai.Model;
using YxLiCai.Model.Account;
using YxLiCai.Model.Authority;
using YxLiCai.Model.Project;
using YxLiCai.Model.Refund;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Tools;
using YxLiCai.Tools.Enums;

namespace YxLiCai.Server.UserFinancingManagement
{
    /// <summary>
    /// 融资方用户账户信息业务逻辑
    /// </summary>
    public class UserInfo_FinancingManagement_Services
    {
        YxLiCai.Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao dao = new Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao();
        private static string ThisType = "YxLiCai.Server.UserInfo_FinancingManagement_Services.UserInfo_FinancingManagement";

        #region 融资方平台登录
        private static string cookieName = YxLiCai.Tools.Const.SystemConst.FinauserInfoCookieName;

        public void SignIn(string data)
        {
            CookieHelper.WriteCookie(cookieName, data, DateTime.Now.AddDays(7));
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public void SignOut()
        {
            CookieHelper.RemoveCookie(cookieName);
        }

        /// <summary>
        /// 用户登录       
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultInfo<UserLoginResults> ValidateUser(string userName, string password)
        {
            var result = new ResultInfo<UserLoginResults> { Message = "", Result = false, Data = UserLoginResults.Failed };
            try
            {
                result.Data = dao.ValidateUser(userName, password);
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
        public ResultInfo<FerAccountModel> GetAccountByName(string name)
        {
            var result = new ResultInfo<FerAccountModel> { Message = "", Result = false, Data = null };
            try
            {
                result.Data = dao.GetAccountInfoByLoginName(name);
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
        #endregion


        #region 添加融资方信息
        /// <summary>
        /// 添加融资方信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<int> AddUser(UserInfo_FinancingManagement_Model model)
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

        #region 修改融资方信息
        /// <summary>
        /// 修改融资方信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUser(UserInfo_FinancingManagement_Model model)
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

        #region 判断融资方是否信息存在
        /// <summary>
        /// 判断融资方是否信息存在
        /// </summary>
        /// <param name="account_id">用户（账户ID）</param>    
        /// <returns></returns>
        public ResultInfo<int> ISUserExist(int account_id)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.ISUserExist(account_id);
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

        #region 判断融资方是否存在账户金额信息
        /// <summary>
        /// 判断融资方是否存在账户金额信息
        /// </summary>
        /// <param name="account_id">用户（账户ID）</param>    
        /// <returns></returns>
        public ResultInfo<int> ISUser_financier_accountExist(int account_id)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.ISUser_financier_accountExist(account_id);
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

        #region 得到用户简单实体信息
        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public ResultInfo<UserSmallReads> GetUserSimall(int id)
        {
            ResultInfo<UserSmallReads> result = new ResultInfo<UserSmallReads>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetUserSimall(id);
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

        #region 融资方登录信息
        /// <summary>
        /// 判断融资方登录密码是否正确（用户登录）
        /// </summary>
        /// <param name="Phone">用户手机（账户）</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ResultInfo<bool> IsTruePassWordLogin(string Phone, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTruePassWordLogin(Phone, password);
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

        #region 修改手机认证
        /// <summary>
        /// 修改手机认证
        /// </summary>
        /// <param name="account_id">用户ID</param>
        /// <param name="Phone">手机号</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserPhone(int account_id, string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserPhone(account_id, Phone);
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

        #region 修改融资方登录密码
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="account_id">账户ID</param>
        /// <param name="Password">登录密码</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserLoginPassWord(int account_id, string Password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserLoginPassWord(account_id, Password);
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

        #region 交易密码
        /// <summary>
        /// 判断交易密码是否正确
        /// </summary>
        /// <param name="Phone">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ResultInfo<bool> IsTrueSallpassword(int uid, string password)
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
        #endregion

        #region 修改交易密码
        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="Phone">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserSPassWord(int uid, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserSPassWord(uid, password);
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

        #region 判断登录密码是否正确
        /// <summary>
        /// 判断登录密码是否正确
        /// </summary>
        /// <param name="account_id">用户（账户ID）</param>    
        /// <param name="password">密码</param>  
        public ResultInfo<bool> IsTruePassWord(int uid, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.IsTruePassWord(uid, password);
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

        #region 修改登录密码
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserPassWord(int uid, string password)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserPassWord(uid, password);
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

        #region 修改登录密码
        /// <summary>
        /// 修改公司名称
        /// </summary>
        /// <param name="account_id">融资方ID</param>
        /// <param name="Name">公司名称</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateCompanyName(int account_id, string Name)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateCompanyName(account_id, Name);
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

        #region 查询操作
        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public ResultInfo<UserInfo_FinancingManagement_Model> GetUserModel(int id)
        {
            ResultInfo<UserInfo_FinancingManagement_Model> result = new ResultInfo<UserInfo_FinancingManagement_Model>();
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
        /// 得到一个对象实体（通过登录名称）
        /// </summary>
        /// <param name="id">登录名称</param>
        /// <returns></returns>
        public ResultInfo<UserInfo_FinancingManagement_Model> GetUserModel(string l_name)
        {
            ResultInfo<UserInfo_FinancingManagement_Model> result = new ResultInfo<UserInfo_FinancingManagement_Model>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetUserModel(l_name);
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
        /// <summary>
        /// 得到一个对象实体（通过用户手机号）
        /// </summary>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public ResultInfo<UserInfo_FinancingManagement_Model> GetUserModelByPhone(string Phone)
        {
            ResultInfo<UserInfo_FinancingManagement_Model> result = new ResultInfo<UserInfo_FinancingManagement_Model>();
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
        #endregion

        #region 绑定银行卡
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="model">信息</param>
        /// <returns></returns>
        public ResultInfo<bool> BoundBankCard(UserInfo_FinancingManagement_Model model)
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

        #region 身份证验证
        /// <summary>
        /// 修改用户的认证状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="MyRealCard">用户身份证</param>
        /// <returns>true：身份验证设置成功 false：身份证设置失败</returns>
        public ResultInfo<bool> UpdateIsRealCard(int UserID, string MyRealCard, string Username)
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
        #endregion

        #region 融资方充值记录操作

        #region 账户充值余额
        /// <summary>
        /// 账户充值余额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserRecharge_Balance(int uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.GetUserRecharge_Balance(uid);
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

        #region 添加融资方充值记录信息
        /// <summary>
        /// 添加融资方充值记录（只是申请，未经过第三方通知成功）
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<bool> Addfina_recharge_record(Fina_Recharge_Record_Model model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Addfina_recharge_record(model);
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

        #region 获取融资方充值记录一条信息
        /// <summary>
        /// 获取融资方充值记录一条信息（通过用户ID）
        /// </summary>
        /// <param name="account_id">用户id</param>
        /// <returns></returns>
        public ResultInfo<Fina_Recharge_Record_Model> GetFina_Recharge_Record_Model(int account_id)
        {
            ResultInfo<Fina_Recharge_Record_Model> result = new ResultInfo<Fina_Recharge_Record_Model>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetFina_Recharge_Record_Model(account_id);
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
        /// 获取融资方充值记录一条信息（通过用户订单）
        /// </summary>
        /// <param name="recharge_order">订单</param>
        /// <returns></returns>
        public ResultInfo<Fina_Recharge_Record_Model> GetFina_Recharge_Record_Model(long recharge_order)
        {
            ResultInfo<Fina_Recharge_Record_Model> result = new ResultInfo<Fina_Recharge_Record_Model>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetFina_Recharge_Record_Model(recharge_order);
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
        /// 获取融资方充值记录一条信息（通过用户订单）
        /// </summary>
        /// <param name="recharge_order">订单</param>
        /// <returns></returns>
        public ResultInfo<Fina_Recharge_Record_Model> GetFina_Recharge_Record_ModeNE(long recharge_order)
        {
            ResultInfo<Fina_Recharge_Record_Model> result = new ResultInfo<Fina_Recharge_Record_Model>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetFina_Recharge_Record_ModeNE(recharge_order);
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

        #region 修改充值记录(充值成功时改变充值状态并且帮助赵亮插入一条融资方账户日志表)用事务写
        /// <summary>
        /// 修改融资方充值记录信息
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="FID">融资方ID</param>
        /// <param name="FAccount">融资方账户ID</param>
        /// <param name="Money">充值金额</param>
        /// <param name="Money_Befo">变化前的金额</param>
        /// <param name="Money_Last">变化后的金额</param>
        /// <param name="r2_TrxId">易宝流水订单号</param>
        /// <param name="rq_SourceFee">用户手续费</param>
        /// <param name="rq_TargetFee">商户手续费</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserFina_Recharge_Record_Model(string OrderID, string FID, string FAccount, decimal Money, decimal Money_Befo, decimal Money_Last, string r2_TrxId, decimal rq_SourceFee, decimal rq_TargetFee)
        {
            YxLiCai.Tools.LogHelper.Write("不管成功与否进A", "OrderID：" + OrderID +"   "+ DateTime.Now);
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateUserFina_Recharge_Record_Model(OrderID, FID, FAccount, Money, Money_Befo, Money_Last, r2_TrxId, rq_SourceFee, rq_TargetFee);
                result.Result = true;
                YxLiCai.Tools.LogHelper.Write("不管成功与否进B", "OrderID：" + OrderID + " result.Data：" + result.Data + "   " + DateTime.Now);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.Write("不管成功与否进C", "OrderID：" + OrderID + " ex" + ex + "   " + DateTime.Now);
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            YxLiCai.Tools.LogHelper.Write("不管成功与否进D", "OrderID：" + OrderID + " result:" + result.Result + "、" + result.Data + "   " + DateTime.Now);
            return result;
        }
        #endregion

        #region 获取融资方充值记录信息列表
        /// <summary>
        /// 获取融资方充值记录信息列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="SMoney">起始是金钱</param>
        /// <param name="EMoney">截至金钱</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">截止时间</param>
        /// <returns></returns>
        public ResultInfo<List<Fina_Recharge_Record_Model>> GetFina_Recharge_Record_Model_List(int UID, decimal SMoney, decimal EMoney, DateTime Sdate, DateTime Edate, int State)
        {

            ResultInfo<List<Fina_Recharge_Record_Model>> result = new ResultInfo<List<Fina_Recharge_Record_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Fina_Recharge_Record_Model>();
                result.Data = dao.GetFina_Recharge_Record_Model_List(UID, SMoney, EMoney, Sdate, Edate, State);
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

        #endregion

        #region 融资方账户金额信息操作

        #region 添加一条融资方账户金钱信息
        /// <summary>
        /// 添加融资方账户金钱信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<int> AddFina_User_Count_Model(Fina_User_Count_Model model)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.AddFina_User_Count_Model(model);
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

        #region 获取融资方账户金钱余额一条信息
        /// <summary>
        /// 获取融资方账户金钱余额一条信息（通过用户ID）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public ResultInfo<Fina_User_Count_Model> GetFina_User_Count_Model(int account_id)
        {
            ResultInfo<Fina_User_Count_Model> result = new ResultInfo<Fina_User_Count_Model>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetFina_User_Count_Model(account_id);
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

        #region 获取融资方账户金钱列表
        /// <summary>
        /// 获取融资方账户金钱列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public ResultInfo<List<Fina_User_Count_Model>> GetFina_User_Count_Model_List(string strWhere)
        {

            ResultInfo<List<Fina_User_Count_Model>> result = new ResultInfo<List<Fina_User_Count_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Fina_User_Count_Model>();
                result.Data = dao.GetFina_User_Count_Model_List(strWhere);
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

        #region 得到用户账户信息列表
        /// <summary>
        /// 得到账户的金钱余额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserAccountMoney(int uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0m;
            try
            {
                result.Data = dao.GetUserAccountMoney(uid);
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

        #region 修改融资方账户金钱信息
        /// <summary>
        /// 修改融资方账户金钱信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateFina_User_Count_Model(Fina_User_Count_Model model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateFina_User_Count_Model(model);
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
        #endregion

        #region 帮助赵亮插入充值记录信息（只有充值成功的才插入）
        /// <summary>
        /// 添加融资方信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<int> Add_financier_account_log(Financier_account_log model)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.Add_financier_account_log(model);
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

        #region  项目信息偿还资金操作

        #region 利息偿还数据列表
        /// <summary>
        /// 利息偿还数据列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">截止时间</param>
        /// <returns></returns>
        public ResultInfo<List<Fer_account_item>> GetFer_account_item_List(int UID, int LoanPeriod, decimal Money)
        {

            ResultInfo<List<Fer_account_item>> result = new ResultInfo<List<Fer_account_item>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Fer_account_item>();
                result.Data = dao.GetFer_account_item_List(UID, LoanPeriod, Money);
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

        #region 利息偿还数据列表(又换了)
        /// <summary>
        /// 利息偿还数据列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">截止时间</param>
        /// <returns></returns>
        public ResultInfo<List<Project_refund_interest>> GetProject_refund_interest_List(int UID, string Sdate, string Edate)
        {

            ResultInfo<List<Project_refund_interest>> result = new ResultInfo<List<Project_refund_interest>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Project_refund_interest>();
                result.Data = dao.GetProject_refund_interest_List(UID, Sdate, Edate);
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

        #region 项目本金偿还申请数据列表

        #region 申请还款操作
        /// <summary>
        /// 申请偿还本金
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ResultInfo<bool> ApplyProject_refund_principal(Project_refund_principal model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.ApplyProject_refund_principal(model);
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

        #region 申请偿还本金
        /// <summary>
        /// 申请偿还本金
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateProject_refund_principal(Int64 ID, int Status, decimal money)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.UpdateProject_refund_principal(ID, Status, money);
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

        #region 本金偿还数据列表
        /// <summary>
        /// 本金偿还数据列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">截止时间</param>
        /// <returns></returns>
        public ResultInfo<List<Project_refund_principal>> GetProject_refund_principal_Listt(string Pname, int UID, int status, decimal money)
        {

            ResultInfo<List<Project_refund_principal>> result = new ResultInfo<List<Project_refund_principal>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Project_refund_principal>();
                result.Data = dao.GetProject_refund_principal_List(Pname, UID, status, money);
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

        #region 本金偿还列表（以项目为纬度）
        /// <summary>
        /// 本金偿还数据列表
        /// </summary>
        ///  <param name="Pname">项目名称</param>
        /// <param name="UID">融资方ID</param>
        /// <param name="status">项目状态</param>
        /// <param name="money">项目金额</param>
        /// <returns></returns>
        public ResultInfo<List<ProjectModel>> GetProjectModel_List(string Pname, int UID, int status, decimal money)
        {

            ResultInfo<List<ProjectModel>> result = new ResultInfo<List<ProjectModel>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetProjectModel_List(Pname, UID, status, money);
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


        #region 后台判断融资方还款是否超出项目金额(或者账户的还款金额是否够用)(或者账户的还款金额是否够用)
        /// <summary>
        /// 后台判断融资方还款是否超出项目金额(或者账户的还款金额是否够用)
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="PID">项目ID</param>        
        /// <returns></returns>
        public ResultInfo<ProjectModel> GetProjectModel_UseRep(int UID, int PID)
        {
            ResultInfo<ProjectModel> result = new ResultInfo<ProjectModel>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetProjectModel(UID, PID);
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

        #region 申请还款后改变账户表金额（冻结住）
        /// <summary>
        /// 申请还款后改变账户表金额（冻结住）
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ResultInfo<bool> Updatefer_account(int UID, decimal money)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Updatefer_account(UID, money);
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
        /// 申请还款
        /// </summary>
        /// <param name="merchantID">融资方ID</param>
        /// <param name="amount">还款金额</param>
        /// <param name="pjid">项目ID</param>
        /// <returns></returns>
        public ResultInfo<bool> RepayPrincipal(int merchantID, decimal amount, int pjid, int FerAcount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.RepayPrincipal(merchantID, amount, pjid, FerAcount);
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

        #endregion

        #region 还款记录表
        /// <summary>
        /// 还款记录
        /// </summary>
        /// <param name="Pname">项目名称</param>
        /// <param name="UID">融资方ID</param>
        /// <param name="Smoney">还款金额</param>
        /// <param name="Emoney">还款金额</param>
        /// <param name="Sdate">还款日期</param>
        /// <param name="Edate">还款日期</param>
        /// <returns></returns>
        public ResultInfo<List<Pjt_repayment_Model>> GetPjt_repayment_Model_List(string Pname, int UID, decimal Smoney, decimal Emoney, string Sdate, string Edate, int LoanPeriod)
        {

            ResultInfo<List<Pjt_repayment_Model>> result = new ResultInfo<List<Pjt_repayment_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<Pjt_repayment_Model>();
                result.Data = dao.GetPjt_repayment_Model_List(Pname, UID, Smoney, Emoney, Sdate, Edate, LoanPeriod);
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

        #region 还款记录新的
        /// <summary>
        /// 还款记录 还息+还本金
        /// </summary>
        /// <param name="merchantName"></param>
        /// <param name="loanPeriod"></param>
        /// <param name="amount_min"></param>
        /// <param name="amount_max"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<RefundModelExtend> GetRefundLog(string UID, int loanPeriod, decimal amount_min, decimal amount_max, DateTime time_min, DateTime time_max,
           int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<RefundModelExtend>();

            try
            {
                var ds = dao.GetRefundLog(UID, loanPeriod, amount_min, amount_max, time_min, time_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(dao.DataToRefundModel(row as System.Data.DataRow));
                    }
                }
                //记录总数
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out totalCount);
                }
                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }

        #endregion
        #endregion


        #region 融资方提现

        #region 得到融资方提现记录信息
        /// <summary>
        /// 得到融资方提现记录信息
        /// </summary>
        /// <param name="Order">订单号</param>
        /// <param name="UID">UID</param>
        /// <param name="Sdate">开始时间</param>
        /// <param name="Edate">结束时间</param>
        /// <param name="statu">状态</param>
        /// <returns></returns>
        public ResultInfo<List<Fer_Withdraw>> GetFer_Withdraw_List(string Order, int UID, DateTime Sdate, DateTime Edate, int statu)
        {
            ResultInfo<List<Fer_Withdraw>> result = new ResultInfo<List<Fer_Withdraw>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = dao.GetFer_Withdraw_List(Order, UID, Sdate, Edate, statu);
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

        #region 查看融资方提现金额是否充足（或者说是得到融资方可提现的余额-金额）
        /// <summary>
        /// 账号可提现金额
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> GetUserWithdrawals(int uid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.GetUserWithdrawals(uid);
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

        #region 添加提现信息
        /// <summary>
        /// 添加提现信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public ResultInfo<int> Add_fer_withdraw(Fer_Withdraw model)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = dao.Add_fer_withdraw(model);
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


        #region 申请提现后改变账户表金额（冻结住）
        /// <summary>
        /// 申请提现后改变账户表金额（冻结住）
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ResultInfo<bool> Updatefer_account_FZ(int UID, decimal money)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.Updatefer_account_FZ(UID, money);
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

        #region 申请提现用事物

        /// <summary>
        ///申请提现-事物控制
        /// </summary>
        /// <param name="merchantID">融资方账户ID</param>
        /// <param name="amount">提现金额</param>
        /// <param name="Order">订单号</param>
        /// <param name="BankID">银行卡</param>
        /// <returns></returns>
        public ResultInfo<bool> ApplicationWithdrawal(int merchantID, decimal amount, string Order, string BankID,int Fer_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.ApplicationWithdrawal(merchantID, amount, Order, BankID, Fer_id);
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

        #endregion


        #region 判断用户是否手机认证
        /// <summary>
        /// 判断用户手机号是否认证
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public ResultInfo<bool> getCHcekPhone(int userId, string Phone)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Data = dao.getCHcekPhone(userId, Phone);
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
