using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Tools.Const;
using YxLiCai.Model.Account;
using YxLiCai.Tools;
using YxLiCai.Server.UserFinancingManagement;
using YxLiCai.Model;
using YxLiCai.Model.UserFinancingManagement;

namespace YxLiCai.Admin
{
    public class UserContext
    {
        private static SimpleUserInfoModel _simpUser = new SimpleUserInfoModel();
        private static UserSmallReads _simpUserSmallReads = new UserSmallReads();
        /// <summary>
        /// 用户登录实体类
        /// </summary>
        public static SimpleUserInfoModel simpleUserInfoModel
        {
            get
            {
                var cookie = YxLiCai.Tools.CookieHelper.ReadCookie(SystemConst.userInfoCookieName);
                if (cookie == "")
                {
                    return _simpUser;
                }
                cookie = YxLiCai.Tools.SafeEncrypt.DES.DesDecrypt(cookie, SystemConst.encrypt_des);
                return YxLiCai.Tools.SerializeHelper.JsonDeserialize<SimpleUserInfoModel>(cookie);
            }
        }

        /// <summary>
        /// 判断用户是否有某权限()
        /// </summary>
        /// <param name="menuid">权限id</param>
        /// <returns></returns>
        public static bool HasAuthority(int menuid)
        {
            YxLiCai.Service.MenuSet.AuthorityMenuService _iAuhority = new YxLiCai.Service.MenuSet.AuthorityMenuService();
            string cookieValue = CookieHelper.ReadCookie("menulist");
            if (cookieValue == "")
            {
                return _iAuhority.HasAuthority(UserContext.simpleUserInfoModel.Id, menuid);
            }
            else
            {
                var list = YxLiCai.Tools.SerializeHelper.JsonDeserialize<List<int>>(YxLiCai.Tools.SafeEncrypt.DES.DesDecrypt(cookieValue, SystemConst.encrypt_des));
                return list.Contains(menuid);
            }
        }
    }
}