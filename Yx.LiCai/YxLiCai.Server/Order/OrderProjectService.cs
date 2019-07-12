using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.Order;
using YxLiCai.Model;
using YxLiCai.Model.Order;
using YxLiCai.Model.Project;

namespace YxLiCai.Server.Order
{
    /// <summary>
    /// 订单项目关联业务类 2015.6.24 张世晓
    /// </summary>
    public class OrderProjectService
    {
        OrderProjectDao dao = new OrderProjectDao();
        /// <summary>
        /// 根据订单ID、项目ID获取实体
        /// </summary>
        /// <param name="Orderid">订单id</param>
        /// <param name="projectID">项目id</param>
        /// <returns></returns>
        public ResultInfo<List<ProjectModel>> GetListOrderProduct(int Orderid, long user_id)
        {
            ResultInfo<List<ProjectModel>> result = new ResultInfo<List<ProjectModel>>();
            result.Data = null;
            result.Result = false;
            try
            {
                result.Data = dao.GetListOrderProduct(Orderid, user_id);
                result.Result = true;
            }
            catch(Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
            }
            return result;
        }
        
            
    }
}
