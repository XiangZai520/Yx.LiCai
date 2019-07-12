using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using YxLica.Tools; 
namespace Yx.LiCai.Controllers
{
    public class GlobalAPITestController : Controller
    {
       
        public ActionResult Index()
        {
            YxLiCai.Model.User.UserInfoModel ddd = new YxLiCai.Model.User.UserInfoModel();
            ddd.Phone = "111111";
            ddd.Password = "2312312";

            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/RegisterInfo_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(ddd));

            return View();
        }

    }
}
