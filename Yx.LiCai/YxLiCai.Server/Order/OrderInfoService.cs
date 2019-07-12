using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Server.Order
{
    /// <summary>
    /// 订单业务类 2015.6.24 张世晓
    /// </summary>
    public class OrderInfoService
    {
        YxLiCai.Dao.Order.OrderInfoDao dao = new Dao.Order.OrderInfoDao();
        /// <summary>
        /// 自有资产列表（分页）
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">页容量</param>
        /// <returns></returns>
        public ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> GetList(long Id, int ProductType, int CurrentPage, int PageSize)
        {
            ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>> result = new ResultInfo<List<YxLiCai.Model.ExtendModel.UserAssetsEx>>();
            int SCount = (CurrentPage - 1) * PageSize;
            int ECount = PageSize;
            try
            {
                result.Data = dao.GetListOrderByCreateTimeDesc(Id, ProductType, SCount, ECount);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
            }
            return result;
        }
        /// <summary>
        /// 自有资产总数
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品分类</param>
        /// <returns></returns>
        public ResultInfo<int> GetTotalCount(long Id, int ProductType)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            try
            {
                int TotalCount = 0;
                TotalCount = dao.GetTotalCount(Id, ProductType);
                result.Data = TotalCount;
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
            }
            return result;
        }
        /// <summary>
        /// 自有资产总数
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品分类</param>
        /// <returns></returns>
        public ResultInfo<int> GetTotalCount(long Id, List<int> ProductType)
        {
            ResultInfo<int> result = new ResultInfo<int>();

            int TotalCount = 0;
            try
            {
                TotalCount = dao.GetTotalCount(Id, ProductType);
                result.Data = TotalCount;
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
            }
            return result;
        }
        /// <summary>
        /// 根据订单ID、用户ID 获取实体
        /// </summary>
        /// <param name="Id">订单id</param>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public ResultInfo<YxLiCai.Model.Order.order_info> GetEntityByUserIDAndID(long Id, long UserID)
        {
            ResultInfo<YxLiCai.Model.Order.order_info> result = new ResultInfo<Model.Order.order_info>();
            try
            {
                result.Data= dao.GetEntityByUserIDAndID(Id, UserID);
                result.Result = true;
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <param name="ProductType">产品类型</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">页容量</param>
        /// <returns></returns>
        public ResultInfo<List<YxLiCai.Model.Order.order_info>> GetList(long Id, List<int> ProductType, int CurrentPage, int PageSize)
        {
            ResultInfo<List<YxLiCai.Model.Order.order_info>> result = new ResultInfo<List<YxLiCai.Model.Order.order_info>>();
            int SCount = (CurrentPage - 1) * PageSize;
            int ECount = PageSize;
            try
            {
                result.Data = dao.GetListOrderByCreateTimeDesc(Id, ProductType, SCount, ECount);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
            }
            return result;
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="OrderInfoIDs">订单id</param>
        /// <returns></returns>
        public ResultInfo<List<YxLiCai.Model.Order.order_info>> GetListOrderByIDSAndUserID(long UserId, List<long> OrderInfoIDs)
        {
            ResultInfo<List<YxLiCai.Model.Order.order_info>> result = new ResultInfo<List<Model.Order.order_info>>();
            try
            {
                result.Data= dao.GetListOrderByIDSAndUserID(UserId, OrderInfoIDs);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
            }
            return result;
        }
        /// <summary>
        /// 更新订单表 赎回状态为订单赎回申请中
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="ids">订单id</param>
        public ResultInfo UpdateAtoneStatus(long userID, List<long> ids)
        {
            ResultInfo result = new ResultInfo(true,"");
            try
            {
                dao.UpdateAtoneStatus(userID, ids);
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 获取会员投资记录
        /// </summary>
        /// <param name="UserId">会员id</param>
        /// <param name="LoginName">登录名</param>
        /// <returns></returns>
        public ResultInfo<List<PurchaseRecordEx>> GetMemberPurchaseRecord(long UserId)
        {
            ResultInfo<List<PurchaseRecordEx>> result = new ResultInfo<List<PurchaseRecordEx>>();
            try
            {
                result.Data = dao.GetMemberPurchaseRecord(UserId);
                result.Result = true;

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
