using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.User;
using YxLiCai.Model;
using YxLiCai.Model.User;

namespace YxLiCai.Server.User
{
    public class UserAddInterestService
    {
        UserAddInterestDao dao = new UserAddInterestDao();
        /// <summary>
        /// 增加一条用户加息券数据 by张浩然 2015年6月30日
        /// </summary>
        public ResultInfo<bool> AddUserAddInterest(UserAddInterestModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.AddUserAddInterest(model);
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
        /// 根据邀请人编号和活动编号获取邀请人参与加息券活动的次数 by 张浩然 2015-7-8
        /// </summary>
        /// <param name="userId">邀请人编号</param>
        /// <param name="actId">活动编号</param>
        /// <returns></returns>
        public ResultInfo<int> GetUserAddInterestCountByInvitedUserIdAndActId(long userId, int actId)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Data = 0;
            result.Result = false;
            try
            {
                result.Data = dao.GetUserAddInterestCountByInvitedUserIdAndActId(userId, actId);
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

    }
}
