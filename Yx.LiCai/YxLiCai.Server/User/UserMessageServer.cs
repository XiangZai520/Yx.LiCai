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
    /// 消息操作类
    /// </summary>
    public class UserMessageServer
    {
        UserMessageDao dao = new UserMessageDao();

        /// <summary>
        /// 读取消息内容 by平扬 2015年7月2日
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="start"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ResultInfo<List<UserMessageModel>> GetUserMessage(long user_id, int page, int pagesize, ref int count)
        {
            ResultInfo<List<UserMessageModel>> result = new ResultInfo<List<UserMessageModel>>();
            result.Result = false;
            result.Data = null;
            try
            {
                int start = (page - 1) * pagesize;
                result.Result = true;
                result.Data = dao.GetUserMessage(user_id, start, pagesize, ref count);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 更新消息状态
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateUserMessage(long user_id, int id)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            {
                result.Result = true;
                result.Data = dao.UpdateUserMessage(user_id,id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultInfo<bool> AddUserMessage(UserMessageModel model)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            result.Data = false;
            try
            { 
                result.Result = true;
                result.Data = dao.AddUserMessage(model);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

       /// <summary>
       /// 消息未读数量
       /// </summary>
       /// <param name="user_id"></param>
       /// <returns></returns>
        public ResultInfo<int> GetUserMessageCount(long user_id)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Result = true;
                result.Data = dao.GetUserMessageCount(user_id);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
    }
}
