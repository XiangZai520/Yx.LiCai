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
    /// 用户季账户业务类 张世晓
    /// </summary>
    public class UserCountSeasonService
    {
        UserCountSeasonDao dao = new UserCountSeasonDao();
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
                result.Data = dao.GetMyMoney(Id);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = 0;
            }
            return result;
        }
        /// <summary>
        /// 更新赎回账户
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="LockMoney">冻结金额</param>
        public void AddLockMoney(long UserId, decimal LockMoney, decimal order_investment)
        {
            try
            {
                dao.AddLockMoney(UserId, LockMoney, order_investment);
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
            }
        }
    }
}
