using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Server.User;

namespace YxLiCai.FinaAdmin.Controllers
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


            Tools.LogHelper.Write("融资方充值回调", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + url + p1_MerId + r0_Cmd + r1_Code + r2_TrxId + r3_Amt + r4_Cur + r5_Pid + r6_Order + r7_Uid + r8_MP + r9_BType + rp_PayDate + rq_SourceFee + rq_TargetFee + hmac);

            if (YxLica.Tools.Pay.yeepay_online.Bussiness.IsTrueBack(p1_MerId, r0_Cmd, r1_Code, r2_TrxId, r3_Amt,
         r4_Cur, r5_Pid, r6_Order, r7_Uid, r8_MP, r9_BType, rp_PayDate, hmac))
            {
                Tools.LogHelper.Write("融资方充值回调易宝支付接口回调", url);
                //验证是否更新充值（该状态是否被更新了）            
                Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server = new Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();

                //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = server.GetFina_Recharge_Record_ModeNE(long.Parse(r6_Order));
                //获取用户账户信息
                ResultInfo<Fina_User_Count_Model> result = server.GetFina_User_Count_Model(UserContext.simpleUserInfoModel.Fer_account);
                if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 0)
                {
                    //if (decimal.Parse(r3_Amt) != rmodelRecord.Data.amount)
                    //{
                    //    return Content("Failure-用户充值金额和订单回传金额不一致");
                    //}
                    //暂时的修改住
                    //ResultInfo<bool> resultRec = server.UpdateUserFina_Recharge_Record_Model(r6_Order, UserContext.simpleUserInfoModel.Id.ToString(), UserContext.simpleUserInfoModel.Fer_account.ToString(), decimal.Parse(r3_Amt), result.Data.amount, result.Data.amount + decimal.Parse(r3_Amt), r2_TrxId, Convert.ToDecimal(rq_SourceFee), Convert.ToDecimal(rq_TargetFee));
                    //只是回传成功状态，页面写的充值的多少就是多少现在只是借助接口的回传状态
                    ResultInfo<bool> resultRec = server.UpdateUserFina_Recharge_Record_Model(r6_Order, UserContext.simpleUserInfoModel.Id.ToString(), UserContext.simpleUserInfoModel.Fer_account.ToString(), rmodelRecord.Data.amount, result.Data.amount, result.Data.amount + rmodelRecord.Data.amount, r2_TrxId, Convert.ToDecimal(rq_SourceFee), Convert.ToDecimal(rq_TargetFee));
                    //更新充值记录为充值成功       
                    if (resultRec.Result && resultRec.Data)
                    {
                        if (r9_BType.Trim() == "1")
                        {
                            return RedirectToAction("Fer_Recharge", "Financing");
                        }
                        else if (r9_BType.Trim() == "2")
                        {
                            //通知易宝，我们已经知道客户支付成功
                            return Content("success");
                        }
                    }
                }
                else if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 1)
                {
                    if (r9_BType.Trim() == "1")
                    {
                        return RedirectToAction("Fer_Recharge", "Financing");
                    }
                    else if (r9_BType.Trim() == "2")
                    {
                        //通知易宝，我们已经知道客户支付成功
                        return Content("success");
                    }
                }
            }
            return RedirectToAction("Fer_Recharge", "Financing");
            //return Content("支付失败");
        }

    }
}
