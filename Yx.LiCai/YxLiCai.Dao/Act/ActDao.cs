using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.User;
using YxLiCai.Model.ActivityManage;
using YxLiCai.Tools.Util;

namespace YxLiCai.Dao.Act
{
    /// <summary>
    /// 活动管理（前台） by张浩然  2015-6-30 
    /// </summary>
    public class ActDao
    {
        /// <summary>
        /// 根据活动类型判断是否存在活动 by 张浩然 2015-6-30
        /// </summary>
        /// <param name="type">活动类型(0注册1邀请2购买)</param>
        /// <returns></returns>
        public int IsAct(int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id from act_promotion ");
            strSql.Append(" where `status`=1 and s_time<NOW() and e_time>NOW() and type=?type ");
            MySqlParameter[] parameters = {					
                    new MySqlParameter("?type", MySqlDbType.Int32,4)
            };
            parameters[0].Value = type;
            object ob = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据活动类型判断是否存在活动 by 张浩然 2015-6-30", parameters);
            if (ob != null)
            {
                return ParseHelper.ToInt(ob);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据活动类型和规则类型获取活动信息列表 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="type">活动类型（0注册1邀请2购买）</param>
        /// <param name="itemType">规则类型（1特享金2加息券）</param>
        /// <returns></returns>
        public ActPromotionModelView ActInfoByTypeAndItemType(int type, int itemType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.id,b.item_id,a.limit_num from act_promotion_item b inner join act_promotion a ");
            strSql.Append(" on a.id=b.act_id where a.status=1 and a.type=?type and b.item_type=?itemType ");
            strSql.Append(" and CONVERT(a.s_time,CHAR(10))<=CONVERT(NOW(),CHAR(10)) ");
            strSql.Append(" and CONVERT(a.e_time,CHAR(10))>=CONVERT(NOW(),CHAR(10)) order by a.s_time desc limit 0,1 ");

             MySqlParameter[] parameters = {					
                    new MySqlParameter("?type", MySqlDbType.Int32,4),
                    new MySqlParameter("?itemType", MySqlDbType.Int32,4)
            };
            parameters[0].Value = type;
            parameters[1].Value = itemType;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "根据活动类型和规则类型获取活动信息列表 by 张浩然 2015-7-6", parameters);

            IList<ActPromotionModelView> list = ModelConvertHelper<ActPromotionModelView>.ConvertToModel(ds.Tables[0]);
            return list.Count > 0 ? list[0] : null;

        }

        /// <summary>
        /// 根据规则类型编号获取加息券规则信息 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="itemId">加息券规则编号</param>
        /// <returns></returns>
        public ActInterestCoupon GetCouponByItemId(int itemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,interest_rate,enable_day,send_count ");
            strSql.Append(" from act_interest_coupon ");
            strSql.Append(" where is_delete=0 and id=?itemId ");

            MySqlParameter[] parameters = {					
                    new MySqlParameter("?itemId", MySqlDbType.Int32,4)
            };
            parameters[0].Value = itemId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "根据规则类型编号获取加息券规则信息 by 张浩然 2015-7-6", parameters);
            IList<ActInterestCoupon> list = ModelConvertHelper<ActInterestCoupon>.ConvertToModel(ds.Tables[0]);
            return list.Count > 0 ? list[0] : null;

        }

        /// <summary>
        /// 根据规则类型编号获取特享金规则信息 by 张浩然 2015-7-6
        /// </summary>
        /// <param name="itemId">特享金规则编号</param>
        /// <returns></returns>
        public ActSpecialAssets GetAssetsByItemId(int itemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select id,amount,enable_day,rate,rate_increase ");
            strSql.Append(" from act_special_assets ");
            strSql.Append(" where is_delete=0 and id=?itemId ");

            MySqlParameter[] parameters = {					
                    new MySqlParameter("?itemId", MySqlDbType.Int32,4)
            };
            parameters[0].Value = itemId;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "根据规则类型编号获取特享金规则信息 by 张浩然 2015-7-6", parameters);
            IList<ActSpecialAssets> list = ModelConvertHelper<ActSpecialAssets>.ConvertToModel(ds.Tables[0]);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 根据邀请人编号和活动编号获取邀请的数量 by 张浩然 2015-6-30
        /// </summary>
        /// <param name="userId">邀请人编号</param>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public int GetInviteCountByInvitedUserIdAndActId(long userId, int actId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select count(id) as inviteCount ");
            strSql.Append(" from user_invitation ");
            strSql.Append(" where user_id=?userId and act_id=?actId ");

            MySqlParameter[] parameters = {					
                    new MySqlParameter("?userId", MySqlDbType.Int64,20),
                    new MySqlParameter("?actId", MySqlDbType.Int32,11)
            };

            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据邀请人编号和活动编号获取邀请的数量 by 张浩然 2015-6-30", parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return YxLiCai.Tools.Util.ParseHelper.ToInt(obj);
            }
        }

        /// <summary>
        /// 根据活动编号更新参与人数 by张浩然 2015年6月30日
        /// </summary>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public bool  UpdateCurtUserNumByActId(int actId)
        {
            string strSql = @"UPDATE act_promotion SET curt_user_num=curt_user_num+1 WHERE id=?actId;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?actId", MySqlDbType.Int32)
			};
            parameters[0].Value = actId;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "根据活动编号更新参与人数 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 新增一条用户活动日志 by张浩然 2015年7月8日
        /// </summary>
        public bool AddActUserLog(ActUserLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into act_user_log(");
            strSql.Append("user_id,act_id,item_id,c_time,creator_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?act_id,?item_id,?c_time,?creator_id,?version,?remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int32,20),
					new MySqlParameter("?act_id", MySqlDbType.Int32,4),
					new MySqlParameter("?item_id", MySqlDbType.Int32,4),
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,4),
					new MySqlParameter("?version", MySqlDbType.Int32,4),
					new MySqlParameter("?remark", MySqlDbType.VarChar,100)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.act_id;
            parameters[2].Value = model.item_id;
            parameters[3].Value = model.c_time;
            parameters[4].Value = model.creator_id;
            parameters[5].Value = model.version;
            parameters[6].Value = model.remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "新增一条用户活动日志 by张浩然 2015年7月8日", parameters);
        }

    }
}
