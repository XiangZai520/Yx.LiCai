using System;
using System.Collections.Generic;
using System.Data;
using YxLiCai.Dao.ActivityManege;
using YxLiCai.Model;
using YxLiCai.Model.ActivityManage;
using YxLiCai.Model.ExtendModel;
using System.Linq;
using YxLiCai.Model.Plat;
using YxLiCai.Tools.Const;

namespace YxLiCai.Server.ActivityManege
{
    /// <summary>
    /// 活动管理业务层
    /// </summary>
    public class ActivityManegeService
    {
        ActivityManegeDao dao = new ActivityManegeDao();
        #region 加息券
        /// <summary>
        /// 新增活动加息券
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">有效期</param>
        /// <param name="usecondition">使用条件</param>
        /// <param name="creator_id">新增人id</param>
        /// <param name="c_time">新增时间</param>
        /// <returns></returns>
        public ResultInfo<bool> InsertActInterestCoupon(string name, decimal amount, int enableday, string usecondition, int creator_id, DateTime c_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                amount = amount / 100;
                result.Data = dao.InsertActInterestCoupon(name, amount, enableday, usecondition, creator_id, c_time);
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
        /// 获取加息券列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public ResultInfo<List<Model.ActivityManage.ActInterestCoupon>> GetInterestCouponList(string name, decimal amount, DateTime s_ctime, DateTime e_ctime, int status, int CurrentPage, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<Model.ActivityManage.ActInterestCoupon>> result = new ResultInfo<List<Model.ActivityManage.ActInterestCoupon>>();
            try
            {
                int SCount = PageSize * (CurrentPage - 1);
                List<ActInterestCoupon> list = new List<ActInterestCoupon>();
                ActInterestCoupon item;
                DataSet ds = dao.GetInterestCouponList(name, amount, s_ctime, e_ctime, status, SCount, PageSize);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new ActInterestCoupon();
                            //a.id,a.name,a.interest_rate,a.use_condition,a.c_time,a.enable_day,a.status
                            item.id = Convert.ToInt32(dr["id"]);
                            item.name = Convert.ToString(dr["name"]);
                            item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);
                            item.interest_rate = item.interest_rate * 100;
                            item.use_condition = Convert.ToString(dr["use_condition"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.enable_day = Convert.ToInt32(dr["enable_day"]);
                            item.status = Convert.ToInt32(dr["status"]);
                            item.use_count = Convert.ToInt32(dr["use_count"]);
                            list.Add(item);
                        }
                    }
                }

                result.Data = list;
                result.Result = true;
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
        /// 获取加息券实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ResultInfo<ActInterestCoupon> GetActInterestCouponEntity(int id)
        {
            ResultInfo<ActInterestCoupon> result = new ResultInfo<ActInterestCoupon>();
            ActInterestCoupon item = null;
            try
            {
                DataSet ds = dao.GetActInterestCouponEntity(id);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        item = new ActInterestCoupon();
                        //a.id,a.status,a.name,a.interest_rate,a.enable_day,a.use_condition,a.c_time,
                        //a.creator_id,a.m_time,a.m_id,a.is_delete
                        item.id = Convert.ToInt32(dr["id"]);
                        item.name = Convert.ToString(dr["name"]);
                        item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);
                        item.interest_rate = item.interest_rate * 100;
                        item.use_condition = Convert.ToString(dr["use_condition"]);
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.enable_day = Convert.ToInt32(dr["enable_day"]);
                        item.status = Convert.ToInt32(dr["status"]);

                        item.creator_id = Convert.ToInt32(dr["creator_id"]);
                        item.m_time = Convert.ToDateTime(dr["m_time"] == DBNull.Value ? "1900-01-01" : dr["m_time"]);
                        item.m_id = Convert.ToInt32(dr["m_id"] == DBNull.Value ? 0 : dr["m_id"]);
                        item.is_delete = Convert.ToInt32(dr["is_delete"]);
                        item.remark = Convert.ToString(dr["remark"]);
                    }
                }

