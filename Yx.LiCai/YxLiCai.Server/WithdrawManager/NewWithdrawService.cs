using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Server.WithdrawManager
{
    /// <summary>
    /// 提现业务类
    /// </summary>
    public class NewWithdrawService
    {
        #region 用户提现
        YxLiCai.Dao.WithdrawManager.NewWithdrawDao dao = new Dao.WithdrawManager.NewWithdrawDao();
        /// <summary>
        /// 获取用户/保理提现数据
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">姓名</param>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <param name="status">状态(0未审核1审核通过2已提现3审核未通过4支付失败)</param>
        /// <param name="user_type">0用户1保理</param>
        /// <param name="scount"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ResultInfo<List<UserWithDrawEx>> GetUserWithDrawList(string LoginName, string MyRealName, DateTime s_ctime, DateTime e_ctime, int status, int user_type, int PageIndex, int pagesize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<UserWithDrawEx>> result = new ResultInfo<List<UserWithDrawEx>>();
            result.Result = false;
            int scount = pagesize * (PageIndex - 1);
            try
            {
                List<UserWithDrawEx> list = new List<UserWithDrawEx>();
                UserWithDrawEx item;
                DataSet ds = dao.GetUserWithDrawList(LoginName, MyRealName, s_ctime, e_ctime, status, user_type, scount, pagesize);
                if (ds != null && ds.Tables.Count == 2)
                {
                    DataTable dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    }
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new UserWithDrawEx();
                            //b.login_name,b.real_name,c.bank_name,c.bank_card,c.bank_phone,a.amount,a.amount_balance,a.amount_principal,a.c_time,a.auditor_name,a.audit_time,a.loan_name,a.loan_time,a.status
                            item.id = Convert.ToInt32(dr["id"]);
                            item.login_name = Convert.ToString(dr["login_name"] == DBNull.Value ? "" : dr["login_name"]);
                            item.real_name = Convert.ToString(dr["real_name"] == DBNull.Value ? "" : dr["real_name"]);
                            item.bank_name = Convert.ToString(dr["bank_name"] == DBNull.Value ? "" : dr["bank_name"]);
                            item.bank_card = Convert.ToString(dr["bank_card"] == DBNull.Value ? "" : dr["bank_card"]);
                            item.bank_phone = Convert.ToString(dr["bank_phone"] == DBNull.Value ? "" : dr["bank_phone"]);
                            item.amount = Convert.ToDecimal(dr["amount"]);
                            item.amount_balance = Convert.ToDecimal(dr["amount_balance"]);
                            item.amount_principal = Convert.ToDecimal(dr["amount_principal"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.auditor_name = Convert.ToString(dr["auditor_name"] == DBNull.Value ? "" : dr["auditor_name"]);
                            item.audit_time = Convert.ToDateTime(dr["audit_time"] == DBNull.Value ? "1900-01-01" : dr["audit_time"]);
                            item.loan_name = Convert.ToString(dr["loan_name"] == DBNull.Value ? "" : dr["loan_name"]);
                            item.loan_time = Convert.ToDateTime(dr["loan_time"] == DBNull.Value ? "1900-01-01" : dr["loan_time"]);
                            item.status = Convert.ToInt32(dr["status"]);
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
        /// 更新提现状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateWithdrawStatus(int id, int status, int auditor_id, DateTime audit_time, string auditor_name, string remark)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
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
        /// 放款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> WithdrawLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.WithdrawLoan(id, loan_id, loan_name, loan_time);
                if (result.Data)
                {
                    //申请提现
                    YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                    result.Data = yeePayManager.YeePayWithdraw(1, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                    if (!result.Data)
                    {
                        dao.UpdateWithdrawLoanStatus(id, 4, loan_id, loan_time, loan_name, "支付失败");
                    }
                }

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
        /// 重新放款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> WithdrawReLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;

                //申请提现
                YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                result.Data = yeePayManager.YeePayWithdraw(1, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                if (result.Data)
                {
                    dao.UpdateWithdrawLoanStatus(id, 2, loan_id, loan_time, loan_name, "支付成功");
                }
                else
                {
                    dao.UpdateWithdrawLoanStatus(id, 4, loan_id, loan_time, loan_name, "重新支付失败");
                }

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
        /// 获取用户提现的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo<UserWithDrawEx> GetUserWithDrawInfoByID(int id)
        {
            ResultInfo<UserWithDrawEx> result = new ResultInfo<UserWithDrawEx>();
            UserWithDrawEx item = null;
            result.Result = false;
            try
            {
                DataTable dt = dao.GetUserWithDrawInfoByID(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    item = new UserWithDrawEx();
                    //a.user_id,c.bank_card,a.amount
                    item.user_id = Convert.ToInt32(dr["user_id"]);
                    item.bank_card = Convert.ToString(dr["bank_card"] == DBNull.Value ? "" : dr["bank_card"]);
                    item.amount = Convert.ToDecimal(dr["amount"]);
                    item.status = Convert.ToInt32(dr["status"]);
                    item.remark = Convert.ToString(dr["remark"] == DBNull.Value ? "" : dr["remark"]);
                    item.account_amount_blance = Convert.ToDecimal(dr["amount_blance"]);
                    item.account_amount_blance_fz = Convert.ToDecimal(dr["amount_blance_fz"]);
                }
                result.Result = true;
                result.Data = item;
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
        /// 根据状态获取所有提现id
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultInfo<List<int>> GetAllUserWithDraw(int status, int user_type)
        {
            ResultInfo<List<int>> result = new ResultInfo<List<int>>();
            result.Result = false;
            try
            {
                List<int> list = new List<int>();
                DataTable dt = dao.GetAllUserWithDraw(status, user_type);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(Convert.ToInt32(dr["id"]));
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
        #endregion
        #region 用户赎回
        /// <summary>
        /// 获取用户赎回数据
        /// </summary>
        /// <param name="LoginName">用户名</param>
        /// <param name="MyRealName">姓名</param>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <param name="status">状态(0未审核1审核通过2已提现3审核未通过4支付失败)</param>
        /// <param name="user_type">0用户1保理</param>
        /// <param name="scount"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ResultInfo<List<UserRedemptionEx>> GetUserRedemptionList(string LoginName, string MyRealName, DateTime s_ctime, DateTime e_ctime, int status, int PageIndex, int pagesize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<UserRedemptionEx>> result = new ResultInfo<List<UserRedemptionEx>>();
            result.Result = false;
            int scount = pagesize * (PageIndex - 1);
            try
            {
                List<UserRedemptionEx> list = new List<UserRedemptionEx>();
                UserRedemptionEx item;
                DataSet ds = dao.GetUserRedemptionList(LoginName, MyRealName, s_ctime, e_ctime, status, scount, pagesize);
                if (ds != null && ds.Tables.Count == 2)
                {
                    DataTable dt = ds.Tables[1];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    }
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new UserRedemptionEx();

                            item.id = Convert.ToInt32(dr["id"]);
                            item.login_name = Convert.ToString(dr["login_name"] == DBNull.Value ? "" : dr["login_name"]);
                            item.real_name = Convert.ToString(dr["real_name"] == DBNull.Value ? "" : dr["real_name"]);
                            item.bank_name = Convert.ToString(dr["bank_name"] == DBNull.Value ? "" : dr["bank_name"]);
                            item.bank_card = Convert.ToString(dr["bank_card"] == DBNull.Value ? "" : dr["bank_card"]);
                            item.bank_phone = Convert.ToString(dr["bank_phone"] == DBNull.Value ? "" : dr["bank_phone"]);
                            item.amount = Convert.ToDecimal(dr["amount"]);
                            item.amount_penalty = Convert.ToDecimal(dr["amount_penalty"]);
                            item.amount_Actual = item.amount - item.amount_penalty;
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.auditor_name = Convert.ToString(dr["auditor_name"] == DBNull.Value ? "" : dr["auditor_name"]);
                            item.audit_time = Convert.ToDateTime(dr["audit_time"] == DBNull.Value ? "1900-01-01" : dr["audit_time"]);
                            item.loan_name = Convert.ToString(dr["loan_name"] == DBNull.Value ? "" : dr["loan_name"]);
                            item.loan_time = Convert.ToDateTime(dr["loan_time"] == DBNull.Value ? "1900-01-01" : dr["loan_time"]);
                            item.status = Convert.ToInt32(dr["status"]);
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
        /// 获取用户赎回的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo<UserRedemptionEx> GetUserRedemptionInfoByID(int id)
        {
            ResultInfo<UserRedemptionEx> result = new ResultInfo<UserRedemptionEx>();
            UserRedemptionEx item = null;
            result.Result = false;
            try
            {
                DataTable dt = dao.GetUserRedemptionInfoByID(id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    item = new UserRedemptionEx();
                    //a.user_id,c.bank_card,a.amount
                    item.user_id = Convert.ToInt32(dr["user_id"]);
                    item.bank_card = Convert.ToString(dr["bank_card"] == DBNull.Value ? "" : dr["bank_card"]);
                    item.amount = Convert.ToDecimal(dr["amount"]);
                    item.amount_penalty = Convert.ToDecimal(dr["amount_penalty"]);
                    item.amount_Actual = item.amount - item.amount_penalty;
                    item.status = Convert.ToInt32(dr["status"]);
                    item.remark = Convert.ToString(dr["remark"] == DBNull.Value ? "" : dr["remark"]);
                    item.account_amount_blance = Convert.ToDecimal(dr["amount_blance"]);
                    item.account_amount_blance_fz = Convert.ToDecimal(dr["amount_blance_fz"]);
                }
                result.Result = true;
                result.Data = item;
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
        /// 更新赎回状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateRedemptionStatus(int id, int status, int auditor_id, DateTime audit_time, string auditor_name, string remark)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateRedemptionStatus(id, status, auditor_id, audit_time, auditor_name, remark);
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
        /// 放款（赎回）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> RedemptionLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.RedemptionLoan(id, loan_id, loan_name, loan_time, amount);
                if (result.Data)
                {
                    //申请提现
                    YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                    result.Data = yeePayManager.YeePayWithdraw(2, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                    if (!result.Data)
                    {
                        dao.UpdateRedemptionLoanStatus(id, 4, loan_id, loan_time, loan_name, "支付失败");
                    }
                }

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
        /// 重新放款(赎回)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> RedemptionReLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;

                //申请提现
                YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                result.Data = yeePayManager.YeePayWithdraw(2, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                if (result.Data)
                {
                    dao.UpdateRedemptionLoanStatus(id, 2, loan_id, loan_time, loan_name, "支付成功");
                }
                else
                {
                    dao.UpdateRedemptionLoanStatus(id, 4, loan_id, loan_time, loan_name, "重新支付失败");
                }

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
        /// 根据状态获取所有赎回id
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultInfo<List<int>> GetAllUserRedemption(int status)
        {
            ResultInfo<List<int>> result = new ResultInfo<List<int>>();
            result.Result = false;
            try
            {
                List<int> list = new List<int>();
                DataTable dt = dao.GetAllUserRedemption(status);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(Convert.ToInt32(dr["id"]));
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

        #endregion
        #region 保理提现
        /// <summary>
        /// 放款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> FactoringWithdrawLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.WithdrawLoan(id, loan_id, loan_name, loan_time);
                if (result.Data)
                {
                    //申请提现
                    YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                    result.Data = yeePayManager.YeePayWithdraw(3, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                    if (!result.Data)
                    {
                        dao.UpdateWithdrawLoanStatus(id, 4, loan_id, loan_time, loan_name, "支付失败");
                    }
                }

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
        /// 重新放款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loan_id"></param>
        /// <param name="loan_name"></param>
        /// <param name="loan_time"></param>
        /// <returns></returns>
        public ResultInfo<bool> FactoringWithdrawReLoan(int id, int loan_id, string loan_name, DateTime loan_time, int user_id, string cardno, string card_top, string card_last, decimal amount)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;

                //申请提现
                YxLiCai.Server.Common.YeePayManager yeePayManager = new Common.YeePayManager();
                result.Data = yeePayManager.YeePayWithdraw(3, id, loan_id, user_id.ToString(), 2, cardno, card_top, card_last, amount);
                if (result.Data)
                {
                    dao.UpdateWithdrawLoanStatus(id, 2, loan_id, loan_time, loan_name, "支付成功");
                }
                else
                {
                    dao.UpdateWithdrawLoanStatus(id, 4, loan_id, loan_time, loan_name, "重新支付失败");
                }

            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = false;
            }
            return result;
        }
        #endregion
    }
}
