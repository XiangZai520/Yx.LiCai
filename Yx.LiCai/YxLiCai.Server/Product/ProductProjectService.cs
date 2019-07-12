using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.Product;
using YxLiCai.Model;
using YxLiCai.Model.Product;

namespace YxLiCai.Server.Product
{
    /// <summary>
    /// 产品项目关联业务类 2015.6.24 张世晓
    /// </summary>
    public class ProductProjectService
    {
        ProductProjectDao dao = new ProductProjectDao();
       /// <summary>
       /// 根据产品ID 获取产品可投项目列表
       /// </summary>
       /// <param name="ProductID">产品id</param>
       /// <returns></returns>
        public ResultInfo<List<Product_Project>> GetListByProductID(int ProductID)
        {
            ResultInfo<List<Product_Project>> result = new ResultInfo<List<Product_Project>>();
            try
            {
                result.Data= dao.GetListByProductID(ProductID);
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
