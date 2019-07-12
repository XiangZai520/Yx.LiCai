using YxLiCai.DataHelper;
using YxLiCai.Tools.Enums;
using YxLiCai.Model.Authority;
using YxLiCai.Tools.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Dao.Account
{
    public class AccountDao
    {
        /// <summary>
        /// 用户登录
        /// 平扬-20150430
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserLoginResults ValidateUser(string userName, string password)
        {

            var user = GetAccountInfoByLoginName(userName);
            if (user == null)
            {
                return UserLoginResults.UserNotExist;
            }
            if (user.Password != password)
            {
                return UserLoginResults.WrongPassword;
            } 
            if (user.Status == 0)
            {
                return UserLoginResults.AccountClosed;
            } 
            return UserLoginResults.Successful;
        }
        /// <summary>
        /// 根据用户名获取用户信息
        /// 平扬-20150430
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AccountModel GetAccountInfoByLoginName(string name)
        {
            string sql = @" SELECT Id
                                ,Password
                                ,UserName
                                ,LoginName
                                ,Status
                                ,AccountType
                                ,AddDate
                                ,GroupId
                                ,RoleId
                                FROM account  limit1 where LoginName=?LoginName AND Status = 1";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?LoginName",MySqlDbType.VarChar,50)
            }; 
            parms[0].Value = name;
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, CommandType.Text, parms); 
            if (dr.Read())
            {
                AccountModel am= new AccountModel
                {
                    Id = ParseHelper.ToInt(dr["Id"]),
                    Password = ParseHelper.ToString(dr["Password"]),
                    UserName = ParseHelper.ToString(dr["UserName"]),
                    LoginName = ParseHelper.ToString(dr["LoginName"]),
                    Status = ParseHelper.ToInt(dr["Status"]),
                    AccountType = ParseHelper.ToInt(dr["AccountType"]),
                    AddDate = ParseHelper.ToDatetime(dr["AddDate"]),
                    GroupId = ParseHelper.ToInt(dr["GroupId"]),
                    RoleId = ParseHelper.ToInt(dr["RoleId"])
                };
                dr.Close();
                dr.Dispose();
                return am;
            }
            return new AccountModel();
        }
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AccountModel GetAccountInfoByID(int id)
        {
            string sql = @" SELECT Id
                                ,Password
                                ,UserName
                                ,LoginName
                                ,Status
                                ,AccountType
                                ,AddDate
                                ,GroupId
                                ,RoleId
                                FROM account where id=?id";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?id",MySqlDbType.Int32)
            };
            parms[0].Value = id;
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {

                AccountModel am= new AccountModel
                {
                    Id = ParseHelper.ToInt(dr["Id"]),
                    Password = ParseHelper.ToString(dr["Password"]),
                    UserName = ParseHelper.ToString(dr["UserName"]),
                    LoginName = ParseHelper.ToString(dr["LoginName"]),
                    Status = ParseHelper.ToInt(dr["Status"]),
                    AccountType = ParseHelper.ToInt(dr["AccountType"]),
                    AddDate = ParseHelper.ToDatetime(dr["AddDate"]),
                    GroupId = ParseHelper.ToInt(dr["GroupId"]),
                    RoleId = ParseHelper.ToInt(dr["RoleId"])
                };
                dr.Close();
                dr.Dispose();
                return am;
            }
            return new AccountModel();
        }

        /// <summary>
        /// 添加账号
        /// 平扬-20150526
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AddAccountInfo(AccountModel model)
        {
            string sql = @" INSERT INTO account ( 
                          PASSWORD,
                          UserName,
                          LoginName,
                          STATUS,
                          AccountType,
                          GroupId,
                          RoleId,
                          ADDDATE
                        ) 
                        VALUES
                          ( 
                            ?Password,
                            ?UserName,
                            ?LoginName,
                            ?Status,
                            ?AccountType,
                            ?GroupId,
                            ?RoleId,
                            ?AddDate
                          ) ;select @@IDENTITY as InsertId;";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?Password",MySqlDbType.VarChar,255),
                 new MySqlParameter("?UserName",MySqlDbType.VarChar,50),
                 new MySqlParameter("?LoginName",MySqlDbType.VarChar,50),
                 new MySqlParameter("?Status",MySqlDbType.Int32,11),
                 new MySqlParameter("?AccountType",MySqlDbType.Int32,11),
                 new MySqlParameter("?GroupId",MySqlDbType.Int32,11),
                 new MySqlParameter("?RoleId",MySqlDbType.Int32,11),
                 new MySqlParameter("?AddDate",MySqlDbType.DateTime)
            };
            parms[0].Value = model.Password;
            parms[1].Value = model.UserName;
            parms[2].Value = model.LoginName;
            parms[3].Value = 1;
            parms[4].Value = 1;
            parms[5].Value = 1;
            parms[6].Value = model.RoleId;
            parms[7].Value = DateTime.Now;
            object i = DataBaseOperator.AdminDbHelperWrite.ExecuteScalar(sql, CommandType.Text, parms);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false; 
        } 

        /// <summary>
        /// 是否存在该账号
        /// 平扬-20150526
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAccountInfo(string name)
        {
            string sql = @" SELECT Id  FROM account  limit1 where LoginName=?LoginName";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?LoginName",MySqlDbType.VarChar,50)
            };
            parms[0].Value = name;
            var dr = DataBaseOperator.AdminDbHelperRead.ExecuteScalar(sql, CommandType.Text, parms);
            if (dr != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改账户密码
        /// 平扬-20150526 
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool UpdateAccountPwd(int aid,string pwd)
        {
            string sql = @" UPDATE account  set  PASSWORD=?PASSWORD where id=?Id";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?PASSWORD",MySqlDbType.VarChar,50),
                new MySqlParameter("?Id",MySqlDbType.Int32,11)
            };
            parms[0].Value = pwd;
            parms[1].Value = aid;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(sql, CommandType.Text, parms); 
        }

        /// <summary>
        /// 获取用户所有菜单权限
        /// </summary>
        /// <param name="AccountID">账户id</param>
        /// <returns></returns>
        public List<AccountMenuEx> GetAccountMenuByAccountID(int AccountID)
        {
            string sqlstr = @"SELECT a.AccoutId,b.Id TirID,c.Id SecID,d.Id FirD
FROM account_menu a
INNER JOIN menu_class b ON a.MenuId=b.Id
INNER JOIN menu_class c ON b.ParId=c.Id
INNER JOIN menu_class d ON c.ParId=d.Id
WHERE a.AccoutId=?AccountID AND b.IsButton=0 AND b.MenuType=3";
            List<AccountMenuEx> list = new List<AccountMenuEx>();
            MySqlParameter[] parms= new MySqlParameter[]
            { 
                new MySqlParameter("?AccountID",MySqlDbType.Int32)
            };
            parms[0].Value = AccountID;
            DataSet ds = DataBaseOperator.AdminDbHelperRead.ExecuteDataSet(sqlstr, CommandType.Text, parms);
            if (ds != null && ds.Tables.Count > 0)
            {
                AccountMenuEx item;
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new AccountMenuEx();
                        item.AccountID = Convert.ToInt32(dr["AccoutId"]);
                        item.TirID = Convert.ToInt32(dr["TirID"]);
                        item.SecID = Convert.ToInt32(dr["SecID"]);
                        item.FirD = Convert.ToInt32(dr["FirD"]);

                        list.Add(item);
                    }
                }
            }
            return list;
        }
    }
}
