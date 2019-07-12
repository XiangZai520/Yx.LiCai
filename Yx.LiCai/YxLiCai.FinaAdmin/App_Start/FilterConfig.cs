using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YxLiCai.Tools;

namespace YxLiCai.FinaAdmin
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
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            else
            {
                YxLiCai.Service.MenuSet.AuthorityMenuService iauthority = new YxLiCai.Service.MenuSet.AuthorityMenuService();
                var myMenus = iauthority.GetMenusByAccountId(UserContext.simpleUserInfoModel.Id);//获取用户的所有权限             
         
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
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute(),1);
            filters.Add(new HandleErrorAttribute(),2);
            filters.Add(new XSSFilterAttribute(),3);
        }
    }
}