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
    /// <summary>
    /// 用户邀请记录 by张浩然 2015年6月30日
    /// </summary>
    public class UserInvitationService
    {
        UserInvitationDao dao = new UserInvitationDao();

        /// <summary>
        /// 新增一条用户邀请数据 by张浩然 2015年6月30日
        /// </summary>
        public ResultInfo<bool> AddUserInvitation(UserInvitationModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.AddUserInvitation(model);
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
        /// 获取后5条邀请记录（前台） by张浩然 2015-7-1
        /// </summary>
        /// <returns></returns>
        public ResultInfo<List<UserInvitationModel>> GetUserInvitationList(long userid) 
        {
            ResultInfo<List<UserInvitationModel>> result = new ResultInfo<List<UserInvitationModel>>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.GetUserInvitationList(userid);
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
