using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using YxLiCai.Server.Common;

namespace Yx.LiCai.Controllers
{
    public class BaseController : Controller
    {  
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //用户cookie-存放的是用户的ID
            var cookie_UID = YxLiCai.Tools.CookieHelper.ReadCookie(YxLiCai.Tools.Const.SystemConst.userInfoCookieName);
            if (string.IsNullOrEmpty(cookie_UID))
            {
                filterContext.Result = RedirectToAction("RegisteredOrLogin", "Login");
            }
            base.OnActionExecuting(filterContext);
        }
 
        /// <summary>
        /// Controller 出错方法
        /// 跳转到错误页面
        /// </summary>
        /// <returns></returns>
        public void OnError()
        {
            //跳转至出错页面 
            Response.Redirect("/home/Error");
        }
    }
}
