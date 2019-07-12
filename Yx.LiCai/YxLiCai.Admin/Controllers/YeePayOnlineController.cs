using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;

namespace YxLiCai.Admin.Controllers
{
    public class YeePayOnlineController : Controller
    {
        //
        // GET: /YeePayOnline/
        /// <summary>
        /// 易宝支付借口回调
        /// </summary>
        /// <returns></returns>
        public ActionResult PayCallBack()
        {

            string url = Request.Url.ToString();

            string p1_MerId = Request["p1_MerId"];
            string r0_Cmd = Request["r0_Cmd"];
            string r1_Code = Request["r1_Code"];
            string r2_TrxId = Request["r2_TrxId"];
            string r3_Amt = Request["r3_Amt"];
            string r4_Cur = Request["r4_Cur"];
            string r5_Pid = Request["r5_Pid"];
            string r6_Order = Request["r6_Order"];
            string r7_Uid = Request["r7_Uid"];
            string r8_MP = Request["r8_MP"];
            string r9_BType = Request["r9_BType"];
            string rp_PayDate = Request["rp_PayDate"];

            string rq_SourceFee = Request["rq_SourceFee"];
            string rq_TargetFee = Request["rq_TargetFee"];
            string hmac = Request["hmac"];

            if (YxLica.Tools.Pay.yeepay_online.Bussiness.IsTrueBack(p1_MerId, r0_Cmd, r1_Code, r2_TrxId, r3_Amt,
            r4_Cur, r5_Pid, r6_Order, r7_Uid, r8_MP, r9_BType, rp_PayDate, hmac))
            {
                YxLiCai.Tools.LogHelper.Write("易宝支付接口(大额)回调", url);
                //验证是否更新充值
                YxLiCai.Server.FactoringManage.FactoringManageService factoringManageService = new Server.FactoringManage.FactoringManageService();
                ResultInfo<UserRecharge> result = factoringManageService.GetUserRecharge(r6_Order);
                if (result.Result && result.Data != null && result.Data.status == 0)
                {
                    //更新充值记录为充值成功
                    UserCountService userCountService = new UserCountService();
                    long user_id = userCountService.GetUserIDByUserType().Data;
                    ResultInfo<bool> ri = factoringManageService.UpdateUserRechargeForRechargeSuccess(result.Data.id, user_id, Convert.ToDecimal(result.Data.amount), DateTime.Now, r2_TrxId, Convert.ToDecimal(rq_SourceFee), Convert.ToDecimal(rq_TargetFee));
                    if (ri.Result && ri.Data)
                    {

                        if (r9_BType.Trim() == "1")
                        {
                            return RedirectToAction("index", "sys");
                        }
                        else if (r9_BType.Trim() == "2")
                        {
                            //通知易宝，我们已经知道客户支付成功
                            return Content("success");
                        }
                    }
                }
                else if(result.Result && result.Data != null && result.Data.status == 1)
                {
                    if (r9_BType.Trim() == "1")
                    {
                        return RedirectToAction("index", "sys");
                    }
                    else if (r9_BType.Trim() == "2")
                    {
                        //通知易宝，我们已经知道客户支付成功
                        return Content("success");
                    }
                }
            }
            return Content("支付失败");
        }

    }
}
