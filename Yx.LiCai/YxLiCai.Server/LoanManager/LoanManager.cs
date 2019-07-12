using System;
using System.Collections.Generic;
using System.Data;
using YxLiCai.Dao.LoanManager;
using YxLiCai.Model;
using YxLiCai.Model.Loan;
using YxLiCai.Server.Common;
using YxLiCai.Server.MenuSet;
using YxLiCai.Tools;

namespace YxLiCai.Server.LoanManager
{
    /// <summary>
    /// 财务放款 相关操作
    /// add by lxm
    /// </summary>
    public class LoanManager
    {
        private LoanManagerDao _dao = new LoanManagerDao();

        #region Get
        /// <summary>
        /// 根据查询条件获取待放款列表
        /// </summary>
        /// <param name="pJname">融资方名称</param>
        /// <param name="loanPeriod">项目借款期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录</param>
        /// <param name="PageSize">页显示数</param>
        /// <param name="totalCount">总记录</param>
        /// <returns></returns>
        public List<LoanModelExtend> GetAll(string mName, int loanPeriod, decimal amount_min, decimal amount_max,
            int startIndex, int PageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<LoanModelExtend>();

            try
            {
                var ds = _dao.GetLoanList(mName, loanPeriod, amount_min, amount_max, startIndex, PageSize);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(DataToLoanModel(row as System.Data.DataRow));
                    }
                }

