﻿using System.Collections.Generic;
using YxLiCai.Service.Account;
using System.Linq;

namespace YxLiCai.Admin.Controllers
{
    using System.Web.Mvc;
    using YxLiCai.Admin;
    using YxLiCai.Model;
    using YxLiCai.Service.MenuSet;
    using YxLiCai.Model.Authority;
    using YxLiCai.Admin.Models;

    /// <summary>
    /// 权限业务操作类--平扬 2015.3.18
    /// </summary>
    public class MenuManagerController : BaseController
    {
        /// <summary>
        /// 菜单权限服务操作类
        /// </summary>
        readonly AuthorityMenuService _iAuhority = new AuthorityMenuService();

        #region 菜单操作

        /// <summary>
        /// 返回权限菜单视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Menu(int? id)
        {
            int parid = id ?? 0;
            ViewBag.ParId = parid;
            if (parid > 0)
            {
                ViewBag.PartMenu = _iAuhority.GetMenuById(parid);
            }
            var list = _iAuhority.GetMenuList(parid);
            return View(list);
        }
        /// <summary>
        /// 返回权限菜单视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult MenuNew()
        {

            MenuNewOut outModel = new MenuNewOut();                        
            List<AuthorityMenuModel> list_all = _iAuhority.GetAllMenuList();
            int ParID = Request["ParID"] == null ? 0 : ConvertInt(Request["ParID"]);
            outModel.ParID = ParID;
            outModel.ParParID = 0;
            if (ParID == 0)
            {
                outModel.ParMenuType = 0;
                outModel.ParMenuName = "";
                outModel.ParUrl = "";
            }
            else
            {
                List<AuthorityMenuModel> list_temp = list_all.Where(a => a.Id == ParID).ToList();
                if (list_temp != null && list_temp.Count > 0)
                {
                    outModel.ParMenuType = list_temp[0].MenuType;
                    outModel.ParMenuName = list_temp[0].MenuName;
                    outModel.ParUrl = "backurl";
                    outModel.ParParID = list_temp[0].ParId;
                }
            }
            outModel.list_menu = list_all.Where(a => a.ParId == ParID).ToList();
            return View(outModel);
        }

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="AuthorityMenuModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMenu(AuthorityMenuModel mod)
        {
            int ParMenuType = Request["ParMenuType"] == null ? 0 : ConvertInt(Request["ParMenuType"]);
            mod.MenuType = ParMenuType + 1;
            bool b = _iAuhority.AddMenu(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="AuthorityMenuModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateMenu(AuthorityMenuModel mod)
        {
            bool b = _iAuhority.UpdateMenu(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 角色管理操作

        /// <summary>
        /// 返回角色视图
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult RoleManager()
        {
            var list = _iAuhority.GetListRoles();
            ViewBag.AllMenu = _iAuhority.GetAllMenuList();
            return View(list);
        }


        /// <summary>
        /// 返回角色权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMenusByRole(int roleid)
        {
            var list = _iAuhority.GetMenuIdsByRoloId(roleid) ?? new List<int>();
            return Json(new ResultModel(true, "", list), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRole(AuthorityRoleModel mod)
        {
            bool b = _iAuhority.AddRole(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRole(AuthorityRoleModel mod)
        {
            bool b = _iAuhority.UpdateRole(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改角色的权限
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditRoleMenus(int rid, string ids)
        {
            bool b = _iAuhority.UpdateRoloMenus(rid, ids);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 部门设置

        /// <summary>
        /// 返回部门视图
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Department(int? id)
        {
            int parid = id ?? 0;
            ViewBag.ParId = parid;
            if (parid > 0)
            {
                ViewBag.PartMenu = _iAuhority.GetDepartmentById(parid);
            }
            var list = _iAuhority.GetDepartmentList(parid);
            return View(list);
        }
        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDepart(AuthorityDepartmentModel mod)
        {
            bool b = _iAuhority.AddDepartment(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDepart(AuthorityDepartmentModel mod)
        {
            bool b = _iAuhority.UpdateDepartment(mod);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 个人账户操作

        /// <summary>
        /// 返回账号视图
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AccountManager()
        {
            var list = _iAuhority.GetListAccount();
            ViewBag.AllMenu = _iAuhority.GetAllMenuList();
            ViewBag.ListRoles = _iAuhority.GetListRoles();
            return View(list);
        }


        /// <summary>
        /// 返回账户权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenusByAccount(int aid)
        {
            var list = _iAuhority.GetMenuIdsByAccountId(aid);
            return Json(list != null ? new ResultModel(true, string.Empty, list) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改账户的密码
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateAccountPwd(int aid, string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(new ResultModel(false, "密码不能为空!"), JsonRequestBehavior.AllowGet);
            }
            string password = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(pwd, 32);
            ResultInfo<bool> reslut = new AdminAuthenticationService().UpdateAccountPwd(aid, password);
            if (reslut.Result && reslut.Data)
            {
                return Json(new ResultModel(true, string.Empty, reslut), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(false, "修改账户的密码失败!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改账户的权限
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditAccountMenus(int aid, string ids)

        {
            bool b = _iAuhority.AddPermission(aid, ids);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改账户的角色
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditAccountRole(int aid, int rid)
        {
            bool b = _iAuhority.UpdateRole(aid, rid);
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "设置失败!"), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 添加账户
        /// </summary>
        /// <param name="AccountName"></param> 
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAccount(string AccountName)
        {
            if (string.IsNullOrEmpty(AccountName))
            {
                return Json(new ResultModel(false, "输入账户!"), JsonRequestBehavior.AllowGet);
            }
            bool b = new AdminAuthenticationService().HasAccountInfo(AccountName).Data;
            if (b)
            {
                return Json(new ResultModel(false, "已存在该账号!"), JsonRequestBehavior.AllowGet);
            }
            AccountModel mod = new AccountModel();
            mod.RoleId = 0;
            mod.LoginName = AccountName;
            mod.UserName = AccountName;
            mod.Password = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert("123456", 32);
            b = new AdminAuthenticationService().AddAccountInfo(mod).Data;
            return Json(b ? new ResultModel(true, string.Empty) : new ResultModel(false, "添加账户失败!"), JsonRequestBehavior.AllowGet);

        }


        #endregion


        /// <summary>
        /// 返回按钮视图
        /// </summary>
        /// <returns></returns> 
        public ActionResult MenuButton(int pid)
        {
            ViewBag.ParId = pid;
            ViewBag.PartMenu = _iAuhority.GetMenuById(pid);
            if (ViewBag.PartMenu == null)
            {
                Response.Redirect("/MenuManager/Menu/" + pid);
                return null;
            }
            var list = _iAuhority.GetMenuList(pid);
            return View(list);
        }



    }
}