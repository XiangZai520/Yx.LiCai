using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model.TransactionRecords;
using YxLiCai.Tools;

namespace YxLiCai.Server.TransactionRecords
{
    /// <summary>
    /// 用户交易记录业务操作累
    /// </summary>
    public class TransactionRecordsService
    {
        YxLiCai.Dao.TransactionRecords.TransactionRecordsDAO dao = new YxLiCai.Dao.TransactionRecords.TransactionRecordsDAO();
        #region 购买记录
        /// <summary>
        /// 购买记录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="InvestmentProducts">产品类型</param>
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public List<Ord_infoModel> GetBuyLog(string UserName, int InvestmentProducts, DateTime time_min, DateTime time_max,
           int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<Ord_infoModel>();

            try
            {
                var ds = dao.GetBuyLog(UserName, InvestmentProducts, time_min, time_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(dao.DataToRefundModel(row as System.Data.DataRow));
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

        #region 赎回记录
        /// <summary>
        /// 赎回记录
        /// </summary>
        /// <param name="UserName">用户名</param>     
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public List<User_redemptionModel> Getuser_redemptionLog(string UserName, DateTime time_min, DateTime time_max,
           int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<User_redemptionModel>();
            try
            {
                var ds = dao.Getuser_redemptionLog(UserName, time_min, time_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(dao.DataToRefund(row as System.Data.DataRow));
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

        #region 提现记录
        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="UserName">用户名</param>     
        /// <param name="time_min">开始时间</param>
        /// <param name="time_max">结束时间</param>
        /// <param name="startIndex">起始记录数</param>
        /// <param name="pageSize">记录数</param>
        /// <returns></returns>
        public List<User_withdrawModel> Getuser_withdrawLog(string UserName, DateTime time_min, DateTime time_max,
           int startIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            var list = new List<User_withdrawModel>();
            try
            {
                var ds = dao.Getuser_withdrawLog(UserName, time_min, time_max, startIndex, pageSize);
                //列表
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(dao.DataToRefundwithdraw(row as System.Data.DataRow));
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
    }
}
