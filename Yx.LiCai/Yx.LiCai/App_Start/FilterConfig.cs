using System;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Tools;

namespace Yx.LiCai
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebHandleErrorAttribute(), 1);
            filters.Add(new HandleErrorAttribute(),2);
        }
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
}