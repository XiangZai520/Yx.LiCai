using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using YxLiCai.Tools.Encrypt;

namespace YxLiCai.Tools.Pay.Yeepay
{
    public class YJPay
    {
        //商户账户编号
        public static string merchantAccount = YeepayConfig.getMerchantAccount();

        //商户私钥（商户公钥对应的私钥）
        public static string merchantPrivatekey = YeepayConfig.getMerchantPrivateKey();

        //易宝支付分配的公钥（进入商户后台公钥管理，报备商户的公钥后分派的字符串）
        public static string yibaoPublickey = YeepayConfig.getYeepayPublicKey();

        //一键支付URL前缀
        public string apiprefix = APIURLConfig.apiprefix;
        //一键支付商户通用接口URL前缀
        public string apimercahntprefix = APIURLConfig.merchantPrefix;

        /// <summary>
        /// 易宝向用户发生短信验证码，即用户必须输入接收到的短信验证码后才能完成支付
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public string sendValidateCode(string orderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("orderid", orderid);
            sd.Add("merchantaccount", merchantAccount);

            //发生短信验证码
            string uri = APIURLConfig.sendValidateCodeURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }
         

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderid">商户订单号</param>
        /// <param name="validatecode">短信验证码</param>
        /// <returns></returns>
        public string confirmPay(string orderid, string validatecode)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("orderid", orderid);
            if (validatecode != null)
            {
                if (validatecode != "")
                {
                    sd.Add("validatecode", validatecode);
                }
            }

            sd.Add("merchantaccount", merchantAccount);

            //确认支付
            string uri = APIURLConfig.confirmPayURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 获取绑卡关系列表
        /// </summary>
        /// <param name="identityid">支付身份标识</param>
        /// <param name="identitytype">支付身份类型</param>
        /// <returns></returns>
        public string getBindList(string identityid, int identitytype)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("merchantaccount", merchantAccount);

            string uri = APIURLConfig.bindlistURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, false);

