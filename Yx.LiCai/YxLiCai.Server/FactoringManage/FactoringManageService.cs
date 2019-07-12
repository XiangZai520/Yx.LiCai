using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.FactoringManage;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Model.User;

namespace YxLiCai.Server.FactoringManage
{
    //保理业务层
    public class FactoringManageService
    {
        FactoringManageDao dao = new FactoringManageDao();
        /// <summary>
        /// 获取用户账户日志
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="s_time"></param>
        /// <param name="e_time"></param>
        /// <returns></returns>
        public ResultInfo<List<FactoringUserAccountLog>> GetUserAccountLog(long user_id, DateTime s_time, DateTime e_time)
        {
            ResultInfo<List<FactoringUserAccountLog>> result = new ResultInfo<List<FactoringUserAccountLog>>();
            List<FactoringUserAccountLog> list = new List<FactoringUserAccountLog>();
            FactoringUserAccountLog item;
            try
            {
                DataTable dt = dao.GetUserAccountLog(user_id, s_time, e_time);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new FactoringUserAccountLog();
                        //a.pjt_id,b.name,b.amount,a.change_amount,a.type
                        item.pjt_id = Convert.ToInt32(dr["pjt_id"]);
                        item.pjt_name = Convert.ToString(dr["name"]);
                        item.pjt_amount = Convert.ToDecimal(dr["amount"]);
                        item.change_amount = Convert.ToDecimal(dr["change_amount"]);
                        item.type = Convert.ToInt32(dr["type"]);
                        list.Add(item);
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取保理债权列表（默认当前只有一个保理）
        /// </summary>
        /// <returns></returns>
        public ResultInfo<List<InvestmentDetailEx>> GetInvestmentDetail()
        {
            ResultInfo<List<InvestmentDetailEx>> result = new ResultInfo<List<InvestmentDetailEx>>();
            List<InvestmentDetailEx> list = new List<InvestmentDetailEx>();
            InvestmentDetailEx item;
            try
            {
                DataTable dt = dao.GetInvestmentDetail();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new InvestmentDetailEx();
                        //a.pjt_id,b.name,b.amount,a.change_amount,a.type
                        item.pjt_id = Convert.ToInt32(dr["pjt_id"]);
                        item.pjt_name = Convert.ToString(dr["pjt_name"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        list.Add(item);
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 插入客户申请充值记录（只是申请，未经过第三方通知成功）
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="c_time"></param>
        /// <param name="type">1易宝2连连</param>
        /// <param name="amount"></param>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public ResultInfo<bool> InsertUserRecharge(long user_id, DateTime c_time, int type, decimal amount, string mer_ord_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            try
            {
                result.Result = true;
                result.Data = dao.InsertUserRecharge(user_id, c_time, type, amount, mer_ord_id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = false;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据商户编号获取充值记录
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public ResultInfo<UserRecharge> GetUserRecharge(string mer_ord_id)
        {
            ResultInfo<UserRecharge> result = new ResultInfo<UserRecharge>();
            UserRecharge item = null;
            try
            {
                DataTable dt = dao.GetUserRecharge(mer_ord_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    //id,c_time,user_id,type,amount,status,mer_ord_id,version,remark,pay_time,third_ord_id,user_poundage,mer_poundage
                    item = new UserRecharge();
                    item.id = Convert.ToInt32(dr["id"]);
                    item.c_time = Convert.ToDateTime(dr["c_time"]);
                    item.user_id = Convert.ToInt64(dr["user_id"]);
                    item.type = Convert.ToInt32(dr["type"]);
                    item.amount = Convert.ToDecimal(dr["amount"]);
                    item.status = Convert.ToInt32(dr["status"]);
                    item.mer_ord_id = Convert.ToString(dr["mer_ord_id"]);
                    item.pay_time = Convert.ToDateTime(dr["pay_time"] == DBNull.Value ? "1900-01-01" : dr["pay_time"]);
                    item.third_ord_id = Convert.ToString(dr["third_ord_id"] == DBNull.Value ? "" : dr["third_ord_id"]);
                    item.user_poundage = Convert.ToDecimal(dr["user_poundage"]);
                    item.mer_poundage = Convert.ToDecimal(dr["mer_poundage"]);
                }
                result.Result = true;
                result.Data = item;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;

        }
        /// <summary>
        /// 更新充值申请为充值成功
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <param name="amount"></param>
        /// <param name="pay_time"></param>
        /// <param name="third_ord_id"></param>
        /// <param name="user_poundage"></param>
        /// <param name="mer_poundage"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserRechargeForRechargeSuccess(int id, long user_id, decimal amount, DateTime pay_time, string third_ord_id, decimal user_poundage, decimal mer_poundage)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            try
            {
                //查询user_balance_item 是否存在type=6的
                int op_type = 1;
                if(dao.GetUserBalanceItemCountByUserIDAndType(user_id,1)>0)
                {
                    op_type = 2;
                }
                result.Result = true;
                result.Data = dao.UpdateUserRechargeForRechargeSuccess(id, user_id, amount, pay_time, third_ord_id, user_poundage, mer_poundage, op_type);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = false;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据user_id获取充值记录
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public ResultInfo<List<UserRecharge>> GetUserRechargeByUserID(long user_id, DateTime s_ctime, DateTime e_ctime, int status)
        {
            ResultInfo<List<UserRecharge>> result = new ResultInfo<List<UserRecharge>>();
            List<UserRecharge> list = new List<UserRecharge>();
            UserRecharge item = null;
            try
            {
                DataTable dt = dao.GetUserRechargeByUserID(user_id, s_ctime, e_ctime,status);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        //id,c_time,user_id,type,amount,status,mer_ord_id,version,remark,pay_time,third_ord_id,user_poundage,mer_poundage
                        item = new UserRecharge();
                        item.id = Convert.ToInt32(dr["id"]);
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.user_id = Convert.ToInt64(dr["user_id"]);
                        item.type = Convert.ToInt32(dr["type"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        item.status = Convert.ToInt32(dr["status"]);
                        item.mer_ord_id = Convert.ToString(dr["mer_ord_id"]);
                        item.pay_time = Convert.ToDateTime(dr["pay_time"] == DBNull.Value ? "1900-01-01" : dr["pay_time"]);
                        item.third_ord_id = Convert.ToString(dr["third_ord_id"] == DBNull.Value ? "" : dr["third_ord_id"]);
                        item.user_poundage = Convert.ToDecimal(dr["user_poundage"]);
                        item.mer_poundage = Convert.ToDecimal(dr["mer_poundage"]);
                        list.Add(item);
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;

        }

        /// <summary>
        /// 保理新增提现申请
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="WithdrawAmount">申请提现金额</param>
        /// <param name="l_name">登录名</param>
        /// <param name="r_name">真实姓名</param>
        /// <param name="u_phone">手机号</param>
        /// <param name="bk_name">银行名称</param>
        /// <param name="bk_card">银行卡</param>
        /// <returns></returns>
        public ResultInfo<bool> WithdrawApply(long user_id, decimal WithdrawAmount, string l_name, string r_name, string u_phone, string bk_name, string bk_card)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            try
            {
                result.Result = true;
                result.Data = dao.WithdrawApply(user_id, WithdrawAmount, l_name, r_name, u_phone, bk_name, bk_card);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = false;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据user_id获取提现记录
        /// </summary>
        /// <param name="mer_ord_id"></param>
        /// <returns></returns>
        public ResultInfo<List<UserWithdraw>> GetUserWithdrawByUserID(long user_id, DateTime s_ctime, DateTime e_ctime, int status)
        {
            ResultInfo<List<UserWithdraw>> result = new ResultInfo<List<UserWithdraw>>();
            List<UserWithdraw> list = new List<UserWithdraw>();
            UserWithdraw item = null;
            try
            {
                DataTable dt = dao.GetUserWithdrawByUserID(user_id, s_ctime, e_ctime, status);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        //id,  user_id,  l_name,  r_name,  u_phone,  bk_name,  bk_card,  c_time,  amount,  amount_balance,  amount_principal,  status,                         op_status,  auditor_id,  audit_time,  rec_summary_id,  rfd_balance_id,  rfd_principal_id,  remark 
                        item = new UserWithdraw();
                        item.id = Convert.ToInt32(dr["id"]);
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.user_id = Convert.ToInt64(dr["user_id"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        item.status = Convert.ToInt32(dr["status"]);

                        item.l_name = Convert.ToString(dr["l_name"]);
                        item.r_name = Convert.ToString(dr["r_name"]);
                        item.u_phone = Convert.ToString(dr["u_phone"]);
                        item.bk_name = Convert.ToString(dr["bk_name"]);
                        item.bk_card = Convert.ToString(dr["bk_card"]);
                        list.Add(item);
                    }
                }
                result.Result = true;
                result.Data = list;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;

        }
    }
}