                result.Data = item;
                result.Result = true;
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
        /// 修改活动加息券
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">有效期</param>
        /// <param name="usecondition">使用条件</param>
        /// <param name="creator_id">新增人id</param>
        /// <param name="c_time">新增时间</param>
        /// <returns></returns>
        public ResultInfo<bool> ModifyActInterestCoupon(int id, string name, decimal amount, int enableday, string usecondition, int m_id, DateTime m_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                amount = amount / 100;
                result.Data = dao.ModifyActInterestCoupon(id, name, amount, enableday, usecondition, m_id, m_time);
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
        /// 更新加息券状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateInterestCouponStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateInterestCouponStatus(id, status, m_id, m_time, remark);
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
        /// 删除加息券
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> DelInterestCoupon(int id, int m_id, DateTime m_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.DelInterestCoupon(id, m_id, m_time);
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
        /// 获取加息券发放使用明细
        /// </summary>
        /// <param name="name">特享金名称</param>
        /// <param name="user_id">用户id</param>
        /// <param name="s_ctime">开始时间</param>
        /// <param name="e_ctime">结束时间</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        public ResultInfo<List<InterestCouponSendDetailEx>> InterestCouponSendDetail(string name, long user_id, int use_status, DateTime s_ctime, DateTime e_ctime, int CurrentPage, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<InterestCouponSendDetailEx>> result = new ResultInfo<List<InterestCouponSendDetailEx>>();
            try
            {
                int SCount = PageSize * (CurrentPage - 1);
                List<InterestCouponSendDetailEx> list = new List<InterestCouponSendDetailEx>();
                InterestCouponSendDetailEx item;
                DataSet ds = dao.InterestCouponSendDetail(name, user_id, use_status, s_ctime, e_ctime, SCount, PageSize);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new InterestCouponSendDetailEx();
                            //a.user_id,b.name,b.interest_rate,a.c_time,a.enable_day,a.use_status,c.pdt_type,c.c_time
                            item.user_id = Convert.ToInt64(dr["user_id"]);
                            item.name = Convert.ToString(dr["name"]);
                            item.interest_rate = Convert.ToDecimal(dr["interest_rate"]);
                            item.interest_rate = item.interest_rate * 100;
                            item.use_status = Convert.ToInt32(dr["use_status"]);
                            item.enable_day = Convert.ToInt32(dr["enable_day"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.e_time = item.c_time.AddDays(item.enable_day);
                            item.pdt_type = Convert.ToInt32(dr["pdt_type"] == DBNull.Value ? "0" : dr["pdt_type"]);
                            item.use_date = Convert.ToDateTime(dr["use_date"] == DBNull.Value ? "1900-01-01" : dr["use_date"]);
                            list.Add(item);
                        }
                    }
                }

                result.Data = list;
                result.Result = true;
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
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<int> GetInterestCouponCountByName(string name, int id = -1)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetInterestCouponCountByName(name, id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = -1;
            }
            return result;
        }
        #endregion
        #region 特享金

        /// <summary>
        /// 新增活动特享金
        /// </summary>
        /// <param name="name">名次</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">使用期限</param>
        /// <param name="rate">利率</param>
        /// <param name="rate_increase">递增利率</param>
        /// <param name="creator_id">创建人ID</param>
        /// <param name="c_time">创建时间</param>
        /// <returns></returns>
        public ResultInfo<bool> InsertActSpecialAssets(string name, decimal amount, int enableday, decimal rate, decimal rate_increase, int creator_id, DateTime c_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                rate = rate / 100;
                rate_increase = rate_increase / 100;
                result.Data = dao.InsertActSpecialAssets(name, amount, enableday, rate, rate_increase, creator_id, c_time);
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
        /// 获取特享金列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public ResultInfo<List<Model.ActivityManage.ActSpecialAssets>> GetSpecialAssetsList(string name, decimal amount, DateTime s_ctime, DateTime e_ctime, int status, int CurrentPage, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<Model.ActivityManage.ActSpecialAssets>> result = new ResultInfo<List<Model.ActivityManage.ActSpecialAssets>>();
            try
            {
                int SCount = PageSize * (CurrentPage - 1);
                List<ActSpecialAssets> list = new List<ActSpecialAssets>();
                ActSpecialAssets item;
                DataSet ds = dao.GetSpecialAssetsList(name, amount, s_ctime, e_ctime, status, SCount, PageSize);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new ActSpecialAssets();
                            //a.id,a.name,a.interest_rate,a.use_condition,a.c_time,a.enable_day,a.status
                            item.id = Convert.ToInt32(dr["id"]);
                            item.name = Convert.ToString(dr["name"]);
                            item.amount = Convert.ToDecimal(dr["amount"]);
                            item.rate = Convert.ToDecimal(dr["rate"]);
                            item.rate = item.rate * 100;
                            item.rate_increase = Convert.ToDecimal(dr["rate_increase"]);
                            item.rate_increase = item.rate_increase * 100;
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.enable_day = Convert.ToInt32(dr["enable_day"]);
                            item.status = Convert.ToInt32(dr["status"]);
                            item.send_count = Convert.ToInt32(dr["send_count"]);
                            list.Add(item);
                        }
                    }
                }

