using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Server.User;
using YxLiCai.Server.Product;
using YxLiCai;
using YxLiCai.Tools.Util;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Antlr.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YxLica.Tools.Pay.LLWYPay;
using YxLiCai.Model.User;
using YxLiCai.Tools.Const;

namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// 购买页面Controller 平扬
    /// </summary>
    public class BuyController : BaseController
    {
        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult BuyProduct(int ptype, int productId = 0, string addinterests = "", decimal addrate=0m)
        {
            if (ptype < 0)
            {
                return RedirectToAction("index", "home");
            }
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                return RedirectToAction("uc_setting_status", "UserSetting");
            }
            //获取我的余额
            ViewBag.Money = new UserInfoService().GetMyBalance(UserContext.simpleUserInfoModel.Id).Data;  
            //获取银行卡信息
            var result = new UserInfoService().GetBankCard(UserContext.simpleUserInfoModel.Id);
            if (result.Result && result.Data != null)
            {
                ViewBag.IsBindBank = 1;
                ViewBag.BankInfo = result.Data;
            }
            else
            {
                ViewBag.IsBindBank = 0;
            }

            ViewBag.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;

            ViewBag.Ptype = ptype;

            //获取当前售卖的产品id
            if (productId == 0)
            {
                productId = new ProductManager().GetSallProductId(ptype); 
            }
            if (productId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var _manager = new YxLiCai.Server.Product.ProductManager().ProductAvailableAmount(productId);
            if (_manager.Result && _manager.Data != 0)
            {
                ViewBag.AvailableAmount = Math.Round(_manager.Data, 2).ToString();
            }
            else
            {
                ViewBag.AvailableAmount = 0;
            }

            switch (ptype)
            {
                case 1:
                    ViewBag.ptypeDp = "月月盈";
                    break;
                case 2:
                    ViewBag.ptypeDp = "季季享（3个月）";
                    break;
                case 3:
                    ViewBag.ptypeDp = "季季享（6个月）";
                    break;
                case 4:
                    ViewBag.ptypeDp = "年年丰";
                    break;
                default:
                    ViewBag.ptypeDp = "月月盈";
                    break;
            }
            var result2 = new UserInfoService().GetUserAddInterestCount(UserContext.simpleUserInfoModel.Id);
            ViewBag.AddInterestCount = result2.Result == true ? result2.Data : 0;//可使用张数
            ViewBag.AddRate = addrate; //加息额度
            ViewBag.AddRate = addrate; //加息额度
            ViewBag.Addinterests = addinterests; //加息券ids
            ViewBag.ProductId = productId;

            ViewBag.buyMoney = string.Empty;
            if (!string.IsNullOrEmpty(Request["buyMoney"]))
            {
                ViewBag.buyMoney = Request["buyMoney"];
            }

            ViewBag.Ids = string.Empty;
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                ViewBag.Ids = Request["ids"];
            }

            if (!string.IsNullOrEmpty(Request["buytype"]))
            {
                ViewBag.buytype = Request["buytype"];
            }
            else
            {
                if (ViewBag.IsBindBank == 1)
                {
                    ViewBag.buytype = "2";
                }
                else
                {
                    ViewBag.buytype = "1";
                }
                
            }

            return View();
        }


        public ActionResult Buy_Product(int ptype, int productId = 0, string addinterests = "", decimal addrate = 0m)
        {
            if (ptype < 0)
            {
                return RedirectToAction("index", "home");
            }
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                return RedirectToAction("uc_setting_status", "UserSetting");
            }
            var userId = UserContext.simpleUserInfoModel.Id; 
            //获取我的余额
            ViewBag.Money = new UserInfoService().GetMyBalance(userId).Data;
            ViewBag.BankCard = "";
            //获取银行卡信息
            if (UserContext.simpleUserInfoModel.IsBindBank == 1)
            {
                ViewBag.IsBindBank = 1;
                ViewBag.BankInfo = new UserBankCardModel()
                {
                    BankCode = UserContext.simpleUserInfoModel.BankCode,
                    BankName = UserContext.simpleUserInfoModel.BankName,
                    LastNum = UserContext.simpleUserInfoModel.LastBankNum
                };
            } 
            else
            {
                ViewBag.IsBindBank = 0;
                string userBankCacheKey = string.Format(RedisCacheKey.UserBankInfoCache, userId);
                var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                var userbank = redis.Get<UserBankCardModel>(userBankCacheKey);
                if (userbank != null)
                {
                    ViewBag.BankCard = userbank.BankCard;
                } 
            }

            ViewBag.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;

            ViewBag.Ptype = ptype;

            //获取当前售卖的产品id
            if (productId == 0)
            {
                productId = new ProductManager().GetSallProductId(ptype);
            }
            if (productId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var _manager = new YxLiCai.Server.Product.ProductManager().ProductAvailableAmount(productId);
            if (_manager.Result && _manager.Data != 0)
            {
                ViewBag.AvailableAmount = Math.Round(_manager.Data, 2).ToString();
            }
            else
            {
                ViewBag.AvailableAmount = 0;
            }

            switch (ptype)
            {
                case 1:
                    ViewBag.ptypeDp = "月月盈";
                    break;
                case 2:
                    ViewBag.ptypeDp = "季季享（3个月）";
                    break;
                case 3:
                    ViewBag.ptypeDp = "季季享（6个月）";
                    break;
                case 4:
                    ViewBag.ptypeDp = "年年丰";
                    break;
                default:
                    ViewBag.ptypeDp = "月月盈";
                    break;
            }
            var result2 = new UserInfoService().GetUserAddInterestCount(UserContext.simpleUserInfoModel.Id);
            ViewBag.AddInterestCount = result2.Result == true ? result2.Data : 0;//可使用张数
            ViewBag.AddRate = addrate; //加息额度
            ViewBag.AddRate = addrate; //加息额度
            ViewBag.Addinterests = addinterests; //加息券ids
            ViewBag.ProductId = productId;

            ViewBag.buyMoney = string.Empty;
            if (!string.IsNullOrEmpty(Request["buyMoney"]))
            {
                ViewBag.buyMoney = Request["buyMoney"];
            }

            ViewBag.Ids = string.Empty;
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                ViewBag.Ids = Request["ids"];
            }

            if (!string.IsNullOrEmpty(Request["buytype"]))
            {
                ViewBag.buytype = Request["buytype"];
            }
            else
            {
                if (ViewBag.IsBindBank == 1)
                {
                    ViewBag.buytype = "2";
                }
                else
                {
                    ViewBag.buytype = "1";
                }

            } 
            return View();
        }

        /// <summary>
        /// 加息券页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddInterest(int ptype=1)
        {            
            var result = new UserInfoService().GetRateCoupons(UserContext.simpleUserInfoModel.Id);
            ViewBag.ptype = ptype; 
            string buyMoney = Request["buyMoney"];
            ViewBag.Ids=string.Empty;
            if (!string.IsNullOrEmpty(Request["ids"]))
            {
                ViewBag.Ids = Request["ids"];
            }

            if (!string.IsNullOrEmpty(buyMoney))
            {
                ViewBag.buyMoney = buyMoney;
            }
            else
            {
                ViewBag.buyMoney = "";
            }

            if (!string.IsNullOrEmpty(Request["buytype"]))
            {
                ViewBag.buytype = Request["buytype"];
            }
            else
            {
                ViewBag.buytype = "";
            }

            return View(result); 
        }

        /// <summary>
        /// 余额购买
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public JsonResult BalanceBuyProducts(int ptype, decimal money, string ids, string sallpwd)
        {  
            if (string.IsNullOrEmpty(sallpwd))
            {
                return Json(new ResultModel<bool>(2, "请输入交易密码", false), JsonRequestBehavior.AllowGet);
            }
            if (money < 100)
            {
                return Json(new ResultModel<bool>(2, "买入金额需大于100元", false), JsonRequestBehavior.AllowGet);
            }
            int productId = new ProductManager().GetSallProductId(ptype);
            if (productId == 0)
            {
                return Json(new ResultModel<bool>(2, "错误的参数", false), JsonRequestBehavior.AllowGet);
            }
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(sallpwd, 32);
            var result = new UserInfoService().IsTrueSallPassWord(UserContext.simpleUserInfoModel.Id, pwd);
            if (result.Result)
            {
                if (result.Data)
                {
                    lock (new object())
                    {
                        UserRaiseService service = new UserRaiseService();
                        var result1 = service.UserBlanceRaiseProduct(UserContext.simpleUserInfoModel.Id, money, productId, ids, ptype);
                        if (result1 == YxLiCai.Tools.Enums.UserRaiseProductEnmu.Success)
                        {
                            money = SystemConst.MoenyConvert(money);
                            #region 发送短信
                            string P_type = YxLiCai.Tools.Const.SystemConst.GetProductType(ptype);
                            string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("2");
                            content = string.Format(content, P_type, money);
                            string mobile = UserContext.simpleUserInfoModel.Account;
                            YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);
                            #endregion
                            #region 发送消息
                            UserMessageModel mod = new UserMessageModel();
                            mod.user_id = UserContext.simpleUserInfoModel.Id;
                            mod.isread = 0;
                            mod.sendtime = DateTime.Now;
                            mod.title = "买入成功";
                            mod.content = string.Format("将军大人，您成功的买入了e休理财的{0}产品<i>{1}</i>元，请随时关注它的成长吧。", P_type, money);
                            new UserMessageServer().AddUserMessage(mod);
                            #endregion
                        } 
                        return Json(ResultModel<bool>.Conclude(result1), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new ResultModel<bool>(2, "交易密码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel<bool>(2, "交易密码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 第三方支付购买
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public JsonResult PayBuyProducts(int ptype, decimal money, string ids, string sallpwd)
        {
            if (string.IsNullOrEmpty(sallpwd))
            {
                return Json(new ResultModel<bool>(2, "请输入交易密码", false), JsonRequestBehavior.AllowGet);
            }
            if (money < 100)
            {
                return Json(new ResultModel<bool>(2, "买入金额需大于100元", false), JsonRequestBehavior.AllowGet);
            }
            int productId = new ProductManager().GetSallProductId(ptype);
            if (productId == 0)
            {
                return Json(new ResultModel<bool>(2, "产品已经售罄", false), JsonRequestBehavior.AllowGet);
            }
            //1.先判断交易密码是否正确
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(sallpwd, 32);
            var result = new UserInfoService().IsTrueSallPassWord(UserContext.simpleUserInfoModel.Id, pwd);
            if (result.Result)
            {
                if (result.Data)
                {
                    //2.判断是否绑定了银行卡
                    var bankresult = new UserInfoService().GetBankCard(UserContext.simpleUserInfoModel.Id);
                    if (!(bankresult.Result && bankresult.Data != null))
                    {
                        return Json(new ResultModel<bool>(2, "先绑定银行卡", false), JsonRequestBehavior.AllowGet);
                    }
                    //3.生成接口需要的参数
                    string orderId = YxLiCai.Tools.Util.Helper.generateOrderCode(UserContext.simpleUserInfoModel.Id, productId);
                    DateTime t1 = DateTime.Now;
                    DateTime t2 = new DateTime(1970, 1, 1);
                    double t = t1.Subtract(t2).TotalSeconds;
                    int transtime = (int)t;//交易发生时间，时间戳，例如：1361324896，精确到秒 
                    YxLiCai.Tools.Pay.Yeepay.YJPay pay = new YxLiCai.Tools.Pay.Yeepay.YJPay();
                    string ip = YxLiCai.Tools.Util.Helper.GetIP();
                    string callbackUrl = HttpPageHelper.GetRemoteNetURL(SystemSiteEnum.MallWeb, YxLiCai.Tools.Const.SystemConst.YeePaytCallBackUrl);
                    //4.发起支付请求
                    string str = pay.directBindPay(10, ParseHelper.DecimalToInt(1), 156, UserContext.simpleUserInfoModel.Id.ToString(), 2, orderId, "", "", productId.ToString(), productId.ToString(), transtime, ip, callbackUrl, bankresult.Data.FirstNum, bankresult.Data.LastNum);
                    YxLiCai.Tools.LogHelper.Write("易宝支付", "userid:" + UserContext.simpleUserInfoModel.Id +"-"+ str);
                    if (str.IndexOf("error") < 0)
                    {
                        UserRaiseService service = new UserRaiseService();
                        var result2 = service.UserBankRaiseProduct(UserContext.simpleUserInfoModel.Id, money, productId, ids, ptype, orderId);
                        if (result2 == YxLiCai.Tools.Enums.UserRaiseProductEnmu.Success)
                        {
                            //return Json(ResultModel<bool>.Conclude(result2), JsonRequestBehavior.AllowGet);
                            return Json(new ResultModel<bool>(0, orderId, false), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(ResultModel<bool>.Conclude(YxLiCai.Tools.Enums.UserRaiseProductEnmu.Failed), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(ResultModel<bool>.Conclude(YxLiCai.Tools.Enums.UserRaiseProductEnmu.Failed), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new ResultModel<bool>(2, "交易密码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultModel<bool>(2, "交易密码错误，请修改", false), JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 银行卡支付购买-连连支付
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="money"></param>
        /// <param name="ids"></param>
        /// <param name="bankCardNum"></param>
        /// <param name="sallpwd"></param>
        /// <returns></returns>
        public JsonResult PayBuyProductsByLLFirst(int ptype, decimal money, string ids, string bankCardNum,string sallpwd)
        {
            if (string.IsNullOrEmpty(sallpwd))
            {
                return Json(new ResultModel<bool>(2, "请输入交易密码", false), JsonRequestBehavior.AllowGet);
            } 
            if (money < 100)
            {
                return Json(new ResultModel<bool>(2, "买入金额需大于100元", false), JsonRequestBehavior.AllowGet);
            }
            int productId = new ProductManager().GetSallProductId(ptype);
            if (productId == 0)
            {
                return Json(new ResultModel<bool>(2, "产品已经售罄", false), JsonRequestBehavior.AllowGet);
            }
            //1.先判断交易密码是否正确
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(sallpwd, 32);
            var result = new UserInfoService().IsTrueSallPassWord(UserContext.simpleUserInfoModel.Id, pwd);
            if (result.Result && result.Data)
            { 
                var userId = UserContext.simpleUserInfoModel.Id;
                //2.判断是否绑定了银行卡 
                var bankresult = new UserInfoService().GetBankCard(userId);
                if (bankresult.Result && bankresult.Data != null) //判断是否绑定银行卡
                {
                    bankCardNum = bankresult.Data.BankCard;
                }
                else
                {
                    if (string.IsNullOrEmpty(bankCardNum))
                    {
                        return Json(new ResultModel<bool>(2, "请输入银行卡号", false), JsonRequestBehavior.AllowGet);
                    }
                     SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>(); 
                sParaTemp = YxLica.Tools.Pay.LLWYPay.LLPay.bankcardfirstquery(bankCardNum); //判读银行卡是否正确
                    if (sParaTemp != null && sParaTemp["ret_code"] == "0000")
                    {
                        if (sParaTemp["card_type"] == "3")
                        {
                            return Json(new ResultModel<bool>(4, "抱歉不支持信用卡支付，请更换储蓄卡", false), JsonRequestBehavior.AllowGet);
                        }

                        string userBankCacheKey = string.Format(RedisCacheKey.UserBankInfoCache,userId);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache(); 
                        UserBankCardModel model = new UserBankCardModel
                        { 
                            BankCard = bankCardNum,
                            BankName = sParaTemp["bank_name"], 
                            BankCode = GetBankCode(sParaTemp["bank_code"]) 
                        }; 
                        redis.Add(userBankCacheKey,model,DateTime.Now.AddHours(1));
                    }
                    else
                    {
                        return Json(new ResultModel<bool>(2, "请重新输入银行卡号", false), JsonRequestBehavior.AllowGet);
                    }
                }           
                string orderId = YxLiCai.Tools.Util.Helper.generateOrderCode(UserContext.simpleUserInfoModel.Id, productId);
                UserRaiseService service = new UserRaiseService();
                var result2 = service.UserBankRaiseProduct(UserContext.simpleUserInfoModel.Id, money, productId, ids, ptype, orderId);
                if (result2 == YxLiCai.Tools.Enums.UserRaiseProductEnmu.Success)
                {
                    string url = "/Buy/PayBuyProductsByLLFirstView?ptype=" + ptype + "&money=" + money + "&ids=" +
                                ids +
                                "&bankCardNum=" + bankCardNum + "&orderId=" + orderId;
                    return Json(new ResultModel<bool>(0, url, true), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResultModel<bool>(1, "交易失败，请重试", false), JsonRequestBehavior.AllowGet);
                }  
            } 
            return Json(new ResultModel<bool>(3, "交易密码错误，请修改", false), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// post连连支付
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="money"></param>
        /// <param name="ids"></param>
        /// <param name="bankCardNum"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult PayBuyProductsByLLFirstView(int ptype, decimal money, string ids, string bankCardNum, string orderId)
        {
 
            string NOTIFY_URL = HttpPageHelper.GetRemoteNetURL(SystemSiteEnum.MallWeb, YxLiCai.Tools.Const.SystemConst.LLPayNotifyUrl);
            string URL_RETURN = HttpPageHelper.GetRemoteNetURL(SystemSiteEnum.MallWeb, YxLiCai.Tools.Const.SystemConst.LLPayUrlReturn); 
            string sHtmlText = LLPay.getBaseParamDict(UserContext.simpleUserInfoModel.Id, ptype, money, bankCardNum,
                UserContext.simpleUserInfoModel.MyRealName, UserContext.simpleUserInfoModel.MyRealCard, NOTIFY_URL, URL_RETURN, orderId); 
            return Content(sHtmlText);
        }

        public ActionResult URL_RETURN()
        {
            int type = 1;
            decimal money = 0.00m;
            string res_data = Request["res_data"];
 
            if (string.IsNullOrEmpty(res_data))
            {
                return Redirect("/Buy/BuyFail?ptype=" + type + "&money=" + money);
            }
            SortedDictionary<string, string> sPara = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, string>>(res_data);
            if (!YinTongUtil.checkSign(sPara, PartnerConfig.YT_PUB_KEY, //验证失败
                    PartnerConfig.MD5_KEY))
            {
                return Redirect("/Buy/BuyFail?ptype=" + type + "&money=" + money);
            }  
            if (sPara != null)
            {
                string orderid = string.Empty;
                orderid = sPara["no_order"];
                long userid = 0;
                YxLiCai.Server.User.UserRaiseService userRaiseService = new YxLiCai.Server.User.UserRaiseService();
                YxLiCai.Model.UserRaise.UserRaiseProductModel model = userRaiseService.GetUserRaiseProductModel(sPara["no_order"]);
                if (model != null)
                {
                    userid = model.user_id;
                    type = model.product_type;
                    money = model.purchase_amount;
                }
                if (sPara["result_pay"] == "SUCCESS") //支付成功
                {
                    if (userRaiseService.UserRaiseProductStatus(orderid))
                    {
                        money = YxLiCai.Tools.Const.SystemConst.MoenyConvert(money);

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
                        mod.content = string.Format("将军大人，您成功的买入了e休理财的{0}产品<i>{1}</i>元，请随时关注它的成长吧。", P_type,
                            money);
                        new YxLiCai.Server.User.UserMessageServer().AddUserMessage(mod);

                        #endregion

                    }
                    return Redirect("/Buy/BuySuccess?ptype=" + type + "&money=" + money);

                }
            }
            return Redirect("/Buy/BuyFail?ptype=" + type + "&money=" + money);
        }

        /// <summary>
        /// 购买成功
        /// </summary>
        /// <param name="ptype"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ActionResult BuySuccess(int ptype, decimal money)
        {
            ViewBag.Ptype = YxLiCai.Tools.Const.SystemConst.GetProductType(ptype);
            ViewBag.Money = money;

            ViewBag.NowDay = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.NextDay = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd");

            return View();
        }

        /// <summary>
        /// 买入失败
        /// </summary>
        /// <returns></returns>
        public ActionResult BuyFail()
        {
            if (Request["ptype"] != null)
            {
                ViewBag.ptype = Request["ptype"];
            }
            else
            {
                ViewBag.ptype = 1;
            }

            ViewBag.money = Request["buyMoney"];
            return View();
        }

        /// <summary>
        /// 没有加息券
        /// </summary>
        /// <returns></returns>
        public ActionResult NoCoupon() {
            return View();
        }

        /// <summary>
        /// 加息券说明
        /// </summary>
        /// <returns></returns>
        public ActionResult DocsCoupon() {
            return View();
        }

        /// <summary>
        /// 加息券使用规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleCoupon() {
            return View();
        }

        /// <summary>
        /// 投资咨询及管理服务协议
        /// </summary>
        /// <returns></returns>
        public ActionResult invest_serveAgreement()
        {
            return View();
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult queryOrderResult(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return Json(new ResultModel<bool>(1, "银行卡支付失败", false), JsonRequestBehavior.AllowGet);
            }
            YxLiCai.Tools.Pay.Yeepay.YJPay yy = new YxLiCai.Tools.Pay.Yeepay.YJPay();
            string result = yy.queryPayResult(orderId);
            YxLiCai.Tools.LogHelper.Write("易宝主动查询", "userid:" + UserContext.simpleUserInfoModel.Id + "-" + result);
            if (result.IndexOf("error") < 0)
            {
                MsgOrderStatusT mt = YxLiCai.Tools.SerializeHelper.JsonDeserialize<MsgOrderStatusT>(result);
                if (mt != null)
                {
                    if (mt.status == 1)
                    {
                        string orderid = mt.orderid;
                        YxLiCai.Server.User.UserRaiseService server = new YxLiCai.Server.User.UserRaiseService();
                        if (server.UserRaiseProductStatus(orderid))
                        {
                            #region 发短信
                            int type = 1;
                            decimal money = 0.00m;
                            YxLiCai.Server.User.UserRaiseService us = new YxLiCai.Server.User.UserRaiseService();

                            YxLiCai.Model.UserRaise.UserRaiseProductModel model = us.GetUserRaiseProductModel(orderid);
                            if (model != null)
                            {
                                type = model.product_type;
                                money = SystemConst.MoenyConvert(model.purchase_amount);
                            }
                            string P_type = YxLiCai.Tools.Const.SystemConst.GetProductType(type);
                            string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("2");
                            content = string.Format(content, P_type, money);
                            string mobile = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Account;
                            YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);
                            #endregion
                            #region 发送消息
                            UserMessageModel mod = new UserMessageModel();
                            mod.user_id = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id;
                            mod.isread = 0;
                            mod.sendtime = DateTime.Now;
                            mod.title = "买入成功";
                            mod.content = string.Format("将军大人，您成功的买入了e休理财的{0}产品<i>{1}</i>元，请随时关注它的成长吧。", P_type, money);
                            new YxLiCai.Server.User.UserMessageServer().AddUserMessage(mod);
                            #endregion
                        }
                    }
                    return Json(new ResultModel<bool>(0, mt.status.ToString(), false), JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new ResultModel<bool>(1, "银行卡支付失败", false), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Msssages_Error me = YxLiCai.Tools.SerializeHelper.JsonDeserialize<Msssages_Error>(result);
                return Json(new ResultModel<bool>(2,"支付失败，请重新支付(错误码为:" +me.error_code+")", false), JsonRequestBehavior.AllowGet);
            }
        }

        public class MsgOrderStatusT
        {
            public string merchantaccount { get; set; }
            public string orderid { get; set; }
            public string yborderid { get; set; }
            public int amount { get; set; }
            public string bindid { get; set; }
            public int bindvalidthru { get; set; }
            public string bank { get; set; }
            public string bankcode { get; set; }
            public int closetime { get; set; }
            public int bankcardtype { get; set; }
            public string lastno { get; set; }
            public string identityid { get; set; }
            public int identitytype { get; set; }
            public int status { get; set; }
            public string error_code { get; set; }
            public string error_msg { get; set; }
            public string sign { get; set; }

        }

        public string GetBankCode(string LLBankCode)
        {
            string bankCode = string.Empty;
            switch (LLBankCode)
            {
                case "01020000":
                    bankCode="ccb";//工商银行
                    break;
                case "01030000":
                    bankCode="abc";//农业银行 
                    break;
                case "01040000":
                    bankCode="boc";//中国银行
                    break;
                case "01050000":
                    bankCode="ccb";//建设银行
                    break;
                case "03080000":
                    bankCode="cmb";//招商银行
                    break;
                case "03100000":
                    bankCode="spdb";//浦发银行
                    break;
                case "03030000":
                    bankCode="ceb";//光大银行
                    break;
                case "03070000":
                    bankCode="picc";//平安银行
                    break;
                case "03040000":
                    bankCode="hxb";//华夏银行
                    break;
                case "03090000":
                    bankCode="cib";//兴业银行
                    break;
                case "03020000":
                    bankCode="citic";//中信银行
                    break;
                case "01000000":
                    bankCode= "post";//储蓄银行(邮政)
                    break;
                case "03050000":
                    bankCode="cmbc";//民生银行
                    break;
                case "03060000":
                    bankCode="gdb";//广发银行
                    break;

            }

            return bankCode;
        }
    }
}
