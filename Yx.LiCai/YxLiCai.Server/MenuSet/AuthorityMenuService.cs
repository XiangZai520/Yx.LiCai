﻿using YxLiCai.Tools;
using YxLiCai.Dao.MenuSet;
using YxLiCai.Model.Authority;
using YxLiCai.Tools.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Service.MenuSet
{
    public class AuthorityMenuService
    {
        readonly AuthoritySetDao _dao = new AuthoritySetDao();

        #region 菜单操作

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMenu(AuthorityMenuModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.AddMenu(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMenu(AuthorityMenuModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.UpdateMenu(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenuList(int parId)
        {
            List<AuthorityMenuModel> list;
            try
            {
                list = _dao.GetListMenu(parId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetAllMenuList()
        {
            List<AuthorityMenuModel> list;
            try
            {
                list = _dao.GetAllListMenu();
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 根据id得到一条菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityMenuModel GetMenuById(int id)
        {
            try
            {
                return _dao.GetMenuById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        #endregion

        #region 角色管理操作 
           
        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns> 
        public List<AuthorityMenuModel> GetMenusByRoloId(int roleId)
        {
            List<AuthorityMenuModel> list;
            try
            {
                list = _dao.GetMenusByRoloId(roleId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }


        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns> 
        public List<int> GetMenuIdsByRoloId(int roleId)
        {
            List<int> list;
            try
            {
                list = _dao.GetMenuIdsByRoloId(roleId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        /// <summary>
        /// 根据roleid修改权限菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateRoloMenus(int roleId, string ids)
        {
            bool reslut = false;
            try
            {
                string[] mids = ids.TrimEnd(',').Split(',');
                if (mids.Length > 0)
                {
                    //清楚旧权限
                    _dao.ClareGroupPermission(roleId);
                    foreach (var i in mids)
                    {
                        int mid = ParseHelper.ToInt(i);
                        if (!_dao.CheckGroupPermission(roleId, mid))
                        {
                            _dao.AddGroupPermission(roleId, mid);
                        }
                    }
                    reslut = true;
                }

            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRole(AuthorityRoleModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.AddRole(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateRole(AuthorityRoleModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.updateRole(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<AuthorityRoleModel> GetListRoles()
        {
            try
            {
                return _dao.GetListRoles();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 得到一条角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityRoleModel GetRoleById(int id)
        {
            try
            {
                return _dao.GetRoleById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        #endregion

        #region 部门

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepartment(AuthorityDepartmentModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.AddDepart(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDepartment(AuthorityDepartmentModel model)
        {
            bool reslut;
            try
            {
                reslut = _dao.UpdateDepart(model);
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityDepartmentModel GetDepartmentById(int id)
        {
            try
            {
                return _dao.GetDepartById(id);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityDepartmentModel> GetDepartmentList(int parId)
        {
            List<AuthorityDepartmentModel> list;
            try
            {
                list = _dao.GetListDepart(parId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }

        #endregion


        #region 个人权限操作
        /// <summary>
        /// 获取个人权限
        /// </summary>
        /// <param name="aId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenusByAccountId(int aId)
        {
            try
            {
                return _dao.GetMenusByAccountId(aId);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;

        }


        /// <summary>
        /// 获取个人账户列表
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetListAccount()
        {
            try
            {
                return _dao.GetListAccount();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }
        /// <summary>
        /// 获取个人账号信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AccountModel GetAccountByName(string name)
        {
            try
            {
                return _dao.GetAccountByName(name);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
            }
            return null;
        }


        /// <summary>
        /// 根据用户id获取权限ID列表
        /// </summary>
        /// <param name="aId">roleId</param>
        /// <returns>True or False</returns> 
        public List<int> GetMenuIdsByAccountId(int aId)
        {
            List<int> list;
            try
            {
                list = _dao.GetMenuIdsByAccountId(aId);
            }
            catch (Exception ex)
            {
                list = null;
                LogHelper.LogWriterFromFilter(ex);
            }
            return list;
        }
        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="accoutId"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public bool UpdateRole(int accoutId, int rid)
        {
            bool reslut = false;
            try
            {
                _dao.UpdateAccountRole(accoutId, rid);
                reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }



        /// <summary>
        /// 批量添加权限
        /// </summary>
        /// <param name="accoutId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool AddPermission(int accoutId, string ids)
        {
            bool reslut = false;
            try
            {
                string[] mids = ids.TrimEnd(',').Split(',');
                if (mids.Length > 0)
                {
                    //清除旧权限
                    _dao.ClarePermission(accoutId);
                    foreach (var i in mids)
                    {
                        int mid = ParseHelper.ToInt(i);
                        if (!_dao.CheckPermission(accoutId, mid))
                        {
                            _dao.AddPermission(accoutId, mid);
                        }
                    }
                    reslut = true;
                }

            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return reslut;
        }


        #endregion 

        /// <summary>
        /// 验证用户权限
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public bool HasAuthority(int accountId, int menuid)
        {
            return _dao.CheckPermission(accountId, menuid);
        }
    }
}
