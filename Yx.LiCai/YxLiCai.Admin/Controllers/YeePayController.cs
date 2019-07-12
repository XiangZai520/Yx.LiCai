using System;
using System.Web.Mvc;
using YxLiCai.Admin;
using YxLiCai.Model;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Tools.Pay.Yeepay;

namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// 融资方充值易宝回类调{"amount":1,"card_last":"7831","card_top":"621700","identityid":"100163","merchantaccount":"10000419568","orderid":"150617192854100163109039","sign":"=","status":1,"yborderid":"411506177013478055"}
    /// </summary>   
    public class YeePayCallBackModel
    {
        public string merchantaccount { get; set; }
        public string orderid { get; set; }
        public string yborderid { get; set; }
        public string amount { get; set; }
        public string identityid { get; set; }
        public string card_top { get; set; }
        public string card_last { get; set; }
        public string status { get; set; }
        public string sign { get; set; }
    }
    /// <summary>
    /// 易宝回调地址
    /// </summary>
    public class YeePayController : Controller
    {
        /// <summary>
        /// 易宝回调地址
        /// </summary>
        /// <returns></returns>
        public ActionResult YeepayCallBack()
        {
            string data = Request["data"];
            string encryptkey = Request["encryptkey"];
            ///融资方还款方式
            string selecttype = Request["selecttype"];
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(encryptkey))
            {
                string viewYbResult = YJPayUtil.checkYbCallBackResult(data, encryptkey);
                YxLiCai.Tools.LogHelper.Write("易宝回调", viewYbResult);
                if (viewYbResult != "验签未通过")
                {
                    try
                    {
                        YeePayCallBackModel sd = Newtonsoft.Json.JsonConvert.DeserializeObject<YeePayCallBackModel>(viewYbResult);
                        if (sd != null)
                        {
                            if (sd.status == "1")
                            {
                                ///融资方业务操作类
                                YxLiCai.Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server = new YxLiCai.Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();
                                //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                                ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = server.GetFina_Recharge_Record_Model(long.Parse(sd.orderid));
                                //获取用户账户信息
                                ResultInfo<Fina_User_Count_Model> result = server.GetFina_User_Count_Model(UserContext.simpleUserInfoModel.Id);
                                //账户余额类
                                Fina_User_Count_Model modelCount = new Fina_User_Count_Model();

                                if (rmodelRecord.Result && rmodelRecord.Data != null)
                                {
                                    #region 插入用户充值记录表
                                    //充值成功修改该条充值记录的状态（改变成1充值成功）
                                    rmodelRecord.Data.status = 1;
                                    ResultInfo<bool> resultRec = new ResultInfo<bool>();//server.UpdateUserFina_Recharge_Record_Model(rmodelRecord.Data);
                                    if (result.Result && result.Data != null)
                                    {
                                        #region 帮助赵亮插入数据
                                        Financier_account_log log = new Financier_account_log();
                                        log.fer_id = UserContext.simpleUserInfoModel.Id;
                                        log.type = 1;
                                        log.account_source_id = 0;
                                        log.change_amount = decimal.Parse(sd.amount);
                                        log.remark = "融资方充值";
                                        var res = server.Add_financier_account_log(log);
                                        #endregion

                                        #region 融资方金钱账户存在信息时
                                        result.Data.amount = result.Data.amount + decimal.Parse(sd.amount);
                                        ResultInfo<bool> resultUPCount = server.UpdateFina_User_Count_Model(result.Data);

                                        //modelCount.fer_id = result.Data.fer_id;
                                        //modelCount.c_time = result.Data.c_time;
                                        //modelCount.amount_freeze = result.Data.amount_freeze;
                                        //modelCount.remark = result.Data.remark;
                                        //modelCount.amount = result.Data.amount + decimal.Parse(sd.amount);
                                        #endregion

                                        return Content("success");
                                    }
                                    #endregion

                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                    }

                }
            }
            return Content("faild");
        }

    }
}


