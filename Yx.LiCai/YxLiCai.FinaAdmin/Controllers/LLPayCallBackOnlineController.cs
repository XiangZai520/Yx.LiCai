using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.Mvc;
using YxLica.Tools.Pay.LLWYPay;
using YxLiCai.FinaAdmin.Models;
using YxLiCai.Model;
using YxLiCai.Model.UserFinancingManagement;

namespace YxLiCai.FinaAdmin.Controllers
{
    public class LLPayCallBackOnlineController : Controller
    {
        //
        // GET: /YeePayOnline/
        /// <summary>
        /// 连连网银支付同步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult PayCallBack()
        {

            string url = Request.Url.ToString();

            string oid_partner = Request["oid_partner"];   //商户编号
            string sign_type = Request["sign_type"];      //签名方式
            string sign_Re = Request["sign"];                  //签名
            string dt_order = Request["dt_order"];          //商户订单时间
            string no_order = Request["no_order"];                 //商户唯一订单号
            string oid_paybill = Request["oid_paybill"];                 //连连支付支付单号
            string money_order = Request["money_order"];              //交易金额
            string result_pay = Request["result_pay"];          //支付结果
            string settle_date = Request["settle_date"] == null ? "" : Request["settle_date"];              //清算日期
            string info_order = Request["info_order"] == null ? "" : Request["info_order"];                //订单描述
            string pay_type = Request["pay_type"] == null ? "" : Request["pay_type"];          //支付方式
            string bank_code = Request["bank_code"] == null ? "" : Request["bank_code"];          //银行编号

            StringBuilder message = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：");
            NameValueCollection paras = Request.Form;
            if (paras != null && paras.AllKeys != null & paras.AllKeys.Length > 0)
            {
                foreach (string item in paras.AllKeys)
                {
                    message.Append("," + item + ":" + Request.Form[item]);
                }
            }
            YxLiCai.Tools.LogHelper.Write("融资方充值连连支付接口(大额)同步回调1", message.ToString());
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("sign_type", sign_type);
            //            sParaTemp.Add("sign", sign);
            sParaTemp.Add("dt_order", dt_order);
            sParaTemp.Add("no_order", no_order);
            sParaTemp.Add("oid_paybill", oid_paybill);
            sParaTemp.Add("money_order", money_order);
            sParaTemp.Add("result_pay", result_pay);
            sParaTemp.Add("settle_date", settle_date);
            sParaTemp.Add("info_order", info_order);
            sParaTemp.Add("pay_type", pay_type);
            sParaTemp.Add("bank_code", bank_code);


            Tools.LogHelper.Write("融资方充值连连支付接口(大额)同步回调2", "FER_ID:" + UserContext.simpleUserInfoModel.Id + ",融资方账户ID：" + UserContext.simpleUserInfoModel.Fer_account + "--返回结果:" + url + "---" + oid_partner + "---" + sign_type + "---" + sign_Re + "---" + dt_order + "---" + no_order + "---" + oid_paybill + "---" + money_order + "---" + result_pay + "---" + settle_date + "---" + info_order + "---" + pay_type + "---" + bank_code);
            //加签
            string sign = YxLica.Tools.Pay.LLWYPay.YinTongUtil.addSign(sParaTemp, YxLica.Tools.Pay.LLWYPay.PartnerConfig.TRADER_PRI_KEY, YxLica.Tools.Pay.LLWYPay.PartnerConfig.MD5_KEY);
            YxLiCai.Tools.LogHelper.Write("同步回调类sign", sign + "----" + sign_Re + "----" + result_pay);
            if (sign_Re == sign && result_pay == "SUCCESS")
            {
                //if (decimal.Parse(money_order) != rmodelRecord.Data.amount)
                //{
                //    return Content("Failure-用户充值金额和订单回传金额不一致");
                //}
                Tools.LogHelper.Write("融资方充值连连支付接口(大额)同步回调3", url);
                //验证是否更新充值（该状态是否被更新了）            
                Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server = new Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();

                //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = server.GetFina_Recharge_Record_ModeNE(long.Parse(no_order));
                Tools.LogHelper.Write("同步rmodelRecord", "rmodelRecord.Result" + rmodelRecord.Result + "rmodelRecord.Data:" + rmodelRecord.Data + "rmodelRecord.Data.status :" + rmodelRecord.Data.status + "--" + UserContext.simpleUserInfoModel.Id + "/---" + UserContext.simpleUserInfoModel.Fer_account + "--" + DateTime.Now);
                  
                //获取用户账户信息
                ResultInfo<Fina_User_Count_Model> result = server.GetFina_User_Count_Model(UserContext.simpleUserInfoModel.Fer_account);
                if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 0)
                {
                    //只是回传成功状态，页面写的充值的多少就是多少现在只是借助接口的回传状态
                    ResultInfo<bool> resultRec = server.UpdateUserFina_Recharge_Record_Model(no_order, UserContext.simpleUserInfoModel.Id.ToString(), UserContext.simpleUserInfoModel.Fer_account.ToString(), rmodelRecord.Data.amount, result.Data.amount, result.Data.amount + rmodelRecord.Data.amount, oid_paybill, 0.0m, 0.0m);
                    //更新充值记录为充值成功       
                    if (resultRec.Result && resultRec.Data)
                    {
//                        return Content("<script> window.location.href = 'Financing/NEW_Fer_Recharge';</script>");
                        return RedirectToAction("NEW_Fer_Recharge", "Financing");
                    }
                }
            }
//            return Content("<script> window.location.href = 'Financing/NEW_Fer_Recharge';</script>");
            return RedirectToAction("NEW_Fer_Recharge", "Financing");
        }

