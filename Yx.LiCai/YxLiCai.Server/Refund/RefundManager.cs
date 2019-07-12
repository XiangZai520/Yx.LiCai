using System;
using System.Collections.Generic;
using YxLiCai.Tools;
using YxLiCai.Model.Refund;
using YxLiCai.Model;
using YxLiCai.Server.MenuSet;

namespace YxLiCai.Server.Refund
{
    /// <summary>
    /// 还款业务处理类
    /// </summary>
    public class RefundManager
    {
        private YxLiCai.Dao.Refund.RefundDAO _dao = new Dao.Refund.RefundDAO();
        #region Get
        /// <summary>
        /// 还款利息记录列表
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns></returns>
        public List<RefundModelExtend> GetRefundInterestList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
            int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<RefundModelExtend>();

            try
            {
                var ds = _dao.GetRefundInterestList(merchantName, loanPeriod, amount_min, amount_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataToRefundModel(row as System.Data.DataRow));
                    }
                }
                //记录总数
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
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
        /// 还款本金记录列表
        /// </summary>
        /// <param name="status">还款记录状态：0待处理；1已还款；2通知充值</param>
        /// <param name="merchantName">融资方名称</param>
        /// <param name="loanPeriod">项目期限</param>
        /// <param name="amount_min">项目金额</param>
        /// <param name="amount_max">项目金额</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns></returns>
        public List<RefundModelExtend> GetRefundPrincipalList(string merchantName, int loanPeriod, decimal amount_min, decimal amount_max,
          int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<RefundModelExtend>();

            try
            {
                var ds = _dao.GetRefundPrincipalList(merchantName, loanPeriod, amount_min, amount_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataToRefundModel(row as System.Data.DataRow));
                    }
                }
                //记录总数
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
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
        /// 还款记录 还息+还本金
        /// </summary>
        /// <param name="merchantName"></param>
        /// <param name="loanPeriod"></param>
        /// <param name="amount_min"></param>
        /// <param name="amount_max"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<RefundModelExtend> GetRefundLog(string merchantName, int pjt_id, decimal amount_min, decimal amount_max,
           DateTime? sDate, DateTime? eDate, int type, int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<RefundModelExtend>();

            try
            {
                var ds = _dao.GetRefundLog(merchantName, pjt_id, amount_min, amount_max, sDate, eDate, type, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataToRefundModel(row as System.Data.DataRow));
                    }
                }
                //记录总数
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
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
        #endregion

        #region Update
        /// <summary>
        /// 财务操作 审批通过融资方还款申请
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fer_id"></param>
        /// <param name="amount"></param>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public bool PayPrincipal(long rId, int pjId, int ferId, decimal amount, int adminId)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 4;
                smodel.adminName = adminId.ToString();
                smodel.adminId = adminId;
                smodel.Remark = "Admin[" + adminId + "]" + "执行批量放款操作";
                SysService.AddSysLog(smodel);
                #endregion

                return _dao.PayPrincipal(rId, ferId, amount, pjId, adminId);
            }
             catch(Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
           
        }
        #endregion
    }
}
