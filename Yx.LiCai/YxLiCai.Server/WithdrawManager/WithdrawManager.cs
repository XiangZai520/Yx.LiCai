using System;
using System.Collections.Generic;
using YxLiCai.Dao.WithdrawManager;
using YxLiCai.Model;
using YxLiCai.Model.UserRedemption;
using YxLiCai.Server.Common;
using YxLiCai.Server.MenuSet;
using YxLiCai.Tools;

namespace YxLiCai.Server.WithdrawManager
{
    /// <summary>
    /// 提现、赎回申请 管理业务处理类
    /// </summary>
    public class WithdrawManager
    {
        private WithdrawManagerDAO _dao = new WithdrawManagerDAO();

        #region 融资方提现
        #region 查询
        /// <summary>
        /// 融资方提现记录
        /// </summary>
        public List<MerchantWithdrawModel> GetMerchantWithdrawList(string lName, int status, DateTime? sDate, DateTime? eDate,
            int startIndex, int pageSize, out int countAll)
        {
            countAll = 0;
            var list = new List<MerchantWithdrawModel>();
            try
            {
                var ds = _dao.GetMerchantWithdrawList(lName, status, sDate, eDate, startIndex, pageSize);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(MerchantWithdrawDataToMode(row as System.Data.DataRow));
                    }
                }
                //总记录
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out countAll);
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
        /// 根据选择的id获取融资方提现申请
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public List<MerchantWithdrawModel> GetMerchantWithdrawList(int status, string idList = "")
        {
            var list = new List<MerchantWithdrawModel>();
            try
            {
                var ds = _dao.GetMerchantWithdrawList(status, idList);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(MerchantWithdrawDataToMode(row as System.Data.DataRow));
                    }
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
        /// 提现记录转为model
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private MerchantWithdrawModel MerchantWithdrawDataToMode(System.Data.DataRow row)
        {
            var model = new MerchantWithdrawModel();

            #region matcn
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.ID = int.Parse(row["id"].ToString());
                }
                if (row["fer_id"] != null && row["fer_id"].ToString() != "")
                {
                    model.AccountID = int.Parse(row["fer_id"].ToString());
                }
                if (row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["amount"].ToString());
                }
                if (row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.ApplyTime = DateTime.Parse(row["c_time"].ToString());
                }
                if (row["auditor_id"] != null && row["auditor_id"].ToString() != "")
                {
                    model.AuditorID = int.Parse(row["auditor_id"].ToString());
                }
                if (row["audit_time"] != null && row["audit_time"].ToString() != "")
                {
                    model.AuditTime = DateTime.Parse(row["audit_time"].ToString());
                }
                if (row["operator_id"] != null && row["operator_id"].ToString() != "")
                {
                    model.OperatorID = int.Parse(row["operator_id"].ToString());
                }
                if (row["pay_time"] != null && row["pay_time"].ToString() != "")
                {
                    model.PayTime = DateTime.Parse(row["pay_time"].ToString());
                }
                if (row["status"] != null && row["status"].ToString() != "")
                {
                    model.Status = int.Parse(row["status"].ToString());
                }
                if (row["l_name"] != null && row["l_name"].ToString() != "")
                {
                    model.LoginName = row["l_name"].ToString();
                }
                if (row["myreal_name"] != null && row["myreal_name"].ToString() != "")
                {
                    model.RealName = row["myreal_name"].ToString();
                }
                if (row["bank_name"] != null && row["bank_name"].ToString() != "")
                {
                    model.Bank = row["bank_name"].ToString();
                }
                if (row["bank_card"] != null && row["bank_card"].ToString() != "")
                {
                    model.BankCard = row["bank_card"].ToString();
                }
                if (row["phone"] != null && row["phone"].ToString() != "")
                {
                    model.Phone = row["phone"].ToString();
                }
                if (row["remark"] != null && row["remark"].ToString() != "")
                {
                    model.remark = row["remark"].ToString();
                }
            }
            #endregion

            return model;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核融资方提现记录
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="adminID"></param>
        /// <param name="status">1审核通过待放款,2审核不通过</param>
        /// <returns></returns>
        public bool AuditMerchantWithdrawRecord(int recordID, int adminID, int status, string remark = "")
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "审核融资方提现";
                SysService.AddSysLog(smodel);
                #endregion

                return _dao.UpdateMerchantWithdrawRecord(status, adminID, recordID, remark);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 批量通过
        /// </summary>
        public bool PassMultiMerchantWithdraw(string idList, int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "批量审核通过融资方提现申请";
                SysService.AddSysLog(smodel);
                #endregion

                return _dao.PassMultiMerchantWithdraw(idList, adminID);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 全部审核通过
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public bool PassAllMerchantWithdraw(int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "全部审核通过融资方提现申请";
                SysService.AddSysLog(smodel);
                #endregion

                return _dao.PassAllMerchantWithdraw(adminID);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        #endregion

        #region 财务放款
        /// <summary>
        /// 融资方提现 - 财务打款
        /// </summary>
        public bool PayMerchant(int recordID, int adminID, int accountID, decimal amount)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                // 1.更新融资方提现记录，状态改为放款成功
                if (!_dao.UpdateMerchantWithdrawRecord(3, -1, recordID))
                    return false;

                // 2.调用接口
                var orderID = Guid.NewGuid().ToString("N");
                var identityID = "F" + accountID;
                var fer_info = new YxLiCai.Dao.UserFinancingManagement.UserInfo_FinancingManagement_Dao().GetMerchantInfoByAccountID(accountID);

                if (fer_info == null)
                    return false;

                var result = new YeePayManager().YeePayWithdraw(4, recordID, adminID, identityID, 2,
                    fer_info.BankCard, fer_info.First_num, fer_info.Last_num, amount);

                // 3.调用接口成功/失败 操作数据库
                var status = result ? 3 : 4;
                var remark = "融资方提现 - 财务放款: 金额【" + amount + "】" + (result ? "成功" : "失败");

                // 4.插入记录
                if (_dao.PayMerchant(recordID, accountID, status, adminID, amount, orderID, accountID, remark))
                {
                    return result;
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
        /// 批量放款
        /// </summary>
        public ResultModel PayMultiMerchantWithdraw(string idList, int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行批量放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var list = GetMerchantWithdrawList(1, idList);

                if (list == null || list.Count == 0)
                    return new ResultModel(false, "没有满足放款条件的记录");

                var result = PayAllMerchantWithdraw(list, adminID);
                if (!result)
                    return new ResultModel(false, "批量放款失败");
                return new ResultModel(true, "批量放款成功");
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return new ResultModel(true, "服务异常");
            }
        }
        /// <summary>
        /// 全部放款
        /// </summary>
        public ResultModel PayAllMerchantWithdraw(int adminID)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 5;
                smodel.adminName = adminID.ToString();
                smodel.adminId = adminID;
                smodel.Remark = "Admin[" + adminID + "]" + "执行全部放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                var list = GetMerchantWithdrawList(1);

                if (list == null || list.Count == 0)
                    return new ResultModel(false, "没有满足放款条件的记录");

                var result = PayAllMerchantWithdraw(list, adminID);
                if (result)
                    return new ResultModel(false, "操作失败");
                return new ResultModel(true, "操作成功");
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return new ResultModel(true, "服务异常");
            }
        }
        /// <summary>
        /// 批量/全部放款通用
        /// </summary>
        public bool PayAllMerchantWithdraw(List<MerchantWithdrawModel> list, int adminID)
        {
            var result = true;
            foreach (var item in list)
            {
                if (item.Status != 1)
                    continue;

                var recordID = item.ID;
                var accountID = item.AccountID;
                var amount = item.Amount;

                if (!PayMerchant(recordID, adminID, accountID, amount))
                    result = false;
            }
            return result;
        }
        #endregion
        #endregion
    }
}
