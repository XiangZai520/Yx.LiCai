
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Tools;

namespace YxLiCai.Admin
{

    /// <summary>
    /// 验证用户是否具有某访问权限-平扬 2015.3.20
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorityValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (UserContext.simpleUserInfoModel.Id == 0)
            {
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "account", action = "login" }));
            }
            else
            {
                YxLiCai.Service.MenuSet.AuthorityMenuService iauthority = new YxLiCai.Service.MenuSet.AuthorityMenuService();
                var myMenus = iauthority.GetMenusByAccountId(UserContext.simpleUserInfoModel.Id);//获取用户的所有权限
                var myMenusR = iauthority.GetMenusByRoloId(UserContext.simpleUserInfoModel.RoleId);
                if (myMenusR != null)
                {
                    myMenus.AddRange(myMenusR);
                }
                if (myMenus != null && myMenus.Count > 0)
                {
                    if (
                        !myMenus.Exists(
                            authorityMenuModel =>
                                //authorityMenuModel.Url.Contains("/" + controllerName + "/" + actionName)
                                authorityMenuModel.Url.ToLower() == ("/" + controllerName + "/" + actionName).ToLower()
                                ))
                    {
                        filterContext.Result = new JsonResult { Data = new YxLiCai.Model.ResultModel(false, "抱歉，你不具有当前操作的权限"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
                else
                {
                    filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };
                }
            }
        }
    }
    /// <summary>
    /// XSS过滤验证
    /// </summary> 
    public class XSSFilterAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var parameters = filterContext.ActionDescriptor.GetParameters();
        //    foreach (var parameter in parameters)
        //    {
        //        //只过滤string类型参数 
        //        if (parameter.ParameterType == typeof(string))
        //        {
        //            //获取字符串参数原值 
        //            var orginalValue = filterContext.ActionParameters[parameter.ParameterName];
        //            if (orginalValue != null)
        //            {
        //                filterContext.ActionParameters[parameter.ParameterName] = XSSTool.Filter(orginalValue.ToString());
        //            }
        //        }
        //    }
        //}
    }
    /// <summary>
    /// 自定义异常处理类 
    /// </summary>
    public class WebHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 重写异常处理方法 
        /// </summary>
        /// <param name="filterContext">上下文对象  该类继承于ControllerContext</param>
        public override void OnException(ExceptionContext filterContext)
        {
            LogHelper.LogWriterFromFilter(filterContext.Exception);
        }
    }
    /// <summary>
    /// 保理用户验证是否完善个人信息
    /// </summary>
    public class FactoringManageAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            UserCountService userCountService = new UserCountService();
            UserInfoService userInfoService = new UserInfoService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            ResultInfo<UserInfoModel> result = userInfoService.GetUserModel(user_id);
            string yxlcUrl = System.Configuration.ConfigurationManager.AppSettings["yxlcUrl"];
            if (result.Result && result.Data != null)
            {
                UserInfoModel entity = result.Data;
                if (entity.IsBindBank != 1)
                {
                    filterContext.Result = new RedirectResult(yxlcUrl+"/UserSetting/add_bank");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult(yxlcUrl + "/Login/RegisteredOrLogin");
            }
        }
    }
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute(), 1);
            filters.Add(new HandleErrorAttribute(), 2);
            filters.Add(new XSSFilterAttribute(), 3);
        }
    }

    public class NoCache : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}