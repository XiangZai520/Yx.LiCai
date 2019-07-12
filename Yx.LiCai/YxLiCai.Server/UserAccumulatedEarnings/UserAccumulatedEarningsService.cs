/*
 * 各个账户累计收益业务逻辑层
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using YxLiCai.Dao.UserAccumulatedEarnings;
using YxLiCai.Model;
using YxLiCai.Model.UserAccumulatedEarnings;
using YxLiCai.Tools;

namespace YxLiCai.Server.UserAccumulatedEarnings
{
    /// <summary>
    /// 各个账户累计收益业务逻辑层
    /// </summary>
    public class UserAccumulatedEarningsService
    {
        UserAccumulatedEarningsDao dao = new UserAccumulatedEarningsDao();

        #region 获取用户各个账户累计收益金额
        /// <summary>
        /// 获取用户月账户累计收益金额
        /// </summary>
        /// <param name="userId">用户id</param>  
        /// <returns></returns>
        public decimal GetUserMonth(long userId)
        {
            var balance = dao.GetUserMonth(userId);

            return balance;
        }
        /// <summary>
        /// 获取用户季节账户累计收益金额
        /// </summary>
        /// <param name="userId">用户id</param>  
        /// <returns></returns>
        public decimal GetUserSeason(long userId, string type)
        {
            var balance = dao.GetUserSeason(userId, type);

            return balance;
        }
        /// <summary>
        /// 获取用户年账户累计收益金额
        /// </summary>
        /// <param name="userId">用户id</param>  
        /// <returns></returns>
        public decimal GetUserYear(long userId)
        {
            var balance = dao.GetUserYear(userId);

            return balance;
        }

        #endregion


        #region 获取各个账户的流水收益记录
        /// <summary>
        /// 获取用户月账户流水记录收益
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> GetUserCountMonth_AccumulatedEarnings_List(string strWhere)
        {

            ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<UserCountMonth_AccumulatedEarnings_Model>();
                result.Data = dao.GetUserCountMonth_AccumulatedEarnings_Model(strWhere);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取用户季账户流水记录收益
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> GetUserCountSeason_AccumulatedEarnings_List(string strWhere)
        {

            ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<UserCountSeason_AccumulatedEarnings_Model>();
                result.Data = dao.GetUserCountSeason_AccumulatedEarnings_Model(strWhere);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取用户年账户流水记录收益
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> GetUserCountYear_AccumulatedEarnings_List(string strWhere)
        {

            ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>>();
            result.Result = false;
            result.Data = null;
            try
            {
                result.Data = new List<UserCountYear_AccumulatedEarnings_Model>();
                result.Data = dao.GetUserCountYear_AccumulatedEarnings_Model(strWhere);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        #endregion


        #region 获取各个账户下的分页数据
        /// <summary>
        /// 获取月账户分页数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> GetListOrderByCreateTimeDesc_month(long Id, int CurrentPage, int PageSize)
        {
            ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountMonth_AccumulatedEarnings_Model>>();
            int SCount = (CurrentPage - 1) * PageSize;
            int ECount = PageSize;
            result.Data = dao.GetListOrderByCreateTimeDesc_month(Id, SCount, ECount);
            return result;
        }
        /// <summary>
        /// 获取季节账户分页数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> GetListOrderByCreateTimeDesc_season(long Id, int ProductType, int CurrentPage, int PageSize)
        {
            ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountSeason_AccumulatedEarnings_Model>>();
            int SCount = (CurrentPage - 1) * PageSize;
            int ECount = PageSize;
            result.Data = dao.GetListOrderByCreateTimeDesc_season(Id, ProductType, SCount, ECount);
            return result;
        }
        /// <summary>
        /// 获取年账户分页数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> GetListOrderByCreateTimeDesc_year(long Id, int CurrentPage, int PageSize)
        {
            ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>> result = new ResultInfo<List<UserCountYear_AccumulatedEarnings_Model>>();
            int SCount = (CurrentPage - 1) * PageSize;
            int ECount = PageSize;
            result.Data = dao.GetListOrderByCreateTimeDesc_year(Id, SCount, ECount);
            return result;
        }
        #endregion


        #region 获取各个账户下的记录总数
        /// <summary>
        /// 获取月账户的记录总数
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public int GetTotalCount_month(long Userid)
        {
            int TotalCount = 0;
            TotalCount = dao.GetTotalCount_month(Userid);
            return TotalCount;
        }
        /// <summary>
        /// 获取季账户的记录总数
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public int GetTotalCount_season(long Userid, int type)
        {
            int TotalCount = 0;
            TotalCount = dao.GetTotalCount_season(Userid, type);
            return TotalCount;
        }
        /// <summary>
        /// 获取年账户的记录总数
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public int GetTotalCount_year(long Userid)
        {
            int TotalCount = 0;
            TotalCount = dao.GetTotalCount_year(Userid);
            return TotalCount;
        }
        #endregion
    }
}
