using System;
using YxLiCai.Server.User;
using YxLiCai.Tools;

namespace YxLiCai.Server.Common
{
    /// <summary>
    /// 连连支付通用
    /// </summary>
    public class YeePayManager
    {
        private string IP;
        private string CallBackUrl;
        private string BankCardFirstNum;
        private string BankCardLastNum;

        public YeePayManager(int userType = 0)
        {
            IP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"] ?? "127.0.0.1";
            CallBackUrl = System.Configuration.ConfigurationManager.AppSettings["YeePaytCallBackUrl"];
        }
        /// <summary>
        /// 提现接口 -- 财务打款给 前台用户/融资方
        /// https://ok.yeepay.com/payapi/api/tzt/withdraw HTTP 请求方式：POST
        /// </summary>
        /// <param name="userID">收款用户id</param>
        /// <param name="productID">产品id</param>
        /// <param name="amount">提现金额</param>
        /// <param name="identityID">用户唯一标识</param>
        /// <param name="identityType">标识类型</param>
        /// <param name="userType">帐户类型 0：普通用户；1融资方账户；2平台账户</param>
        public string YeePayWithdraw(long userID, int productID, decimal amount, string identityID, string identityType, int userType = 0)
        {
            var pay = new YxLiCai.Tools.Pay.Yeepay.YJPay();
            amount = 1;//amount * 100;

            string retultStr = string.Empty;
            string logHead = string.Empty;

            if (userType == 0)
            {
                var bankresult = new UserInfoService().GetBankCard(userID);
                if (!(bankresult.Result && bankresult.Data != null))
                    return "error: 未绑定银行卡";

                BankCardFirstNum = bankresult.Data.FirstNum;
                BankCardLastNum = bankresult.Data.LastNum;
                logHead = "用户";
            }
            else
            {
                var fer_info = new YxLiCai.Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao().GetUserSimall(Convert.ToInt32(userID));
                if (fer_info == null || fer_info.AccountID == 0)
                    return "error: 账户不存在";
                else if (fer_info.IsBindBank == 0 || string.IsNullOrEmpty(fer_info.BankCard))
                    return "error: 未绑定银行卡";

                BankCardFirstNum = fer_info.First_num;
                BankCardLastNum = fer_info.Last_num;
                logHead = "融资方";
            }

            var requestID = YxLiCai.Tools.Util.Helper.generateOrderCode(userID, productID);
            var t1 = DateTime.Now;
            var t2 = new DateTime(1970, 1, 1);
            var t = t1.Subtract(t2).TotalSeconds;
            var transtime = (int)t; //交易发生时间，时间戳，例如：1361324896，精确到秒   

            LogHelper.Write(logHead + "易宝提现", "requestID:" + requestID + "-identityID:" + identityID + "-identityType:" + identityType + "-FirstNum:" +
                BankCardFirstNum + "-LastNum" + BankCardLastNum + ",amount:" + amount + ",ip:" + IP);

            retultStr = pay.Withdrawals(requestID, identityID, identityType,
                BankCardFirstNum, BankCardLastNum, amount, "NATRALDAY_NORMAL", IP, "", "");

            LogHelper.Write(logHead + "易宝提现返回", retultStr);

            return retultStr;
        }
    }
}
