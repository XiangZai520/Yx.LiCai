using System;
using System.Collections.Generic;
using YxLiCai.Dao.Redemption;
using YxLiCai.Model;

namespace YxLiCai.Server.Redemption
{
    /// <summary>
    /// 赎回操作业务类
    /// </summary>
    public class RedemptionService
    {
        DedemptionDao dao = new DedemptionDao();
        /// <summary>
        /// 用户赎回
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="orderinfoids"></param>
        /// <returns></returns>
        public ResultInfo<bool> UserDedemption(long Userid, List<long> orderinfoids)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            try
            {
                result.Data = dao.UserDedemption(Userid, orderinfoids);
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = false;
                result.Result = false;
            }
            return result;
        }
    }
}
