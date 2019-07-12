using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Yx.LiCai.App_Start;
using YxLica.Tools.Pay.LLWYPay;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
using YxLiCai.Tools.Const;

namespace Yx.LiCai.Controllers
{
    public class LLPayController : Controller
    {
        //
        // GET: /LLPay/

        public ActionResult NOTIFY_URL()
        {
            YxLiCai.Tools.LogHelper.Write("(前台)连连支付接口异步回调", "");
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0) //判断是否有带返回参数
            {
                if (!YinTongUtil.checkSign(sPara, PartnerConfig.YT_PUB_KEY, //验证失败
                    PartnerConfig.MD5_KEY))
                {
                    return Content(@"{""ret_code"":""9999"",""ret_msg"":""验签失败""}");
                }
                else
                {
                    YxLiCai.Tools.LogHelper.Write("(前台)连连支付接口异步回调交易成功", sPara["no_order"]);
                    string orderid = sPara["no_order"];
                    long userid = 0;
                    int type = 1;
                    decimal money = 0.00m;
                    YxLiCai.Server.User.UserRaiseService userRaiseService = new YxLiCai.Server.User.UserRaiseService();
                    YxLiCai.Model.UserRaise.UserRaiseProductModel userRaiseProductModel = userRaiseService.GetUserRaiseProductModel(orderid);
                    if (userRaiseProductModel != null)
                    {
                        userid = userRaiseProductModel.user_id;
                        type = userRaiseProductModel.product_type;
                        money = userRaiseProductModel.purchase_amount;
                    }
                    if (userRaiseService.UserRaiseProductStatus(orderid))
                    {
                        string userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, userid);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                        var smallUser = redis.Get<SmallUserInfo>(userkey);
                        string userBankCacheKey = string.Format(RedisCacheKey.UserBankInfoCache, userid);
                        var bankCardInfo = redis.Get<UserBankCardModel>(userBankCacheKey);
                        if (bankCardInfo != null)
                        {
                            var result = new UserInfoService().IsTrueBindBankCard(UserContext.simpleUserInfoModel.Id);
                            if (!(result.Result && result.Data))
                            {
                                //插入数据库
                                UserInfoService user = new UserInfoService();
                                UserBankCardModel userBankCardModel = new UserBankCardModel();
                                userBankCardModel.UserId = userid;
                                userBankCardModel.BankCard = bankCardInfo.BankCard;
                                userBankCardModel.BankName = bankCardInfo.BankName;
                                userBankCardModel.BankPhone = string.Empty;
                                userBankCardModel.Status = 1;
                                userBankCardModel.BankCode = bankCardInfo.BankCode;
                                userBankCardModel.FirstNum = bankCardInfo.BankCard.Substring(0, 4);
                                userBankCardModel.LastNum =
                                    bankCardInfo.BankCard.Substring(bankCardInfo.BankCard.Length - 4, 4);


                                if (!string.IsNullOrEmpty(sPara["no_agree"]))
                                {
                                    userBankCardModel.no_agree = sPara["no_agree"];
                                }
                                else
                                {
                                    userBankCardModel.no_agree = string.Empty;
                                }

                                user.BoundBankCard(userBankCardModel);


                                var userSmall = new SmallUserInfo(); //替换缓存数据
                                userSmall.Id = userid;
                                userSmall.Account = smallUser.Account;
                                userSmall.IsBindBank = 1;
                                userSmall.IsRealCard = smallUser.IsRealCard;
                                userSmall.MyCode = smallUser.MyCode;
                                userSmall.MyRealName = smallUser.MyRealName;
                                userSmall.MyRealCard = smallUser.MyRealCard;
                                userSmall.IsJiaoYIPW = smallUser.IsJiaoYIPW;
                                userSmall.BankName = userBankCardModel.BankName;
                                userSmall.LastBankNum = userBankCardModel.LastNum;
                                userSmall.BankCode = userBankCardModel.BankCode;
                                redis.Replace(userkey, userSmall, DateTime.Now.AddDays(5));
                            }
                        }
                        #region 发短信
                        string pType = YxLiCai.Tools.Const.SystemConst.GetProductType(type);
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("2");
                        content = string.Format(content, pType, money);
                        string mobile = smallUser.Account;
                        YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);
                        #endregion
                        #region 发送消息
                        UserMessageModel mod = new UserMessageModel();
                        mod.user_id = userid;
                        mod.isread = 0;
                        mod.sendtime = DateTime.Now;
                        mod.title = "买入成功";
                        mod.content = string.Format("将军大人，您成功的买入了e休理财的{0}产品<i>{1}</i>元，请随时关注它的成长吧。", pType, money);
                        new YxLiCai.Server.User.UserMessageServer().AddUserMessage(mod);
                        #endregion
                    }
                    return Content(@"{""ret_code"":""0000"",""ret_msg"":""交易成功""}");
                }
            }


            return Content(@"{""ret_code"":""9999"",""ret_msg"":""交易失败""}");
        }

        public SortedDictionary<string, string> GetRequestPost()
        {
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            try
            {
                string reqStr = readReqStr();
                if (!string.IsNullOrEmpty(reqStr))
                {
                    sArray = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(reqStr);
                }
            }
            catch(Exception ex)
            { 
                
            }
            return sArray;
        }

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
    }
}