                if (ds.Tables[1] != null)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out totalCount);
                }

                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 查询全部放款记录
        /// 参考GetAll 不分页
        /// </summary>
        /// <param name="mName"></param>
        /// <param name="loanPeriod"></param>
        /// <param name="amount_min"></param>
        /// <param name="amount_max"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetAllLoanRecord(string mName, int loanPeriod, decimal amount_min, decimal amount_max, int status)
        {
            try
            {
                var ds = _dao.GetAllLoanList(mName, loanPeriod, amount_min, amount_max, status);
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 根据查询条件获取待放款列表
        /// </summary>
        /// <param name="pJname">融资方名称</param>
        /// <param name="loanPeriod">项目借款期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录</param>
        /// <param name="PageSize">页显示数</param>
        /// <param name="totalCount">总记录</param>
        /// <returns></returns>
        public List<LoanModelExtend> GetLoanLog(string mName, int loanPeriod, decimal amount_min, decimal amount_max,
            DateTime? sDate, DateTime? eDate, int startIndex, int PageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<LoanModelExtend>();

            try
            {
                var ds = _dao.GetLoanLog(mName, loanPeriod, amount_min, amount_max, sDate, eDate, startIndex, PageSize);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(DataToLoanModel(row as System.Data.DataRow));
                    }
                }

                if (ds.Tables[1] != null)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out totalCount);
                }

                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 根据查询条件获取放款失败列表
        /// </summary>
        public List<LoanModelExtend> GetLoanFailedList(string mName, int loanPeriod, decimal amount_min, decimal amount_max,
         DateTime? sDate, DateTime? eDate, int startIndex, int PageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<LoanModelExtend>();

            try
            {
                var ds = _dao.GetLoanFailedList(mName, loanPeriod, amount_min, amount_max, sDate, eDate, startIndex, PageSize);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(DataToLoanModel(row as System.Data.DataRow));
                    }
                }

                if (ds.Tables[1] != null)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out totalCount);
                }

                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }

        /// <summary>
        /// 转为Model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public LoanModelExtend DataToLoanModel(System.Data.DataRow row)
        {
            var model = new LoanModelExtend();
            model.project = new Model.Project.ProjectModel();
            model.BankCardInfo = new Model.User.UserBankCardModel();

            try
            {
                if (row != null)
                {
                    #region Match
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = long.Parse(row["id"].ToString());
                    }
                    if (row["ctime"] != null && row["ctime"].ToString() != "")
                    {
                        model.ctime = DateTime.Parse(row["ctime"].ToString());
                    }
                    if (row["pjid"] != null && row["pjid"].ToString() != "")
                    {
                        model.pjid = int.Parse(row["pjid"].ToString());
                    }
                    if (row["projectname"] != null && row["projectname"].ToString() != "")
                    {
                        model.project.ProjectName = row["projectname"].ToString();
                    }
                    if (row["Amount"] != null && row["Amount"].ToString() != "")
                    {
                        model.project.Amount = decimal.Parse(row["Amount"].ToString());
                    }
                    if (row["LoanPeriod"] != null && row["LoanPeriod"].ToString() != "")
                    {
                        model.project.LoanPeriod = int.Parse(row["LoanPeriod"].ToString());
                    }
                    if (row.Table.Columns.Contains("amount_lent") && row["amount_lent"] != null && row["amount_lent"].ToString() != "")
                    {
                        model.project.amount_lent = decimal.Parse(row["amount_lent"].ToString());
                    }
                    if (row.Table.Columns.Contains("amount_expect") && row["amount_expect"] != null && row["amount_expect"].ToString() != "")
                    {
                        model.amount_expect = decimal.Parse(row["amount_expect"].ToString());
                    }
                    if (row.Table.Columns.Contains("account_id") && row["account_id"] != null && row["account_id"].ToString() != "")
                    {
                        model.project.account_id = Convert.ToInt32(row["account_id"].ToString());
                        model.MerchantID = Convert.ToInt32(row["account_id"].ToString());
                    }
                    if (row.Table.Columns.Contains("fina_name") && row["fina_name"] != null && row["fina_name"].ToString() != "")
                    {
                        model.MerchantName = row["fina_name"].ToString();
                    }
                    if (row["bank_name"].ToString() != "")
                    {
                        model.BankCardInfo.BankName = row["bank_name"].ToString();
                    }
                    if (row["bank_card"] != null && row["bank_card"].ToString() != "")
                    {
                        model.BankCardInfo.BankCard = row["bank_card"].ToString();
                    }
                    if (row["myreal_name"] != null && row["myreal_name"].ToString() != "")
                    {
                        model.MerchantLegalManName = row["myreal_name"].ToString();
                    }
                    if (row["phone"] != null && row["phone"].ToString() != "")
                    {
                        model.MerchantLegalManPhone = row["phone"].ToString();
                    }
                    if (row.Table.Columns.Contains("adminID") && row["adminID"] != null && row["adminID"].ToString() != "")
                    {
                        model.adminID = Convert.ToInt32(row["adminID"].ToString());
                    }
                    if (row.Table.Columns.Contains("loanTime") && row["loanTime"] != null && row["loanTime"].ToString() != "")
                    {
                        model.loanTime = Convert.ToDateTime(row["loanTime"].ToString());
                    }
                    if (row.Table.Columns.Contains("Remark") && row["Remark"] != null && row["Remark"].ToString() != "")
                    {
                        model.Remark = row["Remark"].ToString();
                    }
                    if (row.Table.Columns.Contains("m_id") && row["m_id"] != null && row["m_id"].ToString() != "")
                    {
                        model.MerchantID = Convert.ToInt32(row["m_id"].ToString());
                    }
                    #endregion
                }

                return model;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return new LoanModelExtend
                {
                    project = new Model.Project.ProjectModel(),
                    BankCardInfo = new Model.User.UserBankCardModel()
                };
            }

        }
        #endregion

        #region Udpate

        #endregion

        #region 支付相关
        /// <summary>
        /// 财务放款 
        /// 打款到融资方绑定银行卡
        /// </summary>
        /// <param name="recordID">放款记录ID</param>
        /// <param name="merchantID">商户ID</param>
        /// <param name="amount">放款金额</param>
        /// <param name="adminID">财务ID</param>
        /// <returns></returns>
        public bool PayMerchant(long recordID, int accountID, int projectID, decimal amount, int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 3;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var bankCardNum = "";
                var r_id = Convert.ToInt32(recordID);

                if (CallYeePay(accountID, r_id, adminID, amount, out bankCardNum)
                    && !string.IsNullOrEmpty(bankCardNum))
                {
                    //放款成功记录
                    return _dao.LoanSucceeded(projectID, amount, adminID, bankCardNum, accountID);
                }
                else
                {
                    //放款失败记录
                    var remark = "财务放款 调用接口失败";
                    _dao.LoanFailed(projectID, amount, adminID, remark);
                }
                return false;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 重新放款
        /// </summary>
        public bool RePayMerchant(int recordID, int accountID, int projectID, decimal amount, int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 3;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行重新放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var bankCardNum = "";

                if (CallYeePay(accountID, recordID, adminID, amount, out bankCardNum)
                    && !string.IsNullOrEmpty(bankCardNum))
                {
                    //重新放款
                    return _dao.LoanSucceeded(projectID, amount, adminID, bankCardNum, accountID, recordID);
                }
                return false;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 线下已付款，只更新数据库
        /// </summary>
        public bool PayMerchantOffline(long recordID, int accountID, int projectID, decimal amount, int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 3;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行线下放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var fer_info = new YxLiCai.Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao().GetMerchantInfoByAccountID(accountID);
                //放款成功记录
                return _dao.LoanSucceeded(projectID, amount, adminID, fer_info.BankCard, accountID);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 调用封装易宝支付接口的通用方法
        /// </summary>
        private bool CallYeePay(int accountID, int recordID, int adminID,
            decimal amount, out string bankCardNum)
        {
            bankCardNum = string.Empty;

            var identityID = "F" + accountID; //标识id
            var orderID = Guid.NewGuid().ToString("N");
            var fer_info = new YxLiCai.Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao().GetMerchantInfoByAccountID(accountID);

            if (fer_info == null)
                return false;

            bankCardNum = fer_info.BankCard;

            return new YeePayManager().YeePayWithdraw(5, recordID, adminID, identityID, 2,
                fer_info.BankCard, fer_info.First_num, fer_info.Last_num, amount);
        }
        /// <summary>
        /// 全部放款
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public bool PayAll(int adminID)
        {
            var result = true;
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 3;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行全部放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var ds = _dao.GetAllLoanList();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        long recordID = -1; //记录id
                        long.TryParse(row["id"].ToString(), out recordID);
                        int merchantID = -1; //融资方id
                        int.TryParse(row["account_id"].ToString(), out merchantID);
                        int projectID = -1; //项目id
                        int.TryParse(row["pjid"].ToString(), out projectID);
                        decimal amount = -1; //待放款金额
                        decimal.TryParse(row["amount_expect"].ToString(), out amount);

                        if (!PayMerchant(recordID, merchantID, projectID, amount, adminID))
                            result = false;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        public bool PayPartial(int adminID, string IDList)
        {
            var result = true;

            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 3;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行批量放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var ds = _dao.GetAllLoanList();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    var list = new List<string>();
                    list.AddRange(IDList.Split(','));

                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        string id = row["id"].ToString();
                        if (!list.Contains(id))
                            continue;

                        long recordID = -1; //记录id
                        long.TryParse(id, out recordID);
                        int merchantID = -1; //融资方id
                        int.TryParse(row["account_id"].ToString(), out merchantID);
                        int projectID = -1; //项目id
                        int.TryParse(row["pjid"].ToString(), out projectID);
                        decimal amount = -1; //待放款金额
                        decimal.TryParse(row["amount_expect"].ToString(), out amount);
                        string bankCardNum = row["bank_card"].ToString();

                        if (!PayMerchant(recordID, merchantID, projectID, amount, adminID))
                            result = false;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        #endregion
    }
}
