using System.Web.Mvc;
using YxLiCai.Model;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 充值管理
    /// add by lxm 2015年7月1日
    /// </summary>
    public class ChargeManagerController : BaseController
    {
        private YxLiCai.Server.Charge.ChargeManager _manager = new Server.Charge.ChargeManager();

        #region Views
        /// <summary>
        /// 充值提醒页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 充值提醒列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReminderList(string name, int uType, decimal amount_min, decimal amount_max)
        {
            return Json(_manager.GetChargeReminderList(name, uType, amount_min, amount_max), 
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 充值提醒
        /// 发送短信
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public JsonResult SendMessage(string phone, decimal amount, decimal balance, string type)
        { 
            if (string.IsNullOrEmpty(phone))
                return null;

            var smsService = new YxLiCai.Server.SendMsg.Send();
            string link = "http://admin."+ System.Configuration.ConfigurationManager.AppSettings["COOKIE_HOST"];//登录地址
            if (type == "融资方账户")
                link = "http://fina." + System.Configuration.ConfigurationManager.AppSettings["COOKIE_HOST"];//登录地址

            string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("1");
            content = string.Format(content, link, balance, amount);

            //发送短信
            var sendResult = YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(phone, content); 

            return Json(sendResult.Result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
