using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using Yx.LiCai.Models;
using Yx.LiCai.Models.UserMoney;
using YxLiCai.Model;
using YxLiCai.Model.Order;
using YxLiCai.Model.User;
using YxLiCai.Model.UserAccumulatedEarnings;
using YxLiCai.Server.Order;
using YxLiCai.Server.Product;
using YxLiCai.Tools.Const;
using UserAssetsListOutModel = Yx.LiCai.Models.UserAssetsListOutModel;
using UserAssets_DefaultViewModel = Yx.LiCai.Models.UserAssets_DefaultViewModel;

namespace Yx.LiCai.Controllers
{
    public class UserAssetsController : BaseController
    {
        //
        // GET: /UserAssets/
        private OrderInfoService service = new OrderInfoService();
        //自有资产首页
        public ActionResult Index()
        {
            return View();
        }

        //个人资金流水页面
        public ActionResult User_record()
        {
            return View();
        }
        #region 交易记录
        #region 交易记录共用方法
        /// <summary>
        /// 交易记录共用方法
        /// </summary>
        /// <param name="ProductType">产品类型</param>
        /// <returns></returns>
        public ActionResult Get_User_record(int ProductType)
        {
            YxLiCai.Server.User.UserInfoService user = new YxLiCai.Server.User.UserInfoService();
            int record = 0;
            ResultInfo<List<UserAmountRecModel>> result =
                user.GetListUserAmountRecModel(UserContext.simpleUserInfoModel.Id, ProductType, 1, 10, out record);
            UserAmountRecModelList outModel = new UserAmountRecModelList
            {
                listModel = null,
                TotalCount = record,
                ProductType = ProductType
            };
            if (result.Result && result.Data.Count > 0)
            {
                outModel.listModel = result.Data;
                outModel.TotalCount = record;
                outModel.ProductType = ProductType;
                return View("UR_list", outModel);
            }
            return View("UR_list", outModel);
        }
        #endregion
  
