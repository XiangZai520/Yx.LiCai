using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.Product;

namespace YxLiCai.Dao.Product
{
    /// <summary>
    /// 产品项目数据层
    /// </summary>
    public class ProductProjectDao
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ProductID">产品id</param>
        /// <returns></returns>
        public List<Product_Project> GetListByProductID(int ProductID)
        {
            List<Product_Project> list = new List<Product_Project>();
            Product_Project entity;
            string strSql = @"SELECT  pdt_id AS ProductID,pjt_id AS ProjectID,a_date AS AddDate,is_delete AS IsDeleted 
                              FROM pdt_pjt WHERE is_delete=0 AND pdt_id=?ProductID";
            MySqlParameter[] parameters = {
					new MySqlParameter("?ProductID", MySqlDbType.Int64,11)
			};
            parameters[0].Value = ProductID;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取产品项目列表", parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        entity = new Product_Project();
                        entity.ProductID =Convert.ToInt32(dr["ProductID"]);
                        entity.ProjectID = Convert.ToInt32(dr["ProjectID"]);
                        entity.AddDate =Convert.ToDateTime( dr["AddDate"]);
                        entity.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);

                        list.Add(entity);
                    }
                }
            }


            return list;
        }
    }
}