            return viewYbResult;
        }


        /// <summary>
        /// 绑卡确认接口
        /// </summary>
        /// <param name="requestid"></param>
        /// <param name="validatecode"></param>
        /// <returns></returns>
        public string confirmbindbankcard(string requestid, string validatecode)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("requestid", requestid);
            sd.Add("validatecode", validatecode);

            //根据银行卡卡号检查银行卡是否可以使用一键支付接口
            string uri = APIURLConfig.confirmbindbankcardURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 绑卡请求接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="requestid"></param>
        /// <param name="cardno"></param>
        /// <param name="idcardno"></param>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string invokebindbankcard(string identityid, string requestid, string cardno, string idcardno, string username, string phone, string ip, int identitytype)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("requestid", requestid);
            sd.Add("cardno", cardno);
            sd.Add("idcardtype", "01");
            sd.Add("idcardno", idcardno);
            sd.Add("username", username);
            sd.Add("phone", phone);
            sd.Add("userip", ip);

            //根据银行卡卡号检查银行卡是否可以使用一键支付接口
            string uri = APIURLConfig.invokebindbankcardURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 获取绑卡关系列表
        /// </summary>
        /// <param name="identityid">支付身份标识</param>
        /// <param name="identitytype">支付身份类型</param>
        /// <returns></returns>
        public string unbind(string card_top, string card_last, string identityid, int identitytype)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("card_last", card_last);
            sd.Add("card_top", card_top);
            sd.Add("merchantaccount", merchantAccount);

            string uri = APIURLConfig.unbindURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }


        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="orderexpdate"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="identityid"></param>
        /// <param name="identitytype"></param>
        /// <param name="orderid"></param>
        /// <param name="ua"></param>
        /// <param name="imei"></param>
        /// <param name="productdesc"></param>
        /// <param name="productname"></param>
        /// <param name="transtime"></param>
        /// <param name="userip"></param>
        /// <param name="callbackurl"></param>
        /// <param name="card_top"></param>
        /// <param name="card_last"></param>
        /// <returns></returns>
        public string directBindPay(int orderexpdate, decimal amount, int currency, string identityid, int identitytype,
            string orderid, string ua, string imei, string productdesc, string productname, int transtime,
            string userip, string callbackurl, string card_top, string card_last)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);
            sd.Add("transtime", transtime);
            sd.Add("currency", currency);
            sd.Add("amount", amount);
            sd.Add("productname", productname);
            sd.Add("productdesc", productdesc);
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("card_top", card_top);
            sd.Add("card_last", card_last);
            sd.Add("orderexpdate ", orderexpdate);
            sd.Add("callbackurl", callbackurl);
            sd.Add("imei", imei);
            sd.Add("userip", userip);
            sd.Add("ua", ua);
            string uri = APIURLConfig.directbindpay;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);
            return viewYbResult;
        }

        /// <summary>
        /// 提现查询接口
        /// </summary> 
        /// </summary>
        /// <param name="requestid"></param>
        /// <param name="ybdrawflowid"></param>
        /// <returns></returns>
        public string drawrecord(string requestid, string ybdrawflowid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("requestid", requestid);
            sd.Add("ybdrawflowid", ybdrawflowid);
            //发生短信验证码
            string uri = APIURLConfig.drawrecordURI; ;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 提现接口
        /// </summary>
        /// <param name="requestid"></param>
        /// <param name="identityid"></param>
        /// <param name="identitytype"></param>
        /// <param name="card_top"></param>
        /// <param name="card_last"></param>
        /// <param name="amount"></param>
        /// <param name="?"></param>
        /// <param name="drawtype"></param>
        /// <param name="userip"></param>
        /// <param name="imei"></param>
        /// <param name="ua"></param>
        /// <returns></returns>
        public string Withdrawals(string requestid, string identityid, string identitytype, string card_top, string card_last, decimal amount, string drawtype, string userip, string imei, string ua)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("requestid", requestid);
            sd.Add("identityid", identityid);
            sd.Add("identitytype",Convert.ToInt32(identitytype));
            sd.Add("card_top", card_top);
            sd.Add("card_last", card_last);
            sd.Add("amount",Convert.ToInt32(amount));
            sd.Add("currency", 156);
            sd.Add("drawtype", drawtype);
            sd.Add("imei", imei);
            sd.Add("userip", userip);
            sd.Add("ua", ua);
            //提现接口
            string uri = APIURLConfig.withdrawURI; ;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }
        /// <summary>
        /// 绑卡支付请求
        /// </summary>
        /// <param name="bindid">绑卡id</param>
        /// <param name="amount">支付金额（单位：分）</param>
        /// <param name="currency">币种</param>
        /// <param name="identityid">支付身份标识</param>
        /// <param name="identitytype">支付身份标识类型</param>
        /// <param name="orderid">商户订单号</param>
        /// <param name="other">其他用户身份信息</param>
        /// <param name="productcatalog">商品类别</param>
        /// <param name="productdesc">商品描述</param>
        /// <param name="productname">商品名称</param>
        /// <param name="transtime">交易时间</param>
        /// <param name="userip">用户ip</param>
        /// <param name="callbackurl">商户后台回调地址</param>
        /// <param name="fcallbackurl">商户前台回调地址</param>
        /// <param name="terminaltype">终端设备类型</param>
        /// <param name="terminalid">终端设备id</param>
        /// <returns></returns>
        public string bindPayRequest(string bindid, int amount, int currency, string identityid, int identitytype,
            string orderid, string other, string productcatalog, string productdesc, string productname, int transtime,
            string userip, string callbackurl, string fcallbackurl, int terminaltype, string terminalid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("bindid", bindid);
            sd.Add("amount", amount);
            sd.Add("currency", currency);
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            sd.Add("orderid", orderid);
            sd.Add("other", other);
            sd.Add("productcatalog", productcatalog);
            sd.Add("productdesc", productdesc);
            sd.Add("productname", productname);
            sd.Add("transtime", transtime);
            sd.Add("userip", userip);
            sd.Add("callbackurl", callbackurl);
            sd.Add("fcallbackurl", fcallbackurl);
            sd.Add("terminaltype", terminaltype);
            sd.Add("terminalid", terminalid);

            string uri = APIURLConfig.bindpayURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 查询支付结果（可以在确认支付接口后调用）
        /// </summary>
        /// <param name="orderid">商户订单号</param>
        /// <returns></returns>
        public string queryPayResult(string orderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);

            string uri = APIURLConfig.queryPayResultURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, false);

            return viewYbResult;
        }


        /// <summary>
        /// 查询绑卡信息
        /// </summary>
        /// <param name="identityid"></param>
        /// <param name="identitytype"></param>
        /// <returns></returns>
        public string authbindlist(string identityid, int identitytype)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount); 
            sd.Add("identityid", identityid);
            sd.Add("identitytype", identitytype);
            string uri = APIURLConfig.authbindlistURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, false);

            return viewYbResult;
        }


        /// <summary>
        /// 交易订单查询（商户通用接口）
        /// </summary>
        /// <param name="orderid">商户订单号</param>
        /// <param name="yborderid">易宝返回的订单号</param>
        /// <returns></returns>
        public string queryPayOrderInfo(string orderid, string yborderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            if (orderid != null)
            {
                if (orderid.Trim() != "")
                {
                    sd.Add("orderid", orderid);
                }
            }
            if (yborderid != null)
            {
                if (yborderid.Trim() != "")
                {
                    sd.Add("yborderid", yborderid);
                }
            }
            string uri = APIURLConfig.queryOrderURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 直接退款（商户通用接口）
        /// </summary>
        /// <param name="orderid">商户退款订单号</param>
        /// <param name="origyborderid">原来易宝支付交易订单号</param>
        /// <param name="amount">退款金额（单位：分）</param>
        /// <param name="currency">币种</param>
        /// <param name="cause">退款原因</param>
        /// <returns></returns>
        public string directRefund(string orderid, string origyborderid, int amount, int currency, string cause)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);
            sd.Add("origyborderid", origyborderid);
            sd.Add("amount", amount);
            sd.Add("currency", currency);
            sd.Add("cause", cause);

            string uri = APIURLConfig.directFundURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, true);

            return viewYbResult;

        }

        /// <summary>
        /// 查询退款订单信息（商户通用接口）
        /// </summary>
        /// <param name="orderid">商户退货订单号</param>
        /// <param name="yborderid">原来易宝支付退款流水号</param>
        /// <returns></returns>
        public string queryRefundOrder(string orderid, string yborderid)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("orderid", orderid);
            sd.Add("yborderid", yborderid);

            string uri = APIURLConfig.queryRefundURI;

            string viewYbResult = createMerchantDataAndRequestYb(sd, uri, false);

            return viewYbResult;

        }


        /// <summary>
        /// 根据银行卡卡号检查银行卡是否可以使用一键支付
        /// </summary>
        /// <param name="cardno">银行卡号</param>
        /// <returns></returns>
        public string bankCardCheck(string cardno)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("cardno", cardno);
            sd.Add("merchantaccount", merchantAccount);

            //根据银行卡卡号检查银行卡是否可以使用一键支付接口
            string uri = APIURLConfig.bankcardCheckURI;

            string viewYbResult = createDataAndRequestYb(sd, uri, true);

            return viewYbResult;
        }

        /// <summary>
        /// 获取消费清算对账单（商户通用接口）
        /// </summary>
        /// <param name="startdate">开始日期，如:2014-03-01</param>
        /// <param name="enddate">结束日期，如:2014-03-01</param>
        /// <returns></returns>
        public string getClearPayData(string startdate, string enddate)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("startdate", startdate);
            sd.Add("enddate", enddate);

            string uri = APIURLConfig.clearPayDataURI;

            string viewYbResult = createMerchantDataAndRequestYb2(sd, uri, false);

            return viewYbResult;

        }

        /// <summary>
        /// 获取退款清算对账单（商户通用接口）
        /// </summary>
        /// <param name="startdate">开始日期，如:2014-03-01</param>
        /// <param name="enddate">结束日期，如:2014-03-01</param>
        /// <returns></returns>
        public string getClearRefundData(string startdate, string enddate)
        {
            SortedDictionary<string, object> sd = new SortedDictionary<string, object>();
            sd.Add("merchantaccount", merchantAccount);
            sd.Add("startdate", startdate);
            sd.Add("enddate", enddate);

            string uri = APIURLConfig.clearRefundDataURI;

            string viewYbResult = createMerchantDataAndRequestYb2(sd, uri, false);

            return viewYbResult;

        }



        /// <summary>
        /// 将请求接口中的业务明文参数加密并请求一键支付接口
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        private string createDataAndRequestYb(SortedDictionary<string, object> sd, string apiUri, bool ispost)
        {
            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            sd.Add("sign", sign);


            //将对象转换为json字符串

            string bpinfo_json = SerializeHelper.JsonSerializer(sd);
            string datastring = AES.AesEncrypt(bpinfo_json, merchantAesKey);

            //将商户merchantAesKey用RSA算法加密
            string encryptkey = RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");

            String ybResult = "";

            if (ispost)
            {
                ybResult = YJPayUtil.payAPIRequest(apiprefix + apiUri, datastring, encryptkey, true);
            }
            else
            {
                ybResult = YJPayUtil.payAPIRequest(apiprefix + apiUri, datastring, encryptkey, false);
            }
            String viewYbResult = YJPayUtil.checkYbResult(ybResult);

            return viewYbResult;

        }

        /// <summary>
        /// 将请求接口中的业务明文参数加密并请求一键支付接口--商户通用接口
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        private string createMerchantDataAndRequestYb(SortedDictionary<string, object> sd, string apiUri, bool ispost)
        {
            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            sd.Add("sign", sign);


            //将对象转换为json字符串
            string bpinfo_json = SerializeHelper.JsonSerializer(sd);
            string datastring = AES.AesEncrypt(bpinfo_json, merchantAesKey);

            //将商户merchantAesKey用RSA算法加密
            string encryptkey = RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");

            String ybResult = "";

            if (ispost)
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, true);
            }
            else
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, false);
            }
            String viewYbResult = YJPayUtil.checkYbResult(ybResult);

            return viewYbResult;

        }

        /// <summary>
        /// 将请求接口中的业务明文参数加密并请求一键支付接口，单不对返回的数据进行解密，用于获取清算对账单接口--商户通用接口
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="apiUri"></param>
        /// <returns></returns>
        private string createMerchantDataAndRequestYb2(SortedDictionary<string, object> sd, string apiUri, bool ispost)
        {
            //随机生成商户AESkey
            string merchantAesKey = AES.GenerateAESKey();

            //生成RSA签名
            string sign = EncryptUtil.handleRSA(sd, merchantPrivatekey);
            sd.Add("sign", sign);


            //将对象转换为json字符串
            string bpinfo_json = SerializeHelper.JsonSerializer(sd);
            string datastring = AES.AesEncrypt(bpinfo_json, merchantAesKey);

            //将商户merchantAesKey用RSA算法加密
            string encryptkey = RSAFromPkcs8.encryptData(merchantAesKey, yibaoPublickey, "UTF-8");

            String ybResult = "";

            if (ispost)
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, true);
            }
            else
            {
                ybResult = YJPayUtil.payAPIRequest(apimercahntprefix + apiUri, datastring, encryptkey, false);
            }

            return YJPayUtil.checkYbClearResult(ybResult);

        }
    }

}