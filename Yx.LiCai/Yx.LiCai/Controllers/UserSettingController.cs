using System;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Tools;
using YxLiCai.Tools.Const;
using YxLiCai.Tools.SafeEncrypt;
using Newtonsoft.Json;
using YxLiCai.Model.UserVerification;
using System.Collections.Generic;
using YxLiCai.Tools.Util;
namespace Yx.LiCai.Controllers
{
    public class UserSettingController : BaseController
    {
        private UserInfoService user;
        private UserInvitationService userInvitationService;
        public ResultInfo<bool> result = null;
        private readonly string SendBoundBankCardCode = System.Configuration.ConfigurationManager.AppSettings["SendBoundBankCard"];
        //
        // GET: /Login/
        public UserSettingController()
        {
            user = new UserInfoService();
            userInvitationService = new UserInvitationService();
        }
        /// <summary>
        /// 个人设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var result = new UserMessageServer().GetUserMessageCount(UserContext.simpleUserInfoModel.Id);
            ViewBag.MessageCount = result.Result == true ? result.Data : 0; 
            return View();
        }
        public ActionResult LoginOut()
        {
            UserContext.SignOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 设置取现密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult fetch_pwd_setting()
        {
            int aa = Convert.ToInt32(UserContext.simpleUserInfoModel.IsJiaoYIPW);
            if (aa == 1)
            {
                return View("Index");
            }

            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
            else
            {
                ViewBag.returnUrl = string.Empty;
            }

            return View();
        }
        /// <summary>
        ///忘记交易密码密码第一步
        /// </summary>
        /// <returns></returns>
        public ActionResult forget_SPFist_pwd()
        {
            int aa = Convert.ToInt32(UserContext.simpleUserInfoModel.IsJiaoYIPW);
            if (aa == 0)
            {
                return View("Index");
            }

            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
            else
            {
                ViewBag.returnUrl = string.Empty;
            }
            return View();
        }
        /// <summary>
        ///忘记交易密码密码第二步
        /// </summary>
        /// <returns></returns>
        public ActionResult forget_SP_pwd()
        {
            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
            else
            {
                ViewBag.returnUrl = string.Empty;
            }
            return View();
        }
        /// <summary>
        /// 用户身份正认证
        /// </summary>
        /// <returns></returns>
        public ActionResult uc_setting_status()
        {
            int aa = Convert.ToInt32(UserContext.simpleUserInfoModel.IsRealCard);
            if (aa == 1)
            {
                return View("Index");
            }

            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
            else 
            {
                ViewBag.returnUrl = string.Empty;
            }

            return View();
        }
        /// <summary>
        /// 用户银行卡展示
        /// </summary>
        /// <returns></returns>
        public ActionResult uc_setting_bank()
        {
            string BankID = user.GetUserBankID(UserContext.simpleUserInfoModel.Id).Data;
            ViewBag.BankID = BankID;
            if (string.IsNullOrEmpty(BankID))
            {
                return View("Index");
            }
            return View();
        }

        /// <summary>
        /// 添加绑定银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult add_bank()
        {
            int aa = Convert.ToInt32(UserContext.simpleUserInfoModel.IsBindBank);
            int bb = Convert.ToInt32(UserContext.simpleUserInfoModel.IsRealCard);
            if (bb != 1 || aa == 1)
            {
                return View("Index");
            }

            string returnUrl = Request["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }
            else
            {
                ViewBag.returnUrl = string.Empty;
            }
            ViewBag.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
            ViewBag.MyRealName = UserContext.simpleUserInfoModel.MyRealName;

            return View();
        }
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <returns></returns>
        public ActionResult user_setting_pwd()
        {
            return View();
        }
        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <returns></returns>
        public ActionResult fetch_pwd_modify()
        {
            return View();
        }
        /// <summary>
        ///修改用户实名认证信息
        /// </summary>
        /// <param name="IDcard">身份证号</param>
        /// <param name="Username">用户名</param>
        /// <returns></returns>
        public JsonResult UpdateIsRealCard(string IDcard, string Username)
        {
            if (!ValidateHelper.IsIDNum(IDcard))
            {
                return Json(new ResultModel<bool>(5, "身份证格式不正确", true), JsonRequestBehavior.AllowGet);
            }

            #region 判断身份证是否已经认证过 By-张浩然 2015-6-24
            YxLiCai.Server.User.UserInfoService userInfoService = new UserInfoService();
            ResultInfo<bool> resultUserInfoService = userInfoService.IsRepeatUserByRealCard(IDcard);
            if (resultUserInfoService.Result && resultUserInfoService.Data)
            {
                return Json(new ResultModel<bool>(2, "该证件已在平台开户", false), JsonRequestBehavior.AllowGet);
            }

            #endregion

            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            string Phone = UserContext.simpleUserInfoModel.Account;
            if (Userid > 0)
            {
                /*在这里先进行实名认证接口*/
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();

                /*一天之内最多只能调用3次验证身份证接口*/
                string tempCode = string.Format("UpdateIsRealCard_{0}", Userid);
                var val = redis.Get<int>(tempCode);
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
                    redis.Add(tempCode, val, DateTime.Now.Date.AddDays(1));
                    string str = CheckIDResult.Data;
                    YxLiCai.Tools.LogHelper.Write("身份验证", "userid:" + Userid + "--返回结果:" + str);
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
                                    result = user.UpdateIsRealCard(Userid, IDcard, Username);
                                    if (result.Result)
                                    {
                                        if (result.Data)
                                        {

                                            ResultInfo<bool> resultUpdateUserAccount = user.UpdateUserAccount(Userid, Username);//user_account 更新数据

                                            string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, Userid);
                                            var userSmall = new SmallUserInfo();
                                            userSmall.Id = UserContext.simpleUserInfoModel.Id;
                                            userSmall.Account = UserContext.simpleUserInfoModel.Account;
                                            userSmall.IsBindBank = UserContext.simpleUserInfoModel.IsBindBank;
                                            userSmall.IsRealCard = 1;
                                            userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                                            userSmall.MyRealName = Username;
                                            userSmall.MyRealCard = IDcard;
                                            userSmall.IsJiaoYIPW = UserContext.simpleUserInfoModel.IsJiaoYIPW;
                                            userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode;
                                            userSmall.BankName = UserContext.simpleUserInfoModel.BankName;
                                            userSmall.LastBankNum = UserContext.simpleUserInfoModel.LastBankNum;
                                            redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));
                                            return Json(new ResultModel<bool>(1, "认证成功", true), JsonRequestBehavior.AllowGet);
                                        }
                                        else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", true), JsonRequestBehavior.AllowGet); }
                                    }
                                    else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                                }
                                else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                            }
                            else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                        }
                        else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                    }
                    else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
                }
                else { return Json(new ResultModel<bool>(2, "姓名或身份证号码输入错误，请重新输入", false), JsonRequestBehavior.AllowGet); }
            }
            else { return Json(new ResultModel<bool>(2, "用户没有登录", false), JsonRequestBehavior.AllowGet); }
        }
        /// <summary>
        ///绑定银行卡
        /// </summary>
        /// <param name="BankName">银行名称</param>
        /// <param name="BankCard">银行卡</param>
        /// <returns></returns>
        public JsonResult SendBoundBankCard(string BankName, string BankCard, string Phone, string Card,string RealName)
        {
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            UserBankCardModel model = new UserBankCardModel();
            if (Userid > 0)
            {
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                #region 防止用户多次刷新页面，重复发送验证码
                string tempCode = string.Format("usersendcode_{0}", Userid);
                var val = redis.Get<string>(tempCode);
                if (!string.IsNullOrEmpty(val))
                {
                    return Json(new ResultModel<bool>(4, "请不要重复发送", true), JsonRequestBehavior.AllowGet);
                }
                #endregion

                #region 生成商户的唯一绑卡请求号
                //商户生成的唯一绑卡请求号
                string requestid = YxLiCai.Tools.Util.Helper.generateOrderCode(Userid);
                string requestidkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeSendBoundBankCard, Userid);
                redis.Add(requestidkey, requestid, DateTime.Now.AddMinutes(5));
                #endregion

                YxLiCai.Tools.Pay.Yeepay.YJPay pay = new YxLiCai.Tools.Pay.Yeepay.YJPay(); 
          
                string ip = YxLiCai.Tools.Util.Helper.GetIP();
                //发起绑卡请求  
                YxLiCai.Tools.LogHelper.Write("绑卡请求", BankCard + ":" + Card + ":" + RealName + ":" + Phone);
                string str = pay.invokebindbankcard(Userid.ToString(), requestid, BankCard, Card, RealName, Phone, ip, 2);
                YxLiCai.Tools.LogHelper.Write("绑卡请求", "userid:" + Userid + "--返回结果:" + str);
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
                    YxLiCai.Tools.LogHelper.Write("绑卡失败", Userid + ":" + BankCard + ":" + UserContext.simpleUserInfoModel.MyRealCard + ":" + UserContext.simpleUserInfoModel.MyRealName + ":" + Phone + "-" + me.error_msg);
                    return Json(new ResultModel<bool>(2, "请检查所填信息是否真实有效,再获取验证码", false), JsonRequestBehavior.AllowGet);
                }
            }
            else { return Json(new ResultModel<bool>(2, "服务器连接失败", false), JsonRequestBehavior.AllowGet); }
        }
        /// <summary>
        ///确定绑定银行卡
        /// </summary>
        /// <param name="BankName">银行名称</param>
        /// <param name="BankCard">银行卡</param>
        /// <returns></returns>
        public JsonResult ConfimBoundBankCard(string BankName, string BankCard, string Phone, string validatecode,string Card,string RealName)
        {
            if (!ValidateHelper.IsIDNum(Card))
            {
                return Json(new ResultModel<bool>(5, "身份证格式不正确", true), JsonRequestBehavior.AllowGet);
            }

            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            UserBankCardModel model = new UserBankCardModel();
            if (Userid > 0)
            {
                YxLiCai.Tools.Pay.Yeepay.YJPay yy = new YxLiCai.Tools.Pay.Yeepay.YJPay();
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                #region 获取商户的唯一绑卡请求号
                string requestidkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeSendBoundBankCard, Userid);
                var requestid = redis.Get<string>(requestidkey);
                #endregion
                string ss = yy.confirmbindbankcard(requestid, validatecode);
                YxLiCai.Tools.LogHelper.Write("确认绑卡请求","Userid:"+Userid+";"+ requestid + ":" + validatecode + "r-" + ss);
                MsssagesT mt = YxLiCai.Tools.SerializeHelper.JsonDeserialize<MsssagesT>(ss);
                if (ss.IndexOf("error") < 0)
                {
                    if (mt != null)
                    {
                        model.BankCode = mt.bankcode;
                        model.FirstNum = mt.card_top;
                        model.LastNum = mt.card_last;
                        model.Requestid = mt.requestid;
                    }
                    model.UserId = Userid;
                    model.BankCard = BankCard;
                    model.BankName = BankName;
                    model.BankPhone = Phone;
                    model.Status = 1;
                    result = user.BoundBankCard(model);
                    if (result.Result)
                    {
                        #region 修改用户信息
                        result = user.UpdateIsRealCard(Userid, Card, RealName);
                        if (result.Result && result.Data)
                        {
                            string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel,
                                Userid);
                            var userSmall = new SmallUserInfo();
                            userSmall.Id = UserContext.simpleUserInfoModel.Id;
                            userSmall.Account = UserContext.simpleUserInfoModel.Account;
                            userSmall.IsBindBank = 1;
                            userSmall.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
                            userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                            userSmall.MyRealName = RealName;
                            userSmall.MyRealCard = Card;
                            userSmall.IsJiaoYIPW = UserContext.simpleUserInfoModel.IsJiaoYIPW;
                            userSmall.BankName = model.BankName;
                            userSmall.LastBankNum = model.LastNum;
                            userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode; 
                            redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));
                        }

                        #endregion
                        return Json(new ResultModel<bool>(1, "绑定成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else { return Json(new ResultModel<bool>(2, "绑定银行卡失败", false), JsonRequestBehavior.AllowGet); }
                }
                else
                {
                    MsssagesT_Error me = YxLiCai.Tools.SerializeHelper.JsonDeserialize<MsssagesT_Error>(ss);
                    YxLiCai.Tools.LogHelper.Write("易宝绑定银行卡","绑定银行卡失败，错误码为" + me.error_code);
                    return Json(new ResultModel<bool>(2, "请检查所填信息是否真实有效,再重新获取验证码", false), JsonRequestBehavior.AllowGet);
                }
            }
            else { return Json(new ResultModel<bool>(2, "服务器连接失败", false), JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        ///判断登录密码是否正确
        /// </summary>
        /// <param name="IDcard">身份证号</param>
        /// <param name="Username">用户名</param>
        /// <returns></returns>
        public JsonResult CheckUserOldPassWord(string PassWord)
        {
            string phone = UserContext.simpleUserInfoModel.Account;
            PassWord = MD5.MD5Convert(PassWord, 32);
            result = user.IsTruePassWord(phone, PassWord);
            if (result.Result)
            {
                if (result.Data)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else { return Json(false, JsonRequestBehavior.AllowGet); }
            }
            else { return Json(false, JsonRequestBehavior.AllowGet); }

        }

        /// <summary>
        /// 判断身份证是否正确
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public JsonResult CheckIDCardTrue(string IdCard)
        {

            string IDcard = UserContext.simpleUserInfoModel.MyRealCard;
            if (IdCard.Trim() == "")
            {
                return Json(new ResultModel<bool>(2, "请输入身份证", false), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (IdCard.ToLower() == IDcard.ToLower())
                {
                    return Json(new ResultModel<bool>(1, "身份证正确", true), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResultModel<bool>(2, "身份证号不正确", false), JsonRequestBehavior.AllowGet);
                }
            }

        }
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public JsonResult UpdateUserOldPassWord(string OldPassWord, string NewPassWord)
        {
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            string phone = UserContext.simpleUserInfoModel.Account;
            OldPassWord = MD5.MD5Convert(OldPassWord, 32);
            NewPassWord = MD5.MD5Convert(NewPassWord, 32);

            result = user.IsTruePassWord(phone, OldPassWord);//判断原登录密码是否正确
            if (result.Result && result.Data)
            {
                result = user.CheckPwdAndFetchPwd(NewPassWord, Userid);//判断原登录密码是否与登录密码相同
                if (result.Result && !result.Data)
                {
                    result = user.UpdateUserPassWord(Userid, NewPassWord);//修改登录密码
                    if (result.Result && result.Data)
                    {
                        string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, Userid);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                        var userSmall = new SmallUserInfo();
                        userSmall.Id = UserContext.simpleUserInfoModel.Id;
                        userSmall.Account = UserContext.simpleUserInfoModel.Account;
                        userSmall.IsBindBank = UserContext.simpleUserInfoModel.IsBindBank;
                        userSmall.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
                        userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                        userSmall.MyRealName = UserContext.simpleUserInfoModel.MyRealName;
                        userSmall.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
                        userSmall.IsJiaoYIPW = 1;
                        userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode;
                        userSmall.BankName = UserContext.simpleUserInfoModel.BankName;
                        userSmall.LastBankNum = UserContext.simpleUserInfoModel.LastBankNum;
                        redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));//更新缓存

                        return Json(new ResultModel<bool>(0, "登录密码设置成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(1, "登录密码设置失败", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(2, "登录密码不可与交易密码相同，请重新输入", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(3, "原登录密码输入错误", false), JsonRequestBehavior.AllowGet);
            } 
        }
        /// <summary>
        ///判断取现密码密码是否正确
        /// </summary>
        /// <param name="IDcard">身份证号</param>
        /// <param name="Username">用户名</param>
        /// <returns></returns>
        public JsonResult CheckUserSallpassword(string PassWord)
        {
            string phone = UserContext.simpleUserInfoModel.Account;
            PassWord = MD5.MD5Convert(PassWord, 32);
            result = user.IsTrueSallpassword(phone, PassWord);
            if (result.Result)
            {
                if (result.Data)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else { return Json(false, JsonRequestBehavior.AllowGet); }
            }
            else { return Json(false, JsonRequestBehavior.AllowGet); }

        }

        /// <summary>
        /// 设置交易密码
        /// </summary>
        /// <param name="PassWord">交易密码</param>
        /// <returns></returns>
        public JsonResult SetUserSallpassword(string PassWord)
        {
            if (!string.IsNullOrEmpty(PassWord))
            {
                if (ValidateHelper.IsFetchPwd(PassWord))
                {
                    Int64 Userid = UserContext.simpleUserInfoModel.Id;
                    string phone = UserContext.simpleUserInfoModel.Account;
                    PassWord = MD5.MD5Convert(PassWord, 32);
                    result = user.CheckFetchPwdAndPwd(PassWord, Userid);//判断原交易密码是否与登录密码相同
                    if (result.Result && !result.Data)
                    {

                        result = user.UpdateUserSallpassword(Userid, PassWord);//修改交易密码
                        if (result.Result && result.Data)
                        {

                            string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, Userid);
                            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                            var userSmall = new SmallUserInfo();
                            userSmall.Id = UserContext.simpleUserInfoModel.Id;
                            userSmall.Account = UserContext.simpleUserInfoModel.Account;
                            userSmall.IsBindBank = UserContext.simpleUserInfoModel.IsBindBank;
                            userSmall.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
                            userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                            userSmall.MyRealName = UserContext.simpleUserInfoModel.MyRealName;
                            userSmall.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
                            userSmall.IsJiaoYIPW = 1;
                            userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode;
                            userSmall.BankName = UserContext.simpleUserInfoModel.BankName;
                            userSmall.LastBankNum = UserContext.simpleUserInfoModel.LastBankNum;
                            redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));

                            return Json(new ResultModel<bool>(0, "交易密码设置成功", true), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new ResultModel<bool>(1, "交易密码设置失败", false), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(2, "交易密码不可与登录密码相同，请重新输入", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(3, "交易密码格式输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(4, "交易密码不能为空", false), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="OldPassWord">旧交易密码</param>
        /// <param name="NewPassWord">新交易密码</param>
        /// <returns></returns>
        public JsonResult UpdateUserSallpassword(string OldPassWord, string NewPassWord)
        {
            if (string.IsNullOrEmpty(OldPassWord))
            {
                return Json(new ResultModel<bool>(4, "原交易密码不能为空", false), JsonRequestBehavior.AllowGet);
            }

            if (!ValidateHelper.IsFetchPwd(OldPassWord))
            {
                return Json(new ResultModel<bool>(5, "原交易密码输入格式错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(NewPassWord))
            {
                return Json(new ResultModel<bool>(6, "交易密码不能为空", false), JsonRequestBehavior.AllowGet);
            }

            if (!ValidateHelper.IsFetchPwd(NewPassWord))
            {
                return Json(new ResultModel<bool>(7, "交易密码输入格式错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            string phone = UserContext.simpleUserInfoModel.Account;
            OldPassWord = MD5.MD5Convert(OldPassWord, 32);
            NewPassWord = MD5.MD5Convert(NewPassWord, 32);

            result = user.IsTrueSallpassword(phone, OldPassWord);//判断原交易密码是否正确
            if (result.Result && result.Data)
            {
                result = user.CheckFetchPwdAndPwd(NewPassWord, Userid);//判断原交易密码是否与登录密码相同
                if (result.Result && !result.Data)
                {
                    result = user.UpdateUserSallpassword(Userid, NewPassWord);//修改交易密码
                    if (result.Result && result.Data)
                    {

                        string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, Userid);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                        var userSmall = new SmallUserInfo();
                        userSmall.Id = UserContext.simpleUserInfoModel.Id;
                        userSmall.Account = UserContext.simpleUserInfoModel.Account;
                        userSmall.IsBindBank = UserContext.simpleUserInfoModel.IsBindBank;
                        userSmall.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
                        userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                        userSmall.MyRealName = UserContext.simpleUserInfoModel.MyRealName;
                        userSmall.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
                        userSmall.IsJiaoYIPW = 1;
                        userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode;
                        userSmall.BankName = UserContext.simpleUserInfoModel.BankName;
                        userSmall.LastBankNum = UserContext.simpleUserInfoModel.LastBankNum;
                        redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));

                        return Json(new ResultModel<bool>(0, "交易密码设置成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(1, "交易密码设置失败", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(2, "交易密码不可与登录密码相同，请重新输入", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(3, "原交易密码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 忘记交易密码 By张浩然 2015-7-7
        /// </summary>
        /// <param name="passWord">交易密码</param>
        /// <param name="phoneCode">验证码</param>
        /// <returns></returns>
        public JsonResult ForgetUserSallpassword(string passWord, string phoneCode)
        {
            if (string.IsNullOrEmpty(phoneCode))
            {
                return Json(new ResultModel<bool>(3, "验证码不能为空", false), JsonRequestBehavior.AllowGet);
            }

            if (!ValidateHelper.IsCaptcha(phoneCode))
            {
                return Json(new ResultModel<bool>(2, "验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(passWord))
            {
                return Json(new ResultModel<bool>(4, "交易密码不能为空", false), JsonRequestBehavior.AllowGet);
            }

            if (!ValidateHelper.IsFetchPwd(passWord))
            {
                return Json(new ResultModel<bool>(5, "交易密码格式错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            string phone = UserContext.simpleUserInfoModel.Account;
            passWord = MD5.MD5Convert(passWord, 32);

            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();

            string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeYIxiu, phone);
            var captcha = redis.Get<string>(cachekey);
            if (!string.IsNullOrEmpty(captcha))
            {
                if (phoneCode.ToLower() == captcha.ToLower())//判断验证码是否正确
                {
                    result = user.UpdateUserSallpassword(Userid, passWord);//修改交易密码
                    if (result.Result && result.Data)
                    {

                        string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, Userid);
                        var userSmall = new SmallUserInfo();
                        userSmall.Id = UserContext.simpleUserInfoModel.Id;
                        userSmall.Account = UserContext.simpleUserInfoModel.Account;
                        userSmall.IsBindBank = UserContext.simpleUserInfoModel.IsBindBank;
                        userSmall.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
                        userSmall.MyCode = UserContext.simpleUserInfoModel.MyCode;
                        userSmall.MyRealName = UserContext.simpleUserInfoModel.MyRealName;
                        userSmall.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
                        userSmall.IsJiaoYIPW = 1;
                        userSmall.BankCode = UserContext.simpleUserInfoModel.BankCode;
                        userSmall.BankName = UserContext.simpleUserInfoModel.BankName;
                        userSmall.LastBankNum = UserContext.simpleUserInfoModel.LastBankNum;
                        redis.Replace(Userkey, userSmall, DateTime.Now.AddDays(5));

                        return Json(new ResultModel<bool>(0, "交易密码设置成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(1, "交易密码设置失败", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(2, "验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(6, "验证码已过期，请重新获取", false), JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 金钱包页面
        /// </summary>
        /// <returns></returns>
        public ActionResult get_money()
        {
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            int count = 0;
            var result = user.GetUserAllSpecialAssets(Userid, 1, 11, ref count);
            var viewModel = new List<UserSpecialAssetsModel>();
            if (result.Result)
            {
                viewModel = result.Data;
            }
            //特享金总额
            var result1 = user.GetUserAllSpecialAssets(Userid);

            if (result1.Result && result1.Data != 0)
            {
                ViewBag.ALlSpecialAssets = YxLiCai.Tools.Const.SystemConst.MoenyConvertString(result1.Data);
            }
            else
            {
                ViewBag.ALlSpecialAssets = 0;
            }

            //ViewBag.ALlSpecialAssets = result1.Result == true ? YxLiCai.Tools.Const.SystemConst.MoenyConvertString(result1.Data) : "0";
            //加息券总张数 
            var result2 = user.GetUserAllAddInterestCount(Userid);
            ViewBag.AddInterestCount = result2.Result == true ? result2.Data : 0;
            ViewBag.AddInterest = YxLiCai.Tools.Const.SystemConst.RateConvert(user.GetUserAddInterest(Userid).Data * 100);
            return View(viewModel);
        }
        /// <summary>
        /// 加息券页面
        /// </summary>
        /// <returns></returns>
        public ActionResult uc_setting_ticket()
        {
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            var result = new UserInfoService().GetRateCoupons(Userid);
            return View(result);
        }
        /// <summary>
        /// 用户消息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult uc_message()
        {
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            int count = 0;
            var result = new UserMessageServer().GetUserMessage(Userid, 1, 10, ref count);
            ViewBag.PageCount = count;
            List<UserMessageModel> viewModel = new List<UserMessageModel>();
            if (result.Result)
            {
                viewModel = result.Data;
            }
            return View(viewModel);
        }
        /// <summary>
        /// 用户消息页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult get_message(int page)
        {
            long id = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id;
            int count = 0;
            var result = new UserMessageServer().GetUserMessage(id, page, 10, ref count);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (result.Result && result.Data != null)
            {
                foreach (var list_item in result.Data)
                {
                    sb.AppendFormat("<li class=\"J_dope hover\"><h2 id=\"{0}\">", list_item.id);
                    sb.AppendFormat("<b>{0}</b>", list_item.title);
                    sb.AppendFormat("<span><i>{0}</i></span>", list_item.sendtime);
                    sb.AppendFormat("<p>{0}</p></h2>", list_item.content);
                    sb.AppendFormat("<h3><i class=\"{0}\"></i></h3></li>", list_item.isread == 1 ? "" : "hover");
                }
            }
            int IsLastPage = 0;
            //检查是否最后一页
            if (page * 10 >= count)
            {
                IsLastPage = 1;
            }
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });

        }

        /// <summary>
        /// 特享收益规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleIncome()
        {
            return View();
        }

        /// <summary>
        /// 用户消息页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult read_message(int id)
        {
            long uid = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id;
            var result = new UserMessageServer().UpdateUserMessage(uid, id);
            return Json(new
            {
                result = true
            });
        }

        /// <summary>
        /// 异步获取未读消息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMessageCount()
        {
            var result = new UserMessageServer().GetUserMessageCount(UserContext.simpleUserInfoModel.Id);
            if (result.Result && result.Data != 0)
            {
                return Json(new ResultModel<bool>(result.Data, "获取未读消息成功", false), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel<bool>(0, "获取未读消息失败", false), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult user_agreement()
        {
            return View();
        }

    }
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
}