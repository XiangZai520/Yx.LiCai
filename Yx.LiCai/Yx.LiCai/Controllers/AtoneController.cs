using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yx.LiCai.App_Start;
using Yx.LiCai.Models;
using YxLiCai.Model;
using YxLiCai.Model.Order;
using YxLiCai.Model.Product;
using YxLiCai.Server.Order;
using YxLiCai.Server.Product;
using YxLiCai.Server.Redemption;
using YxLiCai.Server.User;
using System.Threading;
using System.Threading.Tasks;
using YxLiCai.Model.User;
using YxLiCai.Tools.Const;

namespace Yx.LiCai.Controllers
{
    public class AtoneController : BaseController
    {
        //
        // GET: /Atone/
        OrderInfoService service = new OrderInfoService();
        /// <summary>
        /// 赎回列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.flag = 0;
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                ViewBag.flag = 1;
                //Redirect("/UserSetting/uc_setting_status/");
            }
            else if (UserContext.simpleUserInfoModel.IsBindBank == 0)
            {
                ViewBag.flag = 2;
                //Redirect("/UserSetting/uc_setting_status/");
            }

            AtoneListOutModel outModel = new AtoneListOutModel();
            List<int> ProductType = new List<int>() { 2, 3, 4 };
            long Userid = UserContext.simpleUserInfoModel.Id;
            ResultInfo<List<YxLiCai.Model.Order.order_info>> ri = service.GetList(Userid, ProductType, 1, 3);
            List<YxLiCai.Model.Order.order_info> list = ri.Data;
            ResultInfo<int> ri_int = service.GetTotalCount(Userid, ProductType);
            int TotalCount = ri_int.Data;

            outModel.CurrentPage = 1;
            outModel.TotalCount = TotalCount;
            outModel.list = new List<AtoneDetaiModel>();
            AtoneDetaiModel item;
            if (list != null && list.Count > 0)
            {
                foreach (YxLiCai.Model.Order.order_info oi in list)
                {
                    item = new AtoneDetaiModel(oi);
                    outModel.list.Add(item);
                }
            }