                result.Data = list;
                result.Result = true;
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
        /// 获取特享金发放明细
        /// </summary>
        /// <param name="name">特享金名称</param>
        /// <param name="user_id">用户id</param>
        /// <param name="s_ctime">开始时间</param>
        /// <param name="e_ctime">结束时间</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        public ResultInfo<List<SpecialAssetsSendDetailEx>> SpecialAssetsSendDetail(string name, long user_id, DateTime s_ctime, DateTime e_ctime, int CurrentPage, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<SpecialAssetsSendDetailEx>> result = new ResultInfo<List<SpecialAssetsSendDetailEx>>();
            try
            {
                int SCount = PageSize * (CurrentPage - 1);
                List<SpecialAssetsSendDetailEx> list = new List<SpecialAssetsSendDetailEx>();
                SpecialAssetsSendDetailEx item;
                DataSet ds = dao.SpecialAssetsSendDetail(name, user_id, s_ctime, e_ctime, SCount, PageSize);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new SpecialAssetsSendDetailEx();
                            //a.user_id, c.name,a.amount,b.type,c.rate,c.enable_day,a.c_time.
                            item.user_id = Convert.ToInt64(dr["user_id"]);
                            item.name = Convert.ToString(dr["name"]);
                            item.amount = Convert.ToDecimal(dr["amount"]);
                            item.type = Convert.ToInt32(dr["type"]);
                            item.rate = Convert.ToDecimal(dr["rate"]);
                            item.rate = item.rate * 100;
                            item.enable_day = Convert.ToInt32(dr["enable_day"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.e_time = item.c_time.AddDays(item.enable_day);
                            list.Add(item);
                        }
                    }
                }

