using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Admin.Models;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 还款管理
    /// </summary>
    public class RefundManagerController : BaseController
    {       
        /// <summary>
        /// 你懂的
        /// </summary>
        private YxLiCai.Server.Refund.RefundManager _manager = new Server.Refund.RefundManager(); 

        #region Views
        [AuthorityValidate]
        /// <summary>
        /// 待处理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        //[AuthorityValidate]
        /// <summary>
        /// 还款记录
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundSuccess()
        {
            return View();
        }
        [AuthorityValidate]
        /// <summary>
        /// 还款失败
        /// </summary>
        /// <returns></returns>
        public ActionResult RefundFailed()
        {
            return View();
        }
        #endregion

        #region Method
        /// <summary>
        /// 待处理利息
        /// </summary>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <param name="skip">起始记录</param>
        /// <param name="take"></param>
        /// <param name="page"></param>
        /// <param name="pageSize">取多少记录</param>
        /// <returns></returns>
        public JsonResult GetRefundInterestList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
             int skip, int take)
        {
            int countAll = 0;
            var list = _manager.GetRefundInterestList(merchantName, loanPeriod, amount_min, amount_max, skip, take, out countAll);

            if (list == null || list.Count == 0)
                return Json(new
                {
                    DataResult = list,
                    TotalCount = 0
                }, JsonRequestBehavior.AllowGet);

            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.id,
                m_id = entity.merchantID,
                m_name = entity.MerchantName,
                pjid = entity.pjid,
                pj_name = entity.ProjectName,
                amount = entity.ProjectAmount / 10000,
                loanPeriod = entity.LoanPeriod,
                amount_paid = entity.interest_paid,
                interest = entity.amount
            
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 待处理本金
        /// </summary>   
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <param name="skip">起始记录</param>
        /// <param name="take"></param>
        /// <param name="page"></param>
        /// <param name="pageSize">取多少记录</param>
        /// <returns></returns>
        public JsonResult GetRefundPrincipalList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
            int skip, int take)
        {
            int countAll = 0;
            var list = _manager.GetRefundPrincipalList(merchantName, loanPeriod, amount_min, amount_max, skip, take, out countAll);

            if (list == null || list.Count == 0)
                return Json(new
                {
                    DataResult = list,
                    TotalCount = 0
                }, JsonRequestBehavior.AllowGet);

            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.id,
                pjid = entity.pjid,
                ProjectName = entity.ProjectName,
                ProjectAmount = entity.ProjectAmount / 10000,
                LoanPeriod = entity.LoanPeriod,
                RepayAmount = entity.RepayAmount,
                merchantID = entity.merchantID,
                MerchantName = entity.MerchantName,
                amount = entity.amount,
                status = entity.status == 0 ? "待审核" : "还款中",
                ctime = entity.ctime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 财务审批通过融资方还本金申请
        /// </summary>
        /// <param name="rId">记录id</param>
        /// <param name="pjId">项目id</param>
        /// <param name="ferId">融资方账户ID</param>
        /// <param name="amount">还款金额</param>
        /// <returns></returns>
        public JsonResult PayPrincipal(long rId, int pjId, int ferId, decimal amount)
        {
            int adminId = UserContext.simpleUserInfoModel.Id; //操作人id
            return Json(_manager.PayPrincipal(rId, pjId, ferId, amount, adminId),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 还款记录
        /// </summary>
        /// <param name="merchantName">融资方姓名</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">还款金额</param>
        /// <param name="amount_max">还款金额</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public JsonResult GetRefundLog(string merchantName, int pjt_id, decimal amount_min, decimal amount_max,
           DateTime? sDate, DateTime? eDate, int type, int skip, int take)
        {
            int countAll = 0;
            var list = _manager.GetRefundLog(merchantName, pjt_id, amount_min, amount_max,
                                sDate, eDate, type, skip, take, out countAll);

            if (list == null || list.Count == 0)
            {
                return Json(new
                {
                    DataResult = list,
                    TotalCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
              
            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.id,
                pjt_id = entity.pjid,
                ProjectName = entity.ProjectName,
                ProjectAmount = entity.ProjectAmount / 10000,
                amount = entity.amount,
                MerchantName = entity.MerchantName,
                ctime = entity.ctime.ToString("yyyy-MM-dd HH:mm:ss"),
                Type = entity.Type
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