        /// <summary>
        ///连连网银支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult CallBack_Notify()
        {
            YxLiCai.Tools.LogHelper.Write("1", "22222   "+DateTime.Now);
            SortedDictionary<string, string> sPara = GetRequestPost();
            YxLiCai.Tools.LogHelper.Write("2", "22222  " + DateTime.Now);
            YxLiCai.Tools.LogHelper.Write("融资方充值连连支付接口(大额)异步回调1", sPara.ToString() + DateTime.Now);
            YxLiCai.Tools.LogHelper.Write("3", "22222   " + DateTime.Now);
            if (sPara.Count > 0) //判断是否有带返回参数
            {
                YxLiCai.Tools.LogHelper.Write("4", "22222  " + DateTime.Now);
                if (YinTongUtil.checkSign(sPara, PartnerConfig.YT_PUB_KEY, //验证失败
                    PartnerConfig.MD5_KEY))
                {
                    YxLiCai.Tools.LogHelper.Write("5", "22222  " + DateTime.Now);
                    string no_order = sPara["no_order"];
                    string oid_paybill = sPara["oid_paybill"];
                    //if (decimal.Parse(money_order) != rmodelRecord.Data.amount)
                    //{
                    //    return Content("Failure-用户充值金额和订单回传金额不一致");
                    //}

                    Tools.LogHelper.Write("融资方充值连连支付接口(大额)异步回调2", "no_order:" + no_order + "----" + "oid_paybill:" + oid_paybill +"  时间："+ DateTime.Now);
                    //验证是否更新充值（该状态是否被更新了）            
                    Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server =
                        new Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();

                    //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                    ResultInfo<Fina_Recharge_Record_Model> rmodelRecord =
                        server.GetFina_Recharge_Record_ModeNE(long.Parse(no_order));
                    YxLiCai.Tools.LogHelper.Write("rmodelRecord", "rmodelRecord.Result" + rmodelRecord.Result + "rmodelRecord.Data:" + rmodelRecord.Data + "rmodelRecord.Data.status :" + rmodelRecord.Data.status+"--" + UserContext.simpleUserInfoModel.Id+"/---"+UserContext.simpleUserInfoModel.Fer_account+"--" + DateTime.Now);
                    //获取用户账户信息
                    ResultInfo<Fina_User_Count_Model> result =
                        server.GetFina_User_Count_Model(UserContext.simpleUserInfoModel.Fer_account);
                    if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 0)
                    {
                        YxLiCai.Tools.LogHelper.Write("6", "22222   " + DateTime.Now + "---" + UserContext.simpleUserInfoModel.Id+"/---"+UserContext.simpleUserInfoModel.Fer_account);
                        //只是回传成功状态，页面写的充值的多少就是多少现在只是借助接口的回传状态
                        ResultInfo<bool> resultRec = null;


                        try
                        {
                            YxLiCai.Tools.LogHelper.Write("进入try", "22222   " + DateTime.Now);
                            resultRec = server.UpdateUserFina_Recharge_Record_Model(no_order,
                             UserContext.simpleUserInfoModel.Id.ToString(),
                             UserContext.simpleUserInfoModel.Fer_account.ToString(), rmodelRecord.Data.amount,
                             result.Data.amount, result.Data.amount + rmodelRecord.Data.amount, oid_paybill, 0.0m,
                             0.0m);
                        }
                        catch (Exception ex)
                        {
                            YxLiCai.Tools.LogHelper.Write("10", "resultRec.Result：" + resultRec.Result + "---resultRec.Data:" + resultRec.Data + DateTime.Now);
                            YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                            YxLiCai.Tools.LogHelper.Write("报错回调", ex + "");
                            throw;
                        }

                        YxLiCai.Tools.LogHelper.Write("resultRec", "resultRec.Result" + resultRec.Result + "resultRec.Data:" + resultRec.Data + "--" + UserContext.simpleUserInfoModel.Id + "/---" + UserContext.simpleUserInfoModel.Fer_account + "--" + DateTime.Now);
                    
                        YxLiCai.Tools.LogHelper.Write("9", "resultRec.Result：" + resultRec.Result + "---resultRec.Data:" + resultRec.Data + DateTime.Now);
                        //更新充值记录为充值成功       
                        if (resultRec.Result && resultRec.Data)
                        {
                            YxLiCai.Tools.LogHelper.Write("7", "22222  " + DateTime.Now);
                            return Content(@"{""ret_code"":""0000"",""ret_msg"":""交易成功""}");
//                            return Json(new
//                            {
//                                ret_code = "0000",
//                                ret_msg = "交易成功"
//                            });
                        }
                    }
                    else if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 1)
                    {
                        YxLiCai.Tools.LogHelper.Write("8", "22222  " + DateTime.Now);
                        return Content(@"{""ret_code"":""0000"",""ret_msg"":""交易成功""}");
//                        return Json(new
//                        {
//                            ret_code = "0000",
//                            ret_msg = "交易成功"
//                        });
                    }
                }
            }
            Tools.LogHelper.Write("融资方充值连连支付接口(大额)异步回调3", "");
            YxLiCai.Tools.LogHelper.Write("9", "22222");
            return Content(@"{""ret_code"":""9999"",""ret_msg"":""交易失败""}");
