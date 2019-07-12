using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;

namespace Yx.LiCai.Controllers
{
    public class ActivityController : Controller
    {
        private UserInvitationService userInvitationService=new UserInvitationService();
        private UserInfoService user = new UserInfoService();

        /// <summary>
        /// 邀请好友   by张浩然  2015-6-29 14：48：00
        /// </summary>
        /// <returns></returns>
        public ActionResult invite()
        {
            string inviteCode = Request["inviteCode"];
            if (!string.IsNullOrEmpty(inviteCode))
            {
                var cookie_UID = YxLiCai.Tools.CookieHelper.ReadCookie(YxLiCai.Tools.Const.SystemConst.userInfoCookieName);
                var myCode = UserContext.simpleUserInfoModel.MyCode;
                if (string.IsNullOrEmpty(cookie_UID) || myCode != inviteCode)
                {
                    Response.Redirect("/Activity/invite_show?inviteCode=" + inviteCode);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(UserContext.simpleUserInfoModel.Account))
                {
                    Response.Redirect("/Activity/invite?inviteCode=" + UserContext.simpleUserInfoModel.MyCode);
                }
                else {
                    return RedirectToAction("RegisteredOrLogin", "Login");
                }
            }

            return View();
        }

        /// <summary>
        /// 邀请活动详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult invite_show()
        {
            ViewBag.inviteCode = string.Empty;
            List<UserInvitationModel> viewModel = new List<UserInvitationModel>();
            string inviteCode = Request["inviteCode"];
            if (!string.IsNullOrEmpty(inviteCode))
            {
                ViewBag.inviteCode = inviteCode;
                var usermodel = user.GetUserModelByMyCode(inviteCode);
                if (usermodel.Result && usermodel.Data != null)
                {
                    ResultInfo<List<UserInvitationModel>> resultList = userInvitationService.GetUserInvitationList(usermodel.Data.id);
                    if (resultList.Result)
                    {
                        viewModel = resultList.Data;
                    }
                }

            }
            
            return View(viewModel);
        }

    }
}
