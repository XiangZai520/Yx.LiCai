using System.Collections.Generic;
using System.Web.Mvc;
using YxLiCai.Model.User;

namespace Yx.LiCai.Controllers
{
    /// <summary>
    /// 用户特享金投资页
    /// </summary>
    public class SpecialAssetsController : BaseController
    {
        /// <summary>
        /// 用户数据操作类
        /// </summary>
        YxLiCai.Server.User.UserInfoService service = new YxLiCai.Server.User.UserInfoService(); 

       
        /// <summary>
        /// 特享金首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            long id = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id;
            YxLiCai.Server.User.UserInfoService service=new YxLiCai.Server.User.UserInfoService(); 
            //分页获取特享金列表
            int Count = 0;
            var result = service.GetUserSpecialAssets(id, 1, 10, ref Count);
            ViewBag.PageCount = Count;
            List<UserSpecialAssetsModel> viewModel=new List<UserSpecialAssetsModel>();
            if (result.Result)
            {
                viewModel = result.Data; 
            }
            //特享金总额
            var result1  = service.GetUserSpecialAssets(id); 
            ViewBag.ALlSpecialAssets = result1.Result==true ? result1.Data.ToString("0.00"):"0.00";
            return View(viewModel);
        }


        /// <summary>
        /// 特享金分页方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserSpecialAssets(int page)
        {
            long id = Yx.LiCai.App_Start.UserContext.simpleUserInfoModel.Id; 
            int count = 0;
            var result = service.GetUserSpecialAssets(id, page, 10, ref count);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (result.Result && result.Data!=null)
            {
                List<UserSpecialAssetsModel> viewModel = result.Data; 
                foreach (var list_item in viewModel)
                {
                    sb.Append("<div class='assets-item'><p>" + list_item.c_time.ToString() + "</p><dl class='assets-box'><dt><span>" + YxLiCai.Tools.Const.SystemConst.MoenyConvert((list_item.rate_added+ list_item.rate) * 100).ToString() + "%</span></dt><dd><div class='assets-rate'>当前收益率" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.rate * 100).ToString() + "% + <span>特享" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.rate_added * 100).ToString() + "%</span></div><ul class='assets-get clearfix'><li><strong>实投本金</strong><span><i>" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.amount).ToString() + "</i>元</span></li><li><strong>产生收益</strong><span><i class='color-r'>" + YxLiCai.Tools.Const.SystemConst.MoenyConvert(list_item.interest_added).ToString() + "</i>元</span></li></ul></dd></dl></div>");
                }
            }
            int IsLastPage = 0;
            //检查是否最后一页
            if (page * 10>= count)
            {
                IsLastPage = 1;
            }  
            return Json(new
            {
                IsLastPage = IsLastPage,
                Content = sb.ToString()
            });
        }

    }
}
