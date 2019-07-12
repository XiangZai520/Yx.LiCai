using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Collections.Specialized;
using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Admin.Models;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 连连网银回调
    /// </summary>
    public class LLWYPayController : Controller
    {
        //
        // GET: /LLWYPay/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CallBack_Return()
        {
            string oid_partner = Request["oid_partner"].ToString();
            string sign_type = Request["sign_type"].ToString();
            string sign_back = Request["sign"].ToString();
            string dt_order = Request["dt_order"].ToString();
            string no_order = Request["no_order"].ToString();
            string oid_paybill = Request["oid_paybill"].ToString();
            string money_order = Request["money_order"].ToString();
            string result_pay = Request["result_pay"].ToString();
            string settle_date = Request["settle_date"] == null ? "" : Request["settle_date"].ToString();
            string info_order = Request["info_order"] == null ? "" : Request["info_order"].ToString();
            string pay_type = Request["pay_type"] == null ? "" : Request["pay_type"].ToString();
            string bank_code = Request["bank_code"] == null ? "" : Request["bank_code"].ToString();
            StringBuilder message = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：");
            NameValueCollection paras = Request.Form;
            if (paras != null && paras.AllKeys != null & paras.AllKeys.Length > 0)
            {
                foreach (string item in paras.AllKeys)
                {
                    message.Append("," + item + ":" + Request.Form[item]);
                }
            }

            YxLiCai.Tools.LogHelper.Write("连连支付接口(大额)同步回调", message.ToString());

            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("sign_type", sign_type);
            sParaTemp.Add("dt_order", dt_order);
            sParaTemp.Add("no_order", no_order);
            sParaTemp.Add("oid_paybill", oid_paybill);
            sParaTemp.Add("money_order", money_order);
            sParaTemp.Add("result_pay", result_pay);
            sParaTemp.Add("settle_date", settle_date);
            sParaTemp.Add("info_order", info_order);
            sParaTemp.Add("pay_type", pay_type);
            sParaTemp.Add("bank_code", bank_code);

            string sign = YxLica.Tools.Pay.LLWYPay.YinTongUtil.addSign(sParaTemp, YxLica.Tools.Pay.LLWYPay.PartnerConfig.TRADER_PRI_KEY, YxLica.Tools.Pay.LLWYPay.PartnerConfig.MD5_KEY);
            if (sign_back == sign && result_pay.ToUpper().Trim() == "SUCCESS")
            {
                YxLiCai.Server.FactoringManage.FactoringManageService factoringManageService = new Server.FactoringManage.FactoringManageService();
                ResultInfo<UserRecharge> result = factoringManageService.GetUserRecharge(no_order);
                if (result.Result && result.Data != null && result.Data.status == 0)
                {
                    //更新充值记录为充值成功
                    UserCountService userCountService = new UserCountService();
                    long user_id = userCountService.GetUserIDByUserType().Data;
                    ResultInfo<bool> ri = factoringManageService.UpdateUserRechargeForRechargeSuccess(result.Data.id, user_id, Convert.ToDecimal(result.Data.amount), DateTime.Now, oid_paybill, 0, 0);
                }
            }
            return RedirectToAction("index", "home");
        }
        public ActionResult CallBack_Notify()
        {
            Byte[] fByteArray = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(fByteArray, 0, fByteArray.Length);
            string josnstr = System.Text.Encoding.Default.GetString(fByteArray);
            StringBuilder message = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：");
            message.Append(josnstr);
            YxLiCai.Tools.LogHelper.Write("连连支付接口(大额)异步回调", message.ToString());
            CallBack_NotifyInModel inModel = YxLiCai.Tools.SerializeHelper.JsonDeserialize<CallBack_NotifyInModel>(josnstr);
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", inModel.oid_partner);
            sParaTemp.Add("sign_type", inModel.sign_type);
            sParaTemp.Add("dt_order", inModel.dt_order);
            sParaTemp.Add("no_order", inModel.no_order);
            sParaTemp.Add("oid_paybill", inModel.oid_paybill);
            sParaTemp.Add("money_order", inModel.money_order);
            sParaTemp.Add("result_pay", inModel.result_pay);
            sParaTemp.Add("settle_date", inModel.settle_date);
            sParaTemp.Add("info_order", inModel.info_order);
            sParaTemp.Add("pay_type", inModel.pay_type);
            sParaTemp.Add("bank_code", inModel.bank_code);
            sParaTemp.Add("no_agree", inModel.no_agree);
            sParaTemp.Add("id_type", inModel.id_type);
            sParaTemp.Add("id_no", inModel.id_no);
            sParaTemp.Add("acct_name", inModel.acct_name);

            string sign = YxLica.Tools.Pay.LLWYPay.YinTongUtil.addSign(sParaTemp, YxLica.Tools.Pay.LLWYPay.PartnerConfig.TRADER_PRI_KEY, YxLica.Tools.Pay.LLWYPay.PartnerConfig.MD5_KEY);
            if (sign == inModel.sign && inModel.result_pay.ToUpper().Trim() == "SUCCESS")
            {
                YxLiCai.Server.FactoringManage.FactoringManageService factoringManageService = new Server.FactoringManage.FactoringManageService();
                ResultInfo<UserRecharge> result = factoringManageService.GetUserRecharge(inModel.no_order);
                if (result.Result && result.Data != null && result.Data.status == 0)
                {
                    //更新充值记录为充值成功
                    UserCountService userCountService = new UserCountService();
                    long user_id = userCountService.GetUserIDByUserType().Data;
                    ResultInfo<bool> ri = factoringManageService.UpdateUserRechargeForRechargeSuccess(result.Data.id, user_id, Convert.ToDecimal(result.Data.amount), DateTime.Now, inModel.oid_paybill, 0, 0);
                    if (ri.Result && ri.Data)
                    {
                        return Json(new
                        {
                            ret_code = "0000",
                            ret_msg = "交易成功"
                        });
                    }
                }
                else if (result.Result && result.Data != null && result.Data.status == 1)
                {
                    return Json(new
                    {
                        ret_code = "0000",
                        ret_msg = "交易成功"
                    });
                }
            }

            return Json(new
            {
                ret_code = "1111",
                ret_msg = "交易失败"
            });
        }

    }
}
