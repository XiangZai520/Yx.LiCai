using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.User;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Server.User
{
    /// <summary>
    /// 用户总账户业务类
    /// </summary>
    public class UserCountService
    {
        UserCountDao dao = new UserCountDao();
        /// <summary>
        /// 根据用户ID获取总资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetZongMoney(long Id)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            try
            {
                result.Result = true;
                result.Data = dao.GetMyMoney(Id);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据用户ID获取总资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetAllMyMoney(long Id)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            try
            {
                result.Result = true;
                result.Data = dao.GetAllMyMoney(Id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取保理userid
        /// </summary>
        /// <param name="Id">用户类型1保理公司</param>
        /// <returns></returns>
        public ResultInfo<long> GetUserIDByUserType(int user_type = 1)
        {
            ResultInfo<long> result = new ResultInfo<long>();
            try
            {
                result.Result = true;
                result.Data = dao.GetUserIDByUserType(user_type);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
                result.Result = false;
            }
            return result;
        }
                /// <summary>
        /// 根据用户id获取账户情况
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ResultInfo<UserAccountEx> GetUserAccountByUserID(long user_id)
        {
            ResultInfo<UserAccountEx> result = new ResultInfo<UserAccountEx>();
            result.Result = false;
            try
            {
                UserAccountEx entity = null;
                DataTable dt = dao.GetUserAccountByUserID(user_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    entity = new UserAccountEx();
                    DataRow dr = dt.Rows[0];
                    entity.user_id = user_id;
                    entity.amount = Convert.ToDecimal(dr["amount_invest"]) + Convert.ToDecimal(dr["amount_blance"]);
                    entity.amount_blance = Convert.ToDecimal(dr["amount_blance"]);
                    entity.amount_blance_fz = Convert.ToDecimal(dr["amount_blance_fz"]);
                    entity.amount_invest = Convert.ToDecimal(dr["amount_invest"]);
                }
                result.Result = true;
                result.Data = entity;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = null;
            }
            return result;
        }
    }
}
