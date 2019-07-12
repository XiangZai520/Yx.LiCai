/**
 * 融资方后台充值异步回调类
 * 
 */
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.UserFinancingManagement;


namespace YxLiCai.FinaAdmin.Controllers
{
    public class LLPayYBCallBackOnlineController : Controller
    {
        //
        // GET: /YeePayOnline/
        /// <summary>
        /// 连连网银支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult PayCallBack()
        {

            string url = Request.Url.ToString();

            string oid_partner = Request["oid_partner"];   //商户编号
            string sign_type = Request["sign_type"];      //签名方式
            string sign = Request["sign"];                  //签名
            string dt_order = Request["dt_order"];          //商户订单时间
            string no_order = Request["no_order"];                 //商户唯一订单号
            string oid_paybill = Request["oid_paybill"];                 //连连支付支付单号
            string money_order = Request["money_order"];              //交易金额
            string result_pay = Request["result_pay"];          //支付结果
            string settle_date = Request["settle_date"];              //清算日期
            string info_order = Request["info_order"];                //订单描述
            string pay_type = Request["pay_type"];          //支付方式
            string bank_code = Request["bank_code"];          //银行编号
            string no_agree = Request["no_agree"];          //签约协议号
            string id_type = Request["id_type"];          //证件类型
            string id_no = Request["id_no"];          //证件号码
            string acct_name = Request["acct_name"];          //银行账号姓名

            Tools.LogHelper.Write("融资方充值回调", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + url + "---" + oid_partner + "---" + sign_type + "---" + sign + "---" + dt_order + "---" + no_order + "---" + oid_paybill + "---" + money_order + "---" + result_pay + "---" + settle_date + "---" + info_order + "---" + pay_type + "---" + bank_code + "-----" + no_agree + "-----" + id_type + "----" + id_no + "----" + acct_name);

            if (result_pay == "SUCCESS")
            {
                //if (decimal.Parse(money_order) != rmodelRecord.Data.amount)
                //{
                //    return Content("Failure-用户充值金额和订单回传金额不一致");
                //}
                Tools.LogHelper.Write("融资方充值回调易宝支付接口回调", url);
                //验证是否更新充值（该状态是否被更新了）            
                Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server = new Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();

                //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = server.GetFina_Recharge_Record_ModeNE(long.Parse(no_order));
                //获取用户账户信息
                ResultInfo<Fina_User_Count_Model> result = server.GetFina_User_Count_Model(UserContext.simpleUserInfoModel.Fer_account);
                if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 0)
                {
                    //只是回传成功状态，页面写的充值的多少就是多少现在只是借助接口的回传状态
                    ResultInfo<bool> resultRec = server.UpdateUserFina_Recharge_Record_Model(no_order, UserContext.simpleUserInfoModel.Id.ToString(), UserContext.simpleUserInfoModel.Fer_account.ToString(), rmodelRecord.Data.amount, result.Data.amount, result.Data.amount + rmodelRecord.Data.amount, oid_paybill, 0.0m, 0.0m);
                    //更新充值记录为充值成功       
                    if (resultRec.Result && resultRec.Data)
                    {
                        return Json(new
                        {
                            ret_code = "0000",
                            ret_msg = "交易成功"
                        });
                    }
                }
                else if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 1)
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
