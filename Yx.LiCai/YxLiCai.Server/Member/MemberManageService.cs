using System;
using System.Collections.Generic;
using System.Data;
using YxLiCai.Dao.Member;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Server.User;

namespace YxLiCai.Server.Member
{
    /// <summary>
    /// 会员管理业务类 2015.6.24 张世晓
    /// </summary>
    public class MemberManageService
    {
        MemberManageDao dao = new MemberManageDao();
        /// <summary>
        /// 获取会员个人信息列表
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <param name="S_RegTime">开始注册时间</param>
        /// <param name="E_RegTime">结束注册时间</param>
        /// <param name="Status">用户状态1正常0拉黑-1全部</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总条数</param>
        /// <returns>列表</returns>
        public ResultInfo<List<MemberInfoEx>> GetMemberInfo(string LoginName, string MyRealName, string Phone, DateTime S_RegTime, DateTime E_RegTime, int Status, int PageIndex, int PageSize, out int TotalCount)
        {
            ResultInfo<List<MemberInfoEx>> result = new ResultInfo<List<MemberInfoEx>>();
            result.Result = false;
            List<MemberInfoEx> list = null;
            MemberInfoEx item;
            long userid = 0;
            UserCountService userCountService = new UserCountService();
            int SCount = PageSize * (PageIndex - 1);
            try
            {
                DataSet ds = dao.GetMemberInfo(LoginName, MyRealName, Phone, S_RegTime, E_RegTime, Status, SCount, PageSize, out TotalCount);
                TotalCount = 0;
                if (ds != null && ds.Tables.Count == 2)
                {
                    list = new List<MemberInfoEx>();
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //a.LoginName,a.MyRealName,a.Phone,a.RegTime,a.login_times,a.last_login_time,a.Status
                            userid = Convert.ToInt64(dr["id"]);
                            item = new MemberInfoEx();
                            item.UserID = userid;
                            item.LoginName = Convert.ToString(dr["LoginName"]);
                            if (!string.IsNullOrEmpty(item.LoginName) && item.LoginName.Length > 8)
                            {
                                item.LoginName = item.LoginName.Substring(0, 3) + "***" + item.LoginName.Substring(7, 4);
                            }
                            item.MyRealName = Convert.ToString(dr["MyRealName"]);
                            item.Phone = Convert.ToString(dr["Phone"]);
                            if (!string.IsNullOrEmpty(item.Phone) && item.Phone.Length > 8)
                            {
                                item.Phone = item.Phone.Substring(0, 3) + "***" + item.Phone.Substring(7, 4);
                            }
                            item.RegTime = Convert.ToDateTime(dr["RegTime"] == DBNull.Value ? "1900-01-01" : dr["RegTime"]);
                            item.login_times = Convert.ToInt32(dr["login_times"]);
                            item.last_login_time = Convert.ToDateTime(dr["last_login_time"] == DBNull.Value ? "1900-01-01" : dr["last_login_time"]);
                            item.Status = Convert.ToInt32(dr["Status"]);
                            item.TotalMoney = userCountService.GetZongMoney(userid).Data;
                            item.RegTime_s = item.RegTime.ToString("yyyy-MM-dd HH:mm:ss");
                            item.last_login_time_s = (item.last_login_time.ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00" ? "" : item.last_login_time.ToString("yyyy-MM-dd HH:mm:ss"));

                            list.Add(item);
                        }
                    }
                    dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                result.Data = list;
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                TotalCount = 0;
                result.Data = null;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取黑名单列表
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">真实姓名</param>
        /// <param name="S_Time">拉黑开始时间</param>
        /// <param name="E_Time">拉黑结束时间</param>
        /// <param name="OprateName">操作人</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总条数</param>
        /// <returns>拉黑列表</returns>
        public ResultInfo<List<UserBlackListEx>> GetBlackList(string LoginName, string MyRealName, DateTime S_Time, DateTime E_Time, string OprateName, int CurrentPage, int PageSize, out int TotalCount)
        {
            ResultInfo<List<UserBlackListEx>> result = new ResultInfo<List<UserBlackListEx>>();
            result.Result = false;
            List<UserBlackListEx> list = null;
            UserBlackListEx item;

            UserCountService userCountService = new UserCountService();
            int SCount = PageSize * (CurrentPage - 1);
            try
            {
                DataSet ds = dao.GetBlackList(LoginName, MyRealName, S_Time, E_Time, OprateName, SCount, PageSize, out TotalCount);
                TotalCount = 0;
                if (ds != null && ds.Tables.Count == 2)
                {
                    list = new List<UserBlackListEx>();
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //a.id,a.user_id,a.login_name,a.my_realname,a.phone,a.create_time,a.oprate_man_name,a.remark
                            item = new UserBlackListEx();
                            item.id = Convert.ToInt32(dr["id"]);
                            item.user_id = Convert.ToInt32(dr["user_id"]);
                            item.login_name = Convert.ToString(dr["login_name"]);
                            if (!string.IsNullOrEmpty(item.login_name) && item.login_name.Length > 8)
                            {
                                item.login_name = item.login_name.Substring(0, 3) + "***" + item.login_name.Substring(7, 4);
                            }
                            item.my_realname = Convert.ToString(dr["my_realname"]);
                            item.phone = Convert.ToString(dr["phone"]);
                            if (!string.IsNullOrEmpty(item.phone) && item.phone.Length > 8)
                            {
                                item.phone = item.phone.Substring(0, 3) + "***" + item.phone.Substring(7, 4);
                            }
                            item.create_time = Convert.ToDateTime(dr["create_time"] == DBNull.Value ? "1900-01-01" : dr["create_time"]);
                            item.oprate_man_name = Convert.ToString(dr["oprate_man_name"]);
                            item.remark = Convert.ToString(dr["remark"]);
                            item.create_time_s = item.create_time.ToString("yyyy-MM-dd HH:mm:ss");

                            list.Add(item);
                        }
                    }
                    dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                result.Data = list;
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                TotalCount = 0;
                result.Data = null;
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 取消会员黑名单
        /// </summary>
        /// <param name="userid">会员id</param>
        /// <param name="accountid">操作人id</param>
        /// <returns></returns>
        public bool CancleBlackList(long userid, int accountid)
        {
            try
            {
                return dao.CancleBlackList(userid, accountid);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                return false;
            }
        }

        /// <summary>
        /// 根据会员id获取提现列表
        /// </summary>
        /// <param name="UserID">会员id</param>
        /// <returns></returns>
        public ResultInfo<List<WithdrawRecordEx>> GetWithDrawListByUserID(long UserID)
        {
            //OrderInfoID,amount,create_time
            List<WithdrawRecordEx> list = new List<WithdrawRecordEx>();
            WithdrawRecordEx item;
            ResultInfo<List<WithdrawRecordEx>> result = new ResultInfo<List<WithdrawRecordEx>>();
            result.Result = false;
            try
            {
                DataSet ds = dao.GetWithDrawListByUserID(UserID);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new WithdrawRecordEx();
                            item.OrderInfoID = Convert.ToInt64(dr["OrderInfoID"]);
                            item.LoginName = dr["l_name"].ToString();
                            item.WithdrawMoney = Convert.ToDecimal(dr["amount"]);
                            item.FinalWithdrawMoney = Convert.ToDecimal(dr["amount"]);
                            item.CreateTime = Convert.ToDateTime(dr["create_time"]);

                            list.Add(item);
                        }
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = null;
            }
            return result;
        }
        /// <summary>
        /// 把会员加入用户黑名单
        /// </summary>
        /// <param name="Remark">拉黑理由</param>
        /// <param name="UserID">会员id</param>
        /// <param name="OprateManID">操作人id</param>
        /// <param name="OprateManName">操作人姓名</param>
        /// <returns></returns>
        public ResultInfo<bool> JoinUserBlacklist(string remark, long userid, int opratemanid, string opratemanname)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Data = dao.JoinUserBlacklist(remark, userid, opratemanid, opratemanname);
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = false;
            }
            return result;
        }


