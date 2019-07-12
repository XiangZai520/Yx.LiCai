/*
 * 融资方用户个人信息
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YxLiCai.FinaAdmin.Models;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Model.UserVerification;
using YxLiCai.Server.UserFinancingManagement;
using YxLiCai.Tools;
using YxLiCai.Tools.SafeEncrypt;
using YxLiCai.Tools.Util;
using UserSmallReads = YxLiCai.Model.Account.UserSmallReads;

namespace YxLiCai.FinaAdmin.Controllers
{
    /// <summary>
    /// 融资方用户个人信息
    /// </summary>
    public class FinancingController : Controller
    {
        private UserInfo_FinancingManagement_Services user;
        //
        // GET: /UserFinancing/
        public FinancingController()
        {
            user = new UserInfo_FinancingManagement_Services();
        }
        #region 融资方个人信息页面视图
        /// <summary>
        /// 融资方个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            string userinfo = CookieHelper.ReadCookie(Tools.Const.SystemConst.FinauserInfoCookieName);
            if (string.IsNullOrEmpty(userinfo))
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserInfo_FinancingManagement_Model> result = user.GetUserModel(UserContext.simpleUserInfoModel.Id);

                if (result.Result && result.Data != null)
                {
                    return View(result.Data);
                }
            }
            return View();
        }
        #endregion

        #region 修改登录密码的视图页面
        /// <summary>
        /// 融资方个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePasswerd()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        #endregion

        #region 交易密码的视图页面
        /// <summary>
        /// 交易密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSPasswerd(int type)
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Type = type;
            ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
            if (reSmall.Result && reSmall.Data != null)
            {
                if (string.IsNullOrEmpty(reSmall.Data.SPassword))
                {
                    ViewBag.Type = 1;
                    ViewBag.Title = "设置交易密码";
                    return View();
                }
                ViewBag.Type = 0;
            }
            if (type == 1)
            {
                ViewBag.Title = "设置交易密码";
            }
            else { ViewBag.Title = "修改交易密码"; }
            return View();
        }
        #endregion

        #region 融资方提现页面
        /// <summary>
        /// 融资方提现页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Fer_withdraw()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null)
                {
                    if (reSmall.Data.IsRealCard != 1 || reSmall.Data.IsBindBank != 1 || string.IsNullOrEmpty(reSmall.Data.SPassword) || string.IsNullOrEmpty(reSmall.Data.Phone))
                    {
                        return Content("<script>alert('你的信息还未填写完整请确保以下信息【实名认证、绑定银行卡、交易密码、手机号】已认证，请去个人中心完善。');   window.location.href = 'Index';</script>");
                    }
                    var resultExist = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                    if (resultExist.Result && resultExist.Data != null)
                    {
                        ResultInfo<decimal> result = user.GetUserWithdrawals(UserContext.simpleUserInfoModel.Id);
                        if (result.Result && result.Data > 0)
                        {
                            ViewBag.isbank_card = resultExist.Data.IsBindBank;
                            ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                            ViewBag.Amount = result.Data;
                            return View("Fer_withdraw");
                        }
                        ViewBag.isbank_card = resultExist.Data.IsBindBank;
                        ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                    }
                }
            }
            ViewBag.Amount = 0;
            return View();
        }

        #endregion

        #region 融资方充值页面（易宝支付接口）
        /// <summary>
        /// 融资方充值页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Fer_Recharge()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                var resultExist = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (resultExist.Result && resultExist.Data != null)
                {
                    ResultInfo<decimal> result = user.GetUserRecharge_Balance(UserContext.simpleUserInfoModel.Id);
                    if (result.Result && result.Data > 0)
                    {
                        ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                        ViewBag.Amount = result.Data;
                        return View("Fer_Recharge");
                    }
                    ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                }
            }
            ViewBag.Amount = 0;
            return View();
        }
        #endregion

        #region 融资方充值页面（连连网银支付）
        /// <summary>
        /// 融资方充值页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NEW_Fer_Recharge()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }

            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                var resultExist = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (resultExist.Result && resultExist.Data != null)
                {
                    ResultInfo<decimal> result = user.GetUserRecharge_Balance(UserContext.simpleUserInfoModel.Id);
                    if (result.Result && result.Data > 0)
                    {
                        ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                        ViewBag.Amount = result.Data;
                        return View("NEW_Fer_Recharge");
                    }
                    ViewBag.SPassword = (string.IsNullOrEmpty(resultExist.Data.SPassword) ? "" : "1380808800");
                }
            }
            ViewBag.Amount = 0;
            return View();
        }
        #endregion


        #region 融资方设置公司名称视图页面
        public ActionResult SetComentName()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        #endregion

        #region 融资方设置公司名称方法
        /// <summary>
        ///融资方设置公司名称方法
        /// </summary>
        /// <param name="CName">公司名称</param>      
        /// <returns></returns>
        public JsonResult SetCompanyName(string CName)
        {
            if (string.IsNullOrEmpty(CName)) { return Json(new ResultModel<bool>(2, "公司名称不能为空", false), JsonRequestBehavior.AllowGet); }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<bool> result = user.UpdateCompanyName(UserContext.simpleUserInfoModel.Id, CName);
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(1, "设置成功", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "网络异常", false), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 身份认证页面视图
        /// <summary>
        /// 身份认证操作页面
        /// </summary>
        /// <returns></returns>
        public ActionResult IdentityAuthentication()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null)
                {
                    if (reSmall.Data.IsRealCard == 1)
                    {
                        return Redirect("Index");
                    }
                }
            }
            return View();
        }
        #endregion

        #region 忘记交易密码视图
        /// <summary>
        /// 忘记交易密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetSPasswerd()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
            if (reSmall.Result && reSmall.Data != null)
            {
                if (string.IsNullOrEmpty(reSmall.Data.Phone))
                {
                    return
                        Content(
                            "<script>alert('你的手机未验证，请验证。');   window.location.href = 'CheckPhone';</script>");
                }
            }
            return View();
        }

        #endregion

        #region 用户忘记密码修改
        /// <summary>
        /// 用户忘记密码修改
        /// </summary>
        /// <param name="SPwassord">新交易密码</param>
        /// <param name="Code">手机验证码</param>
        /// <returns></returns>
        public JsonResult ForgetUPSPasswerd(string Phone, string SPwassord, string Code)
        {
            if (string.IsNullOrEmpty(SPwassord) || string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Phone)) { return Json(new ResultModel<bool>(4, "信息填写不完整", true), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsFetchPwd(SPwassord))
            {
                return Json(new ResultModel<bool>(4, "交易密码格式不正确", true), JsonRequestBehavior.AllowGet);
            }
            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(new ResultModel<bool>(4, "手机号码格式不正确", true), JsonRequestBehavior.AllowGet);
            }
            var redis = new Tools.NoSql.RedisCache.RedisCache();
            string cachekey = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiu, Phone);
            var captcha = redis.Get<string>(cachekey);
            if (string.IsNullOrEmpty(captcha))
            {
                return Json(new ResultModel<bool>(2, "验证码已经过有效期", true), JsonRequestBehavior.AllowGet);
            }
            if (Code.ToLower() == captcha.ToLower())
            {
                SPwassord = MD5.MD5Convert(SPwassord, 32);
                ResultInfo<bool> result = user.UpdateUserSPassWord(UserContext.simpleUserInfoModel.Id, SPwassord);
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(1, "修改成功", true), JsonRequestBehavior.AllowGet);
                } return Json(new ResultModel<bool>(2, "交易密码设置失败", false), JsonRequestBehavior.AllowGet);

            }
            return Json(new ResultModel<bool>(2, "验证码错误", true), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 身份认证
        /// <summary>
        ///用户实名认证信息
        /// </summary>
        /// <param name="IDcard">身份证号</param>
        /// <param name="Username">用户名</param>
        /// <returns></returns>
        public JsonResult UpdateIsRealCard(string IDcard, string Username)
        {
            if (string.IsNullOrEmpty(IDcard)) { return Json(new ResultModel<bool>(4, "请输入身份证", true), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsIDNum(IDcard))
            {
                return Json(new ResultModel<bool>(4, "身份证格式不正确", true), JsonRequestBehavior.AllowGet);
            }
            ResultInfo<bool> result = null;
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                /*在这里先进行实名认证接口*/

                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();

                /*一天之内最多只能调用3次验证身份证接口*/
                string CheckTimes = string.Format("CheckUserIDCard_{0}", UserContext.simpleUserInfoModel.Id);
                var val = redis.Get<int>(CheckTimes);
                if (val >= 3)
                {
                    return Json(new ResultModel<bool>(4, "请不要重复发送", true), JsonRequestBehavior.AllowGet);
                }
                YxLiCai.Server.UserVerificationIDcard.VerificationIDcard CheckUserIDCard = new YxLiCai.Server.UserVerificationIDcard.VerificationIDcard();
                //身份验证
                ResultInfo<string> CheckIDResult = CheckUserIDCard.CheckUserIDCard(Username, IDcard);
                if (CheckIDResult.Result)
                {
                    val++;
                    redis.Add(CheckTimes, val, DateTime.Now.AddDays(1));
                    string str = CheckIDResult.Data;
                    YxLiCai.Tools.LogHelper.Write("后台身份身份验证", "Userid:" + UserContext.simpleUserInfoModel.Id + "--返回结果:" + str);
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (str.IndexOf("error") < 0)
                        {
                            str = str.Remove(str.IndexOf("["), 1);
                            str = str.Remove(str.LastIndexOf("]"), 1);
                            People_IDCard people = SerializeHelper.JsonDeserialize<People_IDCard>(str);
                            if (people != null)
                            {
                                if (people.id_status == 0)
                                {
                                    result = user.UpdateIsRealCard(UserContext.simpleUserInfoModel.Id, IDcard, Username);
                                    if (result.Result && result.Data)
                                    {
                                        return Json(new ResultModel<bool>(1, "认证成功", true), JsonRequestBehavior.AllowGet);
                                    }
                                    else { return Json(new ResultModel<bool>(2, "身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                                }
                                else
                                {
                                    YxLiCai.Tools.LogHelper.Write("后台身份证验证失败", "Userid:" + UserContext.simpleUserInfoModel.Id + "身份证:" + IDcard + ":" + Username + "错误提示：-" + GetEnumValue(people.id_status.ToString()));
                                    return Json(new ResultModel<bool>(2, "姓名与身份证号不一致，请重新输入", false), JsonRequestBehavior.AllowGet);
                                }
                            }
                            else { return Json(new ResultModel<bool>(2, "姓名与身份证号不一致，请重新输入", false), JsonRequestBehavior.AllowGet); }
                        }
                        else { return Json(new ResultModel<bool>(2, "姓名与身份证号不一致，请重新输入", false), JsonRequestBehavior.AllowGet); }
                    }
                    else { return Json(new ResultModel<bool>(2, "姓名与身份证号不一致，请重新输入", false), JsonRequestBehavior.AllowGet); }
                }
                else { return Json(new ResultModel<bool>(2, "姓名与身份证号不一致，请重新输入", false), JsonRequestBehavior.AllowGet); }
            }
            else { return Json(new ResultModel<bool>(2, "用户没有登录", false), JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region 绑定银行卡操作页面
        /// <summary>
        /// 绑定银行卡操作页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BindingBank()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null)
                {
                    if (reSmall.Data.IsBindBank == 1)
                    {
                        return Redirect("Index");
                    }
                }
            }
            return View();
        }
        #endregion

        #region 绑定银行卡操作

        #region 发送绑定银行卡操作
        /// <summary>
        ///绑定银行卡
        /// </summary>
        /// <param name="BankName">银行名称</param>
        /// <param name="BankCard">银行卡</param>
        /// <returns></returns>
        public JsonResult SendBoundBankCard(string BankName, string BankCard, string Phone)
        {
            if (string.IsNullOrEmpty(BankName) || string.IsNullOrEmpty(BankCard) || string.IsNullOrEmpty(Phone))
            {
                return Json(new ResultModel<bool>(4, "填写信息不可为空", true), JsonRequestBehavior.AllowGet);
            }

            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(new ResultModel<bool>(4, "手机号错误，请重新输入", true), JsonRequestBehavior.AllowGet);
            }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(BankCard, @"(^\d{15,22}$)")))
            {
                return Json(new ResultModel<bool>(4, "卡号错误，请重新输入", true), JsonRequestBehavior.AllowGet);
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                #region 防止用户多次刷新页面，重复发送验证码
                string tempCode = string.Format("FinancingManagement_{0}", UserContext.simpleUserInfoModel.Id);
                var val = redis.Get<string>(tempCode);
                if (!string.IsNullOrEmpty(val))
                {
                    return Json(new ResultModel<bool>(4, "请不要重复发送", true), JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region 生成商户的唯一绑卡请求号
                //商户生成的唯一绑卡请求号
                string requestid = YxLiCai.Tools.Util.Helper.generateOrderCode(UserContext.simpleUserInfoModel.Id);
                string requestidkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.UserFinancingCaptchaCodeSendBoundBankCard, UserContext.simpleUserInfoModel.Id);
                redis.Add(requestidkey, requestid, DateTime.Now.AddMinutes(5));
                #endregion
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null)
                {
                    if (reSmall.Data.IsRealCard == 0)
                    {
                        return Json(new ResultModel<bool>(11, "你还没实名认证，请认证", true), JsonRequestBehavior.AllowGet);
                    }
                    YxLiCai.Tools.Pay.Yeepay.YJPay pay = new YxLiCai.Tools.Pay.Yeepay.YJPay();
                    string ip = "127.0.0.1";//YxLiCai.Tools.Util.Helper.GetIP();
                    //解除绑定操作
                    //string srtr = pay.unbind("621700", "7491", reSmall.Data.MyRealCard, 5);
                    //string srtr = pay.unbind("621700", "7491", "F" + UserContext.simpleUserInfoModel.Fer_account.ToString(), 2);
                    //YxLiCai.Tools.LogHelper.Write("解除绑定卡请求", "userid:" + reSmall.Data.MyRealCard + "--返回结果:" + srtr);
                    //发起绑卡请求
                    //string str = pay.invokebindbankcard(reSmall.Data.MyRealCard, requestid, BankCard, reSmall.Data.MyRealCard, reSmall.Data.MyRealName, Phone, ip, 5);
                    string str = pay.invokebindbankcard("F" + UserContext.simpleUserInfoModel.Fer_account.ToString(), requestid, BankCard, reSmall.Data.MyRealCard, reSmall.Data.MyRealName, Phone, ip, 2);
                    YxLiCai.Tools.LogHelper.Write("后台银行卡绑卡请求", "绑卡类型：2 帮卡标识: F" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + str);
                    if (str.IndexOf("error") < 0)
                    {
                        Msssages ob = YxLiCai.Tools.SerializeHelper.JsonDeserialize<Msssages>(str);
                        if (ob.codesender == "MERCHANT")
                        {
                            YxLiCai.Server.SendMsg.Send smsService = new YxLiCai.Server.SendMsg.Send();
                            string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("1");
                            content = string.Format(content, ob.smscode);

                            ResultInfo<string> sendResult = YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(Phone, content); 
                            if (sendResult.Result)
                            {
                                return Json(new ResultModel<bool>(1, "发送成功", true), JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new ResultModel<bool>(3, "发送失败，请重新发送", true), JsonRequestBehavior.AllowGet);
                            }
                        }
                        ///此次操作计入缓存,防止用户刷新页面再次发送
                        redis.Add(tempCode, 1, DateTime.Now.AddMinutes(1));
                        return Json(new ResultModel<bool>(1, "", true), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Msssages_Error me = YxLiCai.Tools.SerializeHelper.JsonDeserialize<Msssages_Error>(str);
                        YxLiCai.Tools.LogHelper.Write("后台银行卡绑卡失败", "融资方ID：" + UserContext.simpleUserInfoModel.Id + "，银行卡:" + BankCard + "，身份证:" + reSmall.Data.MyRealCard + "，真实姓名:" + reSmall.Data.MyRealName + "，银行预留手机:" + Phone + "- 错误码：" + me.error_code + "，错误信息：" + me.error_msg);
                        return me.error_code == "600326" ? Json(new ResultModel<bool>(2, me.error_msg, false), JsonRequestBehavior.AllowGet) : Json(new ResultModel<bool>(2, "请确定银行卡、手机号是开户时信息", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else { return Json(new ResultModel<bool>(2, "服务器连接失败，请稍候再试", false), JsonRequestBehavior.AllowGet); }
            }
            else { return Json(new ResultModel<bool>(2, "服务器连接失败，请稍候再试", false), JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region 确定绑定银行卡操作
        /// <summary>
        ///确定绑定银行卡
        /// </summary>
        /// <param name="BankName">银行名称</param>
        /// <param name="BankCard">银行卡</param>
        /// <returns></returns>
        public JsonResult ConfimBoundBankCard(string BankName, string BankCard, string Phone, string validatecode)
        {
            if (string.IsNullOrEmpty(BankName) || string.IsNullOrEmpty(BankCard) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(validatecode))
            {
                return Json(new ResultModel<bool>(4, "填写信息不可为空", true), JsonRequestBehavior.AllowGet);
            }
            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(new ResultModel<bool>(4, "手机格式不正确", true), JsonRequestBehavior.AllowGet);
            }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(BankCard, @"(^\d{15,22}$)")))
            {
                return Json(new ResultModel<bool>(4, "银行卡号不正确", true), JsonRequestBehavior.AllowGet);
            }
            int Userid = UserContext.simpleUserInfoModel.Id;
            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            if (Userid > 0)
            {
                YxLiCai.Tools.Pay.Yeepay.YJPay yy = new YxLiCai.Tools.Pay.Yeepay.YJPay();
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                #region 获取商户的唯一绑卡请求号
                string requestidkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.UserFinancingCaptchaCodeSendBoundBankCard, Userid);
                var requestid = redis.Get<string>(requestidkey);
                #endregion
                string ss = yy.confirmbindbankcard(requestid, validatecode);
                MsssagesT_Error me = SerializeHelper.JsonDeserialize<MsssagesT_Error>(ss);
                MsssagesT mt = SerializeHelper.JsonDeserialize<MsssagesT>(ss);
                if (ss.IndexOf("error") < 0)
                {
                    if (mt != null)
                    {
                        model.bank_code = mt.bankcode;
                        model.first_num = mt.card_top;
                        model.last_num = mt.card_last;
                        model.requestid = mt.requestid;
                    }
                    model.financier_id = Userid;
                    model.bank_card = BankCard;
                    model.bank_name = BankName;
                    model.bank_phone = Phone;
                    model.isbank_card = 1;
                    ResultInfo<bool> result = user.BoundBankCard(model);
                    if (result.Result)
                    {
                        return Json(new ResultModel<bool>(1, "绑定成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else { return Json(new ResultModel<bool>(2, "绑定银行卡失败", false), JsonRequestBehavior.AllowGet); }
                }
                else
                {
                    YxLiCai.Tools.LogHelper.Write("后台确定绑定银行卡失败", "银行名称：" + BankName + "，银行卡:" + BankCard + "，银行预留手机:" + Phone + ",绑卡请求号：" + requestid + "错误结果-错误码：" + me.error_code + "，错误信息：" + me.error_msg);
                    //return Json(new ResultModel<bool>(2, me.error_msg, false), JsonRequestBehavior.AllowGet);
                    return Json(new ResultModel<bool>(2, "请检查信息是否填写无误", false), JsonRequestBehavior.AllowGet);
                }
            }
            else { return Json(new ResultModel<bool>(2, "服务器连接失败，请稍候再试", false), JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #endregion

        #region 修改用户的登录密码
        /// <summary>
        ///修改用户的登录密码
        /// </summary>
        /// <param name="Passwerd">新密码</param> 
        /// <param name="OldPasswerd">原始密码</param>      
        /// <returns></returns>
        public JsonResult UpdatePassword(string Passwerd, string OldPasswerd)
        {
            if (string.IsNullOrEmpty(Passwerd)) { return Json(new ResultModel<bool>(2, "请输入密码", false), JsonRequestBehavior.AllowGet); }
            if (string.IsNullOrEmpty(OldPasswerd)) { return Json(new ResultModel<bool>(2, "原始密码不能为空", false), JsonRequestBehavior.AllowGet); }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                OldPasswerd = MD5.MD5Convert(OldPasswerd, 32);
                ResultInfo<bool> resultOld = user.IsTruePassWord(UserContext.simpleUserInfoModel.Id, OldPasswerd);
                if (resultOld.Result && resultOld.Data)
                {
                    Passwerd = MD5.MD5Convert(Passwerd, 32);
                    ResultInfo<bool> result = user.UpdateUserPassWord(UserContext.simpleUserInfoModel.Id, Passwerd);
                    if (result.Result && result.Data)
                    {
                        user.SignOut();
                        return Json(new ResultModel<bool>(1, "密码修改成功", true), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new ResultModel<bool>(2, "原始密码输入不正确", false), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel<bool>(3, "原始密码错误，请重新输入", false), JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 检查用户的交易密码是否正确
        /// <summary>
        ///检查用户的交易密码是否正确
        /// </summary>
        /// <param name="Passwerd">密码</param>
        /// <returns></returns>
        public JsonResult CheckUserSPW(string Passwerd)
        {
            if (string.IsNullOrEmpty(Passwerd)) { return Json(new ResultModel<bool>(5, "交易密码不能为空", false), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsFetchPwd(Passwerd)) { return Json(new ResultModel<bool>(4, "交易密码格式不正确", false), JsonRequestBehavior.AllowGet); }

            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null && string.IsNullOrEmpty(reSmall.Data.SPassword))
                {
                    return Json(new ResultModel<bool>(3, "你还没有设置交易密码,请设置", true), JsonRequestBehavior.AllowGet);
                }
                Passwerd = MD5.MD5Convert(Passwerd, 32);
                ResultInfo<bool> result = user.IsTrueSallpassword(UserContext.simpleUserInfoModel.Id, Passwerd);
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(1, "交易密码正确", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "交易密码输入不正确", false), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改用户的交易密码
        /// <summary>
        ///修改用户的交易密码
        /// </summary>
        /// <param name="Passwerd">密码</param>      
        /// <returns></returns>
        public JsonResult UpdateSPassword(string Passwerd, string OldPasswerd, int type)
        {
            if (string.IsNullOrEmpty(Passwerd)) { return Json(new ResultModel<bool>(1, "交易密码不能为空", false), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsFetchPwd(Passwerd)) { return Json(new ResultModel<bool>(2, "交易密码格式不正确", false), JsonRequestBehavior.AllowGet); }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                if (type == 0)//修改交易密码
                {
                    if (string.IsNullOrEmpty(OldPasswerd)) { return Json(new ResultModel<bool>(1, "原交易密码密码不能为空", false), JsonRequestBehavior.AllowGet); }
                    if (!ValidateHelper.IsFetchPwd(OldPasswerd)) { return Json(new ResultModel<bool>(2, "原交易密码格式不正确", false), JsonRequestBehavior.AllowGet); }
                    ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                    if (reSmall.Result && reSmall.Data != null && string.IsNullOrEmpty(reSmall.Data.SPassword))
                    {
                        return Json(new ResultModel<bool>(3, "你还没有设置交易密码", true), JsonRequestBehavior.AllowGet);
                    }
                    OldPasswerd = MD5.MD5Convert(OldPasswerd, 32);
                    ResultInfo<bool> resultOld = user.IsTrueSallpassword(UserContext.simpleUserInfoModel.Id, OldPasswerd);
                    if (resultOld.Result && resultOld.Data)
                    {
                        Passwerd = MD5.MD5Convert(Passwerd, 32);
                        ResultInfo<bool> result = user.UpdateUserSPassWord(UserContext.simpleUserInfoModel.Id, Passwerd);
                        if (result.Result && result.Data)
                        {
                            return Json(new ResultModel<bool>(1, "修改成功", true), JsonRequestBehavior.AllowGet);
                        } return Json(new ResultModel<bool>(2, "交易密码修改失败", false), JsonRequestBehavior.AllowGet);
                    } return Json(new ResultModel<bool>(2, "原始交易密码输入不正确", false), JsonRequestBehavior.AllowGet);
                }
                Passwerd = MD5.MD5Convert(Passwerd, 32);
                ResultInfo<bool> res = user.UpdateUserSPassWord(UserContext.simpleUserInfoModel.Id, Passwerd);
                if (res.Result && res.Data)
                {
                    return Json(new ResultModel<bool>(1, "设置成功", true), JsonRequestBehavior.AllowGet);
                }
                return Json(new ResultModel<bool>(2, "设置失败", false), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel<bool>(2, "你还没有登录", false), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 手机验证视图页面
        /// <summary>
        /// 手机验证视图页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckPhone()
        {
            if (UserContext.simpleUserInfoModel.Id <= 0)
            {
                return RedirectToAction("Login", "Account");
            }

            ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
            if (reSmall.Result && reSmall.Data != null)
            {
                if (!string.IsNullOrEmpty(reSmall.Data.Phone))
                {
                    return Redirect("Index");
                }
            }
            return View();
        }
        #endregion

        #region 发送手机验证码
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult SendValidateCode(string Phone)
        {

            if (!string.IsNullOrEmpty(Phone))
            {
                if (!ValidateHelper.IsPhoneNum(Phone))
                {
                    return Json(new ResultModel<bool>(4, "手机格式不正确", true), JsonRequestBehavior.AllowGet);
                }
                //验证码插入Redis缓存
                var redis = new Tools.NoSql.RedisCache.RedisCache();
                string FinancingCodeYIxiuFS = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiuFS, Phone);
                var Fshua = redis.Get<string>(FinancingCodeYIxiuFS);
                if (!string.IsNullOrEmpty(Fshua))
                {
                    return Json(new ResultModel<bool>(2, "不能频繁发送", true), JsonRequestBehavior.AllowGet);
                }


                Server.SendMsg.Send smsService = new Server.SendMsg.Send();
                //获取四位的随机验证码
                string code = Validate_Code.GetRandomCode(6);
                LogHelper.Write("发送手机验证码", code);
                string content = Tools.Const.SystemConst.MsgContentByType("1");
                content = string.Format(content, code);
                ResultInfo<string> sendResult = YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(Phone, content); 
                if (!sendResult.Result)
                    return Json(new ResultModel<bool>(2, "服务器繁忙请稍候", true), JsonRequestBehavior.AllowGet);
                //发送短信
                string cachekey = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiu, Phone);
                redis.Add(cachekey, code, DateTime.Now.AddMinutes(5));
                //防止刷新重新发送
                redis.Add(FinancingCodeYIxiuFS, "1", DateTime.Now.AddMinutes(1));
                return Json(new ResultModel<bool>(1, "发送成功", true), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel<bool>(2, "手机号不能为空", true), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 忘记交易密码发送验证码
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult SendValidate(string Phone)
        {

            if (!string.IsNullOrEmpty(Phone))
            {
                if (!ValidateHelper.IsPhoneNum(Phone))
                {
                    return Json(new ResultModel<bool>(4, "手机格式不正确", true), JsonRequestBehavior.AllowGet);
                }
                var smle = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (smle.Result && smle.Data != null)
                {
                    if (string.IsNullOrEmpty(smle.Data.Phone))
                    {
                        return Json(new ResultModel<bool>(9, "手机号未认证", true), JsonRequestBehavior.AllowGet);
                    }
                }
                var resultp = user.getCHcekPhone(UserContext.simpleUserInfoModel.Id, Phone);
                if (!(resultp.Result && resultp.Data))
                {
                    return Json(new ResultModel<bool>(4, "输入手机号与认证手机号不一致", true), JsonRequestBehavior.AllowGet);
                }
                //验证码插入Redis缓存
                var redis = new Tools.NoSql.RedisCache.RedisCache();
                string FinancingCodeYIxiuFS = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiuFS, Phone + "521");
                var Fshua = redis.Get<string>(FinancingCodeYIxiuFS);
                if (!string.IsNullOrEmpty(Fshua))
                {
                    return Json(new ResultModel<bool>(4, "不能频繁发送", true), JsonRequestBehavior.AllowGet);
                }
                Server.SendMsg.Send smsService = new Server.SendMsg.Send();
                //获取四位的随机验证码
                string code = Validate_Code.GetRandomCode(6);
                LogHelper.Write("发送手机验证码", code);
                string content = Tools.Const.SystemConst.MsgContentByType("1");
                content = string.Format(content, code);
                ResultInfo<string> sendResult = YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(Phone, content); 
                if (!sendResult.Result)
                    return Json(new ResultModel<bool>(3, "服务器繁忙请稍候", true), JsonRequestBehavior.AllowGet);
                //发送短信
                string cachekey = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiu, Phone);
                redis.Add(cachekey, code, DateTime.Now.AddMinutes(5));
                //防止刷新重新发送
                redis.Add(FinancingCodeYIxiuFS, "1", DateTime.Now.AddMinutes(1));
                return Json(new ResultModel<bool>(1, "发送成功", true), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel<bool>(2, "手机号不能为空", true), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 判断手机验证码是否正确
        /// <summary>
        ///判断用户验手机证码是否正确
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public JsonResult CheckUserCode(string Phone, string Code)
        {
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Code)) { return Json(new ResultModel<bool>(4, "信息填写不完整", true), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(new ResultModel<bool>(4, "手机格式不正确", true), JsonRequestBehavior.AllowGet);
            }
            var redis = new Tools.NoSql.RedisCache.RedisCache();
            string cachekey = string.Format(Tools.Const.RedisCacheKey.FinancingCodeYIxiu, Phone);
            var captcha = redis.Get<string>(cachekey);
            if (string.IsNullOrEmpty(captcha))
            {
                return Json(new ResultModel<bool>(2, "验证码已经过有效期", true), JsonRequestBehavior.AllowGet);
            }
            if (Code.ToLower() == captcha.ToLower())
            {
                ResultInfo<bool> result = user.UpdateUserPhone(UserContext.simpleUserInfoModel.Id, Phone);
                if (result.Result && result.Data) { return Json(new ResultModel<bool>(1, "验证码正确", true), JsonRequestBehavior.AllowGet); }

            }
            return Json(new ResultModel<bool>(2, "验证码错误", true), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取还款记录新的
        /// <summary>
        /// 还款记录
        /// </summary>
        /// <param name="merchantName">融资方姓名</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">还款金额</param>
        /// <param name="amount_max">还款金额</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public JsonResult GetRefundLog(int loanPeriod, decimal amount_min, decimal amount_max, DateTime time_min, DateTime time_max,
           int skip, int take)
        {
            if (amount_min != -1 && amount_max != -1)
            {

                if (amount_max < amount_min)
                {
                    return Json("还款开始金额不能大于结束金额", JsonRequestBehavior.AllowGet);
                }
            }

            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(time_max, time_min);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }

            int countAll = 0;

            var pageResult = new PageViewModel<Model.Refund.RefundModelExtend>
            {
                DataResult = user.GetRefundLog(UserContext.simpleUserInfoModel.Id.ToString(), loanPeriod, amount_min, amount_max, time_min, time_max, skip, take, out countAll),
                TotalCount = countAll
            };
            return Json(pageResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取利息记录信息
        /// <summary>
        /// 获取利息记录信息
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetFer_account_item_List(UserFinancingQueryViewModel queryModel)
        {

            var result = user.GetFer_account_item_List(UserContext.simpleUserInfoModel.Id, queryModel.LoanPeriod, queryModel.interest_payable);

            if (result.Result && result.Data.Count > 0) { return Json(result.Data, JsonRequestBehavior.AllowGet); }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取本金记录
        /// <summary>
        /// 获取本金记录
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetProjectModel_List(UserFinancingQueryViewModel queryModel)
        {

            var result = user.GetProjectModel_List(queryModel.ProjectName, UserContext.simpleUserInfoModel.Id, queryModel.State, queryModel.recharge_moneyStar);

            if (result.Result && result.Data.Count > 0) { return Json(result.Data, JsonRequestBehavior.AllowGet); }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 申请还款操作
        /// <summary>
        /// 融资方申请还款
        /// </summary>
        /// <param name="project_ID">项目ID</param>
        /// <param name="nowamount">本次申请还款金额</param>
        /// <returns></returns>
        public JsonResult ApplicationRepayment(int project_ID, decimal nowamount, string SpWD)
        {
            if (!(project_ID > 0)) { return Json(new ResultModel<bool>(2, "信息错误", false), JsonRequestBehavior.AllowGet); }
            if (!(nowamount > 0)) { return Json(new ResultModel<bool>(2, "请输入整额", false), JsonRequestBehavior.AllowGet); }
            if (string.IsNullOrEmpty(SpWD))
            {
                return Json(new ResultModel<bool>(2, "请输入交易密码！", false), JsonRequestBehavior.AllowGet);
            }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {

                SpWD = MD5.MD5Convert(SpWD, 32);
                ResultInfo<bool> resultOld = user.IsTrueSallpassword(UserContext.simpleUserInfoModel.Id, SpWD);
                if (!(resultOld.Result && resultOld.Data))
                {
                    return Json(new ResultModel<bool>(5, "交易密码不正确！", false), JsonRequestBehavior.AllowGet);
                }
                var result = user.GetProjectModel_UseRep(UserContext.simpleUserInfoModel.Id, project_ID);
                if (result.Result && result.Data != null)
                {
                    if (nowamount > result.Data.AmountSold)
                    {
                        return Json(new ResultModel<bool>(2, "本次项目应还金额为￥：" + result.Data.AmountSold + "，不要超出哦！", true), JsonRequestBehavior.AllowGet);
                    }
                    if (result.Data.interest_payable <= 0)
                    {
                        if (nowamount > (result.Data.amount_repay - result.Data.amount_repay_fz)) { return Json(new ResultModel<bool>(2, "你的可用余额不足，请充值", true), JsonRequestBehavior.AllowGet); }
                    }
                    else
                    {
                        if (nowamount > (result.Data.amount_repay - result.Data.amount_repay_fz - result.Data.interest_payable)) { return Json(new ResultModel<bool>(2, "你还有未还的利息，现在可用来项目还款的金额不足，请充值", true), JsonRequestBehavior.AllowGet); }

                    }
                    if (nowamount + result.Data.Amount_paid > result.Data.Amount) { return Json(new ResultModel<bool>(2, "你所申请的还款将超出项目金额", true), JsonRequestBehavior.AllowGet); }
                    //if (nowamount + result.Data.Amount_paid > result.Data.AmountSold) { return Json(new ResultModel<bool>(2, "你所申请的还款将超出项目金额", true), JsonRequestBehavior.AllowGet); }
                    //申请还款
                    var result_Application = user.RepayPrincipal(UserContext.simpleUserInfoModel.Id, nowamount, project_ID, UserContext.simpleUserInfoModel.Fer_account);
                    if (result_Application.Result && result_Application.Data)
                    {
                        return Json(new ResultModel<bool>(1, "申请成功", true), JsonRequestBehavior.AllowGet);
                    }

                    return Json(new ResultModel<bool>(3, "申请失败", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "申请失败", false), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 提现记录
        /// <summary>
        /// 获得提现记录
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetFer_Withdraw_List(UserFinancingQueryViewModel queryModel)
        {
            if (queryModel.recharge_timeStar != DateTime.MinValue && queryModel.recharge_timeEnd != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(queryModel.recharge_timeEnd, queryModel.recharge_timeStar);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            var result = user.GetFer_Withdraw_List(queryModel.Order, UserContext.simpleUserInfoModel.Fer_account, queryModel.recharge_timeStar, queryModel.recharge_timeEnd, queryModel.State);

            if (result.Result && result.Data != null) { return Json(result.Data, JsonRequestBehavior.AllowGet); }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户提现
        /// <summary>
        /// 用户提现
        /// </summary>
        /// <param name="project_ID"></param>
        /// <param name="amount">提现金额</param>
        /// <returns></returns>
        public JsonResult Add_fer_withdraw(string PassWord, decimal amount)
        {
            if (string.IsNullOrEmpty(PassWord)) { return Json(new ResultModel<bool>(5, "交易密码不能为空", false), JsonRequestBehavior.AllowGet); }
            if (!ValidateHelper.IsFetchPwd(PassWord)) { return Json(new ResultModel<bool>(4, "交易密码格式不正确", false), JsonRequestBehavior.AllowGet); }
            if (!(amount > 0)) { return Json(new ResultModel<bool>(2, "请输入大于零的金额", false), JsonRequestBehavior.AllowGet); }
            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
                if (reSmall.Result && reSmall.Data != null && string.IsNullOrEmpty(reSmall.Data.SPassword))
                {
                    return Json(new ResultModel<bool>(11, "你还没有设置交易密码,请设置", true), JsonRequestBehavior.AllowGet);
                }
                PassWord = MD5.MD5Convert(PassWord, 32);
                ResultInfo<bool> resultPD = user.IsTrueSallpassword(UserContext.simpleUserInfoModel.Id, PassWord);
                if (!resultPD.Result || !resultPD.Data)
                {
                    return Json(new ResultModel<bool>(9, "交易密码不正确正确", true), JsonRequestBehavior.AllowGet);
                }
                if (reSmall.Data.IsBindBank == 0) { return Json(new ResultModel<bool>(3, "未绑定银行卡", true), JsonRequestBehavior.AllowGet); }

                var result = user.GetUserWithdrawals(UserContext.simpleUserInfoModel.Id);
                if (result.Result && result.Data > 0)
                {
                    var OldMoney = Tools.Const.SystemConst.MoenyConvert(result.Data);
                    if (amount > result.Data) { return Json(new ResultModel<bool>(3, "提现金额不能大于先有可提现金额", true), JsonRequestBehavior.AllowGet); }

                    if (OldMoney < 100 && OldMoney != amount)
                    {
                        return Json(new ResultModel<bool>(3, "可用余额小于100时必须全部提现", true), JsonRequestBehavior.AllowGet);
                    }
                    if (OldMoney >= 100 && amount < 100)
                    {
                        return Json(new ResultModel<bool>(3, "提现金额不得小于100元，请重新输入", true), JsonRequestBehavior.AllowGet);
                    }

                    //改变账户金额给冻结住
                    var result_account = user.ApplicationWithdrawal(UserContext.simpleUserInfoModel.Fer_account, amount, Helper.generateOrderCode(Convert.ToInt64(UserContext.simpleUserInfoModel.Id)), reSmall.Data.BankCard, UserContext.simpleUserInfoModel.Id);
                    if (result_account.Result && result_account.Data) { return Json(new ResultModel<bool>(1, "申请成功", true), JsonRequestBehavior.AllowGet); }

                    return Json(new ResultModel<bool>(3, "申请失败", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "申请失败", false), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 融资方充值大接口（易宝支付接口）
        /// <summary>
        /// 融资方充值（易宝支付接口）
        /// </summary>
        /// <param name="Money">充值金钱</param>
        /// <returns></returns>
        public JsonResult UpdateUserAccountMoney(decimal Money)
        {
            if (!(Money > 0)) { return Json(new ResultModel<bool>(2, "请输入整额", false), JsonRequestBehavior.AllowGet); }
            ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
            if (reSmall.Result && reSmall.Data != null)
            {
                string orderId = Helper.generateOrderCode(reSmall.Data.AccountID);
                Fina_Recharge_Record_Model rmodelRecord = new Fina_Recharge_Record_Model();
                rmodelRecord.mer_ord_id = orderId;
                rmodelRecord.status = 0;
                rmodelRecord.repay_type = 0;
                rmodelRecord.amount = Money;
                rmodelRecord.bankcard = "";
                rmodelRecord.fer_id = UserContext.simpleUserInfoModel.Id;
                rmodelRecord.fer_account_id = UserContext.simpleUserInfoModel.Fer_account;
                rmodelRecord.pay_type = 0;
                ResultInfo<bool> resultRec = user.Addfina_recharge_record(rmodelRecord);
                //融资方域名
                string backurl = System.Configuration.ConfigurationManager.AppSettings["FinaStaticUrl"] + "/YeePayOnline/PayCallBack";
                if (resultRec.Result && resultRec.Data)
                {
                    //调用第三方充值
                    YxLica.Tools.Pay.yeepay_online.Bussiness bussiness = new YxLica.Tools.Pay.yeepay_online.Bussiness();
                    Money = 0.01m;

                    string url = bussiness.CreateRechargeUrl(orderId, Money, backurl);

                    LogHelper.Write("融资方充值", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + url + "金额" + Money);
                    return Json(new ResultModel<bool>(1, url, true), JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new ResultModel<bool>(2, "充值失败！", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "操作失败", true), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 连连网银支付接口
        /// <summary>
        /// 融资方充值（连连网银支付接口）
        /// </summary>
        /// <param name="Money">充值金钱</param>
        /// <returns></returns>
        public JsonResult UserAccountMoney(decimal Money)
        {
            if (!(Money > 0)) { return Json(new ResultModel<bool>(2, "请输入整额", false), JsonRequestBehavior.AllowGet); }
            ResultInfo<UserSmallReads> reSmall = user.GetUserSimall(UserContext.simpleUserInfoModel.Id);
            if (reSmall.Result && reSmall.Data != null)
            {
                string orderId = YxLica.Tools.Pay.LLWYPay.YinTongUtil.getCurrentDateTimeStr();
                Fina_Recharge_Record_Model rmodelRecord = new Fina_Recharge_Record_Model();
                rmodelRecord.mer_ord_id = orderId;
                rmodelRecord.status = 0;
                rmodelRecord.repay_type = 0;
                rmodelRecord.amount = Money;
                rmodelRecord.bankcard = "";
                rmodelRecord.fer_id = UserContext.simpleUserInfoModel.Id;
                rmodelRecord.fer_account_id = UserContext.simpleUserInfoModel.Fer_account;
                rmodelRecord.pay_type = 0;
                ResultInfo<bool> resultRec = user.Addfina_recharge_record(rmodelRecord);
                if (resultRec.Result && resultRec.Data)
                {
                    #region 连连网银支付
                    /*
                    //融资方域名  //同步回调地址
                    string backurll = System.Configuration.ConfigurationManager.AppSettings["FinaStaticUrl"] + "/LLPayCallBackOnline/PayCallBack";
                    //异步步回调地址
                    string Ybackurll = System.Configuration.ConfigurationManager.AppSettings["FinaStaticUrl"] + "/LLPayCallBackOnline/CallBack_Notify";
                    //调用第三方充值
                    YxLica.Tools.Pay.LLWYPay.LLWYPayBussiness bussiness = new YxLica.Tools.Pay.LLWYPay.LLWYPayBussiness();
                    string ip = Helper.GetIP();
                    Money = 0.01m;
                    string url = bussiness.CreateRechargeUrl_BY_LL(Money.ToString(), "融资方充值", UserContext.simpleUserInfoModel.Id.ToString(), ip, orderId, backurll, Ybackurll);
                    */
                    #endregion

//                    LogHelper.Write("融资方充值", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + url + "金额" + Money);
//                    return Json(new ResultModel<bool>(1, url, true), JsonRequestBehavior.AllowGet);
                    LogHelper.Write("融资方充值", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "金额" + Money + "、订单：" + orderId);
                    return Json(new ResultModel<bool>(1, orderId, true), JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new ResultModel<bool>(2, "充值失败！", true), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel<bool>(2, "操作失败", true), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 跳转连连中间页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_LL_Redirect()
        {
            string mer_ord_id = Request["mer_ord_id"].Trim();

            if (!string.IsNullOrEmpty(mer_ord_id))
            {
                ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = user.GetFina_Recharge_Record_ModeNE(long.Parse(mer_ord_id));
                if (rmodelRecord.Result && rmodelRecord.Data != null)
                {
                    LLPayOutModel outModel = new LLPayOutModel();
                    outModel.user_id = UserContext.simpleUserInfoModel.Id.ToString();
                    outModel.no_order = mer_ord_id;

                    outModel.dt_order = YxLica.Tools.Pay.LLWYPay.YinTongUtil.getCurrentDateTimeStr();
                    rmodelRecord.Data.amount = 0.01m;
                    outModel.money_order = Tools.Const.SystemConst.MoenyConvert(rmodelRecord.Data.amount).ToString();
                    outModel.notify_url = System.Configuration.ConfigurationManager.AppSettings["FinaStaticUrl"] + "/LLPayCallBackOnline/CallBack_Notify2";
                    outModel.url_return = System.Configuration.ConfigurationManager.AppSettings["FinaStaticUrl"] + "/LLPayCallBackOnline/PayCallBack";

                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("version", outModel.version);
                    sParaTemp.Add("oid_partner", outModel.oid_partner);
                    sParaTemp.Add("user_id", outModel.user_id);
                    sParaTemp.Add("sign_type", outModel.sign_type);
                    sParaTemp.Add("busi_partner", outModel.busi_partner);
                    sParaTemp.Add("no_order", outModel.no_order);
                    sParaTemp.Add("dt_order", outModel.dt_order);
                    sParaTemp.Add("name_goods", outModel.name_goods);
                    sParaTemp.Add("pay_type", outModel.pay_type);
                    //sParaTemp.Add("info_order", outModel.info_order);
                    sParaTemp.Add("money_order", outModel.money_order);
                    sParaTemp.Add("notify_url", outModel.notify_url);
                    sParaTemp.Add("url_return", outModel.url_return);
                    //sParaTemp.Add("userreq_ip", outModel.userreq_ip);
                    //sParaTemp.Add("url_order", outModel.url_order);
                    //sParaTemp.Add("valid_order", outModel.valid_order);
                    sParaTemp.Add("timestamp", outModel.timestamp);
                    //sParaTemp.Add("risk_item", outModel.createRiskItem());

                    string sign = YxLica.Tools.Pay.LLWYPay.YinTongUtil.addSign(sParaTemp, YxLica.Tools.Pay.LLWYPay.PartnerConfig.TRADER_PRI_KEY, YxLica.Tools.Pay.LLWYPay.PartnerConfig.MD5_KEY);

                    outModel.sign = sign;

                    return View(outModel);
                }

            }
            return RedirectToAction("index", "Financing");

        }

        #endregion

        #region 获取充值记录信息
        /// <summary>
        /// 获取充值记录信息
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetUserFinancigRechargeRecordList(UserFinancingQueryViewModel queryModel)
        {
            if (queryModel.recharge_timeStar != DateTime.MinValue && queryModel.recharge_timeEnd != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(queryModel.recharge_timeEnd, queryModel.recharge_timeStar);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            var result = user.GetFina_Recharge_Record_Model_List(UserContext.simpleUserInfoModel.Id, queryModel.recharge_moneyStar, queryModel.recharge_moneyEnd, queryModel.recharge_timeStar, queryModel.recharge_timeEnd, queryModel.State);

            if (result.Result && result.Data != null) { return Json(result.Data, JsonRequestBehavior.AllowGet); }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 身份证认证错误提示
        /// <summary>
        /// 根据key获取枚举值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        private string GetEnumValue(string key)
        {
            string name = Enum.Parse(typeof(ErrorCode), key).ToString();
            return name;
        }

        /// <summary>
        /// 错误值枚举
        /// </summary>
        public enum ErrorCode
        {
            姓名和身份证不一致 = 1,
            请到户籍所在地进行核实 = 2,
            条件项非法 = 3,
            请求正在处理中 = 4
        }
        #endregion

    }
}
#region 绑定银行卡返回数据返回信息类
/// <summary>
/// 发送绑定银行卡(成功时返回数据)
/// </summary>
public class Msssages
{
    public string codesender { get; set; }
    public string merchantaccount { get; set; }
    public string requestid { get; set; }
    public string sign { get; set; }
    public string smscode { get; set; }
}
/// <summary>
/// 发送绑定银行卡(错误时返回数据)
/// </summary>
public class Msssages_Error
{
    public string error_code { get; set; }
    public string error_msg { get; set; }
    public string sign { get; set; }
}
/// <summary>
/// 确认绑定卡成功
/// </summary>
public class MsssagesT
{
    public string merchantaccount { get; set; }
    public string requestid { get; set; }
    public string bankcode { get; set; }
    public string card_top { get; set; }
    public string card_last { get; set; }
    public string sign { get; set; }
}
/// <summary>
/// 确认绑定卡失败
/// </summary>
public class MsssagesT_Error
{
    public string error_code { get; set; }
    public string error_msg { get; set; }
    public string sign { get; set; }
}
#endregion