        #region 个人流水资金数据分页获取（Ajax）

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ProductType"></param>
        /// <returns></returns>
        public ActionResult GetData_User_record(int CurrentPage, int Prodtype)
        {
            int IsLastPage = 0;
            int record = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            YxLiCai.Server.User.UserInfoService user = new YxLiCai.Server.User.UserInfoService();
            ResultInfo<List<UserAmountRecModel>> result =
                user.GetListUserAmountRecModel(UserContext.simpleUserInfoModel.Id, Prodtype, CurrentPage, 10, out record);

            if (result.Result && result.Data.Count > 0)
            {
                //检查是否最后一页
                if (CurrentPage * 10 >= record)
                {
                    IsLastPage = 1;
                }
                foreach (UserAmountRecModel list_item in result.Data)
                {
                    string type = "+";
                    sb.Append(" <li> <a href='#'><h3> ");
                    if (list_item.type == 0)
                    {
                        sb.Append("  <i class=\"plus-blue\">买</i>");
                        type = "+";
                    }
                    else if (list_item.type == 1)
                    {
                        sb.Append("  <i class=\"plus-red\">提</i>");
                        type = "-";
                    }
                    else if (list_item.type == 2)
                    {
                        sb.Append("  <i class=\"plus-red\">赎</i>");
                        type = "-";
                    }
                    else
                    {
                        sb.Append("  <i class=\"plus-ash\">买</i>");
                        type = "";
                    }
                    sb.Append(" <b>" + type + list_item.amount + "</b></h3><span class=\"record-time\">" + list_item.c_time +
                              "</span>");
                    sb.Append(" </a></li>");
                }
            }
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });
        }

        #endregion
        #endregion


        /// <summary>
        /// 月数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Month()
        {
            int ProductType = 1;
            long id = UserContext.simpleUserInfoModel.Id;
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = service.GetList(id, ProductType, 1, 10);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserAssetsViewModel> list = new List<UserAssetsViewModel>();
                UserAssetsViewModel item;
                foreach (YxLiCai.Model.ExtendModel.UserAssetsEx _item in result.Data)
                {
                    item = new UserAssetsViewModel(_item);
                    list.Add(item);
                }
                outModel.list = list;
                ResultInfo<int> ri_int = service.GetTotalCount(id, ProductType);
                outModel.TotalCount = ri_int.Data;
                return View("_list", outModel);
            }
            else
            {
                Yx.LiCai.Models.UserAssets_DefaultViewModel OModel = new UserAssets_DefaultViewModel();
                OModel.ProductType = ProductType;
                OModel.MoneyValue = "7.00";
                OModel.detail_type = "逐月涨0.5%，最高12%";
                OModel.detail_rule = "100起，随存随取";
                OModel.detail_Intro = "让每一分钱属于你的时候都在拼命创造价值";
                OModel.detail_Intro1 = "让每一分钱";
                OModel.detail_Intro2 = "属于你的时候";
                OModel.detail_Intro3 = "都在拼命创造价值";
                return View("_default", OModel);
            }
        }

        /// <summary>
        /// 季数据——3个月
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Quarter_3()
        {
            long id = UserContext.simpleUserInfoModel.Id;
            int ProductType = 2;
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = service.GetList(id, ProductType, 1, 10);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserAssetsViewModel> list = new List<UserAssetsViewModel>();
                UserAssetsViewModel item;
                foreach (YxLiCai.Model.ExtendModel.UserAssetsEx _item in result.Data)
                {
                    item = new UserAssetsViewModel(_item);
                    list.Add(item);
                }
                outModel.list = list;
                ResultInfo<int> ri_int = service.GetTotalCount(id, ProductType);
                outModel.TotalCount = ri_int.Data;
                return View("_list", outModel);
            }
            else
            {
                Yx.LiCai.Models.UserAssets_DefaultViewModel OModel = new UserAssets_DefaultViewModel();
                OModel.ProductType = ProductType;
                OModel.MoneyValue = "9.00";
                OModel.detail_type = "当日起息，每月付息";
                OModel.detail_rule = "支持赎回，100%本息保障";
                OModel.detail_Intro = "看钱生钱，爽爽哒";
                OModel.detail_Intro1 = "季季专享，";
                OModel.detail_Intro2 = "看钱生钱，";
                OModel.detail_Intro3 = "爽爽哒";
                return View("_default", OModel);
            }
        }

        /// <summary>
        /// 季数据——6个月
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Quarter_6()
        {
            long id = UserContext.simpleUserInfoModel.Id;
            int ProductType = 3;
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = service.GetList(id, ProductType, 1, 10);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserAssetsViewModel> list = new List<UserAssetsViewModel>();
                UserAssetsViewModel item;
                foreach (YxLiCai.Model.ExtendModel.UserAssetsEx _item in result.Data)
                {
                    item = new UserAssetsViewModel(_item);
                    list.Add(item);
                }
                outModel.list = list;
                ResultInfo<int> ri_int = service.GetTotalCount(id, ProductType);
                outModel.TotalCount = ri_int.Data;
                return View("_list", outModel);
            }
            else
            {
                Yx.LiCai.Models.UserAssets_DefaultViewModel OModel = new UserAssets_DefaultViewModel();
                OModel.ProductType = ProductType;
                OModel.MoneyValue = "11.00";
                OModel.detail_type = "当日起息，每月付息";
                OModel.detail_rule = "支持赎回，100%本息保障";
                OModel.detail_Intro = "看钱生钱，爽爽哒";
                OModel.detail_Intro1 = "季季专享，";
                OModel.detail_Intro2 = "看钱生钱，";
                OModel.detail_Intro3 = "爽爽哒";
                return View("_default", OModel);
            }
        }

        /// <summary>
        /// 年数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData_Year()
        {
            long id = UserContext.simpleUserInfoModel.Id;
            int ProductType = 4;
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = service.GetList(id, ProductType, 1, 10);
            if (result.Data != null && result.Data.Count > 0)
            {
                UserAssetsListOutModel outModel = new UserAssetsListOutModel(id, ProductType);
                List<UserAssetsViewModel> list = new List<UserAssetsViewModel>();
                UserAssetsViewModel item;
                foreach (YxLiCai.Model.ExtendModel.UserAssetsEx _item in result.Data)
                {
                    item = new UserAssetsViewModel(_item);
                    list.Add(item);
                }
                outModel.list = list;
                ResultInfo<int> ri_int = service.GetTotalCount(id, ProductType);
                outModel.TotalCount = ri_int.Data;
                return View("_list", outModel);
            }
            else
            {
                Yx.LiCai.Models.UserAssets_DefaultViewModel OModel = new UserAssets_DefaultViewModel();
                OModel.ProductType = ProductType;
                OModel.MoneyValue = "13.00";
                OModel.detail_type = "当日起息，每月付息";
                OModel.detail_rule = "支持赎回，100%本息保障";
                OModel.detail_Intro = "停不下来，365天赚不休";
                OModel.detail_Intro1 = "";
                OModel.detail_Intro2 = "停不下来，";
                OModel.detail_Intro3 = "365天赚不休";
                return View("_default", OModel);
            }
        }

        /// <summary>
        /// 异步获取自有资产列表
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ProductType"></param>
        /// <returns></returns>
        public ActionResult GetData_ajax(int CurrentPage, int ProductType)
        {
            int IsLastPage = 0;
            long id = UserContext.simpleUserInfoModel.Id;
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = service.GetList(id, ProductType,
                CurrentPage, 10);
            ResultInfo<int> ri_int = service.GetTotalCount(id, ProductType);
            int TotalCount = ri_int.Data;
            //检查是否最后一页
            if (CurrentPage * 10 >= TotalCount)
            {
                IsLastPage = 1;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (result.Data != null && result.Data.Count > 0)
            {
                List<UserAssetsViewModel> list = new List<UserAssetsViewModel>();
                UserAssetsViewModel item;
                foreach (YxLiCai.Model.ExtendModel.UserAssetsEx _item in result.Data)
                {
                    item = new UserAssetsViewModel(_item);
                    list.Add(item);
                }
                foreach (UserAssetsViewModel list_item in list)
                {
                    sb.Append("<div class='assets-item'>                        <p>" +
                              list_item.CreateTime.ToString("yyyy-MM-dd") +
                              "</p>                        <dl class='assets-box'>                            <dt>                                <span>" +
                              YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.TotalRate).ToString() +
                              "%</span>                            </dt>                            <dd>                                <div class='assets-rate'>                                    当前收益率" +
                              YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.CurrentRate).ToString() +
                              "% + <span>特享" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.ExRate).ToString() +
                              "%</span>                                </div>                                <ul class='assets-get clearfix'>                                    <li>                                        <strong>实投本金</strong>                                        <span><i>" +
                              YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.ActualMoney).ToString() +
                              "</i>元</span>                                    </li>                                    <li>                                        <strong>产生收益</strong>                                        <span><i class='color-r'>" +
                              YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.Income).ToString() +
                              "</i>元</span>                                    </li>                                </ul>                                <a  href='/UserAssets/PointerIncome?orderinfoid=" +
                              list_item.OrderInfoID.ToString() +
                              "' class='assets-to'></a>                            </dd>                        </dl>                    </div>");
                }
            }
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });
        }

        //自有账户资金流向
        public ActionResult PointerIncome()
        {
            long id = UserContext.simpleUserInfoModel.Id;
            int OrderInfoid = Convert.ToInt32(Request["orderinfoid"]);
            DateTime ctime = YxLiCai.Tools.Util.ParseHelper.ToDatetime(Request["ctime"]);
            string amount = Request["amount"];

            ViewBag.OrderInfoid = OrderInfoid;
            ViewBag.Ctime = ctime;
            ViewBag.Amount = amount;
            if (OrderInfoid == 0)
            {
                return View();
            }
            else
            {
                List<Yx.LiCai.Models.PointerIncomeOutModel> OutModel = new List<PointerIncomeOutModel>();
                var ri = new OrderProjectService().GetListOrderProduct(OrderInfoid, id);
                if (ri.Result && ri.Data != null)
                {
                    foreach (YxLiCai.Model.Project.ProjectModel item in ri.Data)
                    {
                        var item_outmodel = new PointerIncomeOutModel();
                        item_outmodel.Company = item.Borrower;
                        item_outmodel.RepaymentPeriod = item.LoanPeriod.ToString() + "个月";
                        item_outmodel.LendMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(item.Amount ?? 0);
                        OutModel.Add(item_outmodel);
                    }
                }
                return View(OutModel);
            }

        }

        /// <summary>
        /// 投资咨询及管理服务协议
        /// </summary>
        /// <returns></returns>
        public ActionResult PointerIncome_Agreement()
        {
            long id = UserContext.simpleUserInfoModel.Id;
            DateTime ctime = DateTime.Now.Date;
            string amount = String.Empty;
            if (!string.IsNullOrEmpty(Request["ctime"]))
            {
                ctime = YxLiCai.Tools.Util.ParseHelper.ToDatetime(Request["ctime"]);
            }
            if (!string.IsNullOrEmpty(Request["amount"]))
            {
                amount = Request["amount"];
            }

            ViewBag.Year = ctime.Year;
            ViewBag.Month = ctime.Month;
            ViewBag.Day = ctime.Day;
            ViewBag.RealName = UserContext.simpleUserInfoModel.MyRealName;
            ViewBag.MyRealCard = UserContext.simpleUserInfoModel.MyRealCard;
            ViewBag.Amount = SystemConst.MoenyConvert(YxLiCai.Tools.Util.ParseHelper.ToDecimal(amount));
            return View();
        }
    }
}
