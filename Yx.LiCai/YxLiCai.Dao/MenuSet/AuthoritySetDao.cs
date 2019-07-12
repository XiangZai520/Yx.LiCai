using System.Data;
using System.Linq;

namespace YxLiCai.Dao.MenuSet
{

    using YxLiCai.Tools;
    using YxLiCai.Tools.Util;
    using YxLiCai.DataHelper;
    using YxLiCai.DataHelper.DbProviderFactory;
    using YxLiCai.Model.Authority;
    using System.Collections.Generic; 
    using System.Text;
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using MySql.Data.MySqlClient;
    /// <summary>
    /// 菜单权限数据操作类-平扬 2015.3.18
    /// </summary>
    public class AuthoritySetDao
    {
        #region 角色管理
        
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRole(AuthorityRoleModel model)
        {
            string sql = @" INSERT INTO role (RoleName,BeLock) VALUES (?RoleName,?BeLock) ;select @@IDENTITY as InsertId;";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?RoleName",MySqlDbType.VarChar,50),
                new MySqlParameter("?BeLock",MySqlDbType.Int16,4)
            };
            parms[0].Value = model.RoleName;
            parms[1].Value = model.BeLock;
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, CommandType.Text, parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateRole(AuthorityRoleModel model)
        {
            string sql = @" UPDATE role set RoleName=?RoleName where id=?id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?RoleName",MySqlDbType.VarChar,50),
                new MySqlParameter("?id",MySqlDbType.Int32,4)
            };
            parms[0].Value = model.RoleName;
            parms[1].Value = model.Id;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, CommandType.Text, parms);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<AuthorityRoleModel> GetListRoles()
        {
            string sql = @" SELECT Id,RoleName,BeLock FROM role  where BeLock=0 ";
            var dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, null).Tables[0];
            return (List<AuthorityRoleModel>)DataTypeConvert.ConvertDataTableList<AuthorityRoleModel>(dt);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthorityRoleModel GetRoleById(int id)
        {
            string sql = @" SELECT Id,RoleName,BeLock FROM role  where Id=?id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?Id",MySqlDbType.Int32,4)
            };
            parms[0].Value = id;
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {
                AuthorityRoleModel arm= new AuthorityRoleModel
                {
                    Id = id,
                    RoleName =YxLiCai.Tools.Util.ParseHelper.ToString(dr["RoleName"]),
                    BeLock = YxLiCai.Tools.Util.ParseHelper.ToBool(dr["BeLock"])
                };
                dr.Close();
                dr.Dispose();
                return arm;
            }
            return null;
        }


        #endregion

        #region 菜单管理

        /// <summary>
        /// 增加菜单权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMenu(AuthorityMenuModel model)
        {
            string sql = @" INSERT INTO menu_class (ParId,MenuName,BeLock,Url,IsButton,MenuType) VALUES (?ParId,?MenuName,?BeLock,?Url,?IsButton,?MenuType);select @@IDENTITY as InsertId; ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?ParId",MySqlDbType.Int32,4),
                new MySqlParameter("?MenuName",MySqlDbType.VarChar,50),
                new MySqlParameter("?BeLock",MySqlDbType.Bit,4),
                new MySqlParameter("?Url",MySqlDbType.VarChar,50),
                new MySqlParameter("?IsButton",MySqlDbType.Bit,4),
                new MySqlParameter("?MenuType",MySqlDbType.Int32)
            };
            parms[0].Value = model.ParId;
            parms[1].Value = model.MenuName;
            parms[2].Value = model.BeLock;
            parms[3].Value = model.Url??"";
            parms[4].Value = model.IsButton;
            parms[5].Value = model.MenuType;
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, CommandType.Text, parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMenu(AuthorityMenuModel model)
        {
            string sql = @" UPDATE menu_class set MenuName=?MenuName,Url=?Url,IsButton=?IsButton where id=?id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?id",MySqlDbType.Int32,4),
                new MySqlParameter("?MenuName",MySqlDbType.VarChar,50),
                new MySqlParameter("?Url",MySqlDbType.VarChar,50),
                new MySqlParameter("?IsButton",MySqlDbType.Bit,4)
            };
            parms[0].Value = model.Id;
            parms[1].Value = model.MenuName;
            parms[2].Value = model.Url??"";
            parms[3].Value = model.IsButton;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, CommandType.Text, parms);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityMenuModel GetMenuById(int id)
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton FROM menu_class  where Id=?Id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?id",MySqlDbType.Int32,4) 
            };
            parms[0].Value = id;
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {
                AuthorityMenuModel am= new AuthorityMenuModel
                {
                    Id = id,
                    ParId = ParseHelper.ToInt(dr["ParId"]),
                    MenuName = ParseHelper.ToString(dr["MenuName"]),
                    Url = ParseHelper.ToString(dr["Url"]),
                    IsButton = ParseHelper.ToInt(dr["IsButton"]),
                    BeLock = ParseHelper.ToBool(dr["BeLock"])
                };
                dr.Close();
                dr.Dispose();
                return am;
            }
            return null;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetListMenu(int parId)
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton FROM menu_class  where ParId=?ParId ";

            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?ParId",MySqlDbType.Int32,4) 
            };
            parms[0].Value = parId;
            var dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, parms).Tables[0]; 
            return (List<AuthorityMenuModel>)DataTypeConvert.ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        /// <summary>
        /// 获取全部菜单列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetAllListMenu()
        {
            string sql = @" SELECT Id,ParId,MenuName,BeLock,Url,IsButton,MenuType,Img FROM menu_class  where BeLock=0 ";
            var dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, null).Tables[0];
            return (List<AuthorityMenuModel>)DataTypeConvert.ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        #endregion

        #region 部门管理

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepart(AuthorityDepartmentModel model)
        {
            string sql = @" INSERT INTO department (ParId,DepartName,BeLock) VALUES (?ParId,?DepartName,?BeLock);select @@IDENTITY as InsertId; ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?ParId",MySqlDbType.Int32,4),
                new MySqlParameter("?DepartName",MySqlDbType.VarChar,100),
                new MySqlParameter("?BeLock",MySqlDbType.Bit,4)
            };
            parms[0].Value = model.ParId;
            parms[1].Value = model.DepartName;
            parms[2].Value = model.BeLock; 
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, CommandType.Text, parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateDepart(AuthorityDepartmentModel model)
        {
            string sql = @" UPDATE department set DepartName=?DepartName where id=?id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?id",MySqlDbType.Int32,4),
                new MySqlParameter("?DepartName",MySqlDbType.VarChar,100)
            };
            parms[0].Value = model.ParId;
            parms[1].Value = model.DepartName;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, CommandType.Text, parms); 
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AuthorityDepartmentModel GetDepartById(int id)
        {
            string sql = @" SELECT Id,ParId,DepartName,BeLock FROM department   where Id=?Id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?Id",MySqlDbType.Int32,4)
            };
            parms[0].Value = id; 
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, CommandType.Text, parms); 
            if (dr.Read())
            {
                AuthorityDepartmentModel entity= new AuthorityDepartmentModel
                {
                    Id = id,
                    ParId = ParseHelper.ToInt(dr["ParId"]),
                    DepartName = ParseHelper.ToString(dr["DepartName"]),
                    BeLock = ParseHelper.ToBool(dr["BeLock"])
                };
                dr.Close();
                dr.Dispose();
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parId"></param>
        /// <returns></returns>
        public List<AuthorityDepartmentModel> GetListDepart(int parId)
        {
            string sql = @" SELECT Id,ParId,DepartName,BeLock FROM department  where ParId=?ParId ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?ParId",MySqlDbType.Int32,4)
            };
            parms[0].Value = parId;
            var dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, parms).Tables[0]; 
            return (List<AuthorityDepartmentModel>)DataTypeConvert.ConvertDataTableList<AuthorityDepartmentModel>(dt);
        }

        #endregion

        #region 个人权限管理设置

        /// <summary>
        /// 获取个人账户列表
        /// </summary>
        /// <returns></returns>
        public List<AccountModel> GetListAccount()
        {
            string sql = @" SELECT account.Id
                                  ,Password
                                  ,UserName
                                  ,LoginName
                                  ,Status
                                  ,AccountType
                                  ,AddDate
                                  ,RoleId,role.RoleName FROM account  left join role  on account.RoleId=role.id ";
            var dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, null).Tables[0];
            return (List<AccountModel>)DataTypeConvert.ConvertDataTableList<AccountModel>(dt);
        }

        /// <summary>
        ///  获取个人权限列表
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns></returns>
        public List<AuthorityMenuModel> GetMenusByAccountId(int accoutId)
        {
            string sql =
                " select MenuId,Url,IsButton from account_menu A  inner join menu_class M on M.Id=A.MenuId where AccoutId=?AccoutId;";

            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?AccoutId",MySqlDbType.Int32,4)
            };
            parms[0].Value = accoutId;

            DataTable dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, parms).Tables[0];
            var list = new List<int>();
            return (List<AuthorityMenuModel>)DataTypeConvert.ConvertDataTableList<AuthorityMenuModel>(dt);
        }

        /// <summary>
        ///  获取个人权限id列表
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns></returns>
        public List<int> GetMenuIdsByAccountId(int accoutId)
        {
            string sql = "select MenuId from account_menu  where AccoutId=?AccoutId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?AccoutId",MySqlDbType.Int32,4)
            };
            parms[0].Value = accoutId;
            DataTable dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, parms).Tables[0];
            var list = new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select ParseHelper.ToInt(row["MenuId"]));
            }
            return list; 
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public AccountModel GetAccountByName(string loginName)
        {
            string sql = @" SELECT Id
                                  ,Password
                                  ,UserName
                                  ,LoginName
                                  ,Status
                                  ,AccountType
                                  ,AddDate
                                  ,GroupId,RoleId FROM account  where LoginName=?loginName ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?LoginName",MySqlDbType.VarChar,50)
            };
            parms[0].Value = loginName;

            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, "获取个人信息", parms);
            if (dr.Read())
            {
                AccountModel entity= new AccountModel
                {
                    Id = ParseHelper.ToInt(dr["Id"]),
                    UserName = ParseHelper.ToString(dr["UserName"]),
                    LoginName = ParseHelper.ToString(dr["LoginName"]),
                    Status = ParseHelper.ToInt(dr["Status"]),
                    AccountType = ParseHelper.ToInt(dr["AccountType"]),
                    GroupId = ParseHelper.ToInt(dr["GroupId"]),
                    RoleId = ParseHelper.ToInt(dr["RoleId"])
                };
                dr.Close();
                dr.Dispose();
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 检测是否存在指定权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool CheckPermission(int accoutId, int menuId)
        {
            string sql = "select Id from account_menu  where AccoutId=?AccoutId and MenuId = ?MenuId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?AccoutId",MySqlDbType.Int32,4), new MySqlParameter("?MenuId",MySqlDbType.Int32,4)
            };
            parms[0].Value = accoutId;
            parms[1].Value = menuId;
            object i = DataBaseOperator.AdminDbHelperRead.ExecuteScalar(sql, CommandType.Text, parms); 
            
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 加入权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool AddPermission(int accoutId, int menuId)
        {
            if (CheckPermission(accoutId, menuId)) return false;
            string sql = " INSERT INTO account_menu(AccoutId,MenuId) VALUES  (?AccoutId,?MenuId);select @@IDENTITY as InsertId;";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?AccoutId",MySqlDbType.Int32,4), new MySqlParameter("?MenuId",MySqlDbType.Int32,4)
            };
            parms[0].Value = accoutId;
            parms[1].Value = menuId;
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, CommandType.Text, parms);  
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="accoutId">accoutId</param>
        /// <returns>True or False</returns>
        public bool ClarePermission(int accoutId)
        {
            string sql = "delete from account_menu where AccoutId=?AccoutId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?AccoutId",MySqlDbType.Int32,4)
            };
            parms[0].Value = accoutId;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, CommandType.Text, parms);  
        }

       /// <summary>
       /// 修改个人角色信息 
       /// </summary>
       /// <param name="aid"></param>
       /// <param name="rid"></param>
       /// <returns></returns>
        public bool UpdateAccountRole(int aid,int rid)
        {
            string sql = @" UPDATE account set  RoleId=?RoleId where id=?Id ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?Id",MySqlDbType.Int32,4),
                new MySqlParameter("?RoleId",MySqlDbType.Int32,4)
            };
            parms[0].Value = aid;
            parms[1].Value = rid;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, "#修改个人角色信息", parms); 
        }

        #endregion

        #region 角色权限管理

        /// <summary>
        /// 加入权限
        /// </summary>
        /// <param name="accoutId">用户ID</param>
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool AddGroupPermission(int roleId, int menuId)
        {
            if (CheckGroupPermission(roleId, menuId)) return false;
            string sql = " INSERT INTO role_menu(RoleId,MenuId) VALUES  (?RoleId,?MenuId);select @@IDENTITY as InsertId;";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?RoleId",MySqlDbType.Int32,4),
                new MySqlParameter("?MenuId",MySqlDbType.Int32,4)
            };
            parms[0].Value = roleId;
            parms[1].Value = menuId;
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, "#加入权限", parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 检测是否存在指定权限组
        /// </summary>
        /// <param name="departId">权限组ID</param> 
        /// <param name="menuId">权限ID</param>
        /// <returns></returns>
        public bool CheckGroupPermission(int roleId, int menuId)
        {
            string sql = "select Id from role_menu  where RoleId= ?RoleId and MenuId = ?MenuId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?RoleId",MySqlDbType.Int32,4),
                new MySqlParameter("?MenuId",MySqlDbType.Int32,4)
            };
            parms[0].Value = roleId;
            parms[1].Value = menuId;

            object i = DataBaseOperator.AdminDbHelperRead.ExecuteScalar(sql, "#检测是否存在指定权限组", parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 根据条件删除权限组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>True or False</returns>
        public bool ClareGroupPermission(int roleId)
        {
            string sql = "delete from role_menu where roleId=?roleId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?roleId",MySqlDbType.Int32,4)
            };
            parms[0].Value = roleId;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, "#根据条件删除权限组", parms); 
        }

        /// <summary>
        /// 根据roleid获取权限列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns>
        public List<AuthorityMenuModel> GetMenusByRoloId(int roleId)
        {

            string sql =
               " select MenuId,Url,IsButton from role_menu A  inner join menu_class M on M.Id=A.MenuId where RoleId=?roleId;";

            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?roleId",MySqlDbType.Int32,4)
            };
            parms[0].Value = roleId;

            DataTable dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, CommandType.Text, parms).Tables[0];
            var list = new List<int>();
            return (List<AuthorityMenuModel>)DataTypeConvert.ConvertDataTableList<AuthorityMenuModel>(dt); 
        } 


        /// <summary>
        /// 根据roleid获取权限ID列表
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <returns>True or False</returns>
        public List<int> GetMenuIdsByRoloId(int roleId)
        {
            string sql = "select MenuId from role_menu  where RoleId=?roleId";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?roleId",MySqlDbType.Int32,4)
            };
            parms[0].Value = roleId;
            DataTable dt = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sql, "#根据roleid获取权限ID列表", parms).Tables[0];
            var list=new List<int>();
            if (dt.Rows.Count > 0)
            {
                list.AddRange(from DataRow row in dt.Rows select ParseHelper.ToInt(row["MenuId"]));
            }
            return list; 
        } 
        
        #endregion 

    }
}
