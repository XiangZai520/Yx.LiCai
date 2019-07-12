using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YxLiCai.Admin.Models;
using YxLiCai.Model;
using YxLiCai.Model.Product;
using YxLiCai.Server.MenuSet;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 产品管理 Controller 
    /// add by lxm
    /// </summary>
    public class ProductController : BaseController
    {
        private YxLiCai.Server.Product.ProductManager _iproduct = new Server.Product.ProductManager();
        private DateTime _MinDate = Convert.ToDateTime("2015-01-01");
        private int adminID = UserContext.simpleUserInfoModel.Id;

        //
        // GET: /Product/
        #region Views
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 编辑产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Edit(string id)
        {
            int productID;
            if (int.TryParse(id, out productID))
                return View("Edit", _iproduct.GetProductByID(productID));

            return RedirectToAction("Index");
        }

        #region 审核
        /// <summary>
        /// 产品审核
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Audit(string id)
        {
            int productID;
            if (int.TryParse(id, out productID))
                return View("Audit", _iproduct.GetProductByID(productID));

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 待审核列表
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditList()
        {
            return View();
        }
        [AuthorityValidateAttribute]
        /// <summary>
        /// 审核通过页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditPassed()
        {
            return View();
        }
        /// <summary>
        /// 待上线
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult ToSell()
        {
            return View();
        }
        [AuthorityValidateAttribute]
        /// <summary>
        /// 审核拒绝页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditDenied()
        {
            return View();
        }
        #endregion

        [AuthorityValidateAttribute]
        /// <summary>
        /// 报警设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AlertSet()
        {
            return View();
        }
        /// <summary>
        /// 报警列表
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AlertList()
        {
            return View();
        }
        /// <summary>
        /// 售卖中
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult OnLine()
        {
            return View();
        }
        /// <summary>
        /// 已售罄
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Sellout()
        {
            return View();
        }
        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Detail(string id)
        {
            int productID;
            if (int.TryParse(id, out productID))
                return View("Detail", _iproduct.GetProductByID(productID));

            return RedirectToAction("Index");
        }
        #endregion

        #region Methods

        #region Add
        public JsonResult AddProduct(YxLiCai.Model.Product.ProductModel model)
        {
            if (_iproduct.ProductNameExisted(model.ProductName))
            {
                return Json(new ResultModel(false, "产品名称已存在", null)
                   , JsonRequestBehavior.AllowGet);
            }

            model.CreatorID = adminID;

            var result = _iproduct.AddProject(model);
            var message = "添加产品失败";
            if (result)
            {
                message = "添加产品成功";
            }

            return Json(new ResultModel(result, message, null),
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateProduct(YxLiCai.Model.Product.ProductModel model)
        {
            if (model.AuditorID <= 0 && _iproduct.ProductNameExisted(model.ProductName))
            {
                return Json(new ResultModel(false, "产品名称已存在", null)
                   , JsonRequestBehavior.AllowGet);
            }

            var result = _iproduct.UpdateProject(model);
            var message = "修改产品失败";
            if (result)
            {
                message = "修改产品成功";
            }

            #region 添加日志
            var smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 2;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "更新产品信息";
            SysService.AddSysLog(smodel);
            #endregion

            return Json(new ResultModel(result, message, null),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 产品下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public JsonResult DisableProduct(string IdList)
        {
            int pID;
            if (int.TryParse(IdList, out pID))
            {
                #region 添加日志
                var smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 2;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "产品下架";
                SysService.AddSysLog(smodel);
                #endregion

                return Json(_iproduct.DisableProduct(pID), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        /// <summary>
        /// 产品上架
        /// </summary>
        /// <param name="pdid"></param>
        /// <returns></returns>
        public JsonResult EnableProduct(int pid)
        {
            #region 添加日志
            var smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 2;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "产品上架";
            SysService.AddSysLog(smodel);
            #endregion

            return Json(_iproduct.EnableProduct(pid),
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get
        /// <summary>
        /// 产品 ID 名称  权值
        /// </summary>
        /// <returns></returns>
        public JsonResult GetProjectKeyValuePairList(int categoryID)
        {
            if (categoryID <= 0)
                return null;

            return Json(_iproduct.GetProjectIDNameList(categoryID), 
                JsonRequestBehavior.AllowGet);
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
        public JsonResult GetProductList(string ProductName, int ProductCategory, int ProductStatus,
            DateTime? StartDate, DateTime? EndDate, int take, int skip)
        {
            int countAll = 0;
            var list = _iproduct.GetProductList(ProductName, ProductCategory, ProductStatus, StartDate, EndDate, take, skip, out countAll);

            #region format
            var listFormated = list.ConvertAll(entity => new
            {
                Id = entity.Id,
                ProductName = entity.ProductName,
                PCategory = entity.PCategory,
                ProductAmount = entity.ProductAmount,
                RaisedAmount = entity.ProductCount.RaisedAmount / 10000,
                AvailableAmount = entity.ProductCount.AvailableAmount / 10000,
                ProductOrder = entity.ProductOrder,
                AutoEnable = entity.AutoEnable,
                PStatus = entity.PStatus,
                ExpectedEnableDate = entity.ExpectedEnableDate < _MinDate ? "" : entity.ExpectedEnableDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EnableDate = entity.EnableDate < _MinDate ? "" : entity.EnableDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = entity.EndDate < _MinDate ? "" : entity.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateDate = entity.CreateDate < _MinDate ? "" : entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AuditDate = entity.AuditDate < _MinDate ? "" : entity.AuditDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Remark = entity.Remark,
                IsSellPdt = entity.IsSellPdt
            }).ToList();
            #endregion

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 待上架产品列表
        /// </summary>
        public JsonResult GetToSellProductList(string ProductName, int ProductCategory, int ProductStatus,
            DateTime? StartDate, DateTime? EndDate, int take, int skip)
        {
            int countAll = 0;
            var list = _iproduct.GetProductList(ProductName, ProductCategory, ProductStatus, StartDate, EndDate, take, skip, out countAll);

            //筛选可上架产品
            //月月盈
            var product_month = FindToSellProduct(list, 1);
            if (product_month != null)
                list.Find(p => p.Id == product_month.Id).IsSellPdt = "1";
            //季季享3个月
            var product_q3 = FindToSellProduct(list, 2);
            if (product_q3 != null)
                list.Find(p => p.Id == product_q3.Id).IsSellPdt = "1";
            //季季享6个月
            var product_q6 = FindToSellProduct(list, 3);
            if (product_q6 != null)
                list.Find(p => p.Id == product_q6.Id).IsSellPdt = "1";
            //年年丰
            var product_year = FindToSellProduct(list, 4);
            if (product_year != null)
                list.Find(p => p.Id == product_year.Id).IsSellPdt = "1";

            #region format
            var listFormated = list.ConvertAll(entity => new
            {
                Id = entity.Id,
                ProductName = entity.ProductName,
                PCategory = entity.PCategory,
                ProductAmount = entity.ProductAmount / 10000,
                RaisedAmount = entity.ProductCount.RaisedAmount / 10000,
                AvailableAmount = entity.ProductCount.AvailableAmount / 10000,
                ProductOrder = entity.ProductOrder,
                AutoEnable = entity.AutoEnable,
                PStatus = entity.PStatus,
                ExpectedEnableDate = entity.ExpectedEnableDate < _MinDate ? "" : entity.ExpectedEnableDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EnableDate = entity.EnableDate < _MinDate ? "" : entity.EnableDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = entity.EndDate < _MinDate ? "" : entity.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateDate = entity.CreateDate < _MinDate ? "" : entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AuditDate = entity.AuditDate < _MinDate ? "" : entity.AuditDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Remark = entity.Remark,
                IsSellPdt = entity.IsSellPdt
            }).OrderByDescending(p => p.IsSellPdt).ToList();
            #endregion

            return Json(new
            {
                DataResult = listFormated,
                TotalCount = countAll
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取预警产品列表
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public JsonResult GetAlertList(int ProductCategory)
        {
            return Json(_iproduct.GetAlertList(ProductCategory), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 检查项目名称是否存在
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public JsonResult CheckProductName(string pName)
        {
            if (string.IsNullOrEmpty(pName.Trim()))
                return null;

            return Json(_iproduct.ProductNameExisted(pName), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取售卖中产品
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnlineList()
        {
            var list = _iproduct.GetOnlineList();

            //format
            var listFormated = list.ConvertAll(entity => new
            {
                id = entity.Id,
                name = entity.ProductName,
                category = entity.PCategory,
                amount = entity.ProductAmount,
                amount_raised = entity.ProductCount.RaisedAmount / 10000,
                amount_available = entity.ProductCount.AvailableAmount / 10000,
                c_time = entity.CreateDate < _MinDate ? "" : entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                enable_time = entity.EnableDate < _MinDate ? "" : entity.EnableDate.ToString("yyyy-MM-dd HH:mm:ss"),
                is_alert = entity.IsAlert == "0" ? "否" : "是"
            }).ToList();

            return Json(listFormated, 
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="IdList">id列表</param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public JsonResult DeleteProduct(string IdList)
        {
            #region 添加日志
            var smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 2;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "删除产品，ID[" + IdList + "]";
            SysService.AddSysLog(smodel);
            #endregion

            return Json(new ResultModel(_iproduct.DeleteProject(IdList), "删除成功"),
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Common
        /// <summary>
        /// 根据判断条件筛选出符合待上架的产品
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private ProductModel FindToSellProduct(List<ProductModel> listAll, int category)
        {
            var list = listAll.FindAll(p => p.ProductCategory == category).ToList();

            if (list == null || list.Count == 0)
                return null;

            var product = new ProductModel();

            // 1. 查找预期上线时间小于当前时间的
            var list1 = list.Where(p => p.ExpectedEnableDate <= DateTime.Now).
                OrderBy(p => p.ExpectedEnableDate).ToList();

            if (list1 != null && list1.Count > 0)
            {
                product = list1.First();
            }
            else
            {
                // 2.不存在预期上线时间小于当前时间的，查找所有已设置自动上架的
                var list2 = list.Where(p => p.IsAutoEnable == 1).ToList();
                if (list2 != null && list2.Count > 0)
                {
                    product = list.OrderBy(p => p.ProductOrder).First();
                }
                else
                {
                    // 3.不存在自动上架的，查找预期上线时间最早的
                    product = list.OrderBy(p => p.ExpectedEnableDate).First();
                }

            }

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns></returns>
        public JsonResult UpProductByType(int type)
        {
            ResultInfo<int> resultSallProduct = _iproduct.IsSallProduct(type);//根据类型判断是否存在售卖中的商品
            if (resultSallProduct.Result && resultSallProduct.Data == 0)
            {
                ResultInfo<bool> resultUpProductByType = _iproduct.UpProductByType(type);
                if (resultUpProductByType.Result && resultUpProductByType.Data)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}
