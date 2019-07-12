using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model;
using YxLiCai.Model.Account;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Tools;
using YxLiCai.Tools.Const;

namespace Yx.LiCai.App_Start
{
    public class UserContext
    {
        private static SmallUserInfo _simpUser = new SmallUserInfo();
        //private UserInfoService user;
        /// <summary>
        /// 用户登录实体类
        /// </summary>
        public static SmallUserInfo simpleUserInfoModel
        {
            get
            {
                //用户cook-存放的是用户的ID
                var cookie_UID = YxLiCai.Tools.CookieHelper.ReadCookie(SystemConst.userInfoCookieName);
                if (string.IsNullOrEmpty(cookie_UID))
                {
                    return _simpUser;
                }
                cookie_UID = YxLiCai.Tools.SafeEncrypt.DES.DesDecrypt(cookie_UID, SystemConst.encrypt_des);
                //用户Reades----KEY
                string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, cookie_UID);
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                var SmallUser = redis.Get<SmallUserInfo>(Userkey);
               
                if (SmallUser != null)
                {
                    return SmallUser;
                }
                else
                {
                    UserInfoService user = new UserInfoService();
                    ResultInfo<UserInfoModel> result = user.GetUserModel(Convert.ToInt64(cookie_UID));
                    SmallUser = new SmallUserInfo();
                    if (result.Result)
                    {
                        
                        SmallUser.Id = result.Data.id;
                        SmallUser.Account = result.Data.Phone;
                        SmallUser.IsBindBank = result.Data.IsBindBank;
                        SmallUser.IsJiaoYIPW = 0;
                        if (!string.IsNullOrEmpty(result.Data.Sallpassword))
                        {
                            SmallUser.IsJiaoYIPW = 1;
                        }
                        SmallUser.IsRealCard = result.Data.IsRealCard;
                        SmallUser.MyCode = result.Data.MyCode;
                        SmallUser.MyRealName = result.Data.MyRealName;
                        SmallUser.MyRealCard = result.Data.MyRealCard;
                        SmallUser.BankName = result.Data.BankName;
                        SmallUser.LastBankNum = result.Data.LastBankNum;
                        SmallUser.BankCode = result.Data.BankCode;
                        redis.Add(Userkey, SmallUser, DateTime.Now.AddDays(5));
                    }

                }
                return SmallUser;
            }
        }
        /// <summary>
        /// 用户写入Cookie
        /// </summary>
        /// <param name="data"></param>
        public static void SignIn(string data)
        {
            CookieHelper.WriteCookie(SystemConst.userInfoCookieName, data, DateTime.Now.AddDays(1));
        }
        /// <summary>
        /// 用户退登录删除cookie
        /// </summary>
        public static void SignOut()
        {
            var cookie_UID = YxLiCai.Tools.CookieHelper.ReadCookie(SystemConst.userInfoCookieName);
            if(!string.IsNullOrEmpty(cookie_UID))
            {
                cookie_UID = YxLiCai.Tools.SafeEncrypt.DES.DesDecrypt(cookie_UID, SystemConst.encrypt_des);
                string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, cookie_UID);
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                redis.Delete(Userkey); 
            }
            CookieHelper.RemoveCookie(SystemConst.userInfoCookieName);
        }
    }
}