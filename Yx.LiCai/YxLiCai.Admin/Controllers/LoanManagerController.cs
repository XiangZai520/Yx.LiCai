using System;
using System.Linq;
using System.Web.Mvc;
using YxLiCai.Admin.Models;
using YxLiCai.Server.LoanManager;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 放款管理相关操作
    /// </summary>
    public class LoanManagerController : BaseController
    {
        private LoanManager _iService;
        private int adminID;
        private DateTime _MinDate;

        public LoanManagerController()
        {
            _iService = new LoanManager();
            adminID = YxLiCai.Admin.UserContext.simpleUserInfoModel.Id;
            _MinDate = Convert.ToDateTime("2015-01-01");
        }

        #region Views
        /// <summary>
        /// 待放款页        
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 放款成功
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanSuccess()
        {
            return View();
        }
        /// <summary>
        /// 放款失败
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanFailed()
        {
            return View();
        }
        #endregion

        #region 待放款
        /// <summary>
        /// 待放款列表
        /// </summary>
        /// <param name="projectName">查询条件 融资方姓名</param>
        /// <param name="loanPeriod">查询条件</param>
        /// <param name="amount_min">查询条件</param>
        /// <param name="amount_max">查询条件</param>
        /// <param name="skip">分页相关</param>
        /// <param name="take">分页相关</param>
        /// <returns></returns>
        public JsonResult GetAll(string mName, int loanPeriod, decimal amount_min, decimal amount_max, int status,
            int skip, int take)
        {
            int countAll = 0;
            var list = _iService.GetAll(mName, loanPeriod, amount_min, amount_max, skip, take, out countAll);
            //format
            var listFormated = list.ConvertAll(item => new
            {
                id = item.id,
                pj_id = item.pjid,
                pj_name = item.project.ProjectName,
                period = item.project.LoanPeriod,
                pj_amount = item.project.Amount,
                amount_lent = item.project.amount_lent,
                amount = item.amount_expect,
                account_id = item.MerchantID,
                m_name = item.MerchantName,
                r_name = item.MerchantLegalManName,
                phone = item.MerchantLegalManPhone,
                bank = item.BankCardInfo.BankName,
                bank_card = item.BankCardInfo.BankCard
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 放款操作
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="merchantID">融资方商户ID</param>
        /// <param name="amount">金额</param>
        ///  <param name="projectID">项目id</param>
        /// <returns></returns>
        public JsonResult PayMerchant(long recordID, int merchantID, decimal amount, int projectID)
        {
            var result = _iService.PayMerchant(recordID, merchantID, projectID, amount, adminID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 重新支付
        /// </summary>
        /// <returns></returns>
        public JsonResult RepayMerchant(int recordID, int merchantID, decimal amount, int projectID)
        {
            var result = _iService.RePayMerchant(recordID, merchantID, projectID, amount, adminID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 全部付款
        /// </summary>
        public JsonResult PayAll()
        {
            return Json(_iService.PayAll(adminID), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        public JsonResult PayPartial(string IdList)
        {
            return Json(_iService.PayPartial(adminID, IdList), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 线下放款
        /// </summary>
        public JsonResult PayMerchantOffline(long recordID, int merchantID, decimal amount, int projectID)
        {
           return Json(_iService.PayMerchantOffline(recordID, merchantID, projectID, amount, adminID),
               JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 放款记录
        /// <summary>
        /// 放款记录
        /// </summary>
        public JsonResult GetLoanLog(string mName, int loanPeriod, decimal amount_min, decimal amount_max,
           DateTime? sDate, DateTime? eDate, int skip, int take)
        {
            int countAll;
            var list = _iService.GetLoanLog(mName, loanPeriod, amount_min, amount_max, sDate, eDate, skip, take, out countAll);

            if (list == null || list.Count == 0)
                return null;

            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.id,
                pjid = entity.pjid,
                pjName = entity.project.ProjectName,
                loanPeriod = entity.project.LoanPeriod,
                pjAmount = entity.project.Amount,
                amount = entity.amount_expect,
                mID = entity.MerchantID,
                mName = entity.MerchantName,
                rName = entity.MerchantLegalManName,
                phone = entity.MerchantLegalManPhone,
                bkName = entity.BankCardInfo.BankName,
                bkCard = entity.BankCardInfo.BankCard,
                adminID = entity.adminID,
                loanTime = entity.ctime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 放款失败
        /// <summary>
        /// 放款失败列表
        /// </summary>
        public JsonResult GetLoanFailedList(string mName, int loanPeriod, decimal amount_min, decimal amount_max,
             DateTime? sDate, DateTime? eDate, int skip, int take)
        {
            int countAll;
            var list = _iService.GetLoanFailedList(mName, loanPeriod, amount_min, amount_max, sDate, eDate, skip, take, out countAll);

            if (list == null || list.Count == 0)
                return null;

            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.id,
                pjid = entity.pjid,
                pjName = entity.project.ProjectName,
                loanPeriod = entity.project.LoanPeriod,
                pjAmount = entity.project.Amount,
                amount = entity.amount_expect,
                mID = entity.MerchantID,
                mName = entity.MerchantName,
                rName = entity.MerchantLegalManName,
                phone = entity.MerchantLegalManPhone,
                bkName = entity.BankCardInfo.BankName,
                bkCard = entity.BankCardInfo.BankCard,
                adminID = entity.adminID,
                loanTime = entity.ctime.ToString("yyyy-MM-dd HH:mm:ss"),
                remark = entity.Remark
            }).ToList();

            var pageResult = new
            {
                DataResult = listFormated,
                TotalCount = countAll
            };
            return Json(pageResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出放款记录
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="status">状态:0待放款；3放款成功；5失败</param>
        /// <returns></returns>
        public JsonResult ExportLoanReport(string mName, int loanPeriod, decimal amount_min, decimal amount_max, int status)
        {
            var dt = _iService.GetAllLoanRecord(mName, loanPeriod, amount_min, amount_max, status);
            if (dt != null)
            {
                return Json(ExportExcel(dt), JsonRequestBehavior.AllowGet);
            }

            return null;
        }
        #endregion
    }
}
