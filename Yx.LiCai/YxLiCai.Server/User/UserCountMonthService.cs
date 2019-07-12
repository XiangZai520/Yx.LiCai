using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.User;
using YxLiCai.Model;

namespace YxLiCai.Server.User
{
    /// <summary>
    /// 用户月账户业务类 2015.6.24 张世晓
    /// </summary>
    public class UserCountMonthService
    {
        UserCountMonthDao dao = new UserCountMonthDao();
        /// <summary>
        /// 根据用户ID获取自有资产
        /// </summary>
        /// <param name="Id">用户id</param>
        /// <returns></returns>
        public ResultInfo<decimal> GetZiMoney(long Id)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            try
            {
                result.Data= dao.GetMyMoney(Id);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
            }
            return result;
        }
    }
}