        /// <summary>
        /// 根据会员id获取提现列表
        /// </summary>
        /// <param name="UserID">会员id</param>
        /// <returns></returns>
        public ResultInfo<List<RedemptionRecordEx>> GetRedemptionRecordListByUserID(long UserID)
        {
            //OrderInfoID,amount,create_time
            List<RedemptionRecordEx> list = new List<RedemptionRecordEx>();
            RedemptionRecordEx item;
            ResultInfo<List<RedemptionRecordEx>> result = new ResultInfo<List<RedemptionRecordEx>>();
            result.Result = false;
            try
            {
                DataSet ds = dao.GetRedemptionRecordListByUserID(UserID);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new RedemptionRecordEx();
                            item.OrderInfoID = Convert.ToInt64(dr["ord_id"]);
                            item.LoginName = dr["phone"].ToString();
                            item.RedemptionMoney = Convert.ToDecimal(dr["amount"]);
                            item.WeiYueJin = Convert.ToDecimal(dr["LiquidatedDamages"]);
                            item.FinalRedemptionMoney = Convert.ToDecimal(dr["ActualAmount"]);
                            item.CreateTime = Convert.ToDateTime(dr["c_time"]);

                            list.Add(item);
                        }
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = null;
            }
            return result;
        }


    }
}
