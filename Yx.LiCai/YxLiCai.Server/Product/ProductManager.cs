using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YxLiCai.Model;
using YxLiCai.Model.Product;
using YxLiCai.Server.MenuSet;
using YxLiCai.Tools;

namespace YxLiCai.Server.Product
{
    /// <summary>
    /// 产品管理业务逻辑处理类
    /// add by lxm
    /// </summary>
    public class ProductManager
    {
        private YxLiCai.Dao.Product.ProductDao _dao = new Dao.Product.ProductDao();

        #region Add
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddProject(YxLiCai.Model.Product.ProductModel model)
        {
            try
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 2;
                smodel.adminName = model.CreatorID.ToString();
                smodel.adminId = (int)model.CreatorID;
                smodel.Remark = "Admin[" + model.CreatorID + "]" + "执行添加产品操作";
                SysService.AddSysLog(smodel);
                #endregion

                var productStr = model.ProductStr.Replace("[", "").Replace("]", "").Split(',');
                model.ProjectList = new List<string>();
                model.ProjectList.AddRange(productStr);
                model.ProductAmount = model.ProductAmount * 10000;

                return _dao.Add(model);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// 选择项目
        /// </summary>
        /// <returns></returns>
        public List<YxLiCai.Tools.KeyValuePair> GetProjectIDNameList(int categoryID)
        {
            try
            {
                var result = new YxLiCai.Dao.Project.ProjectDao().GetProjectIDNameList(categoryID);

                if (result != null)
                {
                    var list = new List<YxLiCai.Tools.KeyValuePair>();
                    foreach (var item in result)
                    {
                        var keyValue = new YxLiCai.Tools.KeyValuePair();
                        keyValue.Id = item.Id.ToString();
                        keyValue.Name = "ID:" + item.Id + " | 名称:" + item.ProjectName + " | 权值:" + item.Weight
                            + " | 可售金额:" + Math.Round(Convert.ToDecimal(item.AvailableAmount / 10000), 6) + "万元";
                        list.Add(keyValue);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 根据id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public YxLiCai.Model.Product.ProductModel GetProductByID(int id)
        {
            try
            {
                var ds = _dao.GetModel(id);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                    return null;

                var model = _dao.DataRowToModel(ds.Tables[0].Rows[0]);

                foreach (System.Data.DataRow row in ds.Tables[1].Rows)
                {
                    model.ProductStr += row["pjt_id"].ToString() + ",";
                }

                return model;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 查询产品列表
        /// </summary>
        /// <param name="ProductName">名称</param>
        /// <param name="ProductCategory">类型</param>
        /// <param name="ProductStatus">状态</param>
        /// <param name="StartDate">预期日期</param>
        /// <param name="EndDate">预期日期</param>
        /// <param name="take">查询记录数</param>
        /// <param name="skip">起始记录</param>
        /// <returns></returns>
        public List<YxLiCai.Model.Product.ProductModel> GetProductList(string ProductName, int ProductCategory, int ProductStatus,
            DateTime? StartDate, DateTime? EndDate, int take, int skip, out int TotalCount)
        {
            TotalCount = 0;
            var list = new List<YxLiCai.Model.Product.ProductModel>();

            try
            {
                var ds = _dao.GetList(ProductName, ProductCategory, ProductStatus, StartDate, EndDate, take, skip);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    list.AddRange(from object row in ds.Tables[0].Rows select _dao.DataRowToModel(row as System.Data.DataRow));
                }
                if (ds.Tables[1] != null)
                {
                    int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out TotalCount);
                }

                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 产品预警列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<YxLiCai.Model.Product.ProductModel> GetAlertList(int pCategory)
        {
            try
            {
                var ds = _dao.GetAlertList(pCategory);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    var list = new List<YxLiCai.Model.Product.ProductModel>();
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataRowToModel(row as System.Data.DataRow));
                    }
                    return list;
                }
                return null;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        /// <summary>
        /// 获取售卖中产品信息
        /// </summary>
        /// <param name="categoryID">类型id ： 1.月月赢，2.季季享3月，3.季季享6月，4.年年丰</param>
        /// <returns></returns>
        public YxLiCai.Model.Product.SallProductModel GetOnlineProduct(int category)
        {
            try
            {
                return _dao.GetSallProduct(category);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return null;
            }
        }
        /// <summary>
        /// 获取产品可投金额
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ResultInfo<decimal> ProductAvailableAmount(int pid)
        {
            ResultInfo<decimal> result = new ResultInfo<decimal>();
            result.Result = false;
            result.Data = 0;
            try
            {
                result.Data = _dao.ProductAvailableAmount(pid);
                result.Result = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
        /// <summary>
        /// 产品名称是否存在
        /// </summary>
        /// <param name="pName"></param>
        /// <returns>true：存在；false：不存在</returns>
        public bool ProductNameExisted(string pName)
        {
            try
            {
                var result = _dao.IsProducNameExisted(pName);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return true; //若异常，返回true ： 产品名称存在，不允许插入
            }
        }
        /// <summary>
        /// 获取售卖中产品列表
        /// </summary>
        /// <returns></returns>
        public List<YxLiCai.Model.Product.ProductModel> GetOnlineList()
        {
            var list = new List<YxLiCai.Model.Product.ProductModel>();

            try
            {
                var ds = _dao.GetOnlineList();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataRowToModel(row as System.Data.DataRow));
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateProject(YxLiCai.Model.Product.ProductModel model)
        {
            try
            {
                var productStr = model.ProductStr.Replace("[", "").Replace("]", "").Split(',');
                model.ProjectList = new List<string>();
                model.ProjectList.AddRange(productStr);
                if (model.AuditorID > 0)
                    model.AuditDate = DateTime.Now;
                model.ProductAmount = model.ProductAmount * 10000;

                return _dao.Update(model);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        /// <summary>
        /// 产品状态改为下架
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <returns></returns>
        public bool DisableProduct(int id)
        {
            return _dao.UpdateProductStatus(id);
        }
        /// <summary>
        /// 产品上架
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public bool EnableProduct(int pid)
        {
            try
            {
                return _dao.UpProduct(pid);
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return false;
            }
        }
        #endregion

        #region Delete
        public bool DeleteProject(string list)
        {
            return _dao.Delete(list);
        }
        #endregion

        #region 产品在售、上下架
        /// <summary>
        /// 根据产品ID 获取列表
        /// </summary>
        /// <param name="Ids">产品id</param>
        /// <returns></returns>
        public ResultInfo<List<YxLiCai.Model.Product.ProductInfo>> GetListByIDS(List<int> Ids)
        {
            ResultInfo<List<YxLiCai.Model.Product.ProductInfo>> result = new ResultInfo<List<ProductInfo>>();
            try
            {
                result.Data = _dao.GetListByIDS(Ids);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
            }
            return result;
        }
        /// <summary>
        /// 上架产品----根据下架产品ID(假设此产品已经下架或者即将下架）
        /// </summary>
        /// <param name="ProductID">产品ID</param>
        /// <returns></returns>
        public ResultInfo UpProduct(int ProductID)
        {
            ResultInfo result = new ResultInfo(true, "");
            try
            {
                List<int> pids = new List<int>();
                pids.Add(ProductID);
                int producttype = 0;
                List<ProductInfo> list = _dao.GetListByIDS(pids);

                int Up_PID = 0;
                if (list != null && list.Count > 0)
                {
                    //获取下架产品的类型
                    producttype = list[0].ProductCategory;
                    //根据产品类型获取所有 产品列表
                    List<ProductInfo> list_ByPType = _dao.GetListByPType(producttype);
                    List<ProductInfo> list_temp;
                    //查找预期上架时间在今天之前的
                    list_temp = list_ByPType.Where(a => (a.ExpectedEnableDate < DateTime.Now && a.ProductStatus == 1 && a.IsDeleted == false)).ToList();

                    //有 预期上架时间 在现在之前的 上架 此产品
                    if (list_temp != null && list_temp.Count > 0)
                    {
                        list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                        Up_PID = list_temp[0].Id;
                    }
                    //没有预期上架时间 在现在之前的
                    else
                    {
                        //查找所有已设置自动上架的
                        list_temp = list_ByPType.Where(a => (a.IsAutoEnable == 1 && a.ProductStatus == 1 && a.IsDeleted == false)).ToList();
                        if (list_temp != null && list_temp.Count > 0)
                        {
                            list_temp = list_temp.OrderBy(a => a.ProductOrder).ToList();
                            Up_PID = list_temp[0].Id;
                        }
                        else
                        {
                            //查找预期上线时间最早的，改为上架状态
                            list_temp = list_ByPType.Where(a => (a.ProductStatus == 1 && a.IsDeleted == false)).ToList();
                            if (list_temp != null && list_temp.Count > 0)
                            {
                                list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                                Up_PID = list_temp[0].Id;
                            }
                        }
                    }
                    //上架 产品
                    if (Up_PID != 0)
                    {
                        _dao.UpProductStatusByID(Up_PID, 3);
                    }

                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 获取在售产品
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ResultInfo<SallProductModel> GetSallProduct(int category)
        {
            ResultInfo<SallProductModel> result = new ResultInfo<SallProductModel>();
            result.Data = null;
            result.Result = false;
            result.Message = "";
            try
            {
                result.Data = _dao.GetSallProduct(category);
                result.Result = true;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据商品类型获取累计销售人数 by 张浩然 2015-7-24
        /// </summary>
        /// <param name="ptype">商品类型</param>
        /// <returns></returns>
        public ResultInfo<int> pdtSellCountByptype(int ptype)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = _dao.pdtSellCountByptype(ptype);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = 0;
            }
            return result;
        }

        /// <summary>
        /// 获取在售产品ID
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int GetSallProductId(int category)
        {
            int productId = 0;
            try
            {
                var res = _dao.GetSallProduct(category);
                if (res == null)
                {
                    return 0;
                }
                if (res.IsAutoEnable)
                {
                    productId = res.Id;
                }
                else if (res.ExpectedEnableDate < DateTime.Now)
                {
                    productId = res.Id;
                }
                else
                {
                    productId = 0;
                }
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                productId = 0;
            }
            return productId;
        }
        #endregion

        /// <summary>
        /// 根据类型判断是否存在售卖中的商品 by张浩然 2015-7-17
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns></returns>
        public ResultInfo<int> IsSallProduct(int productType)
        {
            ResultInfo<int> result = new ResultInfo<int>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = _dao.IsSallProduct(productType);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = 0;
            }
            return result;
        }

        /// <summary>
        /// 根据类型判断是否存在可上架的产品,如果存在就上架
        /// </summary>
        /// <param name="producttype">产品类型</param>
        /// <returns></returns>
        public ResultInfo<bool> UpProductByType(int producttype)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;

            try
            {
                int Up_PID = 0;

                //根据产品类型获取所有 产品列表
                List<ProductInfo> list_ByPType = _dao.GetListByPType(producttype);

                List<ProductInfo> list_temp;

                //查找预期上架时间在今天之前的
                list_temp =
                    list_ByPType.Where(
                        a => (a.ExpectedEnableDate < DateTime.Now && a.ProductStatus == 1 && a.IsDeleted == false))
                        .ToList();

                //有 预期上架时间 在现在之前的 上架 此产品
                if (list_temp != null && list_temp.Count > 0)
                {
                    list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                    Up_PID = list_temp[0].Id;
                }
                //没有预期上架时间 在现在之前的
                else
                {
                    //查找所有已设置自动上架的
                    list_temp =
                        list_ByPType.Where(
                            a => (a.IsAutoEnable == 1 && a.ProductStatus == 1 && a.IsDeleted == false))
                            .ToList();
                    if (list_temp != null && list_temp.Count > 0)
                    {
                        list_temp = list_temp.OrderBy(a => a.ProductOrder).ToList();
                        Up_PID = list_temp[0].Id;
                    }
                    else
                    {
                        //查找预期上线时间最早的，改为上架状态
                        list_temp = list_ByPType.Where(a => (a.ProductStatus == 1 && a.IsDeleted == false)).ToList();
                        if (list_temp != null && list_temp.Count > 0)
                        {
                            list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                            Up_PID = list_temp[0].Id;
                        }
                    }
                }
                //上架 产品
                if (Up_PID != 0)
                {
                    result.Result = true;
                    result.Data = _dao.UpProductStatusByID(Up_PID, 3);
                }


            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Data = false;
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
    }
}
