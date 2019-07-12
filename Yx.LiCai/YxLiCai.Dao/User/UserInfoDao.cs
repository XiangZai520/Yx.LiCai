/*
 * 用户账户数据层
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System.Text;
using YxLiCai.Model.User;
using MySql.Data.MySqlClient;
using YxLiCai.Model.Project;
using YxLiCai.DataHelper;
using System.Data;
using System;
using System.Collections.Generic;
using YxLiCai.Model;
using YxLiCai.Tools.Util;
namespace YxLiCai.Dao.User
{
    /// <summary>
    /// 用户账户数据层
    /// </summary>
    public class UserInfoDao
    {
        #region 添加用户信息
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public int AddUser(UserInfoModel model)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("insert into user_info(");
            //strSql.Append("login_name,phone,pwd,sall_pwd,real_name,real_card,is_real_card,my_code,reg_time,status,ip)");
            //strSql.Append(" values (");
            //strSql.Append("?LoginName,?Phone,?Password,?Sallpassword,?MyRealName,?MyRealCard,?IsRealCard,?MyCode,?RegTime,?Status,?ip);select @@IDENTITY as InsertId;");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?LoginName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Sallpassword", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?MyRealCard", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?IsRealCard", MySqlDbType.Int16),
            //        new MySqlParameter("?MyCode", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?RegTime", MySqlDbType.DateTime),
            //        new MySqlParameter("?Status", MySqlDbType.Int32,4),
            //        new MySqlParameter("?ip", MySqlDbType.VarChar,15)};
            //parameters[0].Value = model.LoginName;
            //parameters[1].Value = model.Phone;
            //parameters[2].Value = model.Password;
            //parameters[3].Value = model.Sallpassword;
            //parameters[4].Value = model.MyRealName;
            //parameters[5].Value = model.MyRealCard;
            //parameters[6].Value = model.IsRealCard;
            //parameters[7].Value = model.MyCode;
            //parameters[8].Value = model.RegTime;
            //parameters[9].Value = model.Status;
            //parameters[10].Value = model.IP;

            ////return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 添加一条用户/会员信息", parameters);
            //object i = DataBaseOperator.YxLiCalUserWrite.ExecuteScalar(strSql.ToString(), CommandType.Text, parameters);
            //if (i != null)
            //{
            //    return int.Parse(i.ToString());
            //}
            //return 0;
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/AddUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(model));
            ResultApiInfo<int> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<int>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return 0;

        }
        #endregion

        #region 修改用户的信息
        /// <summary>
        /// 修改用户的信息
        /// </summary>
        /// <param name="model">用户实体信息</param>
        /// <returns></returns>
        public bool UpdateUser(UserInfoModel model)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set ");
            //strSql.Append("login_name=?LoginName,");
            //strSql.Append("phone=?Phone,");
            //strSql.Append("pwd=?Password,");
            //strSql.Append("sall_pwd=?Sallpassword,");
            //strSql.Append("real_name=?MyRealName,");
            //strSql.Append("real_card=?MyRealCard,");
            //strSql.Append("is_real_card=?IsRealCard,");
            //strSql.Append("my_code=?MyCode,");
            //strSql.Append("reg_time=?RegTime,");
            //strSql.Append("status=?Status");
            //strSql.Append(" where id=?id");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?LoginName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Sallpassword", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?MyRealCard", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?IsRealCard", MySqlDbType.Int16),
            //        new MySqlParameter("?MyCode", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?RegTime", MySqlDbType.DateTime),
            //        new MySqlParameter("?Status", MySqlDbType.Int32,4),
            //        new MySqlParameter("?id", MySqlDbType.Int64,11)};
            //parameters[0].Value = model.LoginName;
            //parameters[1].Value = model.Phone;
            //parameters[2].Value = model.Password;
            //parameters[3].Value = model.Sallpassword;
            //parameters[4].Value = model.MyRealName;
            //parameters[5].Value = model.MyRealCard;
            //parameters[6].Value = model.IsRealCard;
            //parameters[7].Value = model.MyCode;
            //parameters[8].Value = model.RegTime;
            //parameters[9].Value = model.Status;
            //parameters[10].Value = model.id;
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:35:29 侯裕祥 修改用户信息", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(model));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }


        #endregion

        #region 删除用户信息
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {

            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("delete from user_info ");
            //strSql.Append(" where id=?id");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?id", MySqlDbType.Int64,11)
            //};
            //parameters[0].Value = id;

            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:46:45 侯裕祥 删除项目信息", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/DeleteUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(id));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        #endregion

        #region select操作

        /// <summary>
        /// 得到一个对象实体（通过用户ID）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public UserInfoModel GetUserModel(long id)
        {

            //StringBuilder strSql = new StringBuilder();

            //strSql.Append("select u.id,login_name,phone,pwd,sall_pwd,real_name,real_card,is_real_card,my_code,reg_time,u.status,b.status AS is_bind_bank,b.bank_name,b.last_num,b.bank_code,no_agree from user_info AS u LEFT JOIN user_bank_card AS b ON u.id=b.user_id AND b.status=1 ");
            //strSql.Append(" where u.id=?id");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?id", MySqlDbType.Int64,11)
            //};
            //parameters[0].Value = id;
            //DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql.ToString(), "2015年5月29日 12:02:02 侯裕祥 得到一条用户信息", parameters);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    return DataRowToModel_Left(ds.Tables[0].Rows[0]);
            //}
            //else
            //{
            //    return null;
            //}
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/GetUserModel_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(id));
            ResultApiInfo<UserInfoModel> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<UserInfoModel>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return null;
        }

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool updateUserLoginTimes(long id)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append(" update user_info set login_times=login_times+1,last_login_time=?time where id=?id");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?id", MySqlDbType.Int64,4),
            //        new MySqlParameter("?time", MySqlDbType.DateTime)
            //};
            //parameters[0].Value = id;
            //parameters[1].Value = DateTime.Now;

            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 12:02:02 侯裕祥 得到一条用户信息", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/updateUserLoginTimes_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(id));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;

        }


        /// <summary>
        /// 得到一个对象实体（通过用户手机号）
        /// </summary>
        /// <param name="Phone">用户手机号</param>
        /// <returns></returns>
        public UserInfoModel GetUserModelByPhone(string Phone)
        {

            //StringBuilder strSql = new StringBuilder();
            ////strSql.Append("select id,LoginName,Phone,Password,Sallpassword,MyRealName,MyRealCard,IsRealCard,MyCode,RegTime,Status from user_info ");
            //strSql.Append("select u.id,login_name,phone,pwd,sall_pwd,real_name,real_card,is_real_card,my_code,reg_time,u.status,b.status AS is_bind_bank,b.bank_name,b.last_num,b.bank_code from user_info AS u LEFT JOIN user_bank_card AS b ON u.id=b.user_id AND b.status=1 ");
            //strSql.Append(" where u.phone=?Phone");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Phone;

            //ProjectModel model = new ProjectModel();
            //DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql.ToString(), "2015年5月29日 12:02:02 侯裕祥 得到一条用户信息", parameters);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    //return DataRowToModel(ds.Tables[0].Rows[0]);
            //    return DataRowToModelByPhone(ds.Tables[0].Rows[0]);
            //}
            //else
            //{
            //    return null;
            //}
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/GetUserModelByPhone_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(Phone));
            ResultApiInfo<UserInfoModel> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<UserInfoModel>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return null;
        }

        /// <summary>
        /// 根据邀请码获得用户信息 by张浩然 2015-7-30
        /// </summary>
        /// <param name="MyCode">邀请码</param>
        /// <returns></returns>
        public UserInfoModel GetUserModelByMyCode(string MyCode)
        {

            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select u.id,login_name,phone,pwd,sall_pwd,real_name,real_card,is_real_card,my_code,reg_time,u.status,b.status AS is_bind_bank,b.bank_name,b.last_num from user_info AS u LEFT JOIN user_bank_card AS b ON u.id=b.user_id AND b.status=1 ");
            //strSql.Append(" where u.my_code=?MyCode");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?MyCode", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = MyCode;

            //ProjectModel model = new ProjectModel();
            //DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql.ToString(), "根据邀请码获得用户信息 by张浩然 2015-7-30", parameters);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    return DataRowToModelByPhone(ds.Tables[0].Rows[0]);
            //}
            //else
            //{
            //    return null;
            //}
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/GetUserModelByMyCode_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(MyCode));
            ResultApiInfo<UserInfoModel> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<UserInfoModel>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return null;
        }

        #endregion


        #region 检查用户是否注册
        /// <summary>
        /// 判断用户账户是否已经注册
        /// </summary>
        /// <param name="Account">用户手机</param>       
        /// <returns></returns>
        public bool IsRepeatUser(string Account)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where phone=?Account and status!=0 ");
            //MySqlParameter[] parameters = {					
            //        new MySqlParameter("?Account", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Account;
            //object ob = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年5月30日 15:10:04-侯裕祥-判断是否已注册", parameters);
            //if (ob != null)
            //{
            //    if (Convert.ToInt64(ob) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月30日 15:10:04-侯裕祥-判断是否已注册", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsRepeatUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(Account));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        /// <summary>
        /// 判断是否是黑名单用户 by张浩然 2015-7-2
        /// </summary>
        /// <param name="phone">电话</param>
        /// <returns></returns>
        public bool IsBlackUser(string phone)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where phone=?phone and status=0 ");
            //MySqlParameter[] parameters = {					
            //        new MySqlParameter("?phone", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = phone;
            //object ob = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年7月2日 by张浩然-判断是否是黑名单用户", parameters);
            //if (ob != null)
            //{
            //    if (Convert.ToInt64(ob) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsBlackUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(phone));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        /// <summary>
        /// 根据身份证判断是否已注册
        /// </summary>
        /// <param name="realCard">身份证</param>
        /// <returns></returns>
        public bool IsRepeatUserByRealCard(string realCard)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("SELECT real_card from user_info ");
            //strSql.Append(" where real_card=?MyRealCard and is_real_card=1 LIMIT 0,1; ");
            //MySqlParameter[] parameters = {					
            //        new MySqlParameter("?MyRealCard", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = realCard;
            //object ob = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月24日 18:00:00-张浩然-根据身份证判断是否已注册", parameters);
            //if (ob != null)
            //{
            //    if (Convert.ToInt64(ob) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsRepeatUserByRealCard_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(realCard));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        #endregion

        #region 绑定银行卡
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        public bool BoundBankCard(UserBankCardModel model)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("insert into user_bank_card(");
            //strSql.Append("user_id,bank,bank_name,bank_card,bank_region,bank_address,status,bank_code,first_num,last_num,bank_phone,request_id,no_agree)");
            //strSql.Append(" values (");
            //strSql.Append("?UserId,?Bank,?BankName,?BankCard,?BankRegion,?BankAddress,?Status,?BankCode,?FirstNum,?LastNum,?BankPhone,?Requestid,?NoAgree)");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?UserId", MySqlDbType.Int64,11),
            //        new MySqlParameter("?Bank", MySqlDbType.Int32,11),
            //        new MySqlParameter("?BankName", MySqlDbType.VarChar,250),
            //        new MySqlParameter("?BankCard", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?BankRegion", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?BankAddress", MySqlDbType.VarChar,150),
            //        new MySqlParameter("?Status", MySqlDbType.Int32,3),
            //        new MySqlParameter("?BankCode", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?FirstNum", MySqlDbType.VarChar,10),
            //        new MySqlParameter("?LastNum", MySqlDbType.VarChar,10),
            //        new MySqlParameter("?BankPhone", MySqlDbType.VarChar,20),
            //        new MySqlParameter("?Requestid", MySqlDbType.VarChar,20),
            //        new MySqlParameter("?NoAgree", MySqlDbType.VarChar,20)};
            //parameters[0].Value = model.UserId;
            //parameters[1].Value = model.Bank;
            //parameters[2].Value = model.BankName;
            //parameters[3].Value = model.BankCard;
            //parameters[4].Value = model.BankRegion;
            //parameters[5].Value = model.BankAddress;
            //parameters[6].Value = model.Status;
            //parameters[7].Value = model.BankCode;
            //parameters[8].Value = model.FirstNum;
            //parameters[9].Value = model.LastNum;
            //parameters[10].Value = model.BankPhone;
            //parameters[11].Value = model.Requestid;
            //parameters[12].Value = model.no_agree;
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 绑定银行卡", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/BoundBankCard_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(model));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        /// <summary>
        /// 获取银行卡
        /// </summary>
        public UserBankCardModel GetBankCard(long uid)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select bank_name, bank_card,first_num,last_num,bank_phone,bank_code from user_bank_card where user_id=?UserId and status=1;");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?UserId", MySqlDbType.Int64,11)};
            //parameters[0].Value = uid;
            //DataTable dt = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql.ToString(), "#2015年6月9日 11:25:21 平扬，获取银行卡", parameters).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    UserBankCardModel mod = new UserBankCardModel();
            //    mod.UserId = uid;
            //    mod.BankName = dt.Rows[0]["bank_name"].ToString();
            //    mod.BankCard = dt.Rows[0]["bank_card"].ToString();
            //    mod.FirstNum = dt.Rows[0]["first_num"].ToString();
            //    mod.LastNum = dt.Rows[0]["last_num"].ToString();
            //    mod.BankPhone = dt.Rows[0]["bank_phone"].ToString();
            //    mod.BankCode = dt.Rows[0]["bank_code"].ToString();
            //    return mod;
            //}
            //return null;
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/GetBankCard_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(uid));
            ResultApiInfo<UserBankCardModel> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<UserBankCardModel>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return null;
        }

        #endregion

        #region 身份证验证


        public void UpdateUserAccountRealName(Int64 UserID, string MyRealName)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_account set real_name=?MyRealName");
            //strSql.Append(" where user_id=?UserID;");
            //MySqlParameter[] parameters = {			
            //        new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?UserID", MySqlDbType.Int64,11)
            //};
            //parameters[0].Value = MyRealName;
            //parameters[1].Value = UserID;
            //DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2014年11月27日-侯裕祥-修改身份证认证情况", parameters);
            var user = new { UserID = UserID, MyRealName = MyRealName};
            YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateUserAccountRealName_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
        }


        /// <summary>
        /// 修改用户的认证状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="MyRealCard">用户身份证</param>
        /// <returns>true：身份验证设置成功 false：身份证设置失败</returns>
        public bool UpdateIsRealCard(Int64 UserID, string MyRealCard, string MyRealName)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set is_real_card=1,real_name=?MyRealName,real_card=?MyRealCard");
            //strSql.Append(" where id=?UserID;");


            //MySqlParameter[] parameters = {			
            //        new MySqlParameter("?MyRealCard", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?UserID", MySqlDbType.Int64,11)
            //};
            //parameters[0].Value = MyRealCard;
            //parameters[1].Value = MyRealName;
            //parameters[2].Value = UserID;

            //UpdateUserAccountRealName(UserID, MyRealName);
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2014年11月27日-侯裕祥-修改身份证认证情况", parameters);
            var user = new { UserID = UserID, MyRealCard = MyRealCard, MyRealName = MyRealName };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateIsRealCard_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        /// <summary>
        /// 更新用户帐户信息 by张浩然 2015-7-3
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <returns></returns>
        public bool UpdateUserAccount(Int64 UserID, string MyRealName)
        {
            //StringBuilder strSql = new StringBuilder();

            //strSql.Append("update user_account set real_name=?MyRealName");
            //strSql.Append(" where user_id=?UserID;");

            //MySqlParameter[] parameters = {		
            //        new MySqlParameter("?MyRealName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?UserID", MySqlDbType.Int64,11)
            //};

            //parameters[0].Value = MyRealName;
            //parameters[1].Value = UserID;
            //return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新用户帐户信息 by张浩然 2015-7-3", parameters);
            var user = new { UserID = UserID,MyRealName = MyRealName };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateUserAccount_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        #endregion

        #region 冻结解封用户
        /// <summary>
        /// 冻结用户
        /// </summary>
        /// <param name="Id">用户ID</param>
        /// <returns></returns>
        public bool UpdateFrozenUser(int Id)
        {

            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set Status=1 ");
            //strSql.Append(" where Status=0 and id=?Id ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Id", MySqlDbType.Int32,11)			};
            //parameters[0].Value = Id;
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:46:45 侯裕祥 冻结用户信息", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateFrozenUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(Id));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        /// <summary>
        /// 解封用户
        /// </summary>
        /// <param name="Id">用户ID</param>
        /// <returns></returns>
        public bool UpdateThawUser(int Id)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set status=0 ");
            //strSql.Append(" where status=1 and id=?Id ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Id", MySqlDbType.Int32,11)			};
            //parameters[0].Value = Id;
            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:46:45 侯裕祥 解封用户信息", parameters);
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateThawUser_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(Id));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        #endregion

        #region 对象转换
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UserInfoModel DataRowToModel(DataRow row)
        {
            UserInfoModel model = new UserInfoModel();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["login_name"] != null)
                {
                    model.LoginName = row["login_name"].ToString();
                }
                if (row["phone"] != null)
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row["pwd"] != null)
                {
                    model.Password = row["pwd"].ToString();
                }
                if (row["sall_pwd"] != null)
                {
                    model.Sallpassword = row["sall_pwd"].ToString();
                }
                if (row["real_name"] != null)
                {
                    model.MyRealName = row["real_name"].ToString();
                }
                if (row["real_card"] != null)
                {
                    model.MyRealCard = row["real_card"].ToString();
                }
                if (row["is_real_card"] != null && row["is_real_card"].ToString() != "")
                {
                    model.IsRealCard = int.Parse(row["is_real_card"].ToString());
                }
                if (row["my_code"] != null)
                {
                    model.MyCode = row["my_code"].ToString();
                }
                if (row["reg_time"] != null && row["reg_time"].ToString() != "")
                {
                    model.RegTime = DateTime.Parse(row["reg_time"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.Status = int.Parse(row["status"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 做关联到银行卡表
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public UserInfoModel DataRowToModel_Left(DataRow row)
        {
            UserInfoModel model = new UserInfoModel();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["login_name"] != null)
                {
                    model.LoginName = row["login_name"].ToString();
                }
                if (row["phone"] != null)
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row["pwd"] != null)
                {
                    model.Password = row["pwd"].ToString();
                }
                if (row["sall_pwd"] != null)
                {
                    model.Sallpassword = row["sall_pwd"].ToString();
                }
                if (row["real_name"] != null)
                {
                    model.MyRealName = row["real_name"].ToString();
                }
                if (row["real_card"] != null)
                {
                    model.MyRealCard = row["real_card"].ToString();
                }
                if (row["is_real_card"] != null && row["is_real_card"].ToString() != "")
                {
                    model.IsRealCard = int.Parse(row["is_real_card"].ToString());
                }
                if (row["my_code"] != null)
                {
                    model.MyCode = row["my_code"].ToString();
                }
                if (row["reg_time"] != null && row["reg_time"].ToString() != "")
                {
                    model.RegTime = DateTime.Parse(row["reg_time"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.Status = int.Parse(row["status"].ToString());
                }
                if (row["is_bind_bank"] != null && row["is_bind_bank"].ToString() != "")
                {
                    model.IsBindBank = int.Parse(row["is_bind_bank"].ToString());
                }
                if (row["bank_name"] != null && row["bank_name"].ToString() != "")
                {
                    model.BankName = row["bank_name"].ToString();
                }
                if (row["last_num"] != null && row["last_num"].ToString() != "")
                {
                    model.LastBankNum = row["last_num"].ToString();
                }

                if (row["no_agree"] != null && row["no_agree"].ToString() != "")
                {
                    model.no_agree = row["no_agree"].ToString();
                }

                if (row["bank_code"] != null && row["bank_code"].ToString() != "")
                {
                    model.BankCode = row["bank_code"].ToString();
                }


            }
            return model;
        }
        public UserInfoModel DataRowToModelByPhone(DataRow row)
        {
            UserInfoModel model = new UserInfoModel();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["login_name"] != null)
                {
                    model.LoginName = row["login_name"].ToString();
                }
                if (row["phone"] != null)
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row["pwd"] != null)
                {
                    model.Password = row["pwd"].ToString();
                }
                if (row["sall_pwd"] != null)
                {
                    model.Sallpassword = row["sall_pwd"].ToString();
                }
                if (row["real_name"] != null)
                {
                    model.MyRealName = row["real_name"].ToString();
                }
                if (row["real_card"] != null)
                {
                    model.MyRealCard = row["real_card"].ToString();
                }
                if (row["is_real_card"] != null && row["is_real_card"].ToString() != "")
                {
                    model.IsRealCard = int.Parse(row["is_real_card"].ToString());
                }
                if (row["my_code"] != null)
                {
                    model.MyCode = row["my_code"].ToString();
                }
                if (row["reg_time"] != null && row["reg_time"].ToString() != "")
                {
                    model.RegTime = DateTime.Parse(row["reg_time"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.Status = int.Parse(row["status"].ToString());
                }
                if (row["is_bind_bank"] != null && row["is_bind_bank"].ToString() != "")
                {
                    model.IsBindBank = int.Parse(row["is_bind_bank"].ToString());
                }
                if (row["bank_name"] != null && row["bank_name"].ToString() != "")
                {
                    model.BankName = row["bank_name"].ToString();
                }
                if (row["last_num"] != null && row["last_num"].ToString() != "")
                {
                    model.LastBankNum = row["last_num"].ToString();
                }
                if (row["bank_code"] != null && row["bank_code"].ToString() != "")
                {
                    model.BankCode = row["bank_code"].ToString();
                }                
            }
            return model;
        }
        #endregion

        #region 用户注册初始化
        /// <summary>
        /// 用户首次注册初始化数据
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool AddUserInitialization(long UserID, string Phone)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("insert into user_account(user_id,amount,amount_invest,interest_yesterday,interest_all,amount_blance_fz,amount_blance,interest_paid,interest_added,principal,user_type,t_amount_invest,t_interest,real_name,phone)");
            //strSql.Append(" values (" + UserID + ",0,0,0,0,0,0,0,0,0,0,0,0," + Phone + "," + Phone + ");");

            //strSql.Append("insert into user_count_month (user_id,amount_invest,interest_all,interest_paid,principal,principal_fz)");
            //strSql.Append(" values (" + UserID + ",0,0,0,0,0);");

            //strSql.Append("insert into user_count_season (user_id,interest_all,amount_invest,interest_paid,interest_added,principal,principal_fz)");
            //strSql.Append(" values (" + UserID + ",0,0,0,0,0,0);");

            //strSql.Append("insert into user_count_year (user_id,interest_all,amount_invest,interest_paid,interest_added,principal_fz,principal)");
            //strSql.Append(" values (" + UserID + ",0,0,0,0,0,0);");

            //return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 用户首次注册初始化数据");
            //DateTime.Now;
            var user = new { UserID = UserID, Phone = Phone };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/AddUserInitialization_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        #endregion


        #region 检查用户输入密码（登录、支付）是否正确
        /// <summary>
        /// 判断登录密码是否正确
        /// </summary>
        /// <param name="Phone">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTruePassWord(string Phone, string password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where phone =?Phone and status=1 and pwd=?Password");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Phone;
            //parameters[1].Value = password;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-侯裕祥-判断密码是否正确", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { Phone = Phone, password = password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsTruePassWord_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        /// <summary>
        /// 判断支付密码是否正确
        /// </summary>
        /// <param name="Phone">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTrueSallpassword(string Phone, string password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where phone =?Phone and sall_pwd=?Sallpassword");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Sallpassword", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Phone;
            //parameters[1].Value = password;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-侯裕祥-判断密码是否正确", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { Phone = Phone, password = password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsTrueSallpassword_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        #endregion

        #region 检查用户验证码是否发送过
        /// <summary>
        /// 检查用户是否发送过验证码
        /// </summary>
        /// <param name="Phone">用户手机</param>
        /// <returns></returns>
        public bool IsSendCode(string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from UserValidateCode ");
            strSql.Append(" where phone =?Phone ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?Phone", MySqlDbType.VarChar,50)                    
            };
            parameters[0].Value = Phone;
            object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-侯裕祥-判断是否发送过验证码", parameters);

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

        #region 检查手机是否已经认证
        /// <summary>
        /// 检查手机是否已经认证
        /// </summary>
        /// <param name="Phone">用户ID</param>     
        /// <returns></returns>
        public bool IsTruePhone(string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from UserValidateCode ");
            strSql.Append(" where phone =?Phone and flag=1");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?Phone", MySqlDbType.VarChar,50)                  
            };
            parameters[0].Value = Phone;
            object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月3日 15:41:59-侯裕祥-判断手机是否已经认证", parameters);

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

        #region 交易密码

        /// <summary>
        /// 判断交易密码是否正确
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool IsTrueSallPassWord(long uid, string password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select 1 from user_info ");
            //strSql.Append(" where id =?id and sall_pwd=?Password");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?id", MySqlDbType.Int64,4),
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = uid;
            //parameters[1].Value = password;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月1日 15:41:59-平扬-判断密码是否正确", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { uid = uid, password = password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/IsTrueSallPassWord_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="Password">支付密码</param>
        /// <returns></returns>
        public bool UpdateUserSallpassword(long id, string Password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set sall_pwd=?Password  where id=?id  ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?id", MySqlDbType.Int64)
                   
            //};
            //parameters[0].Value = Password;
            //parameters[1].Value = id;

            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户交易密码", parameters);
            var user = new { id = id, Password = Password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateUserSallpassword_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        #endregion

        #region 登录密码
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="Password">登录密码</param>
        /// <returns></returns>
        public bool UpdateUserPassWord(long ID, string Password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set pwd=?Password  where id=?id  ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?id", MySqlDbType.Int64)
            //};
            //parameters[0].Value = Password;
            //parameters[1].Value = ID;

            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户密码", parameters);
            var user = new { ID = ID, Password = Password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/UpdateUserPassWord_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        /// <summary>
        /// 忘记登录密码
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="Password">登录密码</param>
        /// <returns></returns>
        public bool ForgetUserPassWord(string Phone, string Password)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update user_info set pwd=?Password  where phone=?Phone  ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Password", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?Phone", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Password;
            //parameters[1].Value = Phone;

            //return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "2015年5月29日 11:25:21 侯裕祥 修改用户密码", parameters);
            var user = new { Phone = Phone, Password = Password };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/ForgetUserPassWord_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        #endregion

        #region 查询用户绑定的的银行卡

        /// <summary>
        /// 得到用户的银行卡
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public string GetUserBankID(long UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select bank_card From user_bank_card where user_id=?UserId  and status=1");
            MySqlParameter[] parameters = {	
                    new MySqlParameter("?UserID", MySqlDbType.UInt64,11)
            };
            parameters[0].Value = UserID;
            object obj = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年5月29日-侯裕祥-获取银行卡", parameters);
            if (obj == null)
            {
                return "0";
            }
            else
            {
                return obj.ToString();
            }
        }
        #endregion

        #region 检查用户是否绑定银行卡
        /// <summary>
        /// 检查用户是否绑定银行卡
        /// </summary>
        /// <param name="UserId">用户ID</param>     
        /// <returns></returns>
        public bool IsTrueBindBankCard(long UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from user_bank_card ");
            strSql.Append(" where user_id =?UserId and status=1");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?UserId", MySqlDbType.VarChar,50)                  
            };
            parameters[0].Value = UserId;
            object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月3日 15:41:59-侯裕祥-判断银行卡是否已经绑定", parameters);

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
        /// <summary>
        /// 更新用户加息券
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public bool UpdateRateCoupon(long userid, int rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update user_add_interest set use_status=1,use_time=?Time where user_id=?UserId and id=?Id;");
            strSql.Append("UPDATE act_interest_coupon SET use_count=use_count+1 WHERE id=(SELECT interest_id FROM user_add_interest WHERE id=?Id)");
            MySqlParameter[] parameters = {			
                    new MySqlParameter("?Id", MySqlDbType.Int32,4),
                    new MySqlParameter("?UserID", MySqlDbType.UInt64,4),
                    new MySqlParameter("?Time", MySqlDbType.DateTime)
            };
            parameters[0].Value = rid;
            parameters[1].Value = userid;
            parameters[2].Value = DateTime.Now;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月29日-平扬-修改加息券状态", parameters);
        }
        /// <summary>
        /// 获取加息券
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public decimal GetRateCoupon(long userid, int rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select interest_rate from user_add_interest where user_id=?UserId and id =?Id  and use_status=0 and e_time > NOW()");
            MySqlParameter[] parameters = {			
                    new MySqlParameter("?Id", MySqlDbType.Int32,4),
                    new MySqlParameter("?UserID", MySqlDbType.UInt64,4)
            };
            parameters[0].Value = rid;
            parameters[1].Value = userid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年5月29日-平扬-获取加息券", parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
        }

        /// <summary>
        /// 获取加息券总额度
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public decimal GetAllRateCoupon(long userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(interest_rate) from user_add_interest where user_id=?UserId and  use_status=0 and e_time > NOW()");
            MySqlParameter[] parameters = {			
                    new MySqlParameter("?UserID", MySqlDbType.Int64,4)
            };
            parameters[0].Value = userid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年5月29日-平扬-获取加息券总额度", parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return YxLiCai.Tools.Util.ParseHelper.ToDecimal(obj);
            }
        }


        /// <summary>
        /// 获取加息券
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<UserRateCouponModel> GetRateCoupons(long userid)
        {
            List<UserRateCouponModel> list = new List<UserRateCouponModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,user_id,TRUNCATE(interest_rate,3) as interest_rate,enable_day,c_time,use_time,e_time,use_status from user_add_interest where user_id=?UserId order by use_status,e_time desc");
            MySqlParameter[] parameters = {			
                    new MySqlParameter("?UserID", MySqlDbType.UInt64,4)
            };
            parameters[0].Value = userid;
            var dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "2015年5月29日-平扬-获取加息券", parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    #region 实体转换
                    UserRateCouponModel model = new UserRateCouponModel();
                    if (row != null)
                    {
                        if (row["id"] != null && row["id"].ToString() != "")
                        {
                            model.id = int.Parse(row["id"].ToString());
                        }
                        if (row["user_id"] != null && row["user_id"].ToString() != "")
                        {
                            model.user_id = long.Parse(row["user_id"].ToString());
                        }
                        if (row["interest_rate"] != null && row["interest_rate"].ToString() != "")
                        {
                            model.interest_rate = decimal.Parse(row["interest_rate"].ToString());
                        }
                        if (row["enable_day"] != null && row["enable_day"].ToString() != "")
                        {
                            model.enable_day = int.Parse(row["enable_day"].ToString());
                        }
                        if (row["c_time"] != null && row["c_time"].ToString() != "")
                        {
                            model.c_time = DateTime.Parse(row["c_time"].ToString());
                        }
                        if (row["use_time"] != null && row["use_time"].ToString() != "")
                        {
                            model.use_time = DateTime.Parse(row["use_time"].ToString());
                        }
                        if (row["e_time"] != null && row["e_time"].ToString() != "")
                        {
                            model.e_time = DateTime.Parse(row["e_time"].ToString());
                        }
                        if (row["use_status"] != null && row["use_status"].ToString() != "")
                        {
                            model.use_status = int.Parse(row["use_status"].ToString());
                        }
                        list.Add(model);
                    }
                    #endregion
                }
            }
            return list;
        }



        /// <summary>
        /// 判断交易密码是否与登录密码相同 By张浩然 2015-2-27 10：47：00
        /// </summary>
        /// <param name="fetchPwd">交易密码</param>
        /// <returns></returns>
        public bool CheckFetchPwdAndPwd(string fetchPwd, long id)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where pwd =?fetchPwd and id=?id ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?fetchPwd", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?id", MySqlDbType.Int64,20)
            //};
            //parameters[0].Value = fetchPwd;
            //parameters[1].Value = id;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年6月27日 10:47:00-张浩然-判断交易密码是否与登录密码相同", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { fetchPwd = fetchPwd, id = id };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/CheckFetchPwdAndPwd_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        /// <summary>
        /// 2015年7月7日 by张浩然 判断登录密码是否与交易密码相同
        /// </summary>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public bool CheckPwdAndFetchPwd(string Pwd, long id)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where sall_pwd =?Pwd and id=?id ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Pwd", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?id", MySqlDbType.Int64,20)
            //};
            //parameters[0].Value = Pwd;
            //parameters[1].Value = id;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年7月7日 by张浩然 判断登录密码是否与交易密码相同", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { Pwd = Pwd, id = id };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/CheckPwdAndFetchPwd_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }
        /// <summary>
        /// 2015年7月7日 by张浩然 判断登录密码是否与交易密码相同
        /// </summary>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public bool CheckPwdAndFetchPwdByPhone(string Pwd, string Phone)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id from user_info ");
            //strSql.Append(" where sall_pwd =?Pwd and phone=?phone ");
            //MySqlParameter[] parameters = {
            //        new MySqlParameter("?Pwd", MySqlDbType.VarChar,50),
            //        new MySqlParameter("?phone", MySqlDbType.VarChar,50)
            //};
            //parameters[0].Value = Pwd;
            //parameters[1].Value = Phone;
            //object i = DataBaseOperator.YxLiCalUserRead.ExecuteScalar(strSql.ToString(), "2015年7月7日 by张浩然 判断登录密码是否与交易密码相同", parameters);

            //if (i != null)
            //{
            //    if (Convert.ToInt64(i) > 0)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            var user = new { Pwd = Pwd, Phone = Phone };
            string rs = YxLica.Tools.GlobalPassportTools.GetApiResult("UserInfo/CheckPwdAndFetchPwdByPhone_Post", YxLiCai.Tools.SerializeHelper.JsonSerializer(user));
            ResultApiInfo<bool> resultApiInfo =
                YxLiCai.Tools.SerializeHelper.JsonDeserialize<ResultApiInfo<bool>>(rs);
            if (resultApiInfo != null && resultApiInfo.code == 0)
            {
                return resultApiInfo.data;

            }

            return false;
        }

        #region 插入短信发送记录
        /// <summary>
        /// 插入短信发送记录
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        public static bool AddUserSendMesg(string Phone, string content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_send_log(");
            strSql.Append("phone,content,remark,c_time)");
            strSql.Append(" values (");
            strSql.Append("@phone,@content,@remark,@c_time)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@phone", MySqlDbType.VarChar,13),
                    new MySqlParameter("@content", MySqlDbType.VarChar,200),
                    new MySqlParameter("@remark", MySqlDbType.VarChar,200),
                    new MySqlParameter("@c_time", MySqlDbType.DateTime)};
            parameters[0].Value = Phone;
            parameters[1].Value = content;
            parameters[2].Value = "发送短信-.NET";
            parameters[3].Value = DateTime.Now;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "发送短息日志表", parameters);
        }
        #endregion

    }
}
