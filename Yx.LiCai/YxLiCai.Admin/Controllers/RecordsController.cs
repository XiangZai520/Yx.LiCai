using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Admin.Models;
using YxLiCai.Model.TransactionRecords;
using YxLiCai.Server.TransactionRecords;
using YxLiCai.Tools;

namespace YxLiCai.Admin.Controllers
{
    public class RecordsController : Controller
    {
        private TransactionRecordsService Dao;
        public RecordsController()
        {
            Dao = new TransactionRecordsService();
        }
        //
        // GET: /Records/
        /// <summary>
        /// 记录交易
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #region 获取购买记录
        /// <summary>
        /// 获取购买记录
        /// </summary>
        /// <param name="Panme">用户名</param>
        /// <param name="InvestmentProducts">购买产品</param>
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public JsonResult GetRefundLog(string Panme, int InvestmentProducts, DateTime time_min, DateTime time_max,
           int skip, int take)
        {
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(time_max, time_min);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }

            int countAll = 0;

            var pageResult = new PageViewModel<Ord_infoModel>
            {
                DataResult = Dao.GetBuyLog(Panme, InvestmentProducts, time_min, time_max, skip, take, out countAll),
                TotalCount = countAll
            };
            return Json(pageResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 提现记录

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="Panme"></param>
        /// <param name="time_min"></param>
        /// <param name="time_max"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public JsonResult GetTiXianLog(string Panme, DateTime time_min, DateTime time_max,
           int skip, int take)
        {
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(time_max, time_min);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            int countAll = 0;
            var pageResult = new PageViewModel<User_withdrawModel>
            {
                DataResult = Dao.Getuser_withdrawLog(Panme, time_min, time_max, skip, take, out countAll),
                TotalCount = countAll
            };
            return Json(pageResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 赎回记录

        /// <summary>
        /// 赎回记录
        /// </summary>
        /// <param name="Panme"></param>
        /// <param name="time_min"></param>
        /// <param name="time_max"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public JsonResult GetSLog(string Panme, DateTime time_min, DateTime time_max,
           int skip, int take)
        {
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(time_max, time_min);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            int countAll = 0;
            var pageResult = new PageViewModel<User_redemptionModel>
            {
                DataResult = Dao.Getuser_redemptionLog(Panme, time_min, time_max, skip, take, out countAll),
                TotalCount = countAll
            };
            return Json(pageResult, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
