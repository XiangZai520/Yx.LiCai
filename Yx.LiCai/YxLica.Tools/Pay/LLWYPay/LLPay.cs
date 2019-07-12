using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Serialization;
using YxLiCai.Tools;

using YxLiCai.Tools.Util;

using YxLiCai.Tools.Const;


namespace YxLica.Tools.Pay.LLWYPay
{
    public class LLPay
    {
        /// <summary>
        /// RSA公钥
        /// </summary>
        private static string rsa_public = PartnerConfig.YT_PUB_KEY;
        /// <summary>
        /// RSA密钥
        /// </summary>
        private static string rsa_private = PartnerConfig.TRADER_PRI_KEY;
        /// <summary>
        /// MD5key
        /// </summary>
        private static string md5_key = PartnerConfig.MD5_KEY;
        /// <summary>
        /// 商户编号
        /// </summary>
        private static string oid_partner = PartnerConfig.OID_PARTNER;

        /// <summary>
        /// 获取POST过来通知消息，并以“参数名=参数值”的形式组成字典
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static SortedDictionary<string, string> GetRequestPost(string reqStr)
        {
            SortedDictionary<string, string> sArray = SerializeHelper.JsonDeserialize<SortedDictionary<string, string>>(reqStr);
            return sArray;
        }


        /// <summary>
        /// 银行卡bin查询接口
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> bankcardquery(string bankCard)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("sign_type", PartnerConfig.SIGN_TYPE);
            sParaTemp.Add("card_no", bankCard);
            sParaTemp.Add("pay_type", "D");
            sParaTemp.Add("flag_amt_limit", "0");
            sParaTemp.Add("sign", YinTongUtil.addSign(sParaTemp, rsa_private, md5_key));
            string req_data = YinTongUtil.dictToJson(sParaTemp);
            string str_result = YxLiCai.Tools.Util.UrlResponse.GetResponseString(ServerURLConfig.QUERY_USER_BANKCARD_URL, req_data);
            SortedDictionary<string, string> sPara = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, string>>(str_result);
            if (sPara != null && sPara.Count > 0)//判断是否有带返回参数
            {
                if (!YinTongUtil.checkSign(sPara, rsa_private, //验证失败
                    md5_key))
                {
                    return null;
                }
                else
                {
                    return sPara;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 银行卡bin查询接口
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        public static SortedDictionary<string, string> bankcardfirstquery(string bankCard)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("sign_type", PartnerConfig.SIGN_TYPE);
            sParaTemp.Add("card_no", bankCard);
            sParaTemp.Add("pay_type", "D");
            sParaTemp.Add("flag_amt_limit", "0");
            sParaTemp.Add("sign", YinTongUtil.addSign(sParaTemp, rsa_private, md5_key));
            string req_data = YinTongUtil.dictToJson(sParaTemp);
            string str_result = YxLiCai.Tools.Util.UrlResponse.GetResponseString(ServerURLConfig.QUERY_BANKCARD_URL, req_data);
            SortedDictionary<string, string> sPara = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, string>>(str_result);
            if (sPara != null && sPara.Count > 0)//判断是否有带返回参数
            {
                if (!YinTongUtil.checkSign(sPara, rsa_private, //验证失败
                    md5_key))
                {
                    return null;
                }
                else
                {
                    return sPara;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptype">商品</param>
        /// <param name="money"></param>
        /// <param name="bankCardNum"></param>
        /// <param name="myRealName"></param>
        /// <param name="myRealCard"></param>
        /// <param name="notify_url"></param>
        /// <param name="url_return"></param>
        /// <returns></returns>
        public static string getBaseParamDict(long user_id,int ptype, decimal money, string bankCardNum, string myRealName, string myRealCard, string notify_url, string url_return, string orderId)
        { 
            /**订单信息**/
            // 商户唯一订单号
            //string no_order = YinTongUtil.getCurrentDateTimeStr();
            string no_order = orderId;
            // 商户订单时间
            string dt_order = YinTongUtil.getCurrentDateTimeStr();
            // 交易金额 单位为RMB-元
            //string money_order = money.ToString();
            string money_order = "0.01";
            // 商品名称
            string name_goods =SystemConst.GetProductType(ptype);
            // 订单描述
            string info_order = "用户购买" + name_goods;

            /** 商户提交参数**/
            string version = PartnerConfig.VERSION;						//接口版本号 
            string sign_type = PartnerConfig.SIGN_TYPE;					//签名类型：RSA/MD5
            string busi_partner = PartnerConfig.BUSI_PARTNER; 			//业务类型 虚拟商品销售
            string userreq_ip = "";        		//IP *
            string url_order = "";
            string valid_order = "10080";								// 订单有效期 单位分钟，可以为空，默认7天
            string timestamp = YinTongUtil.getCurrentDateTimeStr();		//时间戳

            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("version", version);
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("user_id", user_id.ToString()); 
            sParaTemp.Add("app_request", "3");
            sParaTemp.Add("sign_type", sign_type);
            sParaTemp.Add("busi_partner", busi_partner);
            sParaTemp.Add("no_order", no_order);
            sParaTemp.Add("dt_order", dt_order);
            sParaTemp.Add("name_goods", name_goods);
            sParaTemp.Add("info_order", info_order);
            sParaTemp.Add("money_order", money_order);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("url_return", url_return);
            sParaTemp.Add("no_agree", "");
            sParaTemp.Add("valid_order", valid_order);
            sParaTemp.Add("id_type", "0");				    //证件类型
            sParaTemp.Add("id_no", myRealCard);  //身份证
            sParaTemp.Add("acct_name", myRealName);
            sParaTemp.Add("shareing_data", "");
            sParaTemp.Add("risk_item", createRiskItem());
            sParaTemp.Add("card_no", bankCardNum);
            sParaTemp.Add("sign", YinTongUtil.addSign(sParaTemp, rsa_private, md5_key));   
            string payurl = "https://yintong.com.cn/llpayh5/authpay.htm"; 
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id='payBillForm' action='" + payurl + "' method='post'>");
            string json = (new JavaScriptSerializer()).Serialize(sParaTemp);
            sbHtml.Append("<input type='hidden' name='req_data' value='" + json + "'/>");

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='tijiao' style='display:none;'></form>");
            sbHtml.Append("<script>document.forms['payBillForm'].submit();</script>");
            return sbHtml.ToString();

        }


        #region 解除银行卡
        /// <summary>
        /// 解除银行卡
        /// </summary>
        /// <param name="UseriD"></param>
        /// <param name="no_agree"></param>
        /// <returns></returns>
        public static string RelieveBankID(string UseriD, string no_agree)
        {
            string strResult = "";
            try
            {
                string ReliveUrl = "https://yintong.com.cn/traderapi/bankcardunbind.htm";
                SortedDictionary<string, string> sParaTemp = getBankDict(UseriD, no_agree);

                string json = (new JavaScriptSerializer()).Serialize(sParaTemp);
//                string jsonDate =SerializeHelper.JsonSerializer(query);
                strResult = UrlResponse.GetResponseString(ReliveUrl, json);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            
            }
            return strResult;

        }
        public static SortedDictionary<string, string> getBankDict(string UseriD, string no_agree)
        {
            string sign_type = PartnerConfig.SIGN_TYPE;					//签名类型：RSA/MD5
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", oid_partner);
            sParaTemp.Add("user_id", UseriD);
            sParaTemp.Add("platform", "");
            sParaTemp.Add("pay_type", "D");
            sParaTemp.Add("sign_type", sign_type);
            sParaTemp.Add("no_agree", no_agree);
            sParaTemp.Add("sign", YinTongUtil.addSign(sParaTemp, rsa_private, md5_key));
            return sParaTemp;
        }
        #endregion


        /**
         * 根据连连支付风控部门要求的参数进行构造风控参数
         * @return
         */
        private static String createRiskItem()
        {
            return "{\"frms_ware_category\":\"1999\",\"user_info_full_name\":\"你好\"}";
        }

    }
}
