using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.Act;
using YxLiCai.Model;
using YxLiCai.Model.ActivityManage;

namespace YxLiCai.Server.Act
{
    public class ActService
    {
        ActDao dao = new ActDao();

        /// <summary>
        /// 根据活动类型判断是否存在活动 by 张浩然 2015-6-30
        /// </summary>
        /// <param name="type">活动类型(0注册1邀请2购买)</param>
        /// <returns></returns>
        public ResultInfo<int> IsAct(int type)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.IsAct(type);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = 0;
            }
            return result;
        }

        /// <summary>
        /// 根据活动类型和规则类型获取活动信息列表 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="type">活动类型（0注册1邀请2购买）</param>
        /// <param name="itemType">规则类型（1特享金2加息券）</param>
        /// <returns></returns>
        public ResultInfo<ActPromotionModelView> ActInfoByTypeAndItemType(int type, int itemType) 
        {
            ResultInfo<ActPromotionModelView> result = new ResultInfo<ActPromotionModelView>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.ActInfoByTypeAndItemType(type,itemType);
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
        /// 根据规则类型编号获取加息券规则信息 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="itemId">加息券规则编号</param>
        /// <returns></returns>
        public ResultInfo<ActInterestCoupon> GetCouponByItemId(int itemId)
        {
            ResultInfo<ActInterestCoupon> result = new ResultInfo<ActInterestCoupon>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetCouponByItemId(itemId);
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
        /// 根据规则类型编号获取特享金规则信息 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="itemId">特享金规则编号</param>
        /// <returns></returns>
        public ResultInfo<ActSpecialAssets> GetAssetsByItemId(int itemId) 
        {
            ResultInfo<ActSpecialAssets> result = new ResultInfo<ActSpecialAssets>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetAssetsByItemId(itemId);
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
        /// 根据邀请人编号和活动编号获取邀请的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        public ResultInfo<int> GetInviteCountByInvitedUserIdAndActId(long userId, int actId)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            int TotalCount = 0;
            result.Result = false;
            try
            {
                TotalCount = dao.GetInviteCountByInvitedUserIdAndActId(userId, actId);
                result.Data = TotalCount;
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = 0;
            }
            return result;
        }

        /// <summary>
        /// 根据活动编号更新参与人数（前台） by张浩然 2015年6月30日
        /// </summary>
        /// <param name="id">特享金编号</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateCurtUserNumByActId(int actId)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateCurtUserNumByActId(actId);
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
        /// 新增一条用户活动日志 by张浩然 2015年7月8日
        /// </summary>
        public ResultInfo<bool> AddActUserLog(ActUserLog model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.AddActUserLog(model);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = false;
            }
            return result;
        }

    }
}
