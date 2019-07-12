using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Model.UserRaise;
using YxLiCai.Tools.Pay.Yeepay;
using System.Threading;
using System.Threading.Tasks;
using YxLiCai.Model.User;

namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// {"amount":1,"card_last":"7831","card_top":"621700","identityid":"100163","merchantaccount":"10000419568","orderid":"150617192854100163109039","sign":"=","status":1,"yborderid":"411506177013478055"}
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
            string  orderid = Request["order_id"]; 
            string data = Request["data"];
            string encryptkey = Request["encryptkey"];
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(encryptkey))
            {
                string viewYbResult = YJPayUtil.checkYbCallBackResult(data, encryptkey);
                YxLiCai.Tools.LogHelper.Write("易宝回调", "userid:" + Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id + "-" + viewYbResult);
                if (viewYbResult != "验签未通过")
                {
                    try
                    {
                        YeePayCallBackModel sd = Newtonsoft.Json.JsonConvert.DeserializeObject<YeePayCallBackModel>(viewYbResult);

                        if (sd != null)
                        {
                            orderid = sd.orderid;
                            long userid = 0;
                            int type = 1;
                            decimal money = 0.00m;
                            YxLiCai.Server.User.UserRaiseService server = new YxLiCai.Server.User.UserRaiseService();
                            YxLiCai.Server.User.UserRaiseService us = new YxLiCai.Server.User.UserRaiseService(); 
                            YxLiCai.Model.UserRaise.UserRaiseProductModel model = us.GetUserRaiseProductModel(sd.orderid);
                            if (model != null)
                            {
                                userid = model.user_id;
                                type = model.product_type;
                                money = model.purchase_amount;
                            } 
                            if (sd.status == "1")//支付成功
                            {  
                                if (server.UserRaiseProductStatus(orderid))
                                {
                                    money = YxLiCai.Tools.Const.SystemConst.MoenyConvert(money);
                                    #region 发短信
                                    
                                    string P_type = YxLiCai.Tools.Const.SystemConst.GetProductType(type);
                                    string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("2");
                                    content = string.Format(content, P_type, money);
                                    string mobile = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Account;
                                    YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content); 
                                    #endregion
                                    #region 发送消息
                                    UserMessageModel mod = new UserMessageModel();
                                    mod.user_id = userid;
                                    mod.isread = 0;
                                    mod.sendtime = DateTime.Now;
                                    mod.title = "买入成功";
                                    mod.content = string.Format("将军大人，您成功的买入了e休理财的{0}产品<i>{1}</i>元，请随时关注它的成长吧。", P_type, money);
                                    new YxLiCai.Server.User.UserMessageServer().AddUserMessage(mod);
                                    #endregion
                                    return Content("success");
                                }
                            }
                            else 
                            {
                                //更新状态为支付失败
                                server.UpdateUserRaiseProductStatus(orderid);
                                //增加用户资金流水表
                                UserAmountRecModel userAmountRecModel = new UserAmountRecModel();
                                userAmountRecModel.Prodtype = type;
                                userAmountRecModel.amount = money;
                                userAmountRecModel.c_time = DateTime.Now;
                                userAmountRecModel.creator_id = 0;
                                userAmountRecModel.remark = "银行卡购买失败";
                                userAmountRecModel.user_id = userid;
                                userAmountRecModel.type = 3;//充值失败
                                userAmountRecModel.version = 0;
                                server.AddUserAmountRecModel(userAmountRecModel);
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


