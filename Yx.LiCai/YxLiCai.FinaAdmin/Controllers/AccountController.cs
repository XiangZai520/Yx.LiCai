using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.Account;
using YxLiCai.Server.UserFinancingManagement;
using YxLiCai.Tools;
using YxLiCai.Tools.Const;
using YxLiCai.Tools.Util;
namespace YxLiCai.FinaAdmin.Controllers
{
    public class AccountController : Controller
    {
        private UserInfo_FinancingManagement_Services _authenticationService;
        public AccountController()
        {
            _authenticationService = new UserInfo_FinancingManagement_Services();
        }
        public ActionResult Account()
        {
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            string userinfo = YxLiCai.Tools.CookieHelper.ReadCookie(YxLiCai.Tools.Const.SystemConst.FinauserInfoCookieName);
            if (!string.IsNullOrEmpty(userinfo))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginOn(YxLiCai.Model.Account.LoginModel model, string returnUrl)
        {
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
            string cachekey = CookieHelper.ReadCookie("FinaCookie_VerificationLogin");
            if (string.IsNullOrEmpty(cachekey))
            {
                return Json(new ResultInfo(false, "验证码不正确"));
            }
            var captcha = redis.Get<string>(cachekey);
            if (captcha == null || model.Captcha != captcha)
            {
                return Json(new ResultInfo(false, "验证码不正确"));
            }
            var loginResult = _authenticationService.ValidateUser(model.UserName, YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(model.Password, 32));
            if (!loginResult.Result)
            {
                return Json(new ResultInfo(false, "服务器超时")); ;
            }
            switch (loginResult.Data)
            {
                case YxLiCai.Tools.Enums.UserLoginResults.Successful:
                    var reslut = _authenticationService.GetAccountByName(model.UserName);
                    if (reslut.Result && reslut.Data != null)
                    {

                        var userInfo = new SimpleFerUserInfo
                        {
                            Id = reslut.Data.Id,
                            LoginName = reslut.Data.LoginName,
                            Password = model.Password,
                            Fer_account = reslut.Data.Fer_account,
                        };
                        string json = YxLiCai.Tools.SafeEncrypt.DES.DesEncrypt(YxLiCai.Tools.SerializeHelper.JsonSerializer(userInfo), SystemConst.encrypt_des);
                        _authenticationService.SignIn(json);
                        return Json(new ResultInfo(true, "成功"));
                    }
                    else
                    {
                        return Json(new ResultInfo(false, "服务器超时"));
                    }
                case YxLiCai.Tools.Enums.UserLoginResults.UserNotExist:
                    return Json(new ResultInfo(false, "用户不存在"));
                case YxLiCai.Tools.Enums.UserLoginResults.AccountClosed:
                    return Json(new ResultInfo(false, "用户已经锁定"));
                default:
                    return Json(new ResultInfo(false, "密码不正确")); ;
            }
        }

        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult CaptchaImage()
        {
            var captcha = new LiteralCaptcha(80, 25, 4);
            var bytes = captcha.Generatea();
            //验证码插入Redis缓存
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
            string cachekey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaCodeAdminLogin, YxLiCai.Tools.Util.Helper.Uuid());
            redis.Add(cachekey, captcha.Captcha, DateTime.Now.AddHours(2));
            //缓存key放入cookie里存储 
            CookieHelper.WriteCookie("FinaCookie_VerificationLogin", cachekey, DateTime.Now.AddMinutes(10));
            return new FileContentResult(bytes, "image/jpeg"); ;
        }


    }
}
