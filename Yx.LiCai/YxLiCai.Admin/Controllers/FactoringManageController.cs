using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLica.Tools.Dic;
using YxLiCai.Admin.Models;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Model.User;
using YxLiCai.Server.FactoringManage;
using YxLiCai.Server.User;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 保理管理
    /// </summary>
    [FactoringManageAttribute]
    public class FactoringManageController : BaseController
    {
        //
        // GET: /FactoringManage/
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FactoringUserInfoOut outModel = new FactoringUserInfoOut();
            outModel.Today = DateTime.Now.ToString("yyyy-MM-dd");
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            if (user_id != 0)
            {
                outModel.user_id = user_id;
                UserInfoService userInfoService = new UserInfoService();
                ResultInfo<UserInfoModel> result = userInfoService.GetUserModel(user_id);
                if (result.Result && result.Data != null)
                {
                    outModel.RealName = result.Data.MyRealName;
                    if (!string.IsNullOrEmpty(result.Data.Phone))
                    {
                        outModel.Phone = result.Data.Phone.Substring(0, 3) + "***" + result.Data.Phone.Substring(7, 4);
                    }
                    else
                    {
                        outModel.Phone = "";
                    }
                    YxLiCai.Model.User.UserBankCardModel UserBankCard = userInfoService.GetBankCard(user_id).Data;
                    if (UserBankCard != null)
                    {
                        outModel.BankCardNo = UserBankCard.BankCard;
                        if (!string.IsNullOrEmpty(outModel.BankCardNo) && outModel.BankCardNo.Length > 8)
                        {
                            outModel.BankCardNo = outModel.BankCardNo.Substring(0, 6) + "***" + outModel.BankCardNo.Substring((outModel.BankCardNo.Length - 4), 4);
                        }
                    }
                }

                ResultInfo<UserAccountEx> ri = userCountService.GetUserAccountByUserID(user_id);
                if (ri.Result && ri.Data != null)
                {
                    UserAccountEx entity = ri.Data;
                    outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest + entity.amount_blance);
                    outModel.ZhaiQuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest);
                    outModel.QianKuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
                    outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_blance - entity.amount_blance_fz);
                    if (outModel.KeYongMoney < 0)
                    {
                        outModel.QianKuanMoney = 0 - outModel.KeYongMoney;
                        outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);

                    }
                }
            }
            return View(outModel);
        }
        /// <summary>
        /// 获取交易记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTradeByDate_ajax()
        {
            //user_id: user_id, Today: Today
            long user_id = Convert.ToInt64(Request["user_id"]);
            DateTime _today;
            try
            {
                _today = Convert.ToDateTime(Request["Today"]);
            }
            catch
            {
                return Json(new
                {

                });
            }

            DateTime s_time = _today;
            DateTime e_time = _today.AddDays(1);
            FactoringManageService factoringManageService = new FactoringManageService();
            ResultInfo<List<FactoringUserAccountLog>> result = factoringManageService.GetUserAccountLog(user_id, s_time, e_time);
            if (result.Result && result.Data != null)
            {
                var Rows = result.Data.ConvertAll(entity => new
                {
                    pjt_id = entity.pjt_id,
                    pjt_name = entity.pjt_name,
                    pjt_amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.pjt_amount),
                    change_amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.change_amount),
                    type = entity.type == 1 ? "转入" : "转出"
                });
                return Json(new
                {
                    Rows = Rows
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据"
                });
            }
        }
        /// <summary>
        /// 投资明细
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestmentDetail()
        {
            FactoringManageService factoringManageService = new FactoringManageService();
            ResultInfo<List<InvestmentDetailEx>> result = factoringManageService.GetInvestmentDetail();
            List<InvestmentDetailEx> outModel = result.Data;
            return View(outModel);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge()
        {
            FactoringUserInfoOut outModel = new FactoringUserInfoOut();
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            ResultInfo<UserAccountEx> ri = userCountService.GetUserAccountByUserID(user_id);
            if (ri.Result && ri.Data != null)
            {
                UserAccountEx entity = ri.Data;
                outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest + entity.amount_blance);
                outModel.ZhaiQuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest);
                outModel.QianKuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
                outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_blance - entity.amount_blance_fz);
                if (outModel.KeYongMoney < 0)
                {
                    outModel.QianKuanMoney = 0 - outModel.KeYongMoney;
                    outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);

                }
            }
            return View(outModel);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_ajax()
        {
            decimal RechargeAmount = 0;
            try
            {
                RechargeAmount = Convert.ToDecimal(Request["RechargeAmount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "充值金额格式不正确！"
                });
            }
            if (RechargeAmount <= 0)
            {
                return Json(new
                {
                    Message = "充值金额要大于0，请重新输入！"
                });
            }
            string TradePsw = Request["TradePsw"];
            //验证交易密码

            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            UserInfoService userInfoService = new UserInfoService();
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(TradePsw, 32);
            ResultInfo<bool> IsTruePsw = userInfoService.IsTrueSallPassWord(user_id, pwd);
            if (!IsTruePsw.Data)
            {
                return Json(new
                {
                    Message = "交易密码不正确！"
                });
            }

            FactoringManageService factoringManageService = new FactoringManageService();
            string mer_ord_id = Guid.NewGuid().ToString("N");
            ResultInfo<bool> result = factoringManageService.InsertUserRecharge(user_id, DateTime.Now, 1, RechargeAmount, mer_ord_id);
            string Domain = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            string backurl = System.Configuration.ConfigurationManager.AppSettings["adminYxlcUrl"] + "/YeePayOnline/PayCallBack";
            if (result.Result && result.Data)
            {
                //调用第三方充值
                RechargeAmount = 0.01m;
                YxLica.Tools.Pay.yeepay_online.Bussiness bussiness = new YxLica.Tools.Pay.yeepay_online.Bussiness();
                string url = bussiness.CreateRechargeUrl(mer_ord_id, RechargeAmount, backurl);
                return Json(new
                {
                    Message = "ok",
                    url = url
                });
            }
            else
            {
                return Json(new
                {
                    Message = "充值失败！"
                });
            }
        }
        /// <summary>
        /// 充值明细
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeDetail()
        {
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int status = Convert.ToInt32(Request["status"]);
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;

            FactoringManageService factoringManageService = new FactoringManageService();
            ResultInfo<List<UserRecharge>> result = factoringManageService.GetUserRechargeByUserID(user_id, s_ctime, e_ctime, status);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    type = (YxLica.Tools.Dic.CommonDic.RechargeType.Keys.Contains(entity.type) ? YxLica.Tools.Dic.CommonDic.RechargeType[entity.type] : ""),
                    status = entity.status == 1 ? "已支付" : "未支付"
                }).ToList();
                return Json(new
                {
                    Rows = rows
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据"
                });
            }
        }
        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdraw()
        {
            FactoringUserInfoOut outModel = new FactoringUserInfoOut();
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            ResultInfo<UserAccountEx> ri = userCountService.GetUserAccountByUserID(user_id);
            if (ri.Result && ri.Data != null)
            {
                UserAccountEx entity = ri.Data;
                outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest + entity.amount_blance);
                outModel.ZhaiQuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest);
                outModel.QianKuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
                outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_blance - entity.amount_blance_fz);
                if (outModel.KeYongMoney < 0)
                {
                    outModel.QianKuanMoney = 0 - outModel.KeYongMoney;
                    outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);

                }
            }
            return View(outModel);
        }
        /// <summary>
        /// 提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdraw_ajax()
        {
            decimal WithdrawAmount = 0;
            try
            {
                WithdrawAmount = Convert.ToDecimal(Request["WithdrawAmount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "提现金额格式不正确！"
                });
            }
            string TradePsw = Request["TradePsw"];
            //验证交易密码
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            UserInfoService userInfoService = new UserInfoService();
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(TradePsw, 32);
            ResultInfo<bool> IsTruePsw = userInfoService.IsTrueSallPassWord(user_id, pwd);
            if (!IsTruePsw.Data)
            {
                return Json(new
                {
                    Message = "交易密码不正确！"
                });
            }

            FactoringUserInfoOut outModel = new FactoringUserInfoOut();
            ResultInfo<UserAccountEx> ri = userCountService.GetUserAccountByUserID(user_id);
            if (ri.Result && ri.Data != null)
            {
                UserAccountEx entity = ri.Data;
                outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest + entity.amount_blance);
                outModel.ZhaiQuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest);
                outModel.QianKuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
                outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_blance - entity.amount_blance_fz);
                if (outModel.KeYongMoney < 0)
                {
                    outModel.QianKuanMoney = 0 - outModel.KeYongMoney;
                    outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);

                }
            }
            if (WithdrawAmount > outModel.KeYongMoney)
            {
                return Json(new
                {
                    Message = "提现金额超过可用余额，请重新输入！"
                });
            }
            if (outModel.KeYongMoney < 100 && WithdrawAmount != outModel.KeYongMoney)
            {
                return Json(new
                {
                    Message = "可用余额小于100时必须全部提现！"
                });
            }
            if (outModel.KeYongMoney >= 100 && WithdrawAmount < 100)
            {
                return Json(new
                {
                    Message = "提现金额不得小于100元，请重新输入！"
                });
            }
            //增加申请提现记录
            FactoringManageService factoringManageService = new FactoringManageService();

            //用户数据
            UserInfoModel userInfo = userInfoService.GetUserModel(user_id).Data;
            YxLiCai.Model.User.UserBankCardModel UserBankCard = userInfoService.GetBankCard(user_id).Data;
            string bk_name = "";
            string bk_card = "";
            if (UserBankCard != null)
            {
                bk_name = UserBankCard.BankName;
                bk_card = UserBankCard.BankCard;
            }
            ResultInfo<bool> ri_new = factoringManageService.WithdrawApply(user_id, WithdrawAmount, userInfo.LoginName, userInfo.MyRealName, userInfo.Phone, bk_name, bk_card);
            if (ri_new.Result && ri_new.Data)
            {
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "提现申请失败！"
                });
            }

        }
        /// <summary>
        /// 提现明细
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawDetail()
        {
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int status = Convert.ToInt32(Request["status"]);
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;

            FactoringManageService factoringManageService = new FactoringManageService();
            ResultInfo<List<UserWithdraw>> result = factoringManageService.GetUserWithdrawByUserID(user_id, s_ctime, e_ctime, status);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    type = "后台申请",
                    status = (CommonDic.WithdrawStatus.Keys.Contains(entity.status) ? CommonDic.WithdrawStatus[entity.status] : "")
                }).ToList();
                return Json(new
                {
                    Rows = rows
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据"
                });
            }
        }

        /// <summary>
        /// 充值----连连
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_LL()
        {
            FactoringUserInfoOut outModel = new FactoringUserInfoOut();
            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            ResultInfo<UserAccountEx> ri = userCountService.GetUserAccountByUserID(user_id);
            if (ri.Result && ri.Data != null)
            {
                UserAccountEx entity = ri.Data;
                outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest + entity.amount_blance);
                outModel.ZhaiQuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_invest);
                outModel.QianKuanMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
                outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_blance - entity.amount_blance_fz);
                if (outModel.KeYongMoney < 0)
                {
                    outModel.QianKuanMoney = 0 - outModel.KeYongMoney;
                    outModel.KeYongMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);

                }
            }
            return View(outModel);
        }
        /// <summary>
        /// 充值----连连
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_LL_ajax()
        {
            decimal RechargeAmount = 0;
            try
            {
                RechargeAmount = Convert.ToDecimal(Request["RechargeAmount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "充值金额格式不正确！"
                });
            }
            if (RechargeAmount <= 0)
            {
                return Json(new
                {
                    Message = "充值金额要大于0，请重新输入！"
                });
            }
            string TradePsw = Request["TradePsw"];
            //验证交易密码

            UserCountService userCountService = new UserCountService();
            long user_id = userCountService.GetUserIDByUserType().Data;
            UserInfoService userInfoService = new UserInfoService();
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(TradePsw, 32);
            ResultInfo<bool> IsTruePsw = userInfoService.IsTrueSallPassWord(user_id, pwd);
            if (!IsTruePsw.Data)
            {
                return Json(new
                {
                    Message = "交易密码不正确！"
                });
            }

            FactoringManageService factoringManageService = new FactoringManageService();
            string mer_ord_id = Guid.NewGuid().ToString("N");
            ResultInfo<bool> result = factoringManageService.InsertUserRecharge(user_id, DateTime.Now, 2, RechargeAmount, mer_ord_id);

            if (result.Result && result.Data)
            {
                return Json(new
                {
                    Message = "ok",
                    mer_ord_id = mer_ord_id
                });
            }
            else
            {
                return Json(new
                {
                    Message = "充值失败！"
                });
            }
        }
        /// <summary>
        /// 跳转连连中间页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge_LL_Redirect()
        {
            string mer_ord_id = Request["mer_ord_id"].Trim();
            YxLiCai.Server.FactoringManage.FactoringManageService factoringManageService = new Server.FactoringManage.FactoringManageService();
            ResultInfo<UserRecharge> result = factoringManageService.GetUserRecharge(mer_ord_id);
            if (result.Result && result.Data != null)
            {
                UserRecharge entity = result.Data;
                if (entity.status == 0)
                {
                    entity.amount = 0.01m;

                    LLPayOutModel outModel = new LLPayOutModel();
                    outModel.user_id = entity.user_id.ToString();
                    outModel.no_order = mer_ord_id;
                    outModel.dt_order = entity.c_time.ToString("yyyyMMddHHmmss");
                    outModel.money_order = entity.amount.ToString();
                    outModel.notify_url = System.Configuration.ConfigurationManager.AppSettings["adminYxlcUrl"] + "/LLWYPay/CallBack_Notify";
                    outModel.url_return = System.Configuration.ConfigurationManager.AppSettings["adminYxlcUrl"] + "/LLWYPay/CallBack_Return";

                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("version", outModel.version);
                    sParaTemp.Add("oid_partner", outModel.oid_partner);
                    sParaTemp.Add("user_id", outModel.user_id);
                    sParaTemp.Add("sign_type", outModel.sign_type);
                    sParaTemp.Add("busi_partner", outModel.busi_partner);
                    sParaTemp.Add("no_order", outModel.no_order);
                    sParaTemp.Add("dt_order", outModel.dt_order);
                    sParaTemp.Add("name_goods", outModel.name_goods);
                    sParaTemp.Add("pay_type", outModel.pay_type);
                    //sParaTemp.Add("info_order", outModel.info_order);
                    sParaTemp.Add("money_order", outModel.money_order);
                    sParaTemp.Add("notify_url", outModel.notify_url);
                    sParaTemp.Add("url_return", outModel.url_return);
                    //sParaTemp.Add("userreq_ip", outModel.userreq_ip);
                    //sParaTemp.Add("url_order", outModel.url_order);
                    //sParaTemp.Add("valid_order", outModel.valid_order);
                    sParaTemp.Add("timestamp", outModel.timestamp);
                    //sParaTemp.Add("risk_item", outModel.createRiskItem());

                    string sign = YxLica.Tools.Pay.LLWYPay.YinTongUtil.addSign(sParaTemp, YxLica.Tools.Pay.LLWYPay.PartnerConfig.TRADER_PRI_KEY, YxLica.Tools.Pay.LLWYPay.PartnerConfig.MD5_KEY);

                    outModel.sign = sign;

                    return View(outModel);
                }
            }
            return RedirectToAction("index", "home");

        }
    }
}
