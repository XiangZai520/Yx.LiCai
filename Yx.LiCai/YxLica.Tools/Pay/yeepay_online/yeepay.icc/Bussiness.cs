using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Tools.Pay.Yeepay;

namespace YxLica.Tools.Pay.yeepay_online
{
    /// <summary>
    /// 借口业务操作类
    /// </summary>
    public class Bussiness
    {

        //商户账户编号
        public static string merchantAccount = "10001126856";//YeepayConfig.getMerchantAccount();

        //商户私钥（商户公钥对应的私钥）
        public static string merchantPrivatekey = "69cl522AV6q613Ii4W6u8K6XuW8vM1N6bFgyv769220IuYe9u37N4y7rI4Pl";// YeepayConfig.getMerchantPrivateKey();

        //支付url
        public static string payurl = "https://www.yeepay.com/app-merchant-proxy/node";
        /// <summary>
        /// 创建第三方充值URL
        /// </summary>
        /// <param name="p1_MerId">商户编号</param>
        /// <param name="p2_Order">商家订单号</param>
        /// <param name="p3_Amt">充值金额</param>
        /// <param name="p8_Url">回调url</param>
        /// <returns></returns>
        public string CreateRechargeUrl(string p2_Order, decimal p3_Amt, string p8_Url)
        {
            string hmac = GetBuyHmac(p2_Order, p3_Amt.ToString(), "CNY", "", "", "", p8_Url, "", "", "", "1");
            StringBuilder url = new StringBuilder(payurl);
            url.Append("?p0_Cmd=Buy");
            url.Append("&p1_MerId=");
            url.Append(merchantAccount);
            url.Append("&p2_Order=");
            url.Append(p2_Order);
            url.Append("&p3_Amt=");
            url.Append(p3_Amt);
            url.Append("&p4_Cur=CNY");
            url.Append("&p8_Url=");
            url.Append(p8_Url);
            url.Append("&pr_NeedResponse=1");
            url.Append("&hmac=");
            url.Append(hmac);
            return url.ToString();
        }
        /// <summary>
        /// 获取支付Hmac
        /// </summary>
        /// <param name="p2_Order"></param>
        /// <param name="p3_Amt"></param>
        /// <param name="p4_Cur"></param>
        /// <param name="p5_Pid"></param>
        /// <param name="p6_Pcat"></param>
        /// <param name="p7_Pdesc"></param>
        /// <param name="p8_Url"></param>
        /// <param name="p9_SAF"></param>
        /// <param name="pa_MP"></param>
        /// <param name="pd_FrpId"></param>
        /// <param name="pr_NeedResponse"></param>
        /// <param name="merchantId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GetBuyHmac(string p2_Order, string p3_Amt, string p4_Cur, string p5_Pid, string p6_Pcat, string p7_Pdesc,
    string p8_Url, string p9_SAF, string pa_MP, string pd_FrpId, string pr_NeedResponse)
        {
            string sbOld = "";
            //1
            sbOld += "Buy";
            sbOld += merchantAccount;
            sbOld += p2_Order;
            sbOld += p3_Amt;
            sbOld += p4_Cur;
            //2
            sbOld += p5_Pid;
            sbOld += p6_Pcat;
            sbOld += p7_Pdesc;
            sbOld += p8_Url;
            sbOld += p9_SAF;
            //3
            sbOld += pa_MP;
            sbOld += pd_FrpId;
            sbOld += pr_NeedResponse;
            //生成hmac
            string hmac = YxLiCai.Tools.Pay.yeepay_online.Digest.HmacSign(sbOld, merchantPrivatekey);
            return hmac;
        }
        //支付回调hmac是否正确
        public static bool IsTrueBack(string p1_MerId, string r0_Cmd, string r1_Code, string r2_TrxId, string r3_Amt,
            string r4_Cur, string r5_Pid, string r6_Order, string r7_Uid, string r8_MP, string r9_BType, string rp_PayDate, string hmac)
        {
            string sbOld = "";

            sbOld += p1_MerId;
            sbOld += r0_Cmd;
            sbOld += r1_Code;
            sbOld += r2_TrxId;
            sbOld += r3_Amt;

            sbOld += r4_Cur;
            sbOld += r5_Pid;
            sbOld += r6_Order;
            sbOld += r7_Uid;
            sbOld += r8_MP;

            sbOld += r9_BType;

            string nhmac = YxLiCai.Tools.Pay.yeepay_online.Digest.HmacSign(sbOld, merchantPrivatekey);
            return (hmac == nhmac);

        }
        //支付回调hmac是否正确
        public static bool IsTrueBackF(string p1_MerId, string r0_Cmd, string r1_Code, string r2_TrxId, string r3_Amt,
            string r4_Cur, string r5_Pid, string r6_Order, string r7_Uid, string r8_MP, string r9_BType, string rp_PayDate, string rb_BankId, string hmac)
        {
            string sbOld = "";

            sbOld += p1_MerId;
            sbOld += r0_Cmd;
            sbOld += r1_Code;
            sbOld += r2_TrxId;
            sbOld += r3_Amt;

            sbOld += r4_Cur;
            sbOld += r5_Pid;
            sbOld += r6_Order;
            sbOld += r7_Uid;
            sbOld += r8_MP;

            sbOld += r9_BType;
            sbOld += rb_BankId;
            string nhmac = YxLiCai.Tools.Pay.yeepay_online.Digest.HmacSign(sbOld, merchantPrivatekey);
            return (hmac == nhmac);

        }
    }
}
