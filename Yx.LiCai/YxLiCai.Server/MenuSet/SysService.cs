using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model;

namespace YxLiCai.Server.MenuSet
{
    /// <summary>
    /// 系统操作类
    /// </summary>
    public class SysService
    {
        //YxLiCai.Dao.MenuSet.SysDao dao = new Dao.MenuSet.SysDao();
        /// <summary>
        /// 插入系统日志 操作类型(0系统1项目2产品3放款4还款5提现6赎回7活动8会员)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResultInfo AddSysLog(SysActionLogModel model)
        {

            ResultInfo result = new ResultInfo(false, "");
            try
            {
                YxLiCai.Dao.MenuSet.SysDao.AddSysLog(model);
                result.Result = true;
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
