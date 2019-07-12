using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Tools.Encrypt;
using YxLiCai.Tools;
using YxLiCai.Tools.SafeEncrypt;
using YxLiCai.Tools.Util;
using YxLica.Tools.Encrypt;

namespace YxLiCai.Tools.Pay.Yeepay
{
    public class YeepayConfig
    {
          
        /**
	     * 取得商户编号
	    */
	    public static string getMerchantAccount() {
            return "10000420698";//ConfigurationManager.AppSettings["merchantAccount"];
	    }
	
	    /**
	     * 取得商户私钥
	     */
	    public static string getMerchantPrivateKey() {
            return ConfigurationManager.AppSettings["merchantPrivateKey"];
	    }

	    /**
	     * 取得商户AESKey
	     */
	    public static string getMerchantAESKey() {
		    return (YxLiCai.Tools.Util.Helper.getRandom(16));
	    }

	    /**
	     * 取得易宝公玥
	     */
	    public static string getYeepayPublicKey() {
            return ConfigurationManager.AppSettings["yeepayPublicKey"];
	    }

	    /**
	     * 格式化字符串
	     */
	    public static string formatstring(string text) {
		    return (text == null ? "" : text.Trim());
	    }

 

	    /**
	     * 绑卡请求接口请求地址
	     */
	    public static string getBindBankcardURL() {
            return "https://ok.yeepay.com/api/tzt/invokebindbankcard";// ConfigurationManager.AppSettings["bindBankcardURL"];
	    }

	    /**
	     * 绑卡确认接口请求地址
	     */
	    public static string getConfirmBindBankcardURL() {
		    return ConfigurationManager.AppSettings["confirmBindBankcardURL"];
	    }

	    /**
	     * 支付接口请求地址
	     */
	    public static string getDirectBindPayURL() {
            return ConfigurationManager.AppSettings["directBindPayURL"];
	    }

	    /**
	     * 订单查询请求地址
	     */
	    public static string getPaymentQueryURL() {
            return ConfigurationManager.AppSettings["paymentQueryURL"];
	    }

	    /**
	     * 取现接口请求地址
	     */
	    public static string getWithdrawURL() {
            return ConfigurationManager.AppSettings["withdrawURL"];
	    }

	    /**
	     * 取现查询请求地址
	     */
	    public static string getQueryWithdrawURL() {
            return ConfigurationManager.AppSettings["queryWithdrawURL"];
	    }

	    /**
	     * 取现查询请求地址
	     */
	    public static string getQueryAuthbindListURL() {
            return ConfigurationManager.AppSettings["queryAuthbindListURL"];
	    }

	    /**
	     * 银行卡信息查询请求地址 
	     */
	    public static string getBankCardCheckURL() {
            return "https://ok.yeepay.com/payapi/api/bankcard/check";// ConfigurationManager.AppSettings["bankCardCheckURL"];
	    }

	    /**
	     * 支付清算文件下载请求地址 
	     */
	    public static string getPayClearDataURL() {
            return ConfigurationManager.AppSettings["payClearDataURL"];
	    }

	    /**
	     * 单笔退款请求地址 
	     */
	    public static string getRefundURL() {
            return ConfigurationManager.AppSettings["refundURL"];
	    }

	    /**
	     * 退款查询请求地址 
	     */
	    public static string getRefundQueryURL() {
            return ConfigurationManager.AppSettings["refundQueryURL"];
	    }

	    /**
	     * 退款清算文件请求地址 
	     */
	    public static string getRefundClearDataURL() {
		    return ConfigurationManager.AppSettings["refundClearDataURL"];
	    } 

    }

    public class APIURLConfig
    {
        static APIURLConfig()
        {
            //一键支付PC端网页收银台前缀
            //payWebPrefix = "https://ok.yeepay.com/payweb";//生产环境
            payWebPrefix = "http://mobiletest.yeepay.com/payweb";//测试环境

            //一键支付移动终端网页收银台前缀
            //payMobilePrefix = "https://ok.yeepay.com/paymobile";//生产环境
            payMobilePrefix = "http://mobiletest.yeepay.com/paymobile";//测试环境

            //一键支付API前缀
            apiprefix = "https://ok.yeepay.com/payapi";//生产环境
            //apiprefix = "http://mobiletest.yeepay.com/testpayapi";//测试环境


            //商户通用接口前缀
            //merchantPrefix = "https://ok.yeepay.com/merchant";//生产环境
            merchantPrefix = "http://mobiletest.yeepay.com/merchant";//测试环境

            invokebindbankcardURI = "/api/tzt/invokebindbankcard";
            //PC端网页收银台支付地址
            pcwebURI = "/api/pay/request";

            //移动终端网页收银台支付地址
            webpayURI = "/api/pay/request";
            ///解除银行卡绑定
            unbindURI = "/api/tzt/unbind";
            //支付接口
            directbindpay = "/api/tzt/directbindpay";

            confirmbindbankcardURI = "/api/tzt/confirmbindbankcard";

            //绑卡支付接口
            bindpayURI = "/api/bankcard/bind/pay/request";

            //提现查询接口
            drawrecordURI = "/api/tzt/drawrecord";

            //提现接口
            withdrawURI = "/api/tzt/withdraw";

            //发生短信验证码接口
            sendValidateCodeURI = "/api/validatecode/send";

            //确认支付
            confirmPayURI = "/api/async/bankcard/pay/confirm/validatecode";

            //支付结果查询接口
            queryPayResultURI = "/api/query/order";
            

                
            //获取绑卡列表
            authbindlistURI = "/api/bankcard/authbind/list";
            //获取绑卡列表
            bindlistURI = "/api/bankcard/bind/list";

            //根据银行卡卡号检查银行卡是否可以使用一键支付
            bankcardCheckURI = "/api/bankcard/check";
 

            //直接退款
            directFundURI = "/query_server/direct_refund";

            //交易记录查询
            queryOrderURI = "/query_server/pay_single";

 

            //退款订单查询
            queryRefundURI = "/query_server/refund_single";

            //获取消费清算对账单
            clearPayDataURI = "/query_server/pay_clear_data";

            //获取退款清算对账单
            clearRefundDataURI = "/query_server/refund_clear_data";


        }
        /// <summary>
        /// 解除银行卡绑定
        /// </summary>
        public static string unbindURI { get; set; }
        public static string drawrecordURI { get; set; }
        public static string withdrawURI { get; set; }
        public static string directbindpay
        { get; set; }
        
        public static string payWebPrefix
        { get; set; }

        public static string payMobilePrefix
        { get; set; }

        public static string apiprefix
        { get; set; }
 
        
        public static string merchantPrefix
        { get; set; }

        public static string pcwebURI
        { get; set; }

        public static string webpayURI
        { get; set; }
        public static string confirmbindbankcardURI
        { get; set; } 
        public static string creditWebpayURI
        { get; set; }

        public static string debitWebpayURI
        { get; set; }

        public static string bindpayURI
        { get; set; }

        public static string bindlistURI
        { get; set; }

        public static string bindcheckURI
        { get; set; }

        public static string bankcardCheckURI
        { get; set; }
        public static string invokebindbankcardURI
        { get; set; }
        
        public static string queryPayResultURI
        { get; set; }

        public static string authbindlistURI
        { get; set; }
 

        public static string directFundURI
        { get; set; }

        public static string queryOrderURI
        { get; set; }

        public static string queryRefundURI
        { get; set; }

        public static string sendValidateCodeURI
        { get; set; }

        public static string confirmPayURI
        { get; set; }

        public static string clearPayDataURI
        { get; set; }

        public static string clearRefundDataURI
        { get; set; }
    }
}
