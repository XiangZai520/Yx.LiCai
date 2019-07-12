using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Model.InterestBonus;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 加息券相关操作
    /// </summary>
    public class InterestBonusController : BaseController
    {
        private YxLiCai.Server.Bonus.BonusManager _iService = new Server.Bonus.BonusManager();
        //
        // GET: /InterestBonus/
        [AuthorityValidateAttribute]
        public ActionResult Index()
        {
            return View();
        }
        [AuthorityValidateAttribute]
        public ActionResult Add()
        {
            return View();
        }
        [AuthorityValidateAttribute]
        public ActionResult Edit(string id)
        {
            //int productID;
            //if (int.TryParse(id, out productID))
            //    return View("Edit", _iService(productID));

            return RedirectToAction("Index");
        }
        [AuthorityValidateAttribute]
        public ActionResult AddCategory()
        {
            return View();
        }
        [AuthorityValidateAttribute]
        public ActionResult CategoryList()
        {
            return View();
        }
        //[AuthorityValidateAttribute]
        public ActionResult EditCategory(string id)
        {
            int categoryID;
            if (int.TryParse(id, out categoryID))
                return View("EditCategory", _iService.GetCategoryByID(categoryID));

            return RedirectToAction("CategoryList");
        }
        #region Method
        /// <summary>
        /// 添加加息券类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddInterestBonusCategory(InterestBonusCategoryModel model)
        {
            return Json(_iService.AddCategory(model), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取加息券类型列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCategoryList()
        {
            var list = _iService.GetCategoryList(string.Empty);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateCategory(InterestBonusCategoryModel model)
        {
            return Json(_iService.UpdateCategory(model), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
