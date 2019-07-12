using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.Member
{
    /// <summary>
    /// 会员管理
    /// </summary>
    public class MemberManageDao
    {
        /// <summary>
        /// 获取会员个人信息列表
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <param name="Phone">手机号</param>
        /// <param name="S_RegTime">开始注册时间</param>
        /// <param name="E_RegTime">结束注册时间</param>
        /// <param name="Status">用户状态1正常0拉黑-1全部</param>
        /// <param name="SCount">开始计数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        public DataSet GetMemberInfo(string LoginName, string MyRealName, string Phone, DateTime S_RegTime, DateTime E_RegTime, int Status, int SCount, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            LoginName = "%" + LoginName.Trim() + "%";
            MyRealName = "%" + MyRealName.Trim() + "%";

            string strSql = @"select a.id,a.login_name LoginName,a.real_name MyRealName,a.phone Phone,a.reg_time RegTime,a.login_times,a.last_login_time,a.status Status
from user_info a
where (?LoginName='%%' or a.login_name like ?LoginName) and (?MyRealName='%%' or a.real_name like ?MyRealName) and (?Phone='' or a.phone=?Phone) and (?S_RegTime='1900-01-01' or a.reg_time>=?S_RegTime) and (?E_RegTime='9999-01-02' or a.reg_time<?E_RegTime) and (?Status=-1 or a.status=?Status)
order by a.reg_time desc
limit ?SCount,?PageSize;
select count(1)
from user_info a
where (?LoginName='%%' or a.login_name like ?LoginName) and (?MyRealName='%%' or a.real_name like ?MyRealName) and (?Phone='' or a.phone=?Phone) and (?S_RegTime='1900-01-01' or a.reg_time>=?S_RegTime) and (?E_RegTime='9999-01-02' or a.reg_time<?E_RegTime) AND (?Status=-1 or a.status=?Status);  ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?LoginName", MySqlDbType.VarChar),
                    new MySqlParameter("?MyRealName", MySqlDbType.VarChar),
                    new MySqlParameter("?Phone", MySqlDbType.VarChar),
                    new MySqlParameter("?S_RegTime", MySqlDbType.DateTime),
                    new MySqlParameter("?E_RegTime", MySqlDbType.DateTime),
                    new MySqlParameter("?Status", MySqlDbType.Int32),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
			};
            parameters[0].Value = LoginName;
            parameters[1].Value = MyRealName;
            parameters[2].Value = Phone;
            parameters[3].Value = S_RegTime;
            parameters[4].Value = E_RegTime;
            parameters[5].Value = Status;
            parameters[6].Value = SCount;
            parameters[7].Value = PageSize;
            DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取会员个人信息列表", parameters);


            return ds;
        }

        /// <summary>
        /// 获取黑名单列表
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <param name="S_Time">拉黑开始时间</param>
        /// <param name="E_Time">拉黑结束时间</param>
        /// <param name="OprateName">操作人</param>
        /// <param name="SCount">开始计数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总条数</param>
        /// <returns></returns>
        public DataSet GetBlackList(string LoginName, string MyRealName, DateTime S_Time, DateTime E_Time, string OprateName, int SCount, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            LoginName = "%" + LoginName.Trim() + "%";
            MyRealName = "%" + MyRealName.Trim() + "%";
            OprateName = "%" + OprateName.Trim() + "%";

            string strSql = @"select a.id,a.user_id,a.login_name,a.my_realname,a.phone,a.create_time,a.oprate_man_name,a.remark
from user_blacklist a
where a.is_delete=0 and (?LoginName='%%' or a.login_name like ?LoginName) and (?MyRealName='%%' or a.my_realname like ?MyRealName) and (?OprateName='%%' or a.oprate_man_name like ?OprateName) and (?S_Time='1900-01-01' or a.create_time>=?S_Time) and (?E_Time='9999-01-02' or a.create_time<?E_Time)
order by a.create_time desc
limit ?SCount,?PageSize;

select count(1)
from user_blacklist a
where a.is_delete=0 and (?LoginName='%%' or a.login_name like ?LoginName) and (?MyRealName='%%' or a.my_realname like ?MyRealName) and (?OprateName='%%' or a.oprate_man_name like ?OprateName) and (?S_Time='1900-01-01' or a.create_time>=?S_Time) and (?E_Time='9999-01-02' or a.create_time<?E_Time);  ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?LoginName", MySqlDbType.VarChar),
                    new MySqlParameter("?MyRealName", MySqlDbType.VarChar),
                    new MySqlParameter("?OprateName", MySqlDbType.VarChar),
                    new MySqlParameter("?S_Time", MySqlDbType.DateTime),
                    new MySqlParameter("?E_Time", MySqlDbType.DateTime),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
			};
            parameters[0].Value = LoginName;
            parameters[1].Value = MyRealName;
            parameters[2].Value = OprateName;
            parameters[3].Value = S_Time;
            parameters[4].Value = E_Time;
            parameters[5].Value = SCount;
            parameters[6].Value = PageSize;
            DataSet ds = DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, "获取黑名单列表", parameters);


            return ds;
        }
        /// <summary>
        /// 取消会员黑名单
        /// </summary>
        /// <param name="userid">会员id</param>
        /// <param name="accountid">操作人id</param>
        /// <returns></returns>
        public bool CancleBlackList(long userid, int accountid)
        {

            string strSql = @"update user_blacklist set update_man_id=?accountid,update_time=now(),is_delete=1 where user_id=?userid and is_delete=0;
                              update user_info set status=1 where id=?userid";
            MySqlParameter[] parameters = {
					new MySqlParameter("?userid", MySqlDbType.Int64),
                    new MySqlParameter("?accountid", MySqlDbType.Int32)
			};
            parameters[0].Value = userid;
            parameters[1].Value = accountid;
            return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql, "取消会员黑名单", parameters);


        }
        /// <summary>
        /// 根据会员id获取提现列表
        /// </summary>
        /// <param name="UserID">会员id</param>
        /// <returns></returns>
        public DataSet GetWithDrawListByUserID(long UserID)
        {

            string strSql = @"select 0 OrderInfoID,amount,c_time create_time,l_name from user_withdraw where user_id=?UserID";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64)
			};
            parameters[0].Value = UserID;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据会员id获取提现列表", parameters);
        }
        /// <summary>
        /// 根据会员id获取赎回列表
        /// </summary>
        /// <param name="UserID">会员id</param>
        /// <returns></returns>
        public DataSet GetRedemptionRecordListByUserID(long UserID)
        {
            string strSql = @"SELECT phone, 
                                   oid.c_time, 
                                   oid.id, 
                                   oid.ord_id, 
                                   TRUNCATE(oid.amount,2) amount, 
                                   oid.principal, 
                                   oid.interest, 
                                   oid.user_id, 
                                   oid.penalty_rate, 
                                   TRUNCATE(TRUNCATE(oid.amount,2) * TRUNCATE(penalty_rate,2),2)          AS LiquidatedDamages, 
                                   TRUNCATE( TRUNCATE(amount,2) - ( TRUNCATE(amount,2) * TRUNCATE(penalty_rate,2)),2) AS ActualAmount 
                            FROM   user_redemption oid 
                                   INNER JOIN user_info u 
                                           ON u.id = oid.user_id 
                                    where  user_id=?UserID";
//                                  where oid.status=2 and user_id=?UserID";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64)
			};
            parameters[0].Value = UserID;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据会员id获取赎回列表", parameters);
        }
        /// <summary>
        /// 把会员加入用户黑名单
        /// </summary>
        /// <param name="Remark">拉黑理由</param>
        /// <param name="UserID">会员id</param>
        /// <param name="OprateManID">操作人id</param>
        /// <param name="OprateManName">操作人姓名</param>
        /// <returns></returns>
        public bool JoinUserBlacklist(string remark, long userid, int opratemanid, string opratemanname)
        {
            string sqlstr = @"insert into user_blacklist(user_id,login_name,my_realname,phone,create_time,oprate_man_id,oprate_man_name,remark,is_delete,update_time)
select id,login_name,real_name,phone,?ctime,?opratemanid,?opratemanname,?remark,0,?ctime
from user_info
where id=?userid;update user_info set status=0 where id=?userid;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?userid", MySqlDbType.Int64),
                    new MySqlParameter("?ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?opratemanid", MySqlDbType.Int32),
                    new MySqlParameter("?opratemanname", MySqlDbType.VarChar,50),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,500)
			};
            parameters[0].Value = userid;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = opratemanid;
            parameters[3].Value = opratemanname;
            parameters[4].Value = remark;
            return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(sqlstr, "把会员加入用户黑名单", parameters);

        }
    }
}
