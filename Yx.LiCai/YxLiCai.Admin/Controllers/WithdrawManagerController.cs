using System;
using System.Linq;
using System.Web.Mvc;
using YxLiCai.Server.WithdrawManager;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 提现、赎回管理
    /// </summary>
    public class WithdrawManagerController : BaseController
    {
        private WithdrawManager _iService = new WithdrawManager();
        private DateTime _MinDate = Convert.ToDateTime("2015-01-01");
        private int adminID = YxLiCai.Admin.UserContext.simpleUserInfoModel.Id;

        #region 融资方提现
        #region 页面
        //[AuthorityValidateAttribute]
        /// <summary>
        /// 融资方提现页面
        /// </summary>
        public ActionResult MerchantWithdraw()
        {
            return View();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 融资方提现申请列表
        /// </summary>
        public JsonResult GetMerchantWithdrawList(string lName, int status, DateTime? sDate, DateTime? eDate,
            int take, int skip)
        {
            int countAll = 0;
            var list = _iService.GetMerchantWithdrawList(lName, status, sDate, eDate, skip, take, out countAll);
            //format
            var listFormated = list.ConvertAll(item => new
            {
                id = item.ID,
                lName = item.LoginName,
                rName = item.RealName,
                bank = item.Bank,
                bank_card = item.BankCard,
                phone = item.Phone,
                amount = item.Amount,
                c_time = item.ApplyTime < _MinDate ? "" : item.ApplyTime.ToString("yyyy-MM-dd HH:mm:ss"),
                auditor_id = item.AuditorID,
                audit_time = item.AuditTime < _MinDate ? "" : item.AuditTime.ToString("yyyy-MM-dd HH:mm:ss"),
                oprator_id = item.OperatorID,
                pay_time = item.PayTime < _MinDate ? "" : item.PayTime.ToString("yyyy-MM-dd HH:mm:ss"),
                status = ConvertStatusToStr(item.Status),
                remark = item.remark,
                account_id = item.AccountID
            }).ToList();

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 状态转换
        /// 提现状态（0待审核,1待放款,2审核不通过,3提现成功,4提现失败）
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string ConvertStatusToStr(int status)
        {
            var strStatus = string.Empty;
            switch (status)
            {
                case 0: strStatus = "待审核"; break;
                case 1: strStatus = "待放款"; break;
                case 2: strStatus = "审核不通过"; break;
                case 3: strStatus = "提现成功"; break;
                case 4: strStatus = "提现失败"; break;
                default: strStatus = "未定义"; break;
            }
            return strStatus;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核融资方提现记录
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult AuditMerchantWithdraw(int recordID, int status, string remark = "")
        {
            return Json(_iService.AuditMerchantWithdrawRecord(recordID, adminID, status, remark),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批量通过融资方提现申请
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public JsonResult PassMultiMerchantWithdraw(string IDs)
        {
            var result = _iService.PassMultiMerchantWithdraw(IDs, adminID);
            var message = "操作成功";

            if (!result)
                message = "操作失败";

            return Json(new
            {
                IsSuccess = result,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 全部通过融资方提现申请
        /// </summary>
        /// <returns></returns>
        public JsonResult PassAllMerchantWithdraw()
        {
            var result = _iService.PassAllMerchantWithdraw(adminID);
            var message = "操作成功";

            if (!result)
                message = "操作失败";

            return Json(new
            {
                IsSuccess = result,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 放款
        /// <summary>
        /// 财务放款
        /// </summary>
        public JsonResult PayMerchant(int recordID, int accountID, decimal amount)
        {
            return Json(_iService.PayMerchant(recordID, adminID, accountID, amount),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        public JsonResult PayMultiMerchant(string idList)
        {
            if (string.IsNullOrEmpty(idList))
                return null;

            return Json(_iService.PayMultiMerchantWithdraw(idList, adminID),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 全部放款
        /// </summary>    
        public JsonResult PayAllMerchant()
        {
            return Json(_iService.PayAllMerchantWithdraw(adminID),
              JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}
