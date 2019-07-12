using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Tools;
using YxLiCai.Tools.Const;
using YxLiCai.Tools.SafeEncrypt;
using YxLiCai.Tools.Util;
using YxLiCai.Server.Act;
using YxLiCai.Model.ActivityManage;
using YxLiCai.Server.ActivityManege;


namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// 登录页面
    /// </summary>
    public class LoginController : Controller
    {
        private UserInfoService user;
        private ResultInfo<bool> result = null;
        private ActService actservice;//活动
        private ActivityManegeService activityManegeService;//活动管理 
        private UserInvitationService userInvitationService;//邀请记录
        private UserAddInterestService userAddInterestService;//加息券
        private UserSpecialAssetsService userSpecialAssetsService;//特享金
        //
        // GET: /Login/
        public LoginController()
        {
            user = new UserInfoService();
            actservice = new ActService();
            activityManegeService = new ActivityManegeService();
            userInvitationService = new UserInvitationService();
            userAddInterestService = new UserAddInterestService();
            userSpecialAssetsService = new UserSpecialAssetsService();
        }
        /// <summary>
        /// 注册登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisteredOrLogin()
        {
            ViewBag.inviteCode = string.Empty;
            string inviteCode = Request["inviteCode"];
            if (!string.IsNullOrEmpty(inviteCode))
            {
                ViewBag.inviteCode = inviteCode;
            }
            return View();
        }
        /// <summary>
        /// 忘记密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult forget_pwd(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return View("RegisteredOrLogin");
            }
            //解密电话号码
            phone = SystemConst.ConvertPhoneNum2(phone);

            if (!ValidateHelper.IsPhoneNum(phone))
            {
                return View("RegisteredOrLogin");
            }
            ViewBag.Phone = phone;
            return View();
        }

        /// <summary>
        /// 重新设置密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult setting_pwd(string phone, string Code)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(Code) || !ValidateHelper.IsCaptcha(Code))
            {
                return View("RegisteredOrLogin");
            }
            //解密电话号码
            phone = SystemConst.ConvertPhoneNum2(phone);
            if (!ValidateHelper.IsPhoneNum(phone))
            {
                return View("RegisteredOrLogin");
            }
            ViewBag.Phone = phone;
            ViewBag.Code = Code;
            return View();
        }
        /// <summary>
        /// 用户查看协议
        /// </summary>
        /// <returns></returns>
        public ActionResult uc_look()
        {
            return View();
        }
        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Registered(string phone)
        {
            ViewBag.inviteCode = string.Empty;
            string inviteCode = Request["inviteCode"];
            if (!string.IsNullOrEmpty(inviteCode))
            {
                ViewBag.inviteCode = inviteCode;
            }

            if (string.IsNullOrEmpty(phone))
            {
                return View("RegisteredOrLogin");
            }
            //解密电话号码
            phone = SystemConst.ConvertPhoneNum2(phone);

            if (!ValidateHelper.IsPhoneNum(phone))
            {
                return View("RegisteredOrLogin");
            }
            ViewBag.Phone = phone;
            return View();
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string phone)
        {

            if (string.IsNullOrEmpty(phone))
            {
                return View("RegisteredOrLogin");
            }
            //解密电话号码
            phone = SystemConst.ConvertPhoneNum2(phone);

            if (!ValidateHelper.IsPhoneNum(phone))
            {
                return View("RegisteredOrLogin");
            }
            ViewBag.Phone = phone;
            return View();
        }
        /// <summary>
        ///判断用户是否是会员
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public JsonResult CheckUserIsMember(string phone)
        {
            if (!string.IsNullOrEmpty(phone) && ValidateHelper.IsPhoneNum(phone))
            {
                result = user.IsBlackUser(phone);//判断是不是黑名单用户

                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(1, "账户被锁定，如有疑问请致电：400-800-8000", false), JsonRequestBehavior.AllowGet);
                }
                result = user.IsRepeatUser(phone);
                //加密电话号码
                string Phone = SystemConst.ConvertPhoneNum(phone);
                if (result.Result)//代表程序执行成功与否
                {
                    if (result.Data)//代表是否注册用户
                    {
                        return Json(new ResultModel<bool>(0, Phone, false), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(2, Phone, false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(3, "网络异常", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(3, "没有查到用户或手机号码不正确", false), JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        ///判断用户验手机证码是否正确
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public JsonResult CheckUserCode(string phone, string code)
        {
            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(code) && ValidateHelper.IsCaptcha(code))
            {
                //存放用户手机验证码的readis
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                //电话解密
                phone = SystemConst.ConvertPhoneNum2(phone);
                if (!ValidateHelper.IsPhoneNum(phone))
                {
                    return Json(new ResultModel<bool>(0, "手机格式错误，请重新输入", false), JsonRequestBehavior.AllowGet);
                }

                string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeYIxiu, phone);
                var captcha = redis.Get<string>(cachekey);
                //加密电话号码
                string JMPhone = SystemConst.ConvertPhoneNum(phone);
                if (string.IsNullOrEmpty(captcha))
                {
                    return Json(new ResultModel<bool>(0, "验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (!string.IsNullOrEmpty(captcha.ToLower()))
                    {
                        if (code.ToLower() == captcha.ToLower())
                        {
                            string phoneKey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.Phone, phone);//给电话号码写入缓存
                            redis.Add(phoneKey, phone, DateTime.Now.AddMinutes(10));

                            return Json(new ResultModel<bool>(1, JMPhone, false), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new ResultModel<bool>(0, "验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(0, "验证码已过期，请重新获取", false), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new ResultModel<bool>(0, "手机号码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        public bool SendValidateCode(string account)
        {

            if (string.IsNullOrEmpty(account))
            {
                return false;
            }
            //电话进行解密
            account = SystemConst.ConvertPhoneNum2(account);
            string ip = YxLiCai.Tools.Util.Helper.GetIP();
            if (!string.IsNullOrEmpty(account) && YxLiCai.Tools.Util.ValidateHelper.IsPhoneNum(account))
            {
                if (!string.IsNullOrEmpty(account))
                {
                    //验证码插入Redis缓存
                    var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                    string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeYIxiu, account);
                    var Codee = redis.Get<string>(cachekey);
                    YxLiCai.Server.SendMsg.Send smsService = new YxLiCai.Server.SendMsg.Send();
                    if (string.IsNullOrEmpty(Codee))
                    {
                        //获取四位的随机验证码
                        string code = Validate_Code.GetRandomCode(6);
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("1");
                        content = string.Format(content, code);
                        //发送短信 
                        ResultInfo<string> sendResult = YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(account, content);
                        if (sendResult.Result)
                        {
                            redis.Add(cachekey, code, DateTime.Now.AddMinutes(1));
                            return true;
                        }
                    }

                }
            }
            return false;
        }
        /// <summary>
        /// （登录） by张浩然  2015-7-7
        /// </summary>
        /// <param name="phone">登录名</param>
        /// <param name="password">密码</param>
        /// <param name="RandomCode">验证码</param>
        /// <returns></returns>
        public JsonResult CheckLogin(string phone, string password, string RandomCode)
        {

            if (string.IsNullOrEmpty(phone))
            {
                return Json(new ResultModel<bool>(4, "手机号不能为空", false), JsonRequestBehavior.AllowGet);
            }
            //电话进行解密
            phone = SystemConst.ConvertPhoneNum2(phone);
            if (!ValidateHelper.IsPhoneNum(phone))
            {
                return Json(new ResultModel<bool>(5, "手机号不能为空输入格式错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(RandomCode))
            {
                return Json(new ResultModel<bool>(6, "验证码不能为空", false), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json(new ResultModel<bool>(6, "密码不能为空", false), JsonRequestBehavior.AllowGet);
            }
            if (!ValidateHelper.IsPwd(password))
            {
                return Json(new ResultModel<bool>(7, "密码输入格式错误，请修改", false), JsonRequestBehavior.AllowGet);
            }

            password = MD5.MD5Convert(password, 32);
            //strat
            //把用户对象放到Redas  
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
            string cachekey = CookieHelper.ReadCookie("Cookie_YXVerificationLogin");
            var captcha = redis.Get<string>(cachekey);

            if (!string.IsNullOrEmpty(captcha))
            {
                if (RandomCode.ToLower() == captcha.ToLower())//判断验证码是否正确
                {
                    result = user.IsTruePassWord(phone, password);
                    if (result.Result && result.Data)
                    {
                        //得到一个用户的实体信息
                        var usermodel = user.GetUserModelByPhone(phone);
                        if (usermodel.Result && usermodel.Data != null)
                        {
                            user.UpdateUserLoginTimes(usermodel.Data.id);//更新用户登录次数和登录时间
                            SmallUserInfo simUser = new SmallUserInfo();
                            simUser.Id = usermodel.Data.id;
                            simUser.Account = usermodel.Data.Phone;
                            simUser.IsBindBank = usermodel.Data.IsBindBank;
                            simUser.IsJiaoYIPW = 0;
                            if (!(string.IsNullOrEmpty(usermodel.Data.Sallpassword) || usermodel.Data.Sallpassword == ""))
                            {
                                simUser.IsJiaoYIPW = 1;
                            }
                            simUser.IsRealCard = usermodel.Data.IsRealCard;
                            simUser.MyCode = usermodel.Data.MyCode;
                            simUser.MyRealName = usermodel.Data.MyRealName;
                            simUser.MyRealCard = usermodel.Data.MyRealCard;
                            simUser.BankName = usermodel.Data.BankName;
                            simUser.LastBankNum = usermodel.Data.LastBankNum;
                            simUser.BankCode = usermodel.Data.BankCode;
                            //strat
                            //把用户对象放到Redas                    
                            string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, usermodel.Data.id);
                            var SmallUserInfo = redis.Get<SmallUserInfo>(Userkey);
                            if (SmallUserInfo != null)
                            {
                                redis.Replace(Userkey, simUser, DateTime.Now.AddDays(5));
                            }
                            else
                            {
                                redis.Add(Userkey, simUser, DateTime.Now.AddDays(5));
                            }
                            string json = YxLiCai.Tools.SafeEncrypt.DES.DesEncrypt(usermodel.Data.id.ToString(), SystemConst.encrypt_des);
                            UserContext.SignIn(json);

                            return Json(new ResultModel<bool>(0, "登录成功", false), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new ResultModel<bool>(1, "登录密码错误，请修改", false), JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new ResultModel<bool>(1, "登录密码错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResultModel<bool>(2, "验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(3, "验证码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 注册会员 by张浩然  2015-7-7
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="Phone">手机</param>
        /// <param name="InviteCode">邀请码</param>
        /// <param name="RandomCode">验证码</param>
        /// <returns></returns>
        public JsonResult RegisterUser(string password, string Phone, string InviteCode, string RandomCode)
        {
            //判断验证码是否为空或者不是验证码、密码唯恐或者不是密码、手机为空或者不是手机
            if (string.IsNullOrEmpty(RandomCode) || !ValidateHelper.IsCaptcha(RandomCode) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(password) || !ValidateHelper.IsPwd(password))
            {
                return Json(new ResultModel<bool>(5, "信息填写不完整", false), JsonRequestBehavior.AllowGet);
            }
            //电话进行解密
            Phone = SystemConst.ConvertPhoneNum2(Phone);
            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(new ResultModel<bool>(5, "手机号格式错误", false), JsonRequestBehavior.AllowGet);
            }
            //存放手机验证码redis
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
            string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeYIxiu, Phone);
            var captcha = redis.Get<string>(cachekey);

            if (!string.IsNullOrEmpty(captcha))
            {
                if (RandomCode.ToLower() == captcha.ToLower())//判断验证码是否正确
                {
                    result = user.IsRepeatUser(Phone);
                    if (result.Result && !result.Data)
                    {

                        password = MD5.MD5Convert(password, 32);
                        UserInfoModel model = new UserInfoModel();
                        model.Password = password;
                        model.RegTime = DateTime.Now;
                        model.Phone = Phone;
                        model.LoginName = Phone;
                        model.Status = 1;
                        model.MyCode = MD5.MD5Convert(Phone, 16);
                        model.Sallpassword = "";
                        model.IP = YxLiCai.Tools.Util.Helper.GetIP();
                        ResultInfo<int> resultinsert = user.AddUser(model);
                        if (resultinsert.Result && resultinsert.Data != 0)
                        {
                            //strat
                            //把用户对象放到Redas                    
                            string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, resultinsert.Data);
                            var UserReads = new SmallUserInfo
                            {
                                Id = resultinsert.Data,
                                Account = Phone,
                                MyCode = MD5.MD5Convert(Phone, 16),
                                MyRealCard = "",
                                IsRealCard = 0,
                                IsBindBank = 0,
                                IsJiaoYIPW = 0,
                                MyRealName = "",
                                 BankCode="",
                                   BankName="",
                                    LastBankNum="" 
                            };
                            redis.Add(Userkey, UserReads, DateTime.Now.AddDays(5));
                            //end

                            string json = YxLiCai.Tools.SafeEncrypt.DES.DesEncrypt(resultinsert.Data.ToString(), SystemConst.encrypt_des);
                            UserContext.SignIn(json);
                            //初始化操作
                            user.AddUserInitialization(resultinsert.Data, Phone);

                            user.UpdateUserLoginTimes(UserReads.Id);//更新用户登录次数和登录时间

                            Act(resultinsert.Data, InviteCode, Phone);

                            return Json(new ResultModel<bool>(0, "注册成功", false), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new ResultModel<bool>(1, "注册失败，请稍后再试", false), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(2, "您已注册过帐户，请重新登录", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(3, "短信验证码输入错误，请修改", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(4, "短信验证码已过期，请重新获取", false), JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        ///忘记登录密码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public JsonResult UpdateUserPassWord(string Code, string password, string Phone)
        {
            //判断验证码是否为空或者不是验证码、密码唯恐或者不是密码、手机为空或者不是手机
            if (string.IsNullOrEmpty(Code) || !ValidateHelper.IsCaptcha(Code) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(password) || !ValidateHelper.IsPwd(password))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            //电话进行解密
            Phone = SystemConst.ConvertPhoneNum2(Phone);
            if (!ValidateHelper.IsPhoneNum(Phone))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            password = MD5.MD5Convert(password, 32);
            Int64 Userid = UserContext.simpleUserInfoModel.Id;
            if (Userid <= 0)
            {
                result = user.CheckPwdAndFetchPwdByPhone(password, Phone);//判断原登录密码是否与登录密码相同
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(2, "登录密码不可与交易密码相同，请重新输入", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                result = user.CheckPwdAndFetchPwd(password, Userid);//判断原登录密码是否与登录密码相同
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel<bool>(2, "登录密码不可与交易密码相同，请重新输入", false), JsonRequestBehavior.AllowGet);
                }
            }
            //修改用户密码是发送手机时存放的验证码
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
            //用户电话号码缓存
            string phoneKey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.Phone, Phone);
            //用户手机验证码key
            string cachekey = string.Format(RedisCacheKey.CaptchaCodeYIxiu, Phone);
            var redisCode = redis.Get<string>(cachekey);

            var captcha = redis.Get<string>(phoneKey);

            if (captcha != null && captcha == Phone && redisCode != null && Code == redisCode)
            {
                if (!string.IsNullOrEmpty(Phone) && YxLiCai.Tools.Util.ValidateHelper.IsPhoneNum(Phone))
                {
                    result = user.ForgetUserPassWord(Phone, password);
                    if (result.Result && result.Result)
                    {
                        return Json(new ResultModel<bool>(0, "修改登录密码成功", true), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(1, "修改登录密码失败", false), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultModel<bool>(1, "修改登录密码失败", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new ResultModel<bool>(1, "修改登录密码失败", false), JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// 生成登录验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult CaptchaImage()
        {
            var captcha = new LiteralCaptcha(140, 46, 4);
            var bytes = captcha.Generate();
            string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeYIxiuUserCode, YxLiCai.Tools.Util.Helper.Uuid());
            try
            {
                //验证码插入Redis缓存 
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                redis.Add(cachekey, captcha.Captcha, DateTime.Now.AddHours(2));
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            //缓存key放入cookie里存储 
            CookieHelper.WriteCookie("Cookie_YXVerificationLogin", cachekey, DateTime.Now.AddMinutes(10));
            return new FileContentResult(bytes, "image/jpeg"); ;
        }
        /// <summary>
        /// 获取后几位数
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }


        /// <summary> 
        /// 活动方法  by张浩然 2015-7-1 
        /// </summary>
        /// <param name="invited_user_id">注册人id</param>
        /// <param name="inviteCode">邀请人</param>
        /// <param name="phone">`</param>
        public void Act(int invited_user_id, string inviteCode, string phone)
        {
            #region 注册
            ResultInfo<int> resultAct_R = actservice.IsAct(0);//判断是否存在注册活动 
            if (resultAct_R.Result && resultAct_R.Data != 0)
            {
                #region 加息券---注册
                ResultInfo<ActPromotionModelView> resultActRegCouponList = actservice.ActInfoByTypeAndItemType(0, 2);//获取注册活动里面的加息券活动信息(0注册2加息券)
                if (resultActRegCouponList.Result && resultActRegCouponList.Data != null)
                {
                    ResultInfo<ActInterestCoupon> resultAct_Coupon_R = actservice.GetCouponByItemId(resultActRegCouponList.Data.item_id);
                    if (resultAct_Coupon_R.Result && resultAct_Coupon_R.Data != null)
                    {
                        //插入用户加息券
                        UserAddInterestModel userAddInterestModel = new UserAddInterestModel();
                        userAddInterestModel.user_id = invited_user_id;
                        userAddInterestModel.act_id = resultActRegCouponList.Data.id;//活动编号
                        userAddInterestModel.interest_id = resultAct_Coupon_R.Data.id;
                        userAddInterestModel.interest_rate = resultAct_Coupon_R.Data.interest_rate;
                        userAddInterestModel.enable_day = resultAct_Coupon_R.Data.enable_day;
                        userAddInterestModel.c_time = DateTime.Now.Date;
                        userAddInterestModel.e_time = DateTime.Now.AddDays(resultAct_Coupon_R.Data.enable_day-1).Date;
                        userAddInterestModel.use_status = 0;
                        userAddInterestModel.invest_id = 0;
                        userAddInterestModel.version = 0;
                        userAddInterestModel.remark = "插入一条用户加息券（注册）";
                        userAddInterestModel.creator_id = 0;
                        userAddInterestModel.m_time = DateTime.Now.Date;
                        ResultInfo<bool> resultAddCoupon = userAddInterestService.AddUserAddInterest(userAddInterestModel);

                        #region 发短信
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("5");
                        string mobile = UserContext.simpleUserInfoModel.Account;
                        YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);
                        #endregion

                        #region 发送消息
                        YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                        mod.user_id = UserContext.simpleUserInfoModel.Id;
                        mod.isread = 0;
                        mod.sendtime = DateTime.Now;
                        mod.title = "加息券";
                        mod.content = string.Format("将军大人，为庆祝您的成功注册特献上<i>{0}</i>%加息券，使用后可获得更多收益，不要让它白白流失哦，快快买入新产品吧！", YxLiCai.Tools.Const.SystemConst.RateConvert(resultAct_Coupon_R.Data.interest_rate * 100));
                        new UserMessageServer().AddUserMessage(mod);
                        #endregion

                        //更新加息券发放数量
                        ResultInfo<bool> resultUpdateActCouponSendCount = activityManegeService.UpdateActCouponSendCount(resultAct_Coupon_R.Data.id);

                        #region 添加一条用户活动日志
                        ActUserLog actUserLog = new ActUserLog();
                        actUserLog.user_id = invited_user_id;
                        actUserLog.act_id = resultActRegCouponList.Data.id;//活动编号
                        actUserLog.item_id = resultActRegCouponList.Data.item_id;
                        actUserLog.c_time = DateTime.Now;
                        actUserLog.creator_id = 0;
                        actUserLog.version = 0;
                        actUserLog.remark = "用户注册发放加息券";

                        ResultInfo<bool> resultActUserLog_RegCoupn = actservice.AddActUserLog(actUserLog);
                        #endregion
                    }
                }
                #endregion

                #region 特享金---注册
                ResultInfo<ActPromotionModelView> resultActRegAssetsList = actservice.ActInfoByTypeAndItemType(0, 1);//获取注册活动里面的加息券活动信息(0注册1特享金)
                if (resultActRegAssetsList.Result && resultActRegAssetsList.Data != null)
                {

                    ResultInfo<ActSpecialAssets> resultAct_SpecialAssets_R = actservice.GetAssetsByItemId(resultActRegAssetsList.Data.item_id);
                    if (resultAct_SpecialAssets_R.Result && resultAct_SpecialAssets_R.Data != null)
                    {
                        //插入用户特享金
                        UserSpecialAssetsModel userSpecialAssetsModel = new UserSpecialAssetsModel();
                        userSpecialAssetsModel.user_id = invited_user_id;
                        userSpecialAssetsModel.act_id = resultActRegAssetsList.Data.id;
                        userSpecialAssetsModel.special_id = resultAct_SpecialAssets_R.Data.id;
                        userSpecialAssetsModel.amount = resultAct_SpecialAssets_R.Data.amount;
                        userSpecialAssetsModel.enable_day = resultAct_SpecialAssets_R.Data.enable_day;
                        userSpecialAssetsModel.c_time = DateTime.Now.Date;
                        userSpecialAssetsModel.u_time = DateTime.Now;
                        userSpecialAssetsModel.e_time = DateTime.Now.AddDays(resultAct_SpecialAssets_R.Data.enable_day-1).Date;
                        userSpecialAssetsModel.use_status = 1;
                        userSpecialAssetsModel.rate = resultAct_SpecialAssets_R.Data.rate;
                        userSpecialAssetsModel.rate_increase = resultAct_SpecialAssets_R.Data.rate_increase;
                        userSpecialAssetsModel.interest_added = 0.00m;
                        userSpecialAssetsModel.interest_paid = 0.00m;
                        userSpecialAssetsModel.m_time = DateTime.Now.Date;
                        userSpecialAssetsModel.creator_id = 0;
                        userSpecialAssetsModel.version = 0;
                        userSpecialAssetsModel.remark = "插入一条用户特享金（注册）";
                        userSpecialAssetsModel.invited_user_id = 0;
                        userSpecialAssetsModel.invited_user_name = "";
                        ResultInfo<bool> resultAddUserSpecialAssets = userSpecialAssetsService.AddUserSpecialAssets(userSpecialAssetsModel);
                        //更新特享金发放数量
                        ResultInfo<bool> resultUpdateActSpecialAssetsSendCount = activityManegeService.UpdateActSpecialAssetsSendCount(resultAct_SpecialAssets_R.Data.id);
                        #region 发送消息
                        YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                        mod.user_id = UserContext.simpleUserInfoModel.Id;
                        mod.isread = 0;
                        mod.sendtime = DateTime.Now;
                        mod.title = "特享金";
                        mod.content = string.Format("将军大人，为庆祝您成功注册，献上<i>{0}</i>元特享金，给小金库添加砖瓦，收益涨涨涨。快速获取请点击<a href=\"/usersetting/get_money/\">查看</a>", YxLiCai.Tools.Const.SystemConst.MoenyConvert(resultAct_SpecialAssets_R.Data.amount));
                        new UserMessageServer().AddUserMessage(mod);
                        #endregion

                        #region 添加一条用户活动日志
                        ActUserLog actUserLog = new ActUserLog();
                        actUserLog.user_id = invited_user_id;
                        actUserLog.act_id = resultActRegAssetsList.Data.id;//活动编号
                        actUserLog.item_id = resultActRegAssetsList.Data.item_id;
                        actUserLog.c_time = DateTime.Now;
                        actUserLog.creator_id = 0;
                        actUserLog.version = 0;
                        actUserLog.remark = "用户注册发放特享金";
                        actservice.AddActUserLog(actUserLog);
                        #endregion

                    }
                }
                #endregion

                //更新活动参与人数
                ResultInfo<bool> resultActPersonNum = actservice.UpdateCurtUserNumByActId(resultAct_R.Data);

                //更新单次活动的发放次数
                ResultInfo<bool> resultUpdateActSendCount = activityManegeService.UpdateActSendCount(resultAct_R.Data);
            }
            #endregion

            #region 邀请
            long useid = 0;//邀请人id

            if (!string.IsNullOrEmpty(inviteCode))//判断是否存在邀请码
            {
                bool CouponBool = false;
                bool SpecialAssets = false;
                ResultInfo<UserInfoModel> resultUserInfo = user.GetUserModelByMyCode(inviteCode);
                if (resultUserInfo.Result && resultUserInfo.Data != null)//根据邀请码判断是否存在该用户并获得邀请人id
                {
                    useid = resultUserInfo.Data.id;

                    #region 加息券---邀请
                    ResultInfo<ActPromotionModelView> resultActInviteCouponList = actservice.ActInfoByTypeAndItemType(1, 2);//获取邀请活动里面的加息券活动信息(1邀请2加息券)
                    if (resultActInviteCouponList.Result && resultActInviteCouponList.Data != null)
                    {
                        //ResultInfo<int> resultCount = userAddInterestService.GetUserAddInterestCountByInvitedUserIdAndActId(useid, resultActInviteCouponList.Data.id);//获取当前邀请次数
                        //if (resultCount.Result)
                        //{
                        //    if (resultCount.Data < resultActInviteCouponList.Data.limit_num || resultActInviteCouponList.Data.limit_num == 0) //resultCount.Data--邀请次数    resultActInviteAssetsList.Data.limit_num--本次活动限制次数
                        //    {
                        #region 加息券

                        ResultInfo<ActInterestCoupon> resultAct_Coupon_I =
                            actservice.GetCouponByItemId(resultActInviteCouponList.Data.item_id); //加息券
                        if (resultAct_Coupon_I.Result && resultAct_Coupon_I.Data != null)
                        {
                            //插入用户加息券
                            UserAddInterestModel userAddInterestModel = new UserAddInterestModel();
                            userAddInterestModel.user_id = useid;
                            userAddInterestModel.act_id = resultActInviteCouponList.Data.id; //活动编号
                            userAddInterestModel.interest_id = resultAct_Coupon_I.Data.id;
                            userAddInterestModel.interest_rate = resultAct_Coupon_I.Data.interest_rate;
                            userAddInterestModel.enable_day = resultAct_Coupon_I.Data.enable_day;
                            userAddInterestModel.c_time = DateTime.Now.Date;
                            userAddInterestModel.e_time =
                                DateTime.Now.AddDays(resultAct_Coupon_I.Data.enable_day-1).Date;
                            userAddInterestModel.use_status = 0;
                            userAddInterestModel.invest_id = 0;
                            userAddInterestModel.version = 0;
                            userAddInterestModel.remark = "插入一条用户加息券（邀请）";
                            userAddInterestModel.creator_id = 0;
                            userAddInterestModel.m_time = DateTime.Now.Date;
                            ResultInfo<bool> resultAddCoupon =
                                userAddInterestService.AddUserAddInterest(userAddInterestModel);
                            //更新加息券发放数量
                            ResultInfo<bool> resultUpdateActCouponSendCount =
                                activityManegeService.UpdateActCouponSendCount(resultAct_Coupon_I.Data.id);


                            #region 发送消息

                            YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                            mod.user_id = useid;
                            mod.isread = 0;
                            mod.sendtime = DateTime.Now;
                            mod.title = "加息券";
                            mod.content =
                                string.Format(
                                    "将军大人，<i>{0}</i>的成功注册为您献上<i>{1}</i>%加息券，可供购买任何产品均可使用，有效期至<i>{2}</i>，赶紧让它为您效力吧！",
                                    YxLiCai.Tools.Const.SystemConst.PhoneCut(phone),
                                    YxLiCai.Tools.Const.SystemConst.RateConvert(
                                        resultAct_Coupon_I.Data.interest_rate * 100),
                                    userAddInterestModel.e_time.AddDays(-1).Date.ToShortDateString());
                            new UserMessageServer().AddUserMessage(mod);

                            #endregion

                            #region 添加一条用户活动日志

                            ActUserLog actUserLog = new ActUserLog();
                            actUserLog.user_id = useid;
                            actUserLog.act_id = resultActInviteCouponList.Data.id; //活动编号
                            actUserLog.item_id = resultActInviteCouponList.Data.item_id;
                            actUserLog.c_time = DateTime.Now;
                            actUserLog.creator_id = 0;
                            actUserLog.version = 0;
                            actUserLog.remark = "邀请发放加息券";

                            ResultInfo<bool> resultActUserLog_RegCoupn = actservice.AddActUserLog(actUserLog);

                            #endregion

                            //更新活动参与人数
                            ResultInfo<bool> resultActPersonNum =
                                actservice.UpdateCurtUserNumByActId(resultActInviteCouponList.Data.id);

                            //更新单次活动的发放次数
                            ResultInfo<bool> resultUpdateActSendCount_co =
                                activityManegeService.UpdateActSendCount(resultActInviteCouponList.Data.item_id);
                            //加息券邀请活动更新
                            CouponBool = true;
                        }

                        #endregion
                        //    }
                        //}
                    }

                    #endregion

                    #region 特享金---邀请

                    ResultInfo<ActPromotionModelView> resultActInviteAssetsList = actservice.ActInfoByTypeAndItemType(1, 1);//获取邀请活动里面的加息券活动信息(1邀请1特享金)
                    if (resultActInviteAssetsList.Result && resultActInviteAssetsList.Data != null)
                    {
                        ResultInfo<int> resultCount = userSpecialAssetsService.GetUserSpecialAssetsCountByInvitedUserIdAndActId(useid, resultActInviteAssetsList.Data.id);//获取当前邀请次数
                        if (resultCount.Result)
                        {
                            if (resultCount.Data < resultActInviteAssetsList.Data.limit_num || resultActInviteAssetsList.Data.limit_num == 0)  //resultCount.Data--邀请次数    resultActInviteAssetsList.Data.limit_num--本次活动限制次数
                            {
                                #region 特享金相关
                                ResultInfo<ActSpecialAssets> resultAct_SpecialAssets_I = actservice.GetAssetsByItemId(resultActInviteAssetsList.Data.item_id);
                                if (resultAct_SpecialAssets_I.Result && resultAct_SpecialAssets_I.Data != null)
                                {
                                    //插入用户特享金
                                    UserSpecialAssetsModel userSpecialAssetsModel = new UserSpecialAssetsModel();
                                    userSpecialAssetsModel.user_id = useid;
                                    userSpecialAssetsModel.act_id = resultActInviteAssetsList.Data.id;
                                    userSpecialAssetsModel.special_id = resultAct_SpecialAssets_I.Data.id;
                                    userSpecialAssetsModel.amount = resultAct_SpecialAssets_I.Data.amount;
                                    userSpecialAssetsModel.enable_day = resultAct_SpecialAssets_I.Data.enable_day;
                                    userSpecialAssetsModel.c_time = DateTime.Now.Date;
                                    userSpecialAssetsModel.u_time = DateTime.Now;
                                    userSpecialAssetsModel.e_time = DateTime.Now.AddDays(resultAct_SpecialAssets_I.Data.enable_day-1).Date;
                                    userSpecialAssetsModel.use_status = 1;
                                    userSpecialAssetsModel.rate = resultAct_SpecialAssets_I.Data.rate;
                                    userSpecialAssetsModel.rate_increase = resultAct_SpecialAssets_I.Data.rate_increase;
                                    userSpecialAssetsModel.interest_added = 0.00m;
                                    userSpecialAssetsModel.interest_paid = 0.00m;
                                    userSpecialAssetsModel.m_time = DateTime.Now.Date;
                                    userSpecialAssetsModel.creator_id = 0;
                                    userSpecialAssetsModel.version = 0;
                                    userSpecialAssetsModel.remark = "插入一条用户特享金（邀请）";
                                    userSpecialAssetsModel.invited_user_id = invited_user_id;
                                    userSpecialAssetsModel.invited_user_name = phone;
                                    ResultInfo<bool> resultAddUserSpecialAssets = userSpecialAssetsService.AddUserSpecialAssets(userSpecialAssetsModel);
                                    //更新特享金发放数量
                                    ResultInfo<bool> resultUpdateActSpecialAssetsSendCount = activityManegeService.UpdateActSpecialAssetsSendCount(resultAct_SpecialAssets_I.Data.id);

                                    #region 插入分享记录
                                    UserInvitationModel userInvitationModel = new UserInvitationModel();
                                    userInvitationModel.user_id = useid;//邀请人id
                                    userInvitationModel.invited_user_id = invited_user_id;//被邀请人id
                                    userInvitationModel.c_time = DateTime.Now;
                                    userInvitationModel.m_time = DateTime.Now.Date;
                                    userInvitationModel.creator_id = 0;
                                    userInvitationModel.version = 0;
                                    userInvitationModel.remark = "插入一条特享金分享记录（邀请）";
                                    userInvitationModel.act_id = resultActInviteAssetsList.Data.id;
                                    userInvitationModel.invited_login_name = phone;
                                    ResultInfo<bool> resultUserInvitation = userInvitationService.AddUserInvitation(userInvitationModel);
                                    #endregion

                                    #region 发送消息
                                    YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                                    mod.user_id = useid;
                                    mod.isread = 0;
                                    mod.sendtime = DateTime.Now;
                                    mod.title = "特享金";
                                    mod.content = string.Format("将军大人，<i>{0}</i>好友的成功注册为您献上<i>{1}</i>元特享金大礼，小金库涨起来,点击<a href=\"/usersetting/get_money/\">查看</a>", YxLiCai.Tools.Const.SystemConst.PhoneCut(phone), YxLiCai.Tools.Const.SystemConst.MoenyConvert(resultAct_SpecialAssets_I.Data.amount));
                                    new UserMessageServer().AddUserMessage(mod);
                                    #endregion

                                    #region 添加一条用户活动日志
                                    ActUserLog actUserLog = new ActUserLog();
                                    actUserLog.user_id = useid;
                                    actUserLog.act_id = resultActInviteAssetsList.Data.id;//活动编号
                                    actUserLog.item_id = resultActInviteAssetsList.Data.item_id;
                                    actUserLog.c_time = DateTime.Now;
                                    actUserLog.creator_id = 0;
                                    actUserLog.version = 0;
                                    actUserLog.remark = "用户注册发放加息券";

                                    actservice.AddActUserLog(actUserLog);
                                    #endregion

                                    SpecialAssets = true;

                                }
                                #endregion

                            }
                        }

                        //更新活动参与人数
                        ResultInfo<bool> resultActPersonNum = actservice.UpdateCurtUserNumByActId(resultActInviteAssetsList.Data.id);

                        //更新单次活动的发放次数
                        ResultInfo<bool> resultUpdateActSendCount_sp = activityManegeService.UpdateActSendCount(resultActInviteAssetsList.Data.item_id);//特享金邀请活动更新
                    }
                    #endregion

                    #region 发短信

                    if (CouponBool && SpecialAssets)
                    {
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("6");
                        content = string.Format(content, phone);
                        YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(phone, content);
                    }
                    else if (CouponBool && !SpecialAssets)
                    {
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("7");
                        content = string.Format(content, phone); 
                            YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(resultUserInfo.Data.Phone, content); 
               
                    }

                    #endregion

                }
            }
            #endregion

        }

    }
}