                result.Data = list;
                result.Result = true;
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
        /// 获取特享金实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ResultInfo<ActSpecialAssets> GetActSpecialAssetsEntity(int id)
        {
            ResultInfo<ActSpecialAssets> result = new ResultInfo<ActSpecialAssets>();
            ActSpecialAssets item = null;
            try
            {
                DataSet ds = dao.GetActSpecialAssetsEntity(id);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        item = new ActSpecialAssets();
                        //a.id,a.status,a.name,a.interest_rate,a.enable_day,a.use_condition,a.c_time,
                        //a.creator_id,a.m_time,a.m_id,a.is_delete
                        item.id = Convert.ToInt32(dr["id"]);
                        item.name = Convert.ToString(dr["name"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        item.rate = Convert.ToDecimal(dr["rate"]);
                        item.rate = item.rate * 100;
                        item.rate_increase = Convert.ToDecimal(dr["rate_increase"]);
                        item.rate_increase = item.rate_increase * 100;
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.enable_day = Convert.ToInt32(dr["enable_day"]);
                        item.status = Convert.ToInt32(dr["status"]);

                        item.creator_id = Convert.ToInt32(dr["creator_id"]);
                        item.m_time = Convert.ToDateTime(dr["m_time"] == DBNull.Value ? "1900-01-01" : dr["m_time"]);
                        item.m_id = Convert.ToInt32(dr["m_id"]);
                        item.is_delete = Convert.ToInt32(dr["is_delete"]);
                        item.remark = Convert.ToString(dr["remark"]);
                    }
                }

                result.Data = item;
                result.Result = true;
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
        /// 修改活动特享金
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">使用期限</param>
        /// <param name="rate">收益</param>
        /// <param name="rate_increase">递增收益</param>
        /// <param name="m_id">修改人id</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> ModifyActSpecialAssets(int id, string name, decimal amount, int enableday, decimal rate, decimal rate_increase, int m_id, DateTime m_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                rate = rate / 100;
                rate_increase = rate_increase / 100;
                result.Data = dao.ModifyActSpecialAssets(id, name, amount, enableday, rate, rate_increase, m_id, m_time);
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
        /// 更新特享金状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateSpecialAssetsStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateSpecialAssetsStatus(id, status, m_id, m_time, remark);
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
        /// 删除特享金
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> DelSpecialAssets(int id, int m_id, DateTime m_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.DelSpecialAssets(id, m_id, m_time);
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
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<int> GetSpecialAssetsCountByName(string name, int id = -1)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetSpecialAssetsCountByName(name, id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = -1;
            }
            return result;
        }
        #endregion
        #region 活动
        /// <summary>
        /// 获取所有的 特享金、加息券----审核过的
        /// </summary>
        /// <returns></returns>
        public ResultInfo<List<ActPromotionItem>> GetAllICAndSA()
        {
            ResultInfo<List<ActPromotionItem>> result = new ResultInfo<List<ActPromotionItem>>();
            result.Result = false;
            try
            {
                DataSet ds = dao.GetAllICAndSA();
                List<ActPromotionItem> list = new List<ActPromotionItem>();
                ActPromotionItem item;
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new ActPromotionItem();
                            item.item_type = Convert.ToInt32(dr["item_type"]);
                            item.item_id = Convert.ToInt32(dr["item_id"]);
                            item.item_name = Convert.ToString(dr["item_name"]);
                            item.Amount = SystemConst.MoenyConvert(
                                YxLiCai.Tools.Util.ParseHelper.ToDecimal(dr["amount"]));
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
        /// 新增活动(主表)
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型0注册1邀请2购买。。</param>
        /// <param name="max_user">参与人上限</param>
        /// <param name="limit_num">邀请活动限制（0无限制)</param>
        /// <param name="budget">预算</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="ad_content">广告语</param>
        /// <param name="url">链接</param>
        /// <param name="c_time">创建日期</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public ResultInfo<int> InsertActPromotion(string name, int type, int max_user_num, int limit_num, decimal budget, DateTime s_time, DateTime e_time, string ad_content, string url, DateTime c_time, int creator_id)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;

                result.Data = dao.InsertActPromotion(name, type, max_user_num, limit_num, budget, s_time, e_time, ad_content, url, c_time, creator_id);
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
        /// 新增活动(子表)
        /// </summary>
        /// <param name="act_id">活动id</param>
        /// <param name="item_type">项目类型</param>
        /// <param name="item_id">项目id</param>
        /// <param name="item_name">项目名称</param>
        /// <param name="item_qty">数量</param>
        /// <param name="c_time">创建时间</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public ResultInfo<bool> InsertActPromotionItem(int act_id, int item_type, int item_id, string item_name, int item_qty, DateTime c_time, int creator_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.InsertActPromotionItem(act_id, item_type, item_id, item_name, item_qty, c_time, creator_id);
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
        /// 获取活动列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public ResultInfo<List<Model.ActivityManage.ActPromotion>> GetActivityList(string name, int type, DateTime s_ctime, DateTime e_ctime, int status, int CurrentPage, int PageSize, int IsAct, out int TotalCount)
        {
            TotalCount = 0;
            ResultInfo<List<Model.ActivityManage.ActPromotion>> result = new ResultInfo<List<Model.ActivityManage.ActPromotion>>();
            try
            {
                int SCount = PageSize * (CurrentPage - 1);
                List<ActPromotion> list = new List<ActPromotion>();
                ActPromotion item;
                DataSet ds = dao.GetActivityList(name, type, s_ctime, e_ctime, status, SCount, PageSize, IsAct);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    TotalCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = new ActPromotion();
                            //a.id,a.name,a.type,a.limit_num,a.ad_content,a.c_time,a.max_user_num,a.curt_user_num,a.url,a.budget,a.s_time,a.e_time,a.status
                            item.id = Convert.ToInt32(dr["id"]);
                            item.name = Convert.ToString(dr["name"]);
                            item.type = Convert.ToInt32(dr["type"]);
                            item.limit_num = Convert.ToInt32(dr["limit_num"]);
                            item.ad_content = Convert.ToString(dr["ad_content"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.max_user_num = Convert.ToInt32(dr["max_user_num"]);
                            item.curt_user_num = Convert.ToInt32(dr["curt_user_num"]);
                            item.url = Convert.ToString(dr["url"]);
                            item.budget = Convert.ToDecimal(dr["budget"]);
                            item.s_time = Convert.ToDateTime(dr["s_time"]);
                            item.e_time = Convert.ToDateTime(dr["e_time"]);
                            item.status = Convert.ToInt32(dr["status"]);
                            list.Add(item);
                        }
                    }
                }

                result.Data = list;
                result.Result = true;
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
        /// 根据主键获取活动实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo<ActPromotion> GetActPromotionEntityByID(int id)
        {
            ResultInfo<ActPromotion> result = new ResultInfo<ActPromotion>();
            ActPromotion item = null;
            try
            {
                DataSet ds = dao.GetActPromotionEntityByID(id);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        item = new ActPromotion();
                        item.id = Convert.ToInt32(dr["id"]);
                        item.name = Convert.ToString(dr["name"]);
                        item.type = Convert.ToInt32(dr["type"]);
                        item.limit_num = Convert.ToInt32(dr["limit_num"]);
                        item.ad_content = Convert.ToString(dr["ad_content"]);
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.max_user_num = Convert.ToInt32(dr["max_user_num"]);
                        item.curt_user_num = Convert.ToInt32(dr["curt_user_num"]);
                        item.url = Convert.ToString(dr["url"]);
                        item.budget = Convert.ToDecimal(dr["budget"]);
                        item.s_time = Convert.ToDateTime(dr["s_time"]);
                        item.e_time = Convert.ToDateTime(dr["e_time"]);
                        item.status = Convert.ToInt32(dr["status"]);
                        item.remark = Convert.ToString(dr["remark"]);
                    }
                }

                result.Data = item;
                result.Result = true;
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
        /// 获取子活动（根据活动id）
        /// </summary>
        /// <returns></returns>
        public ResultInfo<List<ActPromotionItem>> GetActPromotionItemByActID(int act_id)
        {
            ResultInfo<List<ActPromotionItem>> result = new ResultInfo<List<ActPromotionItem>>();
            result.Result = false;
            try
            {
                DataSet ds = dao.GetActPromotionItemByActID(act_id);
                List<ActPromotionItem> list = new List<ActPromotionItem>();
                ActPromotionItem item;
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //a.id,a.act_id,a.item_type,a.item_id,a.item_name,a.item_qty,a.c_time,a.creator_id
                            item = new ActPromotionItem();
                            item.item_type = Convert.ToInt32(dr["item_type"]);
                            item.item_id = Convert.ToInt32(dr["item_id"]);
                            item.item_name = Convert.ToString(dr["item_name"]);
                            item.id = Convert.ToInt32(dr["id"]);
                            item.act_id = Convert.ToInt32(dr["act_id"]);
                            item.item_qty = Convert.ToInt32(dr["item_qty"]);
                            item.c_time = Convert.ToDateTime(dr["c_time"]);
                            item.creator_id = Convert.ToInt32(dr["creator_id"]);
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
        /// 修改活动(主表)
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型0注册1邀请2购买。。</param>
        /// <param name="max_user">参与人上限</param>
        /// <param name="limit_num">邀请活动限制（0无限制)</param>
        /// <param name="budget">预算</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="ad_content">广告语</param>
        /// <param name="url">链接</param>
        /// <param name="c_time">创建日期</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public ResultInfo<bool> ModifyActPromotion(int id, string name, int type, int max_user_num, int limit_num, decimal budget, DateTime s_time, DateTime e_time, string ad_content, string url, DateTime m_time, int m_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;

                result.Data = dao.ModifyActPromotion(id, name, type, max_user_num, limit_num, budget, s_time, e_time, ad_content, url, m_time, m_id);
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
        /// 删除已有子活动
        /// </summary>
        /// <param name="act_id"></param>
        /// <returns></returns>
        public ResultInfo<bool> DelActPromotionItemByActID(int act_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.DelActPromotionItemByActID(act_id);
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
        /// 更新活动状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateActivityStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateActivityStatus(id, status, m_id, m_time, remark);
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
        /// 删除活动
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public ResultInfo<bool> DelActivity(int id, int m_id, DateTime m_time)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.DelActivity(id, m_id, m_time);
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
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultInfo<int> GetActivityCountByName(string name, int id = -1)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetActivityCountByName(name, id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = -1;
            }
            return result;
        }

        /// <summary>
        /// 同类型活动同时间段重叠存在数量
        /// </summary>
        /// <param name="s_time"></param>
        /// <param name="e_time"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResultInfo<int> GetActivityCountByTypeAndTime(DateTime s_time, DateTime e_time, int type, int id)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetActivityCountByTypeAndTime(s_time, e_time, type, id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = -1;
            }
            return result;
        }

        #endregion

        #region 前台
        /// <summary>
        /// 根据加息券编号更新发放数量 by张浩然 2015年6月30日
        /// </summary>
        /// <param name="id">加息券编号</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateActCouponSendCount(int id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateActCouponSendCount(id);
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
        /// 根据特享金编号更新发放数量 by张浩然 2015年6月30日
        /// </summary>
        /// <param name="id">特享金编号</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateActSpecialAssetsSendCount(int id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateActSpecialAssetsSendCount(id);
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
        /// 根据活动编号更新活动发放次数
        /// </summary>
        /// <param name="act_id">活动编号</param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateActSendCount(int item_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateActSendCount(item_id);
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

        #region 活动充值
        /// <summary>
        /// 根据类型获取平台账户
        /// </summary>
        /// <param name="type">账户类型(1利息账户2活动账户3违约金账户)</param>
        /// <returns></returns>
        public ResultInfo<PlatAccount> GetPlatAccountByType(int type)
        {
            ResultInfo<PlatAccount> result = new ResultInfo<PlatAccount>();
            PlatAccount item = null;
            try
            {
                DataTable dt = dao.GetPlatAccountByType(type);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    item = new PlatAccount();
                    //id,c_time,type,name,amount,amount_recharge,amount_paid,base_amount
                    item.id = Convert.ToInt32(dr["id"]);
                    item.c_time = Convert.ToDateTime(dr["c_time"]);
                    item.name = Convert.ToString(dr["name"]);
                    item.type = Convert.ToInt32(dr["type"]);
                    item.amount = Convert.ToDecimal(dr["amount"]);
                    item.amount_recharge = Convert.ToDecimal(dr["amount_recharge"]);
                    item.amount_paid = Convert.ToDecimal(dr["amount_paid"]);
                    item.base_amount = Convert.ToDecimal(dr["base_amount"]);
                }
                result.Data = item;
                result.Result = true;
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
        /// 活动充值
        /// </summary>
        /// <param name="plataccountid"></param>
        /// <param name="c_time"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public ResultInfo<bool> ActRecharge(int plataccountid, DateTime c_time, decimal amount, int creator_id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            try
            {
                result.Result = true;
                result.Data = dao.ActRecharge(plataccountid, c_time, amount, creator_id);
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
        /// 活动充值记录
        /// </summary>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <returns></returns>
        public ResultInfo<List<ActRechargeRecEx>> ActRechargeRec(DateTime s_ctime, DateTime e_ctime)
        {
            ResultInfo<List<ActRechargeRecEx>> result = new ResultInfo<List<ActRechargeRecEx>>();
            try
            {
                List<ActRechargeRecEx> list = new List<ActRechargeRecEx>();
                ActRechargeRecEx item;
                DataTable dt = dao.ActRechargeRec(s_ctime, e_ctime);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new ActRechargeRecEx();
                        item.c_time = Convert.ToDateTime(dr["c_time"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        item.from_id = Convert.ToInt32(dr["from_id"]);
                        list.Add(item);
                    }
                }
                result.Data = list;
                result.Result = true;
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
        /// 活动支付记录
        /// </summary>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <returns></returns>
        public ResultInfo<List<ActPaidRecEx>> ActPaidRec(DateTime s_ctime, DateTime e_ctime)
        {
            ResultInfo<List<ActPaidRecEx>> result = new ResultInfo<List<ActPaidRecEx>>();
            try
            {
                List<ActPaidRecEx> list = new List<ActPaidRecEx>();
                ActPaidRecEx item;
                DataTable dt = dao.ActPaidRec(s_ctime, e_ctime);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new ActPaidRecEx();
                        item.type = Convert.ToString(dr["type"]);
                        item.amount = Convert.ToDecimal(dr["amount"]);
                        list.Add(item);
                    }
                }
                result.Data = list;
                result.Result = true;
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
    }
}
