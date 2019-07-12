using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Admin.Models;
using YxLiCai.Model;
using YxLiCai.Model.Authority;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Service.Account;
using YxLiCai.Service.MenuSet;

namespace YxLiCai.Admin.Controllers
{
    public class SysController : BaseController
    {
        //
        // GET: /Sys/
        public ActionResult Welcome()
        {
            return View("~/Views/Welcome.cshtml");
        }
        public ActionResult Index()
        {
            
            //获取用户唯一ID
            int AccountID = UserContext.simpleUserInfoModel.Id;
            AdminAuthenticationService aas = new AdminAuthenticationService();
            AuthorityMenuService ams = new AuthorityMenuService();
            List<AuthorityMenuModel> AllMenu = ams.GetAllMenuList();
            AccountMenuOutModel OutModel = new AccountMenuOutModel();
            ResultInfo<List<AccountMenuEx>> ri = aas.GetAccountMenuByAccountID(AccountID);
            OutModel.list_AccountMenu = ri.Data;
            if (AllMenu != null)
            {
                OutModel.list_AllMenu_Fir = AllMenu.Where(a => a.MenuType == 1 && a.IsButton == 0).ToList();
                OutModel.list_AllMenu_Sec = AllMenu.Where(a => a.MenuType == 2 && a.IsButton == 0).ToList();
                OutModel.list_AllMenu_Tir = AllMenu.Where(a => a.MenuType == 3 && a.IsButton == 0).ToList();
            }
            return View("~/Views/Default.cshtml", OutModel);
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Test1()
        {
            return View();
        }
        public ActionResult TestUI()
        {
            //ResultInfo<bool> result = new ResultInfo<bool>();
            //result.Result = false;
            //try
            //{
            //    result.Result = true;
            //    result.Data = true;
            //}
            //catch (Exception ex)
            //{
            //    YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
            //    result.Result = false;
            //    result.Data = false;
            //}
            //return result;
            return View();
        }
    }
}
