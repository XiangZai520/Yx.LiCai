using System.Collections.Generic;
using System.Web.Mvc;
using YxLiCai.Server.User;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Server.UserAccumulatedEarnings;
using YxLiCai.Model.UserAccumulatedEarnings;
using Yx.LiCai.Models.UserMoney;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// 会员中心Controller 平扬
    /// </summary>
    public class MemberController : BaseController
    {
        UserInfoService userService = new UserInfoService();
        UserAccumulatedEarningsService userMoney = new UserAccumulatedEarningsService();
        long id = UserContext.simpleUserInfoModel.Id;
        /// <summary>
        /// 会员首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var uid = UserContext.simpleUserInfoModel.Id;
            var result = userService.GetUserCount(uid);
            //特享金总额
            var result3 = userService.GetUserSpecialAssets(id);
            ViewBag.t_amount_invest = result3.Result == true ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result3.Data) : 0.00m;
            if (result.Result && result.Data != null)
            {
                return View(result.Data);
            }
            else
            {
                return View();
            }
        }
        #region 提现页面

        /// <summary>
        /// 提现页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdrawals()
        {
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                return RedirectToAction("uc_setting_status", "UserSetting", new { returnUrl = "/Member/Withdrawals" });
            }
            if (UserContext.simpleUserInfoModel.IsBindBank == 0)
            {
                //return RedirectToAction("add_bank", "UserSetting", new { returnUrl = "/Member/Withdrawals" });
                return RedirectToAction("Index", "Home");
            }

            var resultBank = new UserInfoService().GetBankCard(UserContext.simpleUserInfoModel.Id);
            if (resultBank.Result && resultBank.Data != null)
            {
                ViewBag.IsBindBank = 1;
                ViewBag.BankInfo = resultBank.Data;
            }
            else
            {
                ViewBag.IsBindBank = 0;
            }

 
 
            var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();//创建redis对象
            string redistkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.WithdrawalsCount, UserContext.simpleUserInfoModel.Id);//获取requestkey

            int WithdrawalsCount = redis.Get<int>(redistkey);//获取requestkey

            ViewBag.WithdrawalsCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WithdrawalsCount"]) - WithdrawalsCount;

            var result = userService.GetUserWithdrawals(UserContext.simpleUserInfoModel.Id);
            ViewBag.Money = result.Result ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result.Data) : 0m;

            var result1 = userService.GetUserPrincipal(UserContext.simpleUserInfoModel.Id);
            ViewBag.Principal = result1.Result ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result1.Data) : 0m;

            var result2 = userService.GetUserInterest(UserContext.simpleUserInfoModel.Id);
            ViewBag.Interest = result2.Result ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result2.Data) : 0m;

            string buyMoney = Request["buyMoney"];
            if (!string.IsNullOrEmpty(buyMoney))
            {
                ViewBag.buyMoney = buyMoney;
            }
            else
            {
                ViewBag.buyMoney = "";
            }

            return View();
        }
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public ActionResult UserWithdrawals(decimal money, string sallpwd)
        {
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                return RedirectToAction("uc_setting_status", "UserSetting");
            }
            if (UserContext.simpleUserInfoModel.IsBindBank == 0)
            {
                //return RedirectToAction("add_bank", "UserSetting");
                return RedirectToAction("Index", "Home");
            }

            if (money > 50000)
            {
                return Json(new ResultModel(false, "每天最多提现金额为50000元，请修改"), JsonRequestBehavior.AllowGet);
            }

            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(sallpwd, 32);
            var result = new UserInfoService().IsTrueSallPassWord(UserContext.simpleUserInfoModel.Id, pwd);
            if (result.Result)
            {
                if (result.Data)
                {
                    var resultBank = new UserInfoService().GetBankCard(UserContext.simpleUserInfoModel.Id);
                    UserRemptionService service = new UserRemptionService();
                    var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();//创建redis对象
                    string redistkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.WithdrawalsCount, UserContext.simpleUserInfoModel.Id);//获取requestkey
                    int WithdrawalsCount = redis.Get<int>(redistkey);//获取requestkey
                    int time = YxLiCai.Tools.Util.ParseHelper.ToInt(System.Configuration.ConfigurationManager.AppSettings["WithdrawalsCount"]) - 1;
                    if (WithdrawalsCount > time)
                    {
                        return Json(new ResultModel(false, "超过最大提现次数"), JsonRequestBehavior.AllowGet);
                    }

                    var b = service.UserWithdrawCash(UserContext.simpleUserInfoModel.Id, money, UserContext.simpleUserInfoModel.Account, UserContext.simpleUserInfoModel.MyRealName, UserContext.simpleUserInfoModel.Account, resultBank.Data.BankName, resultBank.Data.BankCard);
                    if (b)
                    {

                        WithdrawalsCount++;
                        DateTime tomorrow = DateTime.Now.AddDays(1);
                        tomorrow = tomorrow.Date;
                        redis.Add(redistkey, WithdrawalsCount, tomorrow);

                        #region 发短信
                        string ServerPhone = System.Configuration.ConfigurationManager.AppSettings["ServerPhone"];
                        string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("4");
                        content = string.Format(content, money, resultBank.Data.LastNum, resultBank.Data.BankName, ServerPhone);
                        string mobile = UserContext.simpleUserInfoModel.Account;
                        YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);  
                        #endregion

                        #region 发送消息
                        YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                        mod.user_id = UserContext.simpleUserInfoModel.Id;
                        mod.isread = 0;
                        mod.sendtime = DateTime.Now;
                        mod.title = "申请提现";
                        mod.content = string.Format("将军大人，您命令e休理财提现的<i>{0}</i>元，预计1-5个工作日将到账尾号为<i>{1}</i>の<i>{2}</i>银行卡中，请留意您的银行卡资金变化，如有疑问和问题请联系我们的小叶子。", money, resultBank.Data.LastNum, resultBank.Data.BankName);
                        new UserMessageServer().AddUserMessage(mod);
                        #endregion

                        return Json(new ResultModel(true, "提现成功"), JsonRequestBehavior.AllowGet);
                    }
                    return Json(new ResultModel(false, "提现失败"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResultModel(false, "交易密码错误，请修改"), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel(false, "交易密码错误，请修改"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提现页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawalsSuccess(decimal money)
        {
            ViewBag.Money = money;
            return View();
        }

        #endregion




        /// <summary>
        /// 累计收益
        /// </summary>
        /// <returns></returns>
        public ActionResult Plus_income()
        {
            return View();
        }
        /// <summary>
        /// 获取月账户累计收益
        /// </summary>
        /// <returns></returns>
        public JsonResult getMonth()
        {
            decimal money = userMoney.GetUserMonth(UserContext.simpleUserInfoModel.Id);
            return Json(money, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取季账户累计收益
        /// </summary>
        /// <returns></returns>
        public JsonResult getSeason()
        {
            //decimal money = userMoney.GetUserSeason(UserContext.simpleUserInfoModel.Id);
            //return Json(money, JsonRequestBehavior.AllowGet);
            return null;
        }
        /// <summary>
        /// 获取年账户累计收益
        /// </summary>
        /// <returns></returns>
        public JsonResult getYear()
        {
            decimal money = userMoney.GetUserYear(UserContext.simpleUserInfoModel.Id);
            return Json(money, JsonRequestBehavior.AllowGet);
        }

        /*开始分页*/
        /// <summary>
        /// 月数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Month()
        {
            int ProductType = 1;
            ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_month(id, 1, 15);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserMonthViewModel> list = new List<UserMonthViewModel>();
                UserMonthViewModel item;
                foreach (UserCountMonth_AccumulatedEarnings_Model _item in result.Data)
                {
                    item = new UserMonthViewModel(_item);
                    list.Add(item);
                }
                outModel.list1 = list;
                outModel.TotalCount = userMoney.GetTotalCount_month(id);
                return View("_list", outModel);
            }
            else
            {
                return View("_default");
            }
        }
        /// <summary>
        /// 季数据——3个月
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Quarter_3()
        {
            int ProductType = 2;
            ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_season(id, ProductType, 1, 15);

            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserSeanViewModel> list = new List<UserSeanViewModel>();
                UserSeanViewModel item;
                foreach (UserCountSeason_AccumulatedEarnings_Model _item in result.Data)
                {
                    item = new UserSeanViewModel(_item);
                    list.Add(item);
                }
                outModel.list2 = list;
                outModel.TotalCount = userMoney.GetTotalCount_season(id, ProductType);
                return View("_list", outModel);
            }
            else
            {
                return View("_default");
            }
        }
        /// <summary>
        /// 季数据——6个月
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Quarter_6()
        {
            int ProductType = 3;
            ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_season(id, ProductType, 1, 15);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserSeanViewModel> list = new List<UserSeanViewModel>();
                UserSeanViewModel item;
                foreach (UserCountSeason_AccumulatedEarnings_Model _item in result.Data)
                {
                    item = new UserSeanViewModel(_item);
                    list.Add(item);
                }
                outModel.list2 = list;
                outModel.TotalCount = userMoney.GetTotalCount_season(id, ProductType);
                return View("_list", outModel);
            }
            else
            {
                return View("_default");
            }
        }
        /// <summary>
        /// 年数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Year()
        {

            int ProductType = 4;
            ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_year(id, 1, 15); ;
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserYearViewModel> list = new List<UserYearViewModel>();
                UserYearViewModel item;
                foreach (UserCountYear_AccumulatedEarnings_Model _item in result.Data)
                {
                    item = new UserYearViewModel(_item);
                    list.Add(item);
                }
                outModel.list3 = list;
                outModel.TotalCount = userMoney.GetTotalCount_year(id);
                return View("_list", outModel);
            }
            else
            {
                return View("_default");
            }
        }
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ProductType"></param>
        /// <returns></returns>
        public ActionResult GetData_ajax(int CurrentPage, int ProductType)
        {
            int IsLastPage = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(" <div class='plus-income'> <div class='plus-income'>  <ul class='income-list'>");
            if (ProductType == 1)
            {
                ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_month(id, CurrentPage, 10);

                int TotalCount = userMoney.GetTotalCount_month(id);
                //检查是否最后一页
                if (CurrentPage * 10 >= TotalCount)
                {
                    IsLastPage = 1;
                }

                if (result.Data != null && result.Data.Count > 0)
                {
                    List<UserMonthViewModel> list = new List<UserMonthViewModel>();
                    UserMonthViewModel item;
                    foreach (UserCountMonth_AccumulatedEarnings_Model _item in result.Data)
                    {
                        item = new UserMonthViewModel(_item);
                        list.Add(item);
                    }
                    foreach (UserMonthViewModel list_item in list)
                    {
                        sb.Append(" <li> <a href='#'> ");
                        if (list_item.Type == 0)
                        {
                            sb.Append("<h3> <i class='plus-blue'>自</i> <b>+" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.EarningsAmount) + "  </b> </h3> <span>" + list_item.CreateTime.ToString("yyyy-MM-dd") + "</span>");
                        }
                        else
                        {
                            sb.Append("<h3> <i class='plus-red'>特</i> <b>+" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.EarningsAmount) + "</b> </h3>  <span>" + list_item.CreateTime.ToString("yyyy-MM-dd") + "</span>");
                        }
                        sb.Append(" </a> </li>");
                    }
                }

            }
            if (ProductType == 2)
            {
                ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_season(id, ProductType, CurrentPage, 10);

                int TotalCount = userMoney.GetTotalCount_month(id);
                //检查是否最后一页
                if (CurrentPage * 10 >= TotalCount)
                {
                    IsLastPage = 1;
                }

                if (result.Data != null && result.Data.Count > 0)
                {
                    List<UserSeanViewModel> list = new List<UserSeanViewModel>();
                    UserSeanViewModel item;
                    foreach (UserCountSeason_AccumulatedEarnings_Model _item in result.Data)
                    {
                        item = new UserSeanViewModel(_item);
                        list.Add(item);
                    }
                    foreach (UserSeanViewModel list_item in list)
                    {
                        sb.Append(" <li> <a href='#'> ");
                        sb.Append("<h3> <i class='plus-blue'>自</i> <b>+" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.EarningsAmount) + "  </b> </h3> <span>" + list_item.CreateTime.ToString("yyyy-MM-dd") + "</span>");
                        sb.Append(" </a> </li>");
                    }
                }

            }
            if (ProductType == 3)
            {
                ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_season(id, ProductType, CurrentPage, 10);

                int TotalCount = userMoney.GetTotalCount_month(id);
                //检查是否最后一页
                if (CurrentPage * 10 >= TotalCount)
                {
                    IsLastPage = 1;
                }

                if (result.Data != null && result.Data.Count > 0)
                {
                    List<UserSeanViewModel> list = new List<UserSeanViewModel>();
                    UserSeanViewModel item;
                    foreach (UserCountSeason_AccumulatedEarnings_Model _item in result.Data)
                    {
                        item = new UserSeanViewModel(_item);
                        list.Add(item);
                    }
                    foreach (UserSeanViewModel list_item in list)
                    {
                        sb.Append(" <li> <a href='#'> ");
                        sb.Append("<h3> <i class='plus-blue'>自</i> <b>+" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.EarningsAmount) + "  </b> </h3> <span>" + list_item.CreateTime.ToString("yyyy-MM-dd") + "</span>");
                        sb.Append(" </a> </li>");
                    }
                }

            }
            if (ProductType == 4)
            {
                ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> result = userMoney.GetListOrderByCreateTimeDesc_year(id, CurrentPage, 10);

                int TotalCount = userMoney.GetTotalCount_month(id);
                //检查是否最后一页
                if (CurrentPage * 10 >= TotalCount)
                {
                    IsLastPage = 1;
                }

                if (result.Data != null && result.Data.Count > 0)
                {
                    List<UserYearViewModel> list = new List<UserYearViewModel>();
                    UserYearViewModel item;
                    foreach (UserCountYear_AccumulatedEarnings_Model _item in result.Data)
                    {
                        item = new UserYearViewModel(_item);
                        list.Add(item);
                    }
                    foreach (UserYearViewModel list_item in list)
                    {
                        sb.Append(" <li> <a href='#'> ");
                        sb.Append("<h3> <i class='plus-blue'>自</i> <b>+" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.EarningsAmount) + "  </b> </h3> <span>" + list_item.CreateTime.ToString("yyyy-MM-dd") + "</span>");
                        sb.Append(" </a> </li>");
                    }
                }

            }
            sb.Append(" </ul> </div> </div>");
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });
        }

        /// <summary>
        /// 账户资产详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountAsset()
        {
            var uid = UserContext.simpleUserInfoModel.Id;
            var result = userService.GetUserCount(uid);
            //特享金总额
            var result3 = userService.GetUserSpecialAssets(id);
            ViewBag.t_amount_invest = result3.Result == true ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result3.Data) : 0.00m;

            ViewBag.MonthAmount = 0.00m;
            ViewBag.SeasonAmount = 0.00m;
            ViewBag.YearAmount = 0.00m;
            var result_raise = userService.GetUserRaiseMoney(uid);
            if (result_raise.Result && result_raise.Data != null)
            {
                ViewBag.MonthAmount = result_raise.Data.MyMoney;
                ViewBag.SeasonAmount = result_raise.Data.MyBlance;
                ViewBag.YearAmount = result_raise.Data.LockMoney;
            }
            YxLiCai.Model.UserRaise.UserCountModel model = new YxLiCai.Model.UserRaise.UserCountModel();
            if (result.Result && result.Data != null)
            {
                model.MyBlance = result.Data.MyBlance - result.Data.LockMoney;
                model.LockMoney = result.Data.LockMoney + result.Data.principal_fz;
                model.RaiseMoney = result.Data.principal + ViewBag.t_amount_invest - result.Data.principal_fz;
                return View(model);
            }
            else
            {

                model.MyBlance = 0;
                model.LockMoney = 0;
                model.RaiseMoney = 0;
                return View(model);
            }
        }
    }
}