//            return Json(new
//            {
//                ret_code = "9999",
//                ret_msg = "交易失败"
//            });
        }

        /// <summary>
        ///连连网银支付异步回调
        /// </summary>
        /// <returns></returns>
        public ActionResult CallBack_Notify2()
        {
            Byte[] fByteArray = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(fByteArray, 0, fByteArray.Length);
            string josnstr = System.Text.Encoding.Default.GetString(fByteArray);
            StringBuilder message = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：");
            message.Append(josnstr);
            YxLiCai.Tools.LogHelper.Write("融资方充值连连支付接口(大额)异步回调1", message.ToString());
            CallBack_NotifyInModel inModel = null;
            try
            {
                 inModel = YxLiCai.Tools.SerializeHelper.JsonDeserialize<CallBack_NotifyInModel>(josnstr);
                 //            CallBack_NotifyInModel inModel = YxLiCai.Tools.SerializeHelper.JsonDeserialize<CallBack_NotifyInModel>(josnstr);
                 YxLiCai.Tools.LogHelper.Write("异步返回类", inModel.result_pay);
                 SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                 sParaTemp.Add("oid_partner", inModel.oid_partner);
                 sParaTemp.Add("sign_type", inModel.sign_type);
                 sParaTemp.Add("sign", inModel.sign);
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
                 YxLiCai.Tools.LogHelper.Write("异步返回类sign", sign + "----" + inModel.sign + "----" + inModel.result_pay.ToUpper().Trim());
                 if (sign == inModel.sign && inModel.result_pay.ToUpper().Trim() == "SUCCESS")
                 {
                     //if (decimal.Parse(money_order) != rmodelRecord.Data.amount)
                     //{
                     //    return Content("Failure-用户充值金额和订单回传金额不一致");
                     //}

                     Tools.LogHelper.Write("融资方充值连连支付接口(大额)异步回调2", message.ToString());
                     //验证是否更新充值（该状态是否被更新了）            
                     Server.UserFinancingManagement.UserInfo_FinancingManagement_Services server = new Server.UserFinancingManagement.UserInfo_FinancingManagement_Services();

                     //获取用户充值时的记录信息来此（易宝回调时）更新充值记录状态
                     ResultInfo<Fina_Recharge_Record_Model> rmodelRecord = server.GetFina_Recharge_Record_ModeNE(long.Parse(inModel.no_order));
                     YxLiCai.Tools.LogHelper.Write("rmodelRecord", "订单:" + inModel.no_order + "--rmodelRecord.Result" + rmodelRecord.Result + "--rmodelRecord.Data:" + rmodelRecord.Data + "--rmodelRecord.Data.status :" + rmodelRecord.Data.status + "--" + UserContext.simpleUserInfoModel.Id + "/---" + UserContext.simpleUserInfoModel.Fer_account + "查询出来的信息：" + rmodelRecord.Data.fer_account_id + "--" + DateTime.Now);

                     //获取用户账户信息
                     ResultInfo<Fina_User_Count_Model> result = server.GetFina_User_Count_Model(rmodelRecord.Data.fer_account_id);
                     if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 0 && result.Result && result.Data != null)
                     {
                         //只是回传成功状态，页面写的充值的多少就是多少现在只是借助接口的回传状态
                         ResultInfo<bool> resultRec = server.UpdateUserFina_Recharge_Record_Model(inModel.no_order, rmodelRecord.Data.fer_id.ToString(), rmodelRecord.Data.fer_account_id.ToString(), rmodelRecord.Data.amount, result.Data.amount, result.Data.amount + rmodelRecord.Data.amount, inModel.oid_paybill, 0.0m, 0.0m);
                         YxLiCai.Tools.LogHelper.Write("resultRec", "订单:" + inModel.no_order + "resultRec.Result---" + resultRec.Result + "、  resultRec.Data--" + resultRec.Data + "--" + UserContext.simpleUserInfoModel.Id + "/---" + UserContext.simpleUserInfoModel.Fer_account + "--" + DateTime.Now);

                         //更新充值记录为充值成功       
                         if (resultRec.Result && resultRec.Data)
                         {
                             Tools.LogHelper.Write("(成功)异步回调1", message.ToString());
                             return Json(new
                             {
                                 ret_code = "0000",
                                 ret_msg = "交易成功"
                             });
                         }
                     }
                     else if (rmodelRecord.Result && rmodelRecord.Data != null && rmodelRecord.Data.status == 1)
                     {
                         Tools.LogHelper.Write("(成功)异步回调2", message.ToString());
                         return Json(new
                         {
                             ret_code = "0000",
                             ret_msg = "交易成功"
                         });
                     }
                 }
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                YxLiCai.Tools.LogHelper.Write("报错回调", ex+"");
                YxLiCai.Tools.LogHelper.Write("异步返回类", ex + "--" + inModel.result_pay);
                throw;
            }

            return Json(new
            {
                ret_code = "9999",
                ret_msg = "交易失败"
            });
        }




        #region 异步回调demo提供代码
        /// <summary>
        /// 获取POST过来通知消息，并以“参数名=参数值”的形式组成字典
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            string reqStr = readReqStr();

            SortedDictionary<string, string> sArray = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(reqStr);
            return sArray;
        }


        //从request中读取流，组成字符串返回
        public String readReqStr()
        {
            StringBuilder sb = new StringBuilder();
            Stream inputStream = Request.GetBufferlessInputStream();
            StreamReader reader = new StreamReader(inputStream, System.Text.Encoding.UTF8);

            String line = null;
            while ((line = reader.ReadLine()) != null)
            {
                sb.Append(line);
            }
            reader.Close();
            return sb.ToString();

        }
        #endregion


    }
}