            return View(outModel);
        }
        /// <summary>
        /// 异步获取赎回列表数据
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns></returns>
        public ActionResult GetData_ajax(int CurrentPage)
        {
            int IsLastPage = 0;
            List<int> ProductType = new List<int>() { 2, 3, 4 };
            long Userid = UserContext.simpleUserInfoModel.Id;
            ResultInfo<List<YxLiCai.Model.Order.order_info>> ri = service.GetList(Userid, ProductType, CurrentPage, 3);
            List<YxLiCai.Model.Order.order_info> list = ri.Data;
            ResultInfo<int> ri_int = service.GetTotalCount(Userid, ProductType);
            int TotalCount = ri_int.Data;
            //检查是否最后一页
            if (CurrentPage * 3 >= TotalCount)
            {
                IsLastPage = 1;
            }
            List<AtoneDetaiModel> outModel_list = new List<AtoneDetaiModel>();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (list != null && list.Count > 0)
            {
                AtoneDetaiModel item;
                foreach (order_info _item in list)
                {
                    item = new AtoneDetaiModel(_item);
                    outModel_list.Add(item);
                }
                foreach (AtoneDetaiModel list_item in outModel_list)
                {
                    sb.Append("	    <div class='atong'>            <div class='ui-checkbox'>                 <input type='checkbox' name='cb' value='" + list_item.OrderInfoID.ToString() + "' class='atong-left'>            </div>             <div class='atong-right atong-" + (list_item.ProductType == 4 ? "year" : "season") + "'>                 <h2><p>" + list_item.EndTime.ToString("yyyy-MM-dd") + "</p><b>" + list_item.ProductTypeName + "</b></h2>                 <h3>买入金额：<span>" + list_item.BuyMoney.ToString() + "</span>元</h3>             </div>        </div>");
                }
            }
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });
        }
        /// <summary>
        /// 确认赎回
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmAtone()
        {
            if (UserContext.simpleUserInfoModel.IsRealCard == 0)
            {
                Response.Clear();
                Response.Write("<script>window.top.location ='/UserSetting/uc_setting_status'</script>");
                Response.End();
            }
            else if (UserContext.simpleUserInfoModel.IsBindBank == 0)
            {
                Response.Clear();
                Response.Write("<script>window.top.location ='/UserSetting/add_bank'</script>");
                Response.End();
            }
            string OrderInfoIDS = Request["OrderInfoIDS"].ToString();
            ViewBag.OrderInfoIDS = OrderInfoIDS;
            List<string> OrderInfoID = OrderInfoIDS.ToString().Split(',').ToList();
            List<long> OrderInfoIDs = new List<long>();
            long Userid = UserContext.simpleUserInfoModel.Id;

            foreach (string item in OrderInfoID)
            {
                OrderInfoIDs.Add(Convert.ToInt64(item));
            }
            ResultInfo<List<YxLiCai.Model.Order.order_info>> ri = service.GetListOrderByIDSAndUserID(Userid, OrderInfoIDs);
            List<order_info> list_orderinfo = ri.Data;
            ConfirmAtoneOutModel outModel = new ConfirmAtoneOutModel();
            outModel.ProjectNames = "";
            List<int> pids = new List<int>();
            if (list_orderinfo != null && list_orderinfo.Count > 0)
            {
                foreach (order_info oi in list_orderinfo)
                {
                    outModel.AtoneBenJin = outModel.AtoneBenJin + oi.order_investment;
                    outModel.AtoneLiXi = outModel.AtoneLiXi + (oi.interest_added - oi.interest_paid);
                    outModel.AtoneWeiYueJin = outModel.AtoneWeiYueJin + (oi.order_investment - YxLiCai.Tools.Const.SystemConst.MoenyConvert(oi.order_investment * (1 - YxLiCai.Tools.Const.SystemConst.RedemptionRate)));
                    pids.Add(oi.product_id);
                }
            }
            ProductManager productservice = new ProductManager();
            ResultInfo<List<YxLiCai.Model.Product.ProductInfo>> ri_temp3 = productservice.GetListByIDS(pids);
            List<ProductInfo> pis = ri_temp3.Data;
            //string ProjectNames = string.Empty;
            string q = String.Empty;
            string y = String.Empty;
            if (pis != null && pis.Count > 0)
            {
                foreach (ProductInfo pi in pis)
                {
                    if (pi.ProductCategory == 4)
                    {
                        y = "年年丰";
                    }
                    else
                    {
                        q = "季季享";
                    }
                }
                //outModel.ProjectNames=ProjectNames.TrimEnd('+');
                if (!string.IsNullOrEmpty(y) && !string.IsNullOrEmpty(q))
                {
                    outModel.ProjectNames = q + "+" + y;
                }
                else if (string.IsNullOrEmpty(y))
                {
                    outModel.ProjectNames = q;
                }
                else if (string.IsNullOrEmpty(q))
                {
                    outModel.ProjectNames = y;
                }
                else
                {
                    outModel.ProjectNames = "";
                }

            }
            decimal AtoneTotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert((outModel.AtoneBenJin - outModel.AtoneWeiYueJin));
            string oiids = Request["OrderInfoIDS"].ToString();
            outModel.ConfirmUrl = "?OrderInfoIDS=" + Request["OrderInfoIDS"].ToString() + "&AtoneTotalMoney=" + YxLiCai.Tools.Const.SystemConst.MoenyConvert((outModel.AtoneBenJin - outModel.AtoneWeiYueJin)).ToString();
            ViewBag.AtoneTotalMoney = AtoneTotalMoney;
            ViewBag.oiids = oiids;
            return View(outModel);
        }
        /// <summary>
        /// 异步执行赎回操作
        /// </summary>
        /// <returns></returns>
        public ActionResult ExecAtone_ajax()
        {
            string sallpwd = Request["sallpwd"].ToString();
            //验证交易密码
            string pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(sallpwd, 32);
            var IsTruePsw = new UserInfoService().IsTrueSallPassWord(UserContext.simpleUserInfoModel.Id, pwd);
            //交易密码不正确返回1
            if (!IsTruePsw.Data)
            {
                return Json(new
                {
                    result = 1,
                    AtoneTotalMoney = 0
                });
            }
            decimal AtoneTotalMoney = Convert.ToDecimal(Request["AtoneTotalMoney"]);
            string OrderInfoIDS = Request["oiids"].ToString();
            List<string> OrderInfoID = OrderInfoIDS.ToString().Split(',').ToList();
            List<long> OrderInfoIDs = new List<long>();
            foreach (string item in OrderInfoID)
            {
                OrderInfoIDs.Add(Convert.ToInt64(item));
            }
            long Userid = UserContext.simpleUserInfoModel.Id;
            RedemptionService redemptionService = new RedemptionService();


            //执行赎回操作
            ResultInfo<bool> result = redemptionService.UserDedemption(Userid, OrderInfoIDs);
            if (result.Data)
            {
                decimal ShuHuiAmount = 0;
                OrderInfoService os = new OrderInfoService();
                ResultInfo<YxLiCai.Model.Order.order_info> ri;
                foreach (long item in OrderInfoIDs)
                {
                    ri = os.GetEntityByUserIDAndID(item, Userid);
                    order_info oi;
                    oi = ri.Data;
                    if (oi != null && oi.atone_status == 1)
                    {
                        ShuHuiAmount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(oi.order_investment * (1 - YxLiCai.Tools.Const.SystemConst.RedemptionRate));
                        #region 发送消息
                        var resultBank = new UserInfoService().GetBankCard(UserContext.simpleUserInfoModel.Id);
                        string ptype = String.Empty;
                        switch (oi.product_type)
                        {
                            case 1:
                                ptype = "月月赢";
                                break;
                            case 2:
                                ptype = "季季享3个月";
                                break;
                            case 3:
                                ptype = "季季享6个月";
                                break;
                            case 4:
                                ptype = "年年丰";
                                break;
                            default:
                                ptype = "季季享3个月";
                                break;
                        }

                        YxLiCai.Model.User.UserMessageModel mod = new YxLiCai.Model.User.UserMessageModel();
                        mod.user_id = UserContext.simpleUserInfoModel.Id;
                        mod.isread = 0;
                        mod.sendtime = DateTime.Now;
                        mod.title = "申请赎回";
                        mod.content = string.Format("将军大人，您命令e休理财赎回的{0}产品{1}元，预计1-5个工作日将退回尾号到{2}の{3}银行卡，如超时未收到，请联系我们的小叶子。", ptype, ShuHuiAmount, resultBank.Data.LastNum, resultBank.Data.BankName);
                        new UserMessageServer().AddUserMessage(mod);
                        #endregion
                    }

                }
                #region 发短信
                //string P_type = YxLiCai.Tools.Const.SystemConst.GetProductType(oi.product_type);
                string ServerPhone = System.Configuration.ConfigurationManager.AppSettings["ServerPhone"];
                string content = YxLiCai.Tools.Const.SystemConst.MsgContentByType("3");
                content = string.Format(content, YxLiCai.Tools.Const.SystemConst.MoenyConvert(AtoneTotalMoney), UserContext.simpleUserInfoModel.LastBankNum, UserContext.simpleUserInfoModel.BankName, ServerPhone);
                string mobile = UserContext.simpleUserInfoModel.Account; 
                  YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(mobile, content);
      

                #endregion

                return Json(new
                {
                    result = 0,
                    AtoneTotalMoney = AtoneTotalMoney
                });
            }
            else
            {
                return Json(new
                {
                    result = 2,
                    AtoneTotalMoney = 0
                });
            }
        }
        /// <summary>
        /// 赎回后提示信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExecAtone()
        {
            decimal AtoneTotalMoney = Convert.ToDecimal(Request["AtoneTotalMoney"]);
            ViewBag.AtoneTotalMoney = SystemConst.MoenyConvert(AtoneTotalMoney);
            return View();
        }
    }
}
