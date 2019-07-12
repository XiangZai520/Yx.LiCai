 
using System; 
using System.Web.Mvc; 
using System.Text.RegularExpressions;
using YxLiCai.Server.Common;
using YxLiCai.Tools;

namespace YxLiCai.Admin.Controllers
{
    public class BaseController : Controller
    {
        private ConfigurationManagerServer ConfigManager = new ConfigurationManagerServer();

        #region Request方法
        public bool IsNumber(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^[-]{0,1}\d+$");

        }
        /// <summary>
        /// Request一个数字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int RequestInt(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                return 0;
            }
            else
            {
                if (IsNumber(v))
                {
                    return int.Parse(v);
                }
                return 0;
            }
        }
        /// <summary>
        /// Request一个布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RequestBool(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                return false;
            }
            else
            {
                try
                {
                    return bool.Parse(v);
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 请求一个字符串，返回null时返回空字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string RequestString(string name)
        {
            string v = Request[name];
            if (v == null)
            {
                v = string.Empty;
            }
            return v;
        }
        /// <summary>
        /// 请求字符串类型
        /// </summary>
        /// <param name="requestStr">请求的Request值</param>
        /// <returns>返回字符串，null则返回""</returns>
        public string ConvertString(string requestStr)
        {
            if (string.IsNullOrEmpty(requestStr))
            {
                return "";
            }
            else
            {
                return requestStr;
            }
        }
        /// <summary>
        ///   数字类型值
        /// </summary>
        /// <param name="requestStr">获取request值</param>
        /// <returns>返回数字类型,空则返回0</returns>
        public int ConvertInt(string requestStr)
        {
            if (IsNumber(requestStr))
            {
                return Int32.Parse(requestStr);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserContext.simpleUserInfoModel == null || UserContext.simpleUserInfoModel.Id == 0)
            {
                //Response.Redirect("/account/login");
                filterContext.Result = RedirectToAction("login", "account");
            }
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 从配置文件中读取页面Kendo控件等的dataSource
        /// </summary>
        /// <param name="configName">配置文件名称</param>
        /// <param name="element">根节点名称</param>
        public ContentResult GetConfig(string configName, string element)
        {
            if (!string.IsNullOrEmpty(configName) && !string.IsNullOrEmpty(element))
            {
                var xmlConfigList = ConfigManager.GetConfig(configName, element);

                return new ContentResult { Content = Newtonsoft.Json.JsonConvert.SerializeObject(xmlConfigList) };
            }

            return null;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool ExportExcel(System.Data.DataTable dt)
        {
            var eh = new ExcelHelper();
            try
            {
                eh.ExportExcel(dt);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
