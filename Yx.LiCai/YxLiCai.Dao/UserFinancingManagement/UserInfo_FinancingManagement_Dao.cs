/*
 * 融资方用户账户信息数据层
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using YxLiCai.DataHelper;
using YxLiCai.Model.Account;
using YxLiCai.Model.Authority;
using YxLiCai.Model.Project;
using YxLiCai.Model.Refund;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Tools.Enums;
using YxLiCai.Tools.Util;

namespace YxLiCai.Dao.UserFinancingManagement
{
    /// <summary>
    /// 融资方用户账户信息数据层
    /// </summary>
    public class UserInfo_FinancingManagement_Dao
    {
        #region 融资方平台登录
        /// <summary>
        /// 用户登录    
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
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FerAccountModel GetAccountInfoByLoginName(string name)
        {
            string sql = @"SELECT     f.id, 
                                       fa.`id` AS account, 
                                       spassword, 
                                       myreal_name, 
                                       f.status, 
                                       r_time, 
                                       pwd, 
                                       l_name 
                            FROM       fer_info    AS f 
                            INNER JOIN fer_account AS fa 
                            ON         f.`id`=fa.`fer_id` 
                            WHERE      l_name=?loginname 
                            AND         f.status  = 1 limit 1 ";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?LoginName",MySqlDbType.VarChar,50)
            };
            parms[0].Value = name;
            var dr = DataBaseOperator.MoneyReadDbHelper.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {

                FerAccountModel entity = new FerAccountModel
                {
                    Id = ParseHelper.ToInt(dr["id"]),
                    Password = ParseHelper.ToString(dr["pwd"]),
                    UserName = ParseHelper.ToString(dr["myreal_name"]),
                    LoginName = ParseHelper.ToString(dr["l_name"]),
                    Status = ParseHelper.ToInt(dr["status"]),
                    Fer_account = ParseHelper.ToInt(dr["account"]),

                };
                dr.Close();
                dr.Dispose();
                return entity;
            }
            return new FerAccountModel();
        }
        #endregion

        #region 添加融资方信息
        /// <summary>
        /// 添加融资方信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public int AddUser(UserInfo_FinancingManagement_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fer_info(");
            strSql.Append("name,phone,spassword,company_address,myreal_name,myreal_card,bank_name,bank_card,bank_code,first_num,last_num,bank_phone,isreal_card,isbank_card,requestid,status,r_time,pwd,l_name)");
            strSql.Append(" values (");
            strSql.Append("?financier_name,?phone,?spassword,?company_address,?myreal_name,?myreal_card,?bank_name,?bank_card,?bank_code,?first_num,?last_num,?bank_phone,?isreal_card,?isbank_card,?requestid,?status,?reg_time,?pwd,?l_name);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {				
					new MySqlParameter("?financier_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?phone", MySqlDbType.VarChar,50),
					new MySqlParameter("?spassword", MySqlDbType.VarChar,50),
					new MySqlParameter("?company_address", MySqlDbType.VarChar,150),
					new MySqlParameter("?myreal_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?myreal_card", MySqlDbType.VarChar,50),
					new MySqlParameter("?bank_name", MySqlDbType.VarChar,150),
					new MySqlParameter("?bank_card", MySqlDbType.VarChar,50),
					new MySqlParameter("?bank_code", MySqlDbType.VarChar,20),
					new MySqlParameter("?first_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?last_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?bank_phone", MySqlDbType.VarChar,20),
					new MySqlParameter("?isreal_card", MySqlDbType.Int16,6),
					new MySqlParameter("?isbank_card", MySqlDbType.Int16,3),
					new MySqlParameter("?requestid", MySqlDbType.VarChar,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?reg_time", MySqlDbType.DateTime),
                    		new MySqlParameter("?pwd", MySqlDbType.VarChar,50),
					new MySqlParameter("?l_name", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.financier_name;
            parameters[1].Value = model.phone;
            parameters[2].Value = model.spassword;
            parameters[3].Value = model.company_address;
            parameters[4].Value = model.myreal_name;
            parameters[5].Value = model.myreal_card;
            parameters[6].Value = model.bank_name;
            parameters[7].Value = model.bank_card;
            parameters[8].Value = model.bank_code;
            parameters[9].Value = model.first_num;
            parameters[10].Value = model.last_num;
            parameters[11].Value = model.bank_phone;
            parameters[12].Value = model.isreal_card;
            parameters[13].Value = model.isbank_card;
            parameters[14].Value = model.requestid;
            parameters[15].Value = model.status;
            parameters[16].Value = DateTime.Now;
            parameters[17].Value = model.pwd;
            parameters[18].Value = model.l_name;

            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 添加一条融资方信息", parameters);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;
        }
        #endregion

        #region 修改融资方信息
        /// <summary>
        /// 修改融资方信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public bool UpdateUser(UserInfo_FinancingManagement_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set ");
            strSql.Append("name=?financier_name,");
            strSql.Append("phone=?phone,");
            strSql.Append("spassword=?spassword,");
            strSql.Append("company_address=?company_address,");
            strSql.Append("myreal_name=?myreal_name,");
            strSql.Append("myreal_card=?myreal_card,");
            strSql.Append("bank_name=?bank_name,");
            strSql.Append("bank_card=?bank_card,");
            strSql.Append("bank_code=?bank_code,");
            strSql.Append("first_num=?first_num,");
            strSql.Append("last_num=?last_num,");
            strSql.Append("bank_phone=?bank_phone,");
            strSql.Append("isreal_card=?isreal_card,");
            strSql.Append("isbank_card=?isbank_card,");
            strSql.Append("requestid=?requestid,");
            strSql.Append("status=?status,");
            strSql.Append("r_time=?reg_time");
            strSql.Append("pwd=?pwd,");
            strSql.Append("l_name=?l_name");
            strSql.Append(" where financier_id=?financier_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?phone", MySqlDbType.VarChar,50),
					new MySqlParameter("?spassword", MySqlDbType.VarChar,50),
					new MySqlParameter("?company_address", MySqlDbType.VarChar,150),
					new MySqlParameter("?myreal_name", MySqlDbType.VarChar,50),
					new MySqlParameter("?myreal_card", MySqlDbType.VarChar,50),
					new MySqlParameter("?bank_name", MySqlDbType.VarChar,150),
					new MySqlParameter("?bank_card", MySqlDbType.VarChar,50),
					new MySqlParameter("?bank_code", MySqlDbType.VarChar,20),
					new MySqlParameter("?first_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?last_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?bank_phone", MySqlDbType.VarChar,20),
					new MySqlParameter("?isreal_card", MySqlDbType.Int16,6),
					new MySqlParameter("?isbank_card", MySqlDbType.Int16,3),
					new MySqlParameter("?requestid", MySqlDbType.VarChar,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?reg_time", MySqlDbType.DateTime),
                    new MySqlParameter("?pwd", MySqlDbType.VarChar,50),
                    new MySqlParameter("?l_name", MySqlDbType.VarChar,20),
					new MySqlParameter("?id", MySqlDbType.Int32,11),
					new MySqlParameter("?financier_id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.financier_name;
            parameters[1].Value = model.phone;
            parameters[2].Value = model.spassword;
            parameters[3].Value = model.company_address;
            parameters[4].Value = model.myreal_name;
            parameters[5].Value = model.myreal_card;
            parameters[6].Value = model.bank_name;
            parameters[7].Value = model.bank_card;
            parameters[8].Value = model.bank_code;
            parameters[9].Value = model.first_num;
            parameters[10].Value = model.last_num;
            parameters[11].Value = model.bank_phone;
            parameters[12].Value = model.isreal_card;
            parameters[13].Value = model.isbank_card;
            parameters[14].Value = model.requestid;
            parameters[15].Value = model.status;
            parameters[16].Value = model.reg_time;
            parameters[17].Value = model.pwd;
            parameters[18].Value = model.l_name;
            parameters[19].Value = model.financier_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:35:29 侯裕祥 修改用户信息", parameters);
        }
        #endregion

        #region 融资方登录信息
        /// <summary>
        /// 判断融资方登录密码是否正确（用户登录）
        /// </summary>
        /// <param name="LongName">登录名称</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTruePassWordLogin(string LongName, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select 1 from fer_info ");
            strSql.Append(" where l_name =?Phone and status=1 and pwd=?Password");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Phone", MySqlDbType.VarChar,50),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = LongName;
            parameters[1].Value = password;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 18:04:54-侯裕祥-判断密码是否正确", parameters);

            if (i != null)
            {
                if (Convert.ToInt64(i) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 修改融资方登录密码
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="account_id">账户ID</param>
        /// <param name="Password">登录密码</param>
        /// <returns></returns>
        public bool UpdateUserLoginPassWord(int account_id, string Password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set pwd=?Password where id=?account_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?account_id", MySqlDbType.Int32),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = account_id;
            parameters[1].Value = Password;
            return DataBaseOperator.AdminDbHelperWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户登录密码", parameters);
        }
        #endregion

        #region 得到用户实体信息
        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public UserInfo_FinancingManagement_Model GetUserModel(int financier_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT a.id, 
                                   a.NAME, 
                                   a.phone, 
                                   a.spassword, 
                                   a.company_address, 
                                   a.myreal_name, 
                                   a.myreal_card, 
                                   a.bank_name, 
                                   a.bank_card, 
                                   a.bank_code, 
                                   a.first_num, 
                                   a.last_num, 
                                   a.bank_phone, 
                                   a.isreal_card, 
                                   a.isbank_card, 
                                   a.requestid, 
                                   a.status, 
                                   a.r_time, 
                                   TRUNCATE(b.amount,2) amount, 
                                   a.pwd, 
                                   a.l_name 
                            FROM   fer_info AS a 
                                   LEFT JOIN fer_account AS b 
                                          ON a.id = b.fer_id ");
            //strSql.Append("SELECT a.id,a.name,a.phone,a.spassword,a.company_address,a.myreal_name,a.myreal_card,a.bank_name,a.bank_card,a.bank_code,a.first_num,a.last_num,a.bank_phone,a.isreal_card,a.isbank_card,a.requestid,a.status,a.r_time,b.amount,a.pwd,a.l_name FROM fer_info AS a LEFT JOIN fer_account AS b ON a.id=b.fer_id ");
            strSql.Append(" where a.id=?financier_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int32,11)			};
            parameters[0].Value = financier_id;
            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public UserInfo_FinancingManagement_Model GetUserModel(string l_name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.id,a.name,a.phone,a.spassword,a.company_address,a.myreal_name,a.myreal_card,a.bank_name,a.bank_card,a.bank_code,a.first_num,a.last_num,a.bank_phone,a.isreal_card,a.isbank_card,a.requestid,a.status,a.r_time,b.amount,a.pwd,a.l_name FROM fer_info AS a LEFT JOIN fer_account AS b ON a.id=b.fer_id ");
            strSql.Append(" where a.l_name=?l_name ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?l_name", MySqlDbType.VarChar,20)			};
            parameters[0].Value = l_name;
            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体（通过用户手机号）
        /// </summary>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public UserInfo_FinancingManagement_Model GetUserModelByPhone(string Phone)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select fina_name,user_type,phone,password,company_address,myreal_name,myreal_card,bank_name,bank_card,bank_code,first_num,last_num,bank_phone,isreal_card,isbank_card,requestid,status,reg_time from fina_user ");
            strSql.Append(" where phone=?Phone");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Phone", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = Phone;

            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年5月29日 12:02:02 侯裕祥 得到一条用户信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 得到用户简单实体信息
        /// <summary>
        /// 得到用户简单实体信息（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public UserSmallReads GetUserSimall(int financier_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT spassword,id,phone,myreal_name,myreal_card,bank_card,first_num,last_num,isreal_card,isbank_card,bank_phone FROM fer_info ");
            strSql.Append(" where id=?financier_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int32,11)};
            parameters[0].Value = financier_id;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModelUserSmallReads(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据账户id获取融资方信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public UserSmallReads GetMerchantInfoByAccountID(int accountID)
        {
            var strSql = @" SELECT spassword,a.id,phone,myreal_name,myreal_card,bank_card,first_num,last_num,isreal_card,isbank_card,bank_phone 
                            FROM fer_info a
                            INNER JOIN fer_account b ON a.id = b.fer_id
                            WHERE b.id = ?ID";
            MySqlParameter[] parameters = {
					new MySqlParameter("?ID", MySqlDbType.Int32,11)};
            parameters[0].Value = accountID;

            var ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据账户id获取融资方信息", parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModelUserSmallReads(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 判断融资方是否信息存在
        /// <summary>
        /// 判断融资方是否信息存在
        /// </summary>
        /// <param name="account_id">用户（账户ID）</param>    
        /// <returns></returns>
        public int ISUserExist(int account_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from fer_info ");
            strSql.Append(" where id =?financier_id and status=1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int32,50),
                 
			};
            parameters[0].Value = account_id;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 18:04:54-侯裕祥-判断融资方是否信息存在", parameters);

            if (i != null)
            {
                if (Convert.ToInt64(i) > 0)
                {
                    return int.Parse(i.ToString());
                }
            }
            return 0;
        }
        #endregion

        #region 判断融资方是否存在账户金额信息
        /// <summary>
        /// 判断融资方是否存在账户金额信息
        /// </summary>
        /// <param name="account_id">用户（账户ID）</param>    
        /// <returns></returns>
        public int ISUser_financier_accountExist(int financier_account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from fer_account ");
            strSql.Append(" where fer_id =?financier_id  ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int32,50),
                 
			};
            parameters[0].Value = financier_account;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 18:04:54-侯裕祥-判断融资方是否存在账户金额信息", parameters);

            if (i != null)
            {
                if (Convert.ToInt64(i) > 0)
                {
                    return int.Parse(i.ToString());
                }
            }
            return 0;
        }
        #endregion

        #region 修改手机认证
        /// <summary>
        /// 修改手机认证
        /// </summary>
        /// <param name="account_id">融资方ID</param>
        /// <param name="Phone">手机号</param>
        /// <returns></returns>
        public bool UpdateUserPhone(int account_id, string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set phone=?Phone where id=?id ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64,4),
                    new MySqlParameter("?Phone", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = account_id;
            parameters[1].Value = Phone;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户密码", parameters);
        }
        #endregion

        #region 交易密码
        /// <summary>
        /// 判断交易密码是否正确
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTrueSallPassWord(int uid, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select 1 from fer_info ");
            strSql.Append(" where id =?id and spassword=?Password ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64,4),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = uid;
            parameters[1].Value = password;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-hyx-判断密码是否正确", parameters);

            if (i != null)
            {
                if (Convert.ToInt64(i) > 0)
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        #region 修改交易密码
        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="account_id">融资方ID</param>
        /// <param name="Password">交易密码</param>
        /// <returns></returns>
        public bool UpdateUserSPassWord(int account_id, string Password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set spassword=?Password where id=?id ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64,4),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = account_id;
            parameters[1].Value = Password;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户密码", parameters);
        }
        #endregion

        #region 修改登录密码
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="account_id">融资方ID</param>
        /// <param name="Password">登录密码</param>
        /// <returns></returns>
        public bool UpdateUserPassWord(int account_id, string Password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set pwd=?Password where id=?id ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64,4),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = account_id;
            parameters[1].Value = Password;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户密码", parameters);
        }
        #endregion

        #region 修改公司名称
        /// <summary>
        /// 修改公司名称
        /// </summary>
        /// <param name="account_id">融资方ID</param>
        /// <param name="Password">公司名称</param>
        /// <returns></returns>
        public bool UpdateCompanyName(int account_id, string Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set name=?Name where id=?id ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int64,4),
                    new MySqlParameter("?Name", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = account_id;
            parameters[1].Value = Name;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改公司名称", parameters);
        }
        #endregion

        #region 判断登录密码是否正确
        /// <summary>
        /// 判断登录密码是否正确
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTruePassWord(int uid, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select 1 from fer_info ");
            strSql.Append(" where status=1 and pwd=?Password and id=?id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32,4),
                    new MySqlParameter("?Password", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = uid;
            parameters[1].Value = password;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-hyx-判断密码是否正确", parameters);

            if (i != null)
            {
                if (Convert.ToInt32(i) > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 绑定银行卡
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public bool BoundBankCard(UserInfo_FinancingManagement_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info set ");
            strSql.Append("bank_name=?bank_name,");
            strSql.Append("bank_card=?bank_card,");
            strSql.Append("bank_code=?bank_code,");
            strSql.Append("first_num=?first_num,");
            strSql.Append("last_num=?last_num,");
            strSql.Append("bank_phone=?bank_phone,");
            strSql.Append("isbank_card=?isbank_card,");
            strSql.Append("requestid=?requestid");
            strSql.Append(" where id=?financier_id");
            MySqlParameter[] parameters = {		
					new MySqlParameter("?bank_name", MySqlDbType.VarChar,150),
					new MySqlParameter("?bank_card", MySqlDbType.VarChar,50),
					new MySqlParameter("?bank_code", MySqlDbType.VarChar,20),
					new MySqlParameter("?first_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?last_num", MySqlDbType.VarChar,10),
					new MySqlParameter("?bank_phone", MySqlDbType.VarChar,20),		
					new MySqlParameter("?isbank_card", MySqlDbType.Int16,3),
					new MySqlParameter("?requestid", MySqlDbType.VarChar,20),	
					new MySqlParameter("?financier_id", MySqlDbType.Int64,20)};
            parameters[0].Value = model.bank_name;
            parameters[1].Value = model.bank_card;
            parameters[2].Value = model.bank_code;
            parameters[3].Value = model.first_num;
            parameters[4].Value = model.last_num;
            parameters[5].Value = model.bank_phone;
            parameters[6].Value = model.isbank_card;
            parameters[7].Value = model.requestid;
            parameters[8].Value = model.financier_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 绑定银行卡", parameters);
        }
        #endregion

        #region 身份证验证
        /// <summary>
        /// 修改用户的认证状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="MyRealCard">用户身份证</param>
        /// <returns>true：身份验证设置成功 false：身份证设置失败</returns>
        public bool UpdateIsRealCard(int UserID, string MyRealCard, string Username)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_info SET isreal_card=1,myreal_name=?MyRealName,myreal_card=?MyRealCard");
            strSql.Append(" where  id=?UserID");
            MySqlParameter[] parameters = {			
		            new MySqlParameter("?MyRealCard", MySqlDbType.VarChar,50),
                    new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
                    new MySqlParameter("?UserID", MySqlDbType.Int32,11)
			};
            parameters[0].Value = MyRealCard;
            parameters[1].Value = Username;
            parameters[2].Value = UserID;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2014年11月27日-侯裕祥-修改身份证认证情况", parameters);
        }
        #endregion

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UserInfo_FinancingManagement_Model DataRowToModel(DataRow row)
        {
            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.financier_id = int.Parse(row["id"].ToString());
                }
                if (row["name"] != null)
                {
                    model.financier_name = row["name"].ToString();
                }
                if (row["phone"] != null)
                {
                    model.phone = row["phone"].ToString();
                }
                if (row["spassword"] != null)
                {
                    model.spassword = row["spassword"].ToString();
                }
                if (row["company_address"] != null)
                {
                    model.company_address = row["company_address"].ToString();
                }
                if (row["myreal_name"] != null)
                {
                    model.myreal_name = row["myreal_name"].ToString();
                }
                if (row["myreal_card"] != null)
                {
                    model.myreal_card = row["myreal_card"].ToString();
                }
                if (row["bank_name"] != null)
                {
                    model.bank_name = row["bank_name"].ToString();
                }
                if (row["bank_card"] != null)
                {
                    model.bank_card = row["bank_card"].ToString();
                }
                if (row["bank_code"] != null)
                {
                    model.bank_code = row["bank_code"].ToString();
                }
                if (row["first_num"] != null)
                {
                    model.first_num = row["first_num"].ToString();
                }
                if (row["last_num"] != null)
                {
                    model.last_num = row["last_num"].ToString();
                }
                if (row["bank_phone"] != null)
                {
                    model.bank_phone = row["bank_phone"].ToString();
                }
                if (row["isreal_card"] != null && row["isreal_card"].ToString() != "")
                {
                    model.isreal_card = int.Parse(row["isreal_card"].ToString());
                }
                if (row["isbank_card"] != null && row["isbank_card"].ToString() != "")
                {
                    model.isbank_card = int.Parse(row["isbank_card"].ToString());
                }
                if (row["requestid"] != null)
                {
                    model.requestid = row["requestid"].ToString();
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["r_time"] != null && row["r_time"].ToString() != "")
                {
                    model.reg_time = DateTime.Parse(row["r_time"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Money = Decimal.Parse(row["amount"].ToString());
                }
                if (row["pwd"] != null && row["pwd"].ToString() != "")
                {
                    model.pwd = row["pwd"].ToString();
                }
                if (row["l_name"] != null && row["l_name"].ToString() != "")
                {
                    model.l_name = row["l_name"].ToString();
                }

            }
            return model;
        }

        /// <summary>
        /// 得到一个j简体对象实体
        /// </summary>
        public UserSmallReads DataRowToModelUserSmallReads(DataRow row)
        {

            UserSmallReads model = new UserSmallReads();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.AccountID = int.Parse(row["id"].ToString());
                }
                if (row["bank_card"] != null)
                {
                    model.BankCard = row["bank_card"].ToString();
                }
                if (row["first_num"] != null)
                {
                    model.First_num = row["first_num"].ToString();
                }
                if (row["last_num"] != null)
                {
                    model.Last_num = row["last_num"].ToString();
                }
                if (row["isbank_card"] != null)
                {
                    model.IsBindBank = int.Parse(row["isbank_card"].ToString());
                }
                if (row["isreal_card"] != null)
                {
                    model.IsRealCard = int.Parse(row["isreal_card"].ToString());
                }

                if (row["myreal_card"] != null)
                {
                    model.MyRealCard = row["myreal_card"].ToString();
                }
                if (row["myreal_name"] != null)
                {
                    model.MyRealName = row["myreal_name"].ToString();
                }
                if (row["phone"] != null)
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row["bank_phone"] != null)
                {
                    model.Bank_Phone = row["bank_phone"].ToString();
                }
                if (row["spassword"] != null)
                {
                    model.SPassword = row["spassword"].ToString();
                }

            }
            return model;
        }
        #endregion

        #region 融资方充值记录操作

        #region 账户充值余额
        /// <summary>
        /// 账户充值余额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserRecharge_Balance(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT amount_repay   FROM fer_account where fer_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "查看融资方账户充值余额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        #endregion

        #region 添加融资方充值记录
        /// <summary>
        /// 添加融资方充值记录（只是申请，未经过第三方通知成功）
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public bool Addfina_recharge_record(Fina_Recharge_Record_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fer_account_rec(");
            strSql.Append("c_time,creator_id,amount,status,bankcard,third_order,repay_type,version,remark,m_time,fer_id,fer_account_id,pay_type,mer_ord_id,user_poundage,mer_poundage)");
            strSql.Append(" values (");
            strSql.Append("?c_time,?creator_id,?amount,?status,?bankcard,?third_order,?repay_type,?version,?remark,?m_time,?fer_id,?fer_account_id,?pay_type,?mer_ord_id,?user_poundage,?mer_poundage)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?bankcard", MySqlDbType.VarChar,50),
					new MySqlParameter("?third_order", MySqlDbType.VarChar,50),
					new MySqlParameter("?repay_type", MySqlDbType.Int16,3),
					new MySqlParameter("?version", MySqlDbType.Int32,4),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?fer_account_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pay_type", MySqlDbType.Int16,4),
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,50),
					new MySqlParameter("?user_poundage", MySqlDbType.Decimal,20),
					new MySqlParameter("?mer_poundage", MySqlDbType.Decimal,20)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = model.creator_id;
            parameters[2].Value = model.amount;
            parameters[3].Value = model.status;
            parameters[4].Value = model.bankcard;
            parameters[5].Value = model.third_order;
            parameters[6].Value = model.repay_type;
            parameters[7].Value = model.version;
            parameters[8].Value = "融资方充值金额：" + model.amount + "￥";
            parameters[9].Value = DateTime.Now;
            parameters[10].Value = model.fer_id;
            parameters[11].Value = model.fer_account_id;
            parameters[12].Value = model.pay_type;
            parameters[13].Value = model.mer_ord_id;
            parameters[14].Value = model.user_poundage;
            parameters[15].Value = model.mer_poundage;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:48:12 侯裕祥 添加一条融资方充值记录信息", parameters);

        }
        #endregion

        #region 得到用户充值信息
        /// <summary>
        /// 得到用户充值信息（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public Fina_Recharge_Record_Model GetFina_Recharge_Record_Model(int financier_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select financier_id,recharge_type,recharge_time,TRUNCATE(recharge_money,2) recharge_money,recharge_status,recharge_bankcard,recharge_order from financier_account_records ");
            strSql.Append(" where financier_id=?financier_id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int32,11)			};
            parameters[0].Value = financier_id;
            Fina_Recharge_Record_Model model = new Fina_Recharge_Record_Model();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户充值信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel_Fina_Recharge_Record_Model(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到用户充值信息（通过订单）
        /// </summary>
        /// <param name="recharge_order">订单</param>
        /// <returns></returns>
        public Fina_Recharge_Record_Model GetFina_Recharge_Record_Model(long recharge_order)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,c_time,creator_id,amount,status,bankcard,third_order,repay_type,version,remark,m_time,fer_id,fer_account_id,pay_type,mer_ord_id,user_poundage,mer_poundage from fer_account_rec  ");
            strSql.Append(" where mer_ord_id=?recharge_order ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?recharge_order", MySqlDbType.Int64,20)			};
            parameters[0].Value = recharge_order;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户充值信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {

                return DataRowToModel_Fina_Recharge_Record_Model(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到用户充值信息（通过订单）
        /// </summary>
        /// <param name="recharge_order">订单</param>
        /// <returns></returns>
        public Fina_Recharge_Record_Model GetFina_Recharge_Record_ModeNE(long recharge_order)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,amount,status,fer_id,fer_account_id from fer_account_rec  ");
            strSql.Append(" where mer_ord_id=?recharge_order ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?recharge_order", MySqlDbType.Int64,20)			};
            parameters[0].Value = recharge_order;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户充值信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fina_Recharge_Record_Model model = new Fina_Recharge_Record_Model();
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["status"] != null && dr["status"].ToString() != "")
                {
                    model.status = int.Parse(dr["status"].ToString());
                }
                if (dr["amount"] != null && dr["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(dr["amount"].ToString());
                }
                if (dr["fer_id"] != null && dr["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(dr["fer_id"].ToString());
                }
                if (dr["fer_account_id"] != null && dr["fer_account_id"].ToString() != "")
                {
                    model.fer_account_id = int.Parse(dr["fer_account_id"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 得到用户充值记录信息列表
        /// <summary>
        /// 得到用户充值记录信息列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="SMoney">开始金钱</param>
        /// <param name="EMoney">截止金钱</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">结束时间</param>
        /// <returns></returns>
        public List<Fina_Recharge_Record_Model> GetFina_Recharge_Record_Model_List(int UID, decimal SMoney, decimal EMoney, DateTime Sdate, DateTime Edate, int State)
        {
            if (Edate != DateTime.MinValue)
            {
                Edate = new DateTime(Edate.Year, Edate.Month, Edate.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
                           strSql.Append(@"SELECT id, 
                               c_time, 
                               creator_id, 
                               TRUNCATE(amount, 2) amount, 
                               status, 
                               bankcard, 
                               third_order, 
                               repay_type, 
                               version, 
                               remark, 
                               m_time, 
                               fer_id, 
                               fer_account_id, 
                               pay_type, 
                               mer_ord_id 
                        FROM   fer_account_rec 
                        WHERE  fer_id =?UID ");
            //strSql.Append(" select  id,c_time,creator_id,TRUNCATE(amount,2)  amount,status,bankcard,third_order,repay_type,version,remark,m_time,fer_id,fer_account_id,pay_type,mer_ord_id from fer_account_rec  where fer_id=?UID");
            if (State != -1)
            {
                strSql.Append(" And status = ?State ");
            }
            if (SMoney > 0)
            {
                strSql.Append(" And amount >= ?SMoney ");
            }
            if (EMoney > 0)
            {
                strSql.Append(" And amount <= ?EMoney ");
            }
            if (SMoney > 0 && EMoney > 0)
            {
                strSql.Append(" And amount Between ?SMoney  And ?EMoney ");
            }
            string sqlWhere = " ";
            if (Sdate != DateTime.MinValue)
            {
                sqlWhere += " And c_time  >= ?Sdate ";
            }
            if (Edate != DateTime.MinValue)
            {
                sqlWhere += " And c_time  <= ?Edate  ";
            }
            if (Sdate != DateTime.MinValue && Edate != DateTime.MinValue)
            {
                sqlWhere = " And c_time Between ?Sdate  And ?Edate ";
            }
            strSql.Append("  " + sqlWhere + " ");
            strSql.Append(" order by c_time desc ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?UID", MySqlDbType.Int32),
                    new MySqlParameter("?SMoney", MySqlDbType.Decimal),
                     new MySqlParameter("?EMoney", MySqlDbType.Decimal),
                    new MySqlParameter("?Sdate", MySqlDbType.DateTime),           
                    new MySqlParameter("?Edate", MySqlDbType.DateTime),
                    new MySqlParameter("?State", MySqlDbType.Int32),
            };
            parameters[0].Value = UID;
            parameters[1].Value = SMoney;
            parameters[2].Value = EMoney;
            parameters[3].Value = Sdate;
            parameters[4].Value = Edate;
            parameters[5].Value = State;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取所有的用户充值记录", parameters);
            List<Fina_Recharge_Record_Model> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Fina_Recharge_Record_Model>();
                Fina_Recharge_Record_Model mod = new Fina_Recharge_Record_Model();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToModel_Fina_Recharge_Record_Model(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion

        #region 修改充值记录(充值成功时改变充值状态并且帮助赵亮插入一条融资方账户日志表)用事务写
        /// <summary>
        /// 修改融资方充值记录信息
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="FID">融资方ID</param>
        /// <param name="FAccount">融资方账户ID</param>
        /// <param name="Money">充值金额</param>
        /// <param name="Money_Befo">变化前的金额</param>
        /// <param name="Money_Last">变化后的金额</param>
        /// <param name="r2_TrxId">易宝流水订单号</param>
        /// <param name="rq_SourceFee">用户手续费</param>
        /// <param name="rq_TargetFee">商户手续费</param>
        /// <returns></returns>
        public bool UpdateUserFina_Recharge_Record_Model(string OrderID, string FID, string FAccount, decimal Money, decimal Money_Befo, decimal Money_Last, string r2_TrxId, decimal rq_SourceFee, decimal rq_TargetFee)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 充值成功后改变状态    fer_account_rec  
            #region
            var sql1 = @"UPDATE fer_account_rec 
                                SET    status=1, 
                                        third_order=?third_order, 
                                        user_poundage=?user_poundage, 
                                        mer_poundage=?mer_poundage 
                                WHERE  mer_ord_id=?mer_ord_id";
            MySqlParameter[] param1 = {
                    new MySqlParameter("?mer_ord_id", MySqlDbType.VarChar,50),
                    new MySqlParameter("?third_order", MySqlDbType.VarChar,50),
                    new MySqlParameter("?user_poundage", MySqlDbType.Decimal),
                    new MySqlParameter("?mer_poundage", MySqlDbType.Decimal)};
            param1[0].Value = OrderID;
            param1[1].Value = r2_TrxId;
            param1[2].Value = rq_SourceFee;
            param1[3].Value = rq_TargetFee;
            sqlArray.Add(sql1);
            paramArray.Add(param1);

            #endregion

            // 2）充值成功后插入一条融资方账户日志记录   fer_account_log
            #region
            var sql2 = @" INSERT INTO fer_account_log 
                                            (c_time, 
                                             creator_id, 
                                             m_time, 
                                             fer_id, 
                                             fer_account_id, 
                                             type, 
                                             amount_before, 
                                             amount_after, 
                                             change_amount, 
                                             account_source_id, 
                                             from_id, 
                                             pjt_id, 
                                             user_id, 
                                             ord_id, 
                                             rfd_id, 
                                             version, 
                                             remark) 
                                VALUES      (?c_time, 
                                             ?creator_id, 
                                             ?m_time, 
                                             ?fer_id, 
                                             ?fer_account_id, 
                                             ?type, 
                                             ?amount_before, 
                                             ?amount_after, 
                                             ?change_amount, 
                                             ?account_source_id, 
                                             ?from_id, 
                                             ?pjt_id, 
                                             ?user_id, 
                                             ?ord_id, 
                                             ?rfd_id, 
                                             ?version, 
                                             ?remark)";
            MySqlParameter[] param2 = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?fer_account_id", MySqlDbType.Int32,11),
					new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?amount_before", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_after", MySqlDbType.Decimal,20),
					new MySqlParameter("?change_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?account_source_id", MySqlDbType.Int16,4),
					new MySqlParameter("?from_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?user_id", MySqlDbType.Int32,11),
					new MySqlParameter("?ord_id", MySqlDbType.Int64,20),
					new MySqlParameter("?rfd_id", MySqlDbType.Int64,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            param2[0].Value = DateTime.Now;
            param2[1].Value = 0;
            param2[2].Value = DateTime.Now;
            param2[3].Value = FID;
            param2[4].Value = FAccount;
            param2[5].Value = 1;
            param2[6].Value = Money_Befo;
            param2[7].Value = Money_Last;
            param2[8].Value = Money;
            param2[9].Value = 19;
            param2[10].Value = 0;
            param2[11].Value = 0;
            param2[12].Value = 0;
            param2[13].Value = 0;
            param2[14].Value = 0;
            param2[15].Value = 0;
            param2[16].Value = "融资方充值：" + Money + "￥ 【账户金额变化前：" + Money_Befo + "变化后：" + Money_Last + "】";

            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            // 3）充值成功后更新融资方账户金额表   fer_account
            #region
            var sql3 = @"
                        UPDATE fer_account
                        SET amount=amount+?Amount,
                            amount_repay=amount_repay+?Amount
                        WHERE id=?MID";
            MySqlParameter[] param3 = {
				new MySqlParameter("?Amount", MySqlDbType.Decimal),
                new MySqlParameter("?MID", MySqlDbType.Int64)};
            param3[0].Value = Money;
            param3[1].Value = FAccount;
            sqlArray.Add(sql3);
            paramArray.Add(param3);
            #endregion
            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Fina_Recharge_Record_Model DataRowToModel_Fina_Recharge_Record_Model(DataRow row)
        {
            Fina_Recharge_Record_Model model = new Fina_Recharge_Record_Model();
            if (row != null)
            {

                if (row["mer_ord_id"] != null && row["mer_ord_id"].ToString() != "")
                {
                    model.mer_ord_id = row["mer_ord_id"].ToString();
                }
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["creator_id"] != null && row["creator_id"].ToString() != "")
                {
                    model.creator_id = int.Parse(row["creator_id"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["bankcard"] != null)
                {
                    model.bankcard = row["bankcard"].ToString();
                }
                if (row["third_order"] != null)
                {
                    model.third_order = row["third_order"].ToString();
                }
                if (row["repay_type"] != null && row["repay_type"].ToString() != "")
                {
                    model.repay_type = int.Parse(row["repay_type"].ToString());
                }
                if (row["version"] != null && row["version"].ToString() != "")
                {
                    model.version = int.Parse(row["version"].ToString());
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }
                if (row["m_time"] != null && row["m_time"].ToString() != "")
                {
                    model.m_time = DateTime.Parse(row["m_time"].ToString());
                }
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(row["fer_id"].ToString());
                }
                if (row["fer_account_id"] != null && row["fer_account_id"].ToString() != "")
                {
                    model.fer_account_id = int.Parse(row["fer_account_id"].ToString());
                }
                if (row["pay_type"] != null && row["pay_type"].ToString() != "")
                {
                    model.pay_type = int.Parse(row["pay_type"].ToString());
                }
            }
            return model;
        }
        #endregion

        #endregion

        #region 用户账户金钱信息

        #region 添加融资方账户金钱信息
        /// <summary>
        /// 添加融资方账户金钱信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public int AddFina_User_Count_Model(Fina_User_Count_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fer_account(");
            strSql.Append("fer_id,c_time,creator_id,m_time,amount,amount_user,amount_repay,amount_repay_fz,amount_paid,amount_borrow,interest_payable,interest_paid,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?fer_id,?c_time,?creator_id,?m_time,?amount,?amount_user,?amount_repay,?amount_repay_fz,?amount_paid,?amount_borrow,?interest_payable,?interest_paid,?version,?remark);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_user", MySqlDbType.Decimal,20),                    
					new MySqlParameter("?amount_repay", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_repay_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_paid", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_borrow", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_payable", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_paid", MySqlDbType.Decimal,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500)};
            parameters[0].Value = model.fer_id;
            parameters[1].Value = model.c_time;
            parameters[2].Value = model.creator_id;
            parameters[3].Value = model.m_time;
            parameters[4].Value = model.amount;
            parameters[5].Value = model.amount_user;
            parameters[6].Value = model.amount_repay;
            parameters[7].Value = model.amount_repay_fz;
            parameters[8].Value = model.amount_paid;
            parameters[9].Value = model.amount_borrow;
            parameters[10].Value = model.interest_payable;
            parameters[11].Value = model.interest_paid;
            parameters[12].Value = model.version;
            parameters[13].Value = model.remark;

            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 添加一条融资方账户金额信息", parameters);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;
        }

        #endregion

        #region 修改用户账户金额信息
        /// <summary>
        /// 修改用户账户金额信息
        /// </summary>
        /// <param name="model">用户账户金额信息</param>
        /// <returns></returns>
        public bool UpdateFina_User_Count_Model(Fina_User_Count_Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_account set ");
            strSql.Append("fer_id=?fer_id,");
            strSql.Append("c_time=?c_time,");
            strSql.Append("creator_id=?creator_id,");
            strSql.Append("m_time=?m_time,");
            strSql.Append("amount=?amount,");
            strSql.Append("amount_user=?amount_user,");
            strSql.Append("amount_repay=?amount_repay,");
            strSql.Append("amount_repay_fz=?amount_repay_fz,");
            strSql.Append("amount_paid=?amount_paid,");
            strSql.Append("amount_borrow=?amount_borrow,");
            strSql.Append("interest_payable=?interest_payable,");
            strSql.Append("interest_paid=?interest_paid,");
            strSql.Append("version=?version,");
            strSql.Append("remark=?remark");
            strSql.Append(" where id=?id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_user", MySqlDbType.Decimal,20),                 
					new MySqlParameter("?amount_repay", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_repay_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_paid", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_borrow", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_payable", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_paid", MySqlDbType.Decimal,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.fer_id;
            parameters[1].Value = model.c_time;
            parameters[2].Value = model.creator_id;
            parameters[3].Value = model.m_time;
            parameters[4].Value = model.amount;
            parameters[5].Value = model.amount_user;
            parameters[6].Value = model.amount_repay;
            parameters[7].Value = model.amount_repay_fz;
            parameters[8].Value = model.amount_paid;
            parameters[9].Value = model.amount_borrow;
            parameters[10].Value = model.interest_payable;
            parameters[11].Value = model.interest_paid;
            parameters[12].Value = model.version;
            parameters[13].Value = model.remark;
            parameters[14].Value = model.id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:35:29 侯裕祥 修改用户账户金额信息", parameters);
        }

        #endregion

        #region 得到用户账户金额信息
        /// <summary>
        /// 得到用户充值信息（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public Fina_User_Count_Model GetFina_User_Count_Model(int financier_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT id, 
                               fer_id, 
                               c_time, 
                               creator_id, 
                               m_time, 
                               TRUNCATE(amount, 2)           amount, 
                               TRUNCATE(amount_user, 2)      amount_user, 
                               TRUNCATE(amount_repay, 2)     amount_repay, 
                               TRUNCATE(amount_repay_fz, 2)  amount_repay_fz, 
                               TRUNCATE(amount_paid, 2)      amount_paid, 
                               TRUNCATE(amount_borrow, 2)    amount_borrow, 
                               TRUNCATE(interest_payable, 2) interest_payable, 
                               TRUNCATE(interest_paid, 2)    interest_paid, 
                               VERSION, 
                               remark 
                        FROM   fer_account  ");
            strSql.Append(" where id=?id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = financier_id;

            Fina_User_Count_Model model = new Fina_User_Count_Model();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年6月16日 18:11:53 侯裕祥 得到一条用户充值信息", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel_Fina_User_Count_Model(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 得到账户的金钱余额
        /// <summary>
        /// 得到账户的金钱余额
        /// </summary>
        /// <param name="financier_id"></param>
        /// <returns></returns>
        public decimal GetUserAccountMoney(int financier_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select amount from fer_account where fer_id=?financier_id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?financier_id", MySqlDbType.Int64,11)};
            parameters[0].Value = financier_id;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取用户账户的金钱余额", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        #endregion

        #region 得到用户账户信息列表
        /// <summary>
        /// 得到用户账户信息列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<Fina_User_Count_Model> GetFina_User_Count_Model_List(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,fer_id,c_time,creator_id,m_time,amount,amount_user,amount_repay,amount_repay_fz,amount_paid,amount_borrow,interest_payable,interest_paid,version,remark from fer_account");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" Order By c_time Desc ");
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取所有的用户账户金额记录");
            List<Fina_User_Count_Model> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Fina_User_Count_Model>();
                Fina_User_Count_Model mod = new Fina_User_Count_Model();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToModel_Fina_User_Count_Model(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Fina_User_Count_Model DataRowToModel_Fina_User_Count_Model(DataRow row)
        {
            Fina_User_Count_Model model = new Fina_User_Count_Model();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(row["fer_id"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["creator_id"] != null && row["creator_id"].ToString() != "")
                {
                    model.creator_id = int.Parse(row["creator_id"].ToString());
                }
                if (row["m_time"] != null && row["m_time"].ToString() != "")
                {
                    model.m_time = DateTime.Parse(row["m_time"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["amount_user"] != null && row["amount_user"].ToString() != "")
                {
                    model.amount_user = decimal.Parse(row["amount_user"].ToString());
                }
                if (row["amount_repay"] != null && row["amount_repay"].ToString() != "")
                {
                    model.amount_repay = decimal.Parse(row["amount_repay"].ToString());
                }
                if (row["amount_repay_fz"] != null && row["amount_repay_fz"].ToString() != "")
                {
                    model.amount_repay_fz = decimal.Parse(row["amount_repay_fz"].ToString());
                }
                if (row["amount_paid"] != null && row["amount_paid"].ToString() != "")
                {
                    model.amount_paid = decimal.Parse(row["amount_paid"].ToString());
                }
                if (row["amount_borrow"] != null && row["amount_borrow"].ToString() != "")
                {
                    model.amount_borrow = decimal.Parse(row["amount_borrow"].ToString());
                }
                if (row["interest_payable"] != null && row["interest_payable"].ToString() != "")
                {
                    model.interest_payable = decimal.Parse(row["interest_payable"].ToString());
                }
                if (row["interest_paid"] != null && row["interest_paid"].ToString() != "")
                {
                    model.interest_paid = decimal.Parse(row["interest_paid"].ToString());
                }
                if (row["version"] != null && row["version"].ToString() != "")
                {
                    model.version = int.Parse(row["version"].ToString());
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }
            }
            return model;
        }
        #endregion

        #endregion

        #region 帮助赵亮插入充值记录信息（只有充值成功的才插入）
        /// <summary>
        /// 帮助赵亮插入充值记录信息
        /// </summary>
        public int Add_financier_account_log(Financier_account_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fer_account_log(");
            strSql.Append("c_time,creator_id,m_time,fer_id,fer_account_id,type,amount_before,amount_after,change_amount,account_source_id,from_id,pjt_id,user_id,ord_id,rfd_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?c_time,?creator_id,?m_time,?fer_id,?fer_account_id,?type,?amount_before,?amount_after,?change_amount,?account_source_id,?from_id,?pjt_id,?user_id,?ord_id,?rfd_id,?version,?remark);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?fer_account_id", MySqlDbType.Int32,11),
					new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?amount_before", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_after", MySqlDbType.Decimal,20),
					new MySqlParameter("?change_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?account_source_id", MySqlDbType.Int16,4),
					new MySqlParameter("?from_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?user_id", MySqlDbType.Int32,11),
					new MySqlParameter("?ord_id", MySqlDbType.Int32,20),
					new MySqlParameter("?rfd_id", MySqlDbType.Int32,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            parameters[0].Value = model.c_time;
            parameters[1].Value = model.creator_id;
            parameters[2].Value = model.m_time;
            parameters[3].Value = model.fer_id;
            parameters[4].Value = model.fer_account_id;
            parameters[5].Value = model.type;
            parameters[6].Value = model.amount_before;
            parameters[7].Value = model.amount_after;
            parameters[8].Value = model.change_amount;
            parameters[9].Value = model.account_source_id;
            parameters[10].Value = model.from_id;
            parameters[11].Value = model.pjt_id;
            parameters[12].Value = model.user_id;
            parameters[13].Value = model.ord_id;
            parameters[14].Value = model.rfd_id;
            parameters[15].Value = model.version;
            parameters[16].Value = model.remark;

            //return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:48:12 侯裕祥 添加一条融资方充值记录信息", parameters);
            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 帮助赵亮插入充值记录信息", parameters);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;

        }
        #endregion


        #region 个人中心项目信息偿还资金操作

        #region 利息列表操作

        #region 获取利息列表
        /// <summary>
        /// 得到用户利息偿还数据列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">结束时间</param>
        /// <returns></returns>
        public List<Fer_account_item> GetFer_account_item_List(int UID, int LoanPeriod, decimal Money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT p.id  AS PID, 
                                    p.amount AS pamount, 
                                    p.e_time, 
                                    p.loan_period, 
                                    p.name   AS pname, 
                                    f.c_time, 
                                    TRUNCATE(f.interest_paid, 2) interest_paid,
                                    TRUNCATE(f.interest_payable, 2) interest_payable
                            FROM   fer_account_item AS f 
                                    INNER JOIN pjt AS p ON f.pjt_id = p.id                                  
				                    INNER JOIN fer_info I ON I.`id` = f.`fer_id`
                            WHERE  I.id =?UID");
            if (LoanPeriod > 0)
            {
                strSql.Append(" And  p.loan_period=?LoanPeriod");
            }

            if (Money > 0)
            { strSql.Append(" And  f.interest_payable >=?Money"); }
            strSql.Append(" Order By f.c_time Desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UID", MySqlDbType.Int32),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?Money", MySqlDbType.Decimal)
            };
            parameters[0].Value = UID;
            parameters[1].Value = LoanPeriod;
            parameters[2].Value = Money;
            DataTable ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户利息偿还数据列表", parameters).Tables[0];
            List<Fer_account_item> list = null;
            if (ds.Rows.Count > 0)
            {
                list = new List<Fer_account_item>();
                
                foreach (DataRow row in ds.Rows)
                {
                    Fer_account_item mod = new Fer_account_item();
                    if (row["PID"] != null && row["PID"].ToString() != "")
                    {
                        mod.PID = int.Parse(row["PID"].ToString());
                    }
                    if (row["pname"] != null && row["pname"].ToString() != "")
                    {
                        mod.Pname = row["pname"].ToString();
                    }
                    if (row["pamount"] != null && row["pamount"].ToString() != "")
                    {
                        mod.Pmoney = decimal.Parse(row["pamount"].ToString());
                    }
                    if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                    {
                        mod.LoanPeriod = int.Parse(row["loan_period"].ToString());
                    }

                    if (row["interest_paid"] != null && row["interest_paid"].ToString() != "")
                    {
                        mod.interest_paid = decimal.Parse(row["interest_paid"].ToString());
                    }
                    if (row["interest_payable"] != null && row["interest_payable"].ToString() != "")
                    {
                        mod.interest_payable = decimal.Parse(row["interest_payable"].ToString());
                    }
                    list.Add(mod);
                }
            }
            return list;
        }

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体(利息偿还)
        /// </summary>
        public Fer_account_item DataRowToModel_Fer_account_item(DataRow row)
        {
            Fer_account_item model = new Fer_account_item();
            if (row != null)
            {

                if (row["interest_paid"] != null && row["interest_paid"].ToString() != "")
                {
                    model.interest_paid = decimal.Parse(row["interest_paid"].ToString());
                }

                if (row["PID"] != null && row["PID"].ToString() != "")
                {
                    model.PID = int.Parse(row["PID"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Pmoney = decimal.Parse(row["amount"].ToString());
                }
                if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                {
                    model.LoanPeriod = int.Parse(row["loan_period"].ToString());
                }
                if (row["pname"] != null && row["pname"].ToString() != "")
                {
                    model.Pname = row["pname"].ToString();
                }
            }
            return model;
        }

        #endregion

        #endregion

        #region 利息偿还数据列表(换了)
        /// <summary>
        /// 得到用户利息偿还数据列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">结束时间</param>
        /// <returns></returns>
        public List<Project_refund_interest> GetProject_refund_interest_List(int UID, string Sdate, string Edate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT p.id AS PID,p.amount AS pamount,p.e_time,p.loan_period,p.name  AS pname ,f.id,f.c_time,f.creator_id,f.m_time,f.fer_id,f.fer_account_id,
                            f.type,f.amount,f.status,f.op_status,f.third_ord_id,f.version,f.remark FROM fer_repay_interest AS f   
                            INNER JOIN  pjt AS p  ON f.fer_id=p.fer_account_id  ");

            strSql.Append(" where f.fer_id=?UID");

            if (!string.IsNullOrEmpty(Sdate))
            {
                strSql.Append(" And p.e_time  >= ?Sdate");
            }
            if (!string.IsNullOrEmpty(Edate))
            {
                strSql.Append(" And p.e_time  <= ?Edate");
            }
            else if (!string.IsNullOrEmpty(Edate) && !string.IsNullOrEmpty(Sdate))
            {
                strSql.Append(" And p.e_time Between ?Sdate   And  ?Edate ");
            }
            strSql.Append(" Order By c_time Desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UID", MySqlDbType.Int32),
                    new MySqlParameter("?Sdate", MySqlDbType.DateTime),
                    new MySqlParameter("?Edate", MySqlDbType.DateTime)           
                         
            };
            parameters[0].Value = UID;
            parameters[1].Value = Sdate;
            parameters[2].Value = Edate;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户利息偿还数据列表", parameters);
            List<Project_refund_interest> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Project_refund_interest>();
                Project_refund_interest mod = new Project_refund_interest();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToModel_Project_refund_interest(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体(利息偿还)
        /// </summary>
        public Project_refund_interest DataRowToModel_Project_refund_interest(DataRow row)
        {
            Project_refund_interest model = new Project_refund_interest();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["creator_id"] != null && row["creator_id"].ToString() != "")
                {
                    model.creator_id = int.Parse(row["creator_id"].ToString());
                }
                if (row["m_time"] != null && row["m_time"].ToString() != "")
                {
                    model.m_time = DateTime.Parse(row["m_time"].ToString());
                }
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(row["fer_id"].ToString());
                }
                if (row["fer_account_id"] != null && row["fer_account_id"].ToString() != "")
                {
                    model.fer_account_id = int.Parse(row["fer_account_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["op_status"] != null && row["op_status"].ToString() != "")
                {
                    model.op_status = int.Parse(row["op_status"].ToString());
                }
                if (row["third_ord_id"] != null)
                {
                    model.third_ord_id = row["third_ord_id"].ToString();
                }
                if (row["version"] != null && row["version"].ToString() != "")
                {
                    model.version = int.Parse(row["version"].ToString());
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }

                if (row["PID"] != null && row["PID"].ToString() != "")
                {
                    model.PID = int.Parse(row["PID"].ToString());
                }
                if (row["pamount"] != null && row["pamount"].ToString() != "")
                {
                    model.Pamount = decimal.Parse(row["pamount"].ToString());
                }
                if (row["e_time"] != null && row["e_time"].ToString() != "")
                {
                    model.PReturnTime = DateTime.Parse(row["e_time"].ToString());
                }
                if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                {
                    model.Loan_period = int.Parse(row["loan_period"].ToString());
                }
                if (row["pname"] != null && row["pname"].ToString() != "")
                {
                    model.Pname = row["pname"].ToString();
                }
            }
            return model;
        }

        #endregion

        #endregion

        #region 项目本金偿还操作

        #region 本金偿还列表（以项目为纬度）
        /// <summary>
        /// 本金偿还数据列表
        /// </summary>
        ///  <param name="Pname">项目名称</param>
        /// <param name="UID">融资方ID</param>
        /// <param name="status">项目状态</param>
        /// <param name="money">项目金额</param>
        /// <returns></returns>
        public List<ProjectModel> GetProjectModel_List(string Pname, int UID, int status, decimal money)
        {
            var model = new List<ProjectModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT   p.amount_sold,
                                        p.id, 
                                        p.NAME, 
                                        p.amount, 
                                        p.amount_paid, 
                                        p.pjt_status, 
                                        p.c_time, 
                                        p.loan_period, 
                                        p.fer_account_id as account_id, 
                                        p.rep_status,
                                        f.amount_repay 
                            FROM       pjt         AS p 
                            INNER JOIN fer_account AS f ON f.id=p.fer_account_id ");
            strSql.Append(" where p.is_deleted=0 AND p.pjt_status>1 AND p.amount_sold>0  AND f.fer_id=?UID ");
            //strSql.Append(" where p.is_deleted=0 AND  p.rep_status=0 AND f.fer_id=?UID ");

            if (!string.IsNullOrEmpty(Pname))
            {
                Pname = "%" + Pname + "%";
                strSql.Append(" And p.name like ?Panme  ");
            }
            if (status != -1)
            {
                strSql.Append(" And p.rep_status =?status ");
            }
            if (money > 0)
            {
                strSql.Append(" And p.amount =?Amount ");
            }
            strSql.Append(" order by p.c_time desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Panme", MySqlDbType.VarChar),
                    new MySqlParameter("?status", MySqlDbType.Int16),   
                    new MySqlParameter("?UID", MySqlDbType.Int32),     
                    new MySqlParameter("?Amount", MySqlDbType.Decimal)             
            };
            parameters[0].Value = Pname;
            parameters[1].Value = status;
            parameters[2].Value = UID;
            parameters[3].Value = money;
            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户本金偿还数据列表", parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProjectModel mod = new ProjectModel();
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        mod.Id = int.Parse(row["id"].ToString());
                    }
                    if (row["name"] != null)
                    {
                        mod.ProjectName = row["name"].ToString();
                    }
                    if (row["amount"] != null && row["amount"].ToString() != "")
                    {
                        mod.Amount = decimal.Parse(row["amount"].ToString());
                    }
                    if (row["account_id"] != null && row["account_id"].ToString() != "")
                    {
                        mod.account_id = int.Parse(row["account_id"].ToString());
                    }
                    if (row["amount_paid"] != null && row["amount_paid"].ToString() != "")
                    {
                        mod.Amount_paid = decimal.Parse(row["amount_paid"].ToString());
                    }
                    if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                    {
                        mod.LoanPeriod = int.Parse(row["loan_period"].ToString());
                    }
                    if (row["amount_repay"] != null && row["amount_repay"].ToString() != "")
                    {
                        mod.amount_repay = decimal.Parse(row["amount_repay"].ToString());
                    }
                    if (row["rep_status"] != null && row["rep_status"].ToString() != "")
                    {
                        mod.rep_status = int.Parse(row["rep_status"].ToString());
                    }
                    if (row["amount_sold"] != null && row["amount_sold"].ToString() != "")
                    {
                        mod.AmountSold = decimal.Parse(row["amount_sold"].ToString());
                    }


                    model.Add(mod);
                }
            }
            return model;
        }
        #endregion

        #region 后台判断融资方还款是否超出项目金额(或者账户的还款金额是否够用)(或者账户的还款金额是否够用)
        /// <summary>
        /// 后台判断融资方还款是否超出项目金额(或者账户的还款金额是否够用)
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="PID">项目ID</param>
        /// <returns></returns>
        public ProjectModel GetProjectModel(int UID, int PID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT p.id, 
                                    p.amount_sold,
                                    p.amount, 
                                    p.amount_paid, 
                                    f.amount_repay,
                                    f.amount_repay_fz,
                                    f.interest_payable
                            FROM   pjt AS p 
                                    INNER JOIN fer_account AS f 
                                             ON p.fer_account_id = f.id  
                                     INNER JOIN fer_info fi ON f.`fer_id`=fi.`id`");
            strSql.Append(" where p.is_deleted=0 AND p.rep_status=0 AND p.amount_sold>0  AND  fi.`id`=?UID AND p.id=?PID ");
            MySqlParameter[] parameters = {			 
                    new MySqlParameter("?UID", MySqlDbType.Int64),     
                    new MySqlParameter("?PID", MySqlDbType.Int64)             
            };
            parameters[0].Value = UID;
            parameters[1].Value = PID;
            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户本金偿还数据列表", parameters).Tables[0];
            ProjectModel mod = new ProjectModel();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        mod.Id = int.Parse(row["id"].ToString());
                    }
                    if (row["amount"] != null && row["amount"].ToString() != "")
                    {
                        mod.Amount = decimal.Parse(row["amount"].ToString());
                    }
                    if (row["amount_paid"] != null && row["amount_paid"].ToString() != "")
                    {
                        mod.Amount_paid = decimal.Parse(row["amount_paid"].ToString());
                    }
                    if (row["amount_repay"] != null && row["amount_repay"].ToString() != "")
                    {
                        mod.amount_repay = decimal.Parse(row["amount_repay"].ToString());
                    }
                    if (row["amount_repay_fz"] != null && row["amount_repay_fz"].ToString() != "")
                    {
                        mod.amount_repay_fz = decimal.Parse(row["amount_repay_fz"].ToString());
                    }
                    if (row["interest_payable"] != null && row["interest_payable"].ToString() != "")
                    {
                        mod.interest_payable = decimal.Parse(row["interest_payable"].ToString());
                    }
                    if (row["amount_sold"] != null && row["amount_sold"].ToString() != "")
                    {
                        mod.AmountSold = decimal.Parse(row["amount_sold"].ToString());
                    }

                }
            }
            return mod;
        }
        #endregion

        #region 融资方申请偿还本金
        /// <summary>
        /// 融资方申请偿还本金
        /// </summary>
        /// <returns></returns>
        public bool ApplyProject_refund_principal(Project_refund_principal model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE pjt SET rep_status=1 where id=?pjt_id;");
            strSql.Append("insert into fer_repay_principal(");
            strSql.Append("c_time,creator_id,m_time,fer_id,fer_account_id,pjt_id,amount,status,op_status,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?c_time,?creator_id,?m_time,?fer_id,?fer_account_id,?pjt_id,?amount,?status,?op_status,?version,?remark);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?fer_account_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,10),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?op_status", MySqlDbType.Int16,4),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = model.creator_id;
            parameters[2].Value = model.m_time;
            parameters[3].Value = model.fer_id;
            parameters[4].Value = model.fer_account_id;
            parameters[5].Value = model.pjt_id;
            parameters[6].Value = model.amount;
            parameters[7].Value = 1;
            parameters[8].Value = 0;
            parameters[9].Value = model.version;
            parameters[10].Value = "融资方还款申请";

            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 添加还款信息", parameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }
        /// <summary>
        /// 融资方申请偿还本金
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateProject_refund_principal(Int64 ID, int Status, decimal money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_repay_principal set ");
            strSql.Append("status=?status,");
            strSql.Append("amount=?amount_acctual  where id=?id");
            MySqlParameter[] parameters = { 
            new MySqlParameter("?status", MySqlDbType.Int16, 3),
            new MySqlParameter("?id", MySqlDbType.Int64, 20),
            new MySqlParameter("?amount_acctual", MySqlDbType.Decimal)};
            parameters[0].Value = Status;
            parameters[1].Value = ID;
            parameters[2].Value = money;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 融资方申请偿还本金", parameters);
        }
        #endregion

        #region 申请还款后改变账户表金额（冻结住）
        /// <summary>
        /// 申请还款后改变账户表金额（冻结住）
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="money">冻结金额</param>
        /// <returns></returns>
        public bool Updatefer_account(int UID, decimal money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_account set ");
            strSql.Append("amount_repay_fz=amount_repay_fz+?money  where fer_id=?UID");
            MySqlParameter[] parameters = {        
            new MySqlParameter("?UID", MySqlDbType.Int64, 20),
            new MySqlParameter("?money", MySqlDbType.Decimal)};
            parameters[0].Value = UID;
            parameters[1].Value = money;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 融资方申请偿还本金之后冻结住", parameters);
        }
        /// <summary>
        /// 申请还款
        /// </summary>
        /// <param name="merchantID">融资方ID</param>
        /// <param name="amount">还款金额</param>
        /// <param name="pjid">项目ID</param>
        /// <returns></returns>
        public bool RepayPrincipal(int merchantID, decimal amount, int pjid, int FerAcount)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 更新项目表还款申请状态          
            #region
            var sql1 = @"
                        UPDATE pjt SET rep_status=1 where id=?pjt_id;";
            MySqlParameter[] param1 = {
                    new MySqlParameter("?pjt_id", MySqlDbType.Int32)};
            param1[0].Value = pjid;
            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            // 2）申请还款后给晓明插入申请数据 fer_repay_principal
            #region
            var sql2 = @" INSERT INTO fer_repay_principal 
                                                (c_time, 
                                                 creator_id, 
                                                 m_time, 
                                                 fer_id, 
                                                 fer_account_id, 
                                                 pjt_id, 
                                                 amount, 
                                                 status, 
                                                 op_status, 
                                                 version, 
                                                 remark) 
                                    VALUES      (?c_time, 
                                                 ?creator_id, 
                                                 ?m_time, 
                                                 ?fer_id, 
                                                 ?fer_account_id, 
                                                 ?pjt_id, 
                                                 ?amount, 
                                                 ?status, 
                                                 ?op_status, 
                                                 ?version, 
                                                 ?remark);";
            MySqlParameter[] param2 = {
				new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?fer_account_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,10),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?op_status", MySqlDbType.Int16,4),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,200)};
            param2[0].Value = DateTime.Now;
            param2[1].Value = 0;
            param2[2].Value = DateTime.Now;
            param2[3].Value = merchantID;
            param2[4].Value = FerAcount;
            param2[5].Value = pjid;
            param2[6].Value = amount;
            param2[7].Value = 0;
            param2[8].Value = 0;
            param2[9].Value = 0;
            param2[10].Value = "融资方申请还款:" + amount + "￥";

            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            // 3）更新融资方账户表 fer_account (冻结金额)
            #region
            var sql3 = @"
                        UPDATE fer_account
                        SET amount_repay_fz=amount_repay_fz+?Amount
                        WHERE fer_id=?MID";
            MySqlParameter[] param3 = {
				new MySqlParameter("?Amount", MySqlDbType.Decimal),
                new MySqlParameter("?MID", MySqlDbType.Int64)};
            param3[0].Value = amount;
            param3[1].Value = merchantID;

            sqlArray.Add(sql3);
            paramArray.Add(param3);

            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #endregion

        #region 本金偿还列表
        /// <summary>
        /// 本金偿还数据列表
        /// </summary>
        ///  <param name="Pname">项目名称</param>
        /// <param name="UID">融资方ID</param>
        /// <param name="status">项目状态</param>
        /// <param name="money">项目金额</param>
        /// <returns></returns>
        public List<Project_refund_principal> GetProject_refund_principal_List(string Pname, int UID, int status, decimal money)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT b.name,b.amount,b.loan_period,a.id,a.c_time,a.creator_id,a.m_time,a.fer_id,a.fer_account_id,a.pjt_id,a.amount,a.status,a.op_status,a.third_ord_id,a.version,a.remark   FROM fer_repay_principal a INNER JOIN pjt b ON a.pjt_id=b.id ");

            strSql.Append(" where  1=1 ");


            if (!string.IsNullOrEmpty(Pname))
            {
                Pname = "%" + Pname + "%";
                strSql.Append(" And b.name like ?Panme  ");
            }
            if (status != -1)
            {
                strSql.Append(" And a.status =?status ");
            }
            if (!string.IsNullOrEmpty(money.ToString()))
            {
                strSql.Append(" And b.amount =?Amount ");
            }
            strSql.Append(" order by a.c_time desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PJname", MySqlDbType.VarChar),
                    new MySqlParameter("?status", MySqlDbType.Int16),
                    new MySqlParameter("?merchantID", MySqlDbType.Int64),           
                    new MySqlParameter("?Amount", MySqlDbType.Decimal)             
            };
            parameters[0].Value = Pname;
            parameters[1].Value = status;
            parameters[2].Value = UID;
            parameters[3].Value = money;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户利息偿还数据列表", parameters);
            List<Project_refund_principal> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Project_refund_principal>();
                Project_refund_principal mod = new Project_refund_principal();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToModelProject_refund_principal(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion

        #region 实体转换
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Project_refund_principal DataRowToModelProject_refund_principal(DataRow row)
        {
            Project_refund_principal model = new Project_refund_principal();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["creator_id"] != null && row["creator_id"].ToString() != "")
                {
                    model.creator_id = int.Parse(row["creator_id"].ToString());
                }
                if (row["m_time"] != null && row["m_time"].ToString() != "")
                {
                    model.m_time = DateTime.Parse(row["m_time"].ToString());
                }
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(row["fer_id"].ToString());
                }
                if (row["fer_account_id"] != null && row["fer_account_id"].ToString() != "")
                {
                    model.fer_account_id = int.Parse(row["fer_account_id"].ToString());
                }
                if (row["pjt_id"] != null && row["pjt_id"].ToString() != "")
                {
                    model.pjt_id = int.Parse(row["pjt_id"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["op_status"] != null && row["op_status"].ToString() != "")
                {
                    model.op_status = int.Parse(row["op_status"].ToString());
                }
                if (row["third_ord_id"] != null)
                {
                    model.third_ord_id = row["third_ord_id"].ToString();
                }
                if (row["version"] != null && row["version"].ToString() != "")
                {
                    model.version = int.Parse(row["version"].ToString());
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }

                if (row["name"] != null && row["name"].ToString() != "")
                {
                    model.Pname = row["name"].ToString();
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Pmoney = decimal.Parse(row["amount"].ToString());
                }
                if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                {
                    model.LoanPeriod = int.Parse(row["loan_period"].ToString());
                }
            }
            return model;
        }
        #endregion
        #endregion

        #region 还款记录
        /// <summary>
        /// 还款记录
        /// </summary>
        /// <param name="Pname">项目名称</param>
        /// <param name="UID">融资方ID</param>
        /// <param name="Smoney">还款金额</param>
        /// <param name="Emoney">还款金额</param>
        /// <param name="Sdate">还款日期</param>
        /// <param name="Edate">还款日期</param>
        /// <returns></returns>
        public List<Pjt_repayment_Model> GetPjt_repayment_Model_List(string Pname, int UID, decimal Smoney, decimal Emoney, string Sdate, string Edate, int LoanPeriod)
        {
            var model = new List<Pjt_repayment_Model>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT     p.id AS PID, 
                                        p.name, 
                                        p.amount AS pamount,  
                                        p.loan_period, 
                                        p.fer_account_id as account_id,
                                        f.rep_amount,
                                        f.pjt_type,
                                        f.m_time       
                                        FROM   pjt_repayment AS f    
                                        INNER JOIN  pjt AS p  ON f.pjt_id=p.id   ");
            strSql.Append(" where p.is_deleted=0 and p.account_id=?UID ");

            if (!string.IsNullOrEmpty(Pname))
            {
                Pname = "%" + Pname + "%";
                strSql.Append(" And p.name like ?Panme  ");
            }

            if (Smoney > 0)
            {
                strSql.Append(" And f.rep_amount >=?Smoney ");
            }
            if (Emoney > 0)
            {
                strSql.Append(" And f.rep_amount <=?Emoney ");
            }
            if (Smoney > 0 && Emoney > 0)
            {
                strSql.Append(" And  f.rep_amount  Between ?Smoney   And   ?Emoney ");
            }
            if (!string.IsNullOrEmpty(Sdate))
            {
                strSql.Append(" And  f.m_time  >=?Sdate ");
            }
            if (!string.IsNullOrEmpty(Edate))
            { strSql.Append(" And  f.m_time  <=?Edate "); }
            if (!string.IsNullOrEmpty(Edate) && !string.IsNullOrEmpty(Sdate))
            {
                strSql.Append(" And  f.m_time  Between ?Sdate   And   ?Edate ");
            }
            if (LoanPeriod > 0)
            {
                strSql.Append(" And p.loan_period =?LoanPeriod ");
            }

            strSql.Append(" order by f.m_time  desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Panme", MySqlDbType.VarChar,50),
                    new MySqlParameter("?UID", MySqlDbType.Int32,11),   
                    new MySqlParameter("?Smoney", MySqlDbType.Decimal), 
                    new MySqlParameter("?Emoney", MySqlDbType.Decimal),   
                    new MySqlParameter("?Sdate", MySqlDbType.DateTime), 
                    new MySqlParameter("?Edate", MySqlDbType.DateTime),
                     new MySqlParameter("?LoanPeriod", MySqlDbType.Int32)    
            };
            parameters[0].Value = Pname;
            parameters[1].Value = UID;
            parameters[2].Value = Smoney;
            parameters[3].Value = Emoney;
            parameters[4].Value = Sdate;
            parameters[5].Value = Edate;
            parameters[6].Value = LoanPeriod;

            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "得到用户本金偿还数据列表", parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Pjt_repayment_Model mod = new Pjt_repayment_Model();
                    if (row["PID"] != null && row["PID"].ToString() != "")
                    {
                        mod.PID = int.Parse(row["PID"].ToString());
                    }
                    if (row["name"] != null)
                    {
                        mod.Pname = row["name"].ToString();
                    }
                    if (row["pamount"] != null && row["pamount"].ToString() != "")
                    {
                        mod.Pmoney = decimal.Parse(row["pamount"].ToString());
                    }
                    if (row["rep_amount"] != null && row["rep_amount"].ToString() != "")
                    {
                        mod.rep_amount = decimal.Parse(row["rep_amount"].ToString());
                    }
                    if (row["loan_period"] != null && row["loan_period"].ToString() != "")
                    {
                        mod.LoanPeriod = int.Parse(row["loan_period"].ToString());
                    }
                    if (row["pjt_type"] != null && row["pjt_type"].ToString() != "")
                    {
                        mod.pjt_type = int.Parse(row["pjt_type"].ToString());
                    }
                    if (row["m_time"] != null && row["m_time"].ToString() != "")
                    {
                        mod.m_time = DateTime.Parse(row["m_time"].ToString());
                    }

                    model.Add(mod);
                }
            }
            return model;
        }
        #endregion

        #region 还款记录新的
        #region 还款日志
        /// <summary>
        /// 还款日志 - 本金 + 利息
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public DataSet GetRefundLog(string UID, int loanPeriod, decimal amount_min, decimal amount_max, DateTime time_min, DateTime time_max,
            int startIndex, int pageSize)
        {
            if (time_max != DateTime.MinValue)
            {
                time_max = new DateTime(time_max.Year, time_max.Month, time_max.Day, 23, 59, 59);
            } 
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
                    SELECT 	P.id,TRUNCATE(sum(R.amount),2) amount,R.c_time AS `ctime`,
	                    P.`name` AS `ProjectName`,P.`Amount` AS 'ProjectAmount',P.loan_period AS `LoanPeriod`, P.amount_paid AS repay_amount,
	                    I.`name` AS financier_name,R.type
                    FROM fer_repay_log R
                    INNER JOIN pjt P ON R.pjt_id=P.`id`           
                    INNER JOIN fer_account F ON F.`id` = R.`fer_account_id`
                    INNER JOIN fer_info I ON I.`id` =  F.fer_id
                    WHERE (?UID=-1 OR I.`id` = ?UID)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax)");
            string sqlWhere = " ";
            if (time_min != DateTime.MinValue)
            {
                sqlWhere += " AND  R.`c_time` >=?time_min ";
            }
            if (time_max != DateTime.MinValue)
            {
                sqlWhere += " AND  R.`c_time`<=?time_max ";
            }
            if (time_min != DateTime.MinValue && time_max != DateTime.MinValue)
            {
                sqlWhere = " AND R.`c_time`>=?time_min AND R.`c_time`<=?time_max ";
            }
            strSql.Append(" " + sqlWhere + " ");
            strSql.Append("  GROUP BY R.`source_id`,R.`type` ORDER BY  R.`c_time` Desc   ");

            strSql.Append(@" LIMIT ?StartIndex,?PageSize;");

            strSql.Append(@"
                    SELECT COUNT(1)    FROM (
                    SELECT R.id
                    FROM fer_repay_log R
                    INNER JOIN pjt P ON R.pjt_id=P.`id`
                    INNER JOIN fer_account F ON F.`id` = R.`fer_account_id`
                    INNER JOIN fer_info I ON I.`id` =  F.fer_id
                    WHERE (?UID=-1 OR I.`id` = ?UID)
                    AND (?LoanPeriod=-1 OR P.loan_period=?LoanPeriod) 
                    AND (?AmountMin=-1 OR R.`amount`>=?AmountMin) AND (?AmountMax=-1 OR R.`amount`<=?AmountMax)");
            strSql.Append(" " + sqlWhere + "  GROUP BY R.`source_id`,R.`type`  )T");
            //strSql.Append(" LIMIT ?StartIndex,?PageSize;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UID", MySqlDbType.VarChar),
                    new MySqlParameter("?LoanPeriod", MySqlDbType.Int32),
                    new MySqlParameter("?AmountMin", MySqlDbType.Decimal),
                    new MySqlParameter("?AmountMax", MySqlDbType.Decimal),
                    new MySqlParameter("?time_min", MySqlDbType.DateTime),
                    new MySqlParameter("?time_max", MySqlDbType.DateTime),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
            };
            parameters[0].Value = UID;
            parameters[1].Value = loanPeriod;
            parameters[2].Value = amount_min;
            parameters[3].Value = amount_max;
            parameters[4].Value = time_min;
            parameters[5].Value = time_max;
            parameters[6].Value = startIndex;
            parameters[7].Value = pageSize;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取还款记录", parameters);
        }
        #endregion

        /// <summary>
        /// 转为还款model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public RefundModelExtend DataToRefundModel(DataRow row)
        {
            var model = new RefundModelExtend();

            #region match
            if (row != null)
            {
                if (row.Table.Columns.Contains("id") && row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = long.Parse(row["id"].ToString());
                }
                if (row.Table.Columns.Contains("merchantID") && row["merchantID"] != null && row["merchantID"].ToString() != "")
                {
                    model.merchantID = int.Parse(row["merchantID"].ToString());
                }
                if (row.Table.Columns.Contains("pjid") && row["pjid"] != null && row["pjid"].ToString() != "")
                {
                    model.pjid = int.Parse(row["amount"].ToString());
                }
                if (row.Table.Columns.Contains("type") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row.Table.Columns.Contains("ctime") && row["ctime"] != null && row["ctime"].ToString() != "")
                {
                    model.ctime = DateTime.Parse(row["ctime"].ToString());
                }
                if (row.Table.Columns.Contains("rtime") && row["rtime"] != null && row["rtime"].ToString() != "")
                {
                    model.rtime = DateTime.Parse(row["rtime"].ToString());
                }
                if (row.Table.Columns.Contains("ProjectName") && row["ProjectName"] != null && row["ProjectName"].ToString() != "")
                {
                    model.ProjectName = row["ProjectName"].ToString();
                }
                if (row.Table.Columns.Contains("ProjectAmount") && row["ProjectAmount"] != null && row["ProjectAmount"].ToString() != "")
                {
                    model.ProjectAmount = decimal.Parse(row["ProjectAmount"].ToString());
                }
                if (row.Table.Columns.Contains("LoanPeriod") && row["LoanPeriod"] != null && row["LoanPeriod"].ToString() != "")
                {
                    model.LoanPeriod = Convert.ToInt32(row["LoanPeriod"].ToString());
                }
                if (row.Table.Columns.Contains("Balance") && row["Balance"] != null && row["Balance"].ToString() != "")
                {
                    model.MerchantBalance = decimal.Parse(row["Balance"].ToString());
                }
                if (row.Table.Columns.Contains("financier_name") && row["financier_name"] != null && row["financier_name"].ToString() != "")
                {
                    model.MerchantName = row["financier_name"].ToString();
                }
                if (row.Table.Columns.Contains("repay_amount") && row["repay_amount"] != null && row["repay_amount"].ToString() != "")
                {
                    model.RepayAmount = decimal.Parse(row["repay_amount"].ToString());
                }
                if (row.Table.Columns.Contains("type") && row["type"] != null && row["type"].ToString() != "")
                {
                    model.Type = Convert.ToInt32(row["type"].ToString()) == 1 ? "本金" : "利息";
                }
            }
            #endregion

            return model;
        }
        #endregion


        #region 融资方提现操作

        #region 查看融资方提现金额是否充足
        /// <summary>
        /// 查看融资方提现金额是否充足
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUserWithdrawals(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TRUNCATE(amount_repay,2)-TRUNCATE(amount_repay_fz,2)-TRUNCATE(interest_payable,2)   FROM fer_account where fer_id=?UserId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            parameters[0].Value = userId;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "查看融资方提现金额是否充足", parameters);
            if (obj != null)
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
            return 0;
        }
        #endregion

        #region 添加提现信息
        /// <summary>
        /// 添加提现信息
        /// </summary>
        /// <param name="model">融资方提现信息</param>
        /// <returns></returns>
        public int Add_fer_withdraw(Fer_Withdraw model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fer_withdraw(");
            strSql.Append("fer_id,type,c_time,amount,status,bankcard,user_withdraw_id,m_time,creator_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?fer_id,?type,?c_time,?amount,?status,?bankcard,?user_withdraw_id,?m_time,?creator_id,?version,?remark);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {
					new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?bankcard", MySqlDbType.VarChar,50),
					new MySqlParameter("?user_withdraw_id", MySqlDbType.VarChar,50),		
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500)};
            parameters[0].Value = model.fer_id;
            parameters[1].Value = model.type;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = model.amount;
            parameters[4].Value = model.status;
            parameters[5].Value = model.bankcard;
            parameters[6].Value = model.user_withdraw_id;
            parameters[7].Value = model.m_time;
            parameters[8].Value = model.creator_id;
            parameters[9].Value = model.version;
            parameters[10].Value = "融资方提现";
            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 添加融资方提现记录信息", parameters);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;

        }
        #endregion

        #region 申请提现后改变账户表金额（冻结住）
        /// <summary>
        /// 申请提现后改变账户表金额（冻结住）
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="money">冻结金额</param>
        /// <returns></returns>
        public bool Updatefer_account_FZ(int UID, decimal money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fer_account set ");
            strSql.Append("amount_repay_fz=amount_repay_fz+?money where fer_id=?UID");
            MySqlParameter[] parameters = {        
            new MySqlParameter("?UID", MySqlDbType.Int64, 20),
            new MySqlParameter("?money", MySqlDbType.Decimal)};
            parameters[0].Value = UID;
            parameters[1].Value = money;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 申请提现后改变账户表金额", parameters);
        }
        #endregion

        #region 得到融资方提现记录信息
        /// <summary>
        /// 得到用户提现记录信息列表
        /// </summary>
        /// <param name="UID">融资方ID</param>
        /// <param name="SMoney">开始金钱</param>
        /// <param name="EMoney">截止金钱</param>
        /// <param name="Sdate">起始时间</param>
        /// <param name="Edate">结束时间</param>
        /// <returns></returns>
        public List<Fer_Withdraw> GetFer_Withdraw_List(string Order, int UID, DateTime Sdate, DateTime Edate, int statu)
        {
            if (Edate != DateTime.MinValue)
            {
                Edate = new DateTime(Edate.Year, Edate.Month, Edate.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select  id,fer_id,type,c_time,TRUNCATE(amount,2) amount,status,bankcard,user_withdraw_id,id,m_time,creator_id,version,remark FROM fer_withdraw  ");

            strSql.Append(" where fer_id=?UID");

            if (!string.IsNullOrEmpty(Order))
            {
                strSql.Append(" And  user_withdraw_id=?Order ");
            }
            string sqlWhere = " ";
            if (Sdate != DateTime.MinValue)
            {
                sqlWhere += " And c_time >=?Sdate ";
            }
            if (Edate != DateTime.MinValue)
            {
                sqlWhere += " And c_time <=?Edate ";
            }
            if (Sdate != DateTime.MinValue && Edate != DateTime.MinValue)
            {
                sqlWhere = " And c_time Between ?Sdate   And   ?Edate ";
            }

            strSql.Append(" " + sqlWhere + " ");
            if (statu != -1)
            {
                strSql.Append(" And status =?statu ");
            }
            strSql.Append(" Order By c_time Desc ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?Order", MySqlDbType.Int64,11),
					new MySqlParameter("?UID", MySqlDbType.Int16,4),
					new MySqlParameter("?Sdate", MySqlDbType.DateTime),
					new MySqlParameter("?Edate", MySqlDbType.DateTime),
					new MySqlParameter("?statu", MySqlDbType.Int16,3),
		};
            parameters[0].Value = Order;
            parameters[1].Value = UID;
            parameters[2].Value = Sdate;
            parameters[3].Value = Edate;
            parameters[4].Value = statu;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取所有的用户充值记录", parameters);
            List<Fer_Withdraw> list = null;

            if (ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Fer_Withdraw>();
                Fer_Withdraw mod = new Fer_Withdraw();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    mod = DataRowToModel_Fer_Withdraw(row);
                    list.Add(mod);
                }
            }
            return list;
        }
        #endregion


        #region 申请提现用事务
        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="merchantID">融资方账户ID</param>
        /// <param name="amount">提现金额</param>
        /// <param name="Order">订单号</param>
        /// <param name="BankID">银行卡</param>
        /// <returns></returns>
        public bool ApplicationWithdrawal(int merchantID, decimal amount, string Order, string BankID, int Fer_id)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 申请提现记录    fer_withdraw  
            #region
            var sql1 = @" INSERT INTO fer_withdraw 
                                        (fer_id, 
                                         type, 
                                         c_time, 
                                         amount, 
                                         status, 
                                         bankcard, 
                                         user_withdraw_id, 
                                         m_time, 
                                         creator_id, 
                                         version, 
                                         remark) 
                            VALUES      (?fer_id, 
                                         ?type, 
                                         ?c_time, 
                                         ?amount, 
                                         ?status, 
                                         ?bankcard, 
                                         ?user_withdraw_id, 
                                         ?m_time, 
                                         ?creator_id, 
                                         ?version, 
                                         ?remark) ";
            MySqlParameter[] param1 = {
                  new MySqlParameter("?fer_id", MySqlDbType.Int32,11),
					new MySqlParameter("?type", MySqlDbType.Int16,4),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
					new MySqlParameter("?bankcard", MySqlDbType.VarChar,50),
					new MySqlParameter("?user_withdraw_id", MySqlDbType.VarChar,50),		
					new MySqlParameter("?m_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500)};
            param1[0].Value = merchantID;
            param1[1].Value = 0;
            param1[2].Value = DateTime.Now;
            param1[3].Value = amount;
            param1[4].Value = 0;
            param1[5].Value = BankID;
            param1[6].Value = Order;
            param1[7].Value = DateTime.Now;
            param1[8].Value = Fer_id;
            param1[9].Value = 0;
            param1[10].Value = "融资方申请提现";
            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            // 2）申请提现后改变账户冻结金额 fer_account
            #region
            var sql2 = @" update fer_account set amount_repay_fz=amount_repay_fz+?money where id=?UID";

            MySqlParameter[] param2 = {        
            new MySqlParameter("?UID", MySqlDbType.Int64, 20),
            new MySqlParameter("?money", MySqlDbType.Decimal)};
            param2[0].Value = merchantID;
            param2[1].Value = amount;

            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region 实体转换

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Fer_Withdraw DataRowToModel_Fer_Withdraw(DataRow row)
        {
            Fer_Withdraw model = new Fer_Withdraw();
            if (row != null)
            {
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.fer_id = int.Parse(row["fer_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.c_time = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.status = int.Parse(row["status"].ToString());
                }
                if (row["bankcard"] != null)
                {
                    model.bankcard = row["bankcard"].ToString();
                }
                if (row["user_withdraw_id"] != null && row["user_withdraw_id"].ToString() != "")
                {
                    model.user_withdraw_id = row["user_withdraw_id"].ToString();
                }
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["m_time"] != null && row["m_time"].ToString() != "")
                {
                    model.m_time = DateTime.Parse(row["m_time"].ToString());
                }
                if (row["creator_id"] != null && row["creator_id"].ToString() != "")
                {
                    model.creator_id = int.Parse(row["creator_id"].ToString());
                }
                if (row["version"] != null && row["version"].ToString() != "")
                {
                    model.version = int.Parse(row["version"].ToString());
                }
                if (row["remark"] != null)
                {
                    model.remark = row["remark"].ToString();
                }
            }
            return model;
        }
        #endregion

        #endregion

        /// <summary>
        /// 判断用户手机号是否认证
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public bool getCHcekPhone(int userId, string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select 1 from fer_info ");
            strSql.Append(" where status=1 and id=?userId and phone=?Phone");
            MySqlParameter[] parameters = {
					new MySqlParameter("?userId", MySqlDbType.Int64),
                    new MySqlParameter("?Phone", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = userId;
            parameters[1].Value = Phone;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月1日 判断用户手机号是否认证", parameters);

            if (i != null)
            {
                if (Convert.ToInt32(i) > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
