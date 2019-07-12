
using YxLiCai.Tools.Const;
using YxLiCai.Model.Account;


namespace YxLiCai.FinaAdmin
{
    public class UserContext
    {
        private static SimpleFerUserInfo _simpUser = new SimpleFerUserInfo();       
        /// <summary>
        /// 用户登录实体类
        /// </summary>
        public static SimpleFerUserInfo simpleUserInfoModel
        {
            get
            {
                var cookie = YxLiCai.Tools.CookieHelper.ReadCookie(SystemConst.FinauserInfoCookieName);
                if (cookie == "")
                {
                    return _simpUser;
                }
                cookie = YxLiCai.Tools.SafeEncrypt.DES.DesDecrypt(cookie, SystemConst.encrypt_des);
                return YxLiCai.Tools.SerializeHelper.JsonDeserialize<SimpleFerUserInfo>(cookie);
            }
        }
    }
}