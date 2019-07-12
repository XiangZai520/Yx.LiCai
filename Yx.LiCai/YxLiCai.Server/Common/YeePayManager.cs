using System;
using YxLiCai.Dao.WithdrawManager;
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
        public YeePayManager()
        {
            IP = System.Configuration.ConfigurationManager.AppSettings["UserIP"] ?? "127.0.0.1";
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
            else if (userType == 1)
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
        /// <summary>
        /// 支付接口 -- 融资方/财务 充值
        /// 请求地址https://ok.yeepay.com/payapi/api/tzt/directbindpay HTTP 请求方式：POST
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productID">产品ID</param>
        /// <param name="amount">充值金额 单位： 元</param>
        /// <param name="identityID">用户唯一标识</param>
        /// <param name="identityType">标识类型</param>
        /// <returns></returns>
        public string YeePayDirectBindPay(long userID, int productID, decimal amount, string identityID, string identityType)
        {
            var bankresult = new UserInfoService().GetBankCard(userID);
            if (!(bankresult.Result && bankresult.Data != null))
            {
                return "error: 未绑定银行卡";
            }
            var pay = new YxLiCai.Tools.Pay.Yeepay.YJPay();

            string orderId = YxLiCai.Tools.Util.Helper.generateOrderCode(userID, productID);
            DateTime t1 = DateTime.Now;
            DateTime t2 = new DateTime(1970, 1, 1);
            double t = t1.Subtract(t2).TotalSeconds;
            int transtime = (int)t;//交易发生时间，时间戳，例如：1361324896，精确到秒 

            string retultStr = pay.directBindPay(10, Convert.ToInt32(amount * 100), 156, identityID, 2, orderId,
                "", "", productID.ToString(), productID.ToString(), transtime, IP, CallBackUrl, bankresult.Data.FirstNum, bankresult.Data.LastNum);

            LogHelper.Write("易宝支付", retultStr);

            return retultStr;
        }
        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="type">类型1提现2赎回3保理提现4融资方提现5融资方放款</param>
        /// <param name="from_id">type对应表id</param>
        /// <param name="c_time"></param>
        /// <param name="creator_id"></param>
        /// <param name="mer_ord_id"></param>
        /// <param name="identityid"></param>
        /// <param name="identitytype"></param>
        /// <param name="card"></param>
        /// <param name="card_top"></param>
        /// <param name="card_last"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool YeePayWithdraw(int type, int from_id, int creator_id, string identityid, int identitytype, string card, string card_top, string card_last, decimal amount, string mer_ord_id="")
        {
            bool result = false;
            if (mer_ord_id.Trim() == "")
            {
                mer_ord_id = Guid.NewGuid().ToString("N");
            }
            try
            {
                DateTime c_time = DateTime.Now;
                amount = 0.01m;
                result = true;

                NewWithdrawDao newWithdrawDao = new NewWithdrawDao();
                bool b = newWithdrawDao.InsertWithdrawApply(type, from_id, c_time, creator_id, mer_ord_id, identityid, identitytype, card, card_top, card_last, amount);
                //暂时按照财务手动给客户打款，不调用第三方支付接口，所以直接返回
                return b;
                if (b)
                {
                    //调用接口
                    var pay = new YxLiCai.Tools.Pay.Yeepay.YJPay();
                    string retultStr = pay.Withdrawals(mer_ord_id, identityid, identitytype.ToString(),
                        card_top, card_last, amount * 100, "NATRALDAY_NORMAL", IP, "", "");
                    LogHelper.Write("易宝申请提现(总)返回", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + retultStr + ":{商家订单号:" + mer_ord_id + "}");

                    if (retultStr.IndexOf("error_code") < 0)
                    {
                        YxLiCai.Server.Common.ResponseModel.WithdrawResponse withdrawResponse = YxLiCai.Tools.SerializeHelper.JsonDeserialize<YxLiCai.Server.Common.ResponseModel.WithdrawResponse>(retultStr);
                        if (withdrawResponse.status.Trim().ToUpper() == "SUCCESS")
                        {
                            result = true;
                            //更新提现申请为成功
                            newWithdrawDao.UpdateWithdrawApplyStatus(mer_ord_id, withdrawResponse.ybdrawflowid, 1, DateTime.Now);
                        }
                    }
                    //增加错误备注
                    else
                    {
                        if (retultStr.Length > 1000)
                        {
                            retultStr = retultStr.Substring(0, 999);
                        }
                        newWithdrawDao.UpdateWithdrawApplyErrorRemark(mer_ord_id, retultStr, DateTime.Now);
                    }

                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("易宝申请提现(总)返回", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + ex.Message + ":{商家订单号:" + mer_ord_id + "}");
                result = false;
            }
            return result;
        }


    }
}
