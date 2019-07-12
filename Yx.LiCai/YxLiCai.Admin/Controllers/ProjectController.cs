using System;
using System.Net.Mail;
/*
 * 项目管理
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.Project;
using YxLiCai.Server.Project;
using YxLiCai.Tools;
namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 项目管理
    /// </summary>
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        private ProjectService _projectService;
        public ProjectController()
        {
            _projectService = new ProjectService();
        }

        /// <summary>
        /// 项目列表页
        /// </summary>
        /// <returns></returns>
        [AuthorityValidate]
        public ActionResult Index()
        {
            return View();
        }
        #region View
        /// <summary>
        /// 添加项目（页面）
        /// </summary>
        /// <returns></returns>
        [AuthorityValidate]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Edit(string id)
        {
            int ProjectID;
            if (int.TryParse(id, out ProjectID))
            {
                var result = _projectService.GetModel(ProjectID);
                if (result.Result && result.Data != null) { return View("Edit", result.Data); }

            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 审核未通过修改列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AD_Edit(string id)
        {
            int ProjectID;
            if (int.TryParse(id, out ProjectID))
            {
                var result = _projectService.GetModel(ProjectID);
                if (result.Result && result.Data != null) { return View("AD_Edit", result.Data); }

            }
            return RedirectToAction("AuditDenied");
        }
        /// <summary>
        /// 修改项目权值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorityValidate]
        public ActionResult UpdateWeight(string id)
        {
            int ProjectID;
            if (int.TryParse(id, out ProjectID))
            {
                var result = _projectService.GetModel(ProjectID);
                if (result.Result && result.Data != null)
                {
                    return View("UpdateWeight", result.Data);
                }
            }

            return RedirectToAction("UpdateWeightList");
        }
        /// <summary>
        /// 修改项目权值列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateWeightList()
        {
            return View();
        }
        /// <summary>
        /// 项目审核
        /// </summary>
        /// <returns></returns>    
        public ActionResult AuditProject(string id)
        {
            int productID;
            if (int.TryParse(id, out productID))
            {
                var result = _projectService.GetModel(productID);
                if (result.Result && result.Data != null)
                {
                    return View("AuditProject", result.Data);
                }
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 审核未通过重新提交
        /// </summary>
        /// <returns></returns>    
        public ActionResult AD_AuditProject(string id)
        {
            int productID;
            if (int.TryParse(id, out productID))
            {
                var result = _projectService.GetModel(productID);
                if (result.Result && result.Data != null)
                {
                    return View("AD_AuditProject", result.Data);
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 待审核
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditProjectList()
        {
            return View();
        }
        /// <summary>
        /// 审核通过售卖中
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditPassed()
        {
            return View();
        }
        /// <summary>
        /// 审核未通过
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditDenied()
        {
            return View();
        }
        /// <summary>
        /// 项目历史列表
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult ProjectHistory()
        {
            return View();
        }
        #endregion

        #region 获取融资方账户ID、单位名称
        /// <summary>
        /// 获取融资方账户ID、单位名称
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserFinancing()
        {
            var result = _projectService.GetUserInfo_FinancingManagement_Model();
            if (result.Result && result.Data.Count > 0)
            { return Json(result.Data, JsonRequestBehavior.AllowGet); }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Method
        /// <summary>
        /// 添加项目信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>   
        public JsonResult AddProject(ProjectModel model)
        {
            #region 检查是否存在重名的项目
            var a = _projectService.ISProtExist(model.ProjectName, -1);
            if (a.Result && a.Data > 0)
            {
                return Json(new ResultModel(false, "项目名称已经存在!"), JsonRequestBehavior.AllowGet);
            }
            #endregion
        
            SysActionLogModel smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 1;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--添加项目表pjt【name：" + model.ProjectName + "】【ord_number：" + model.OrderNumber + "】【amount：" + model.Amount + "】【borrower：" + model.Borrower + "】【weight：" + model.Weight + "】【amount_sold：" + model.AmountSold + "】【amount_able：" + model.AvailableAmount + "】【amount_able_fz：" + model.Amount_able_fz + "】【c_time：" + DateTime.Now + "】【launch_ime：" + model.LaunchTime + "】【e_time：" + model.EndTime + "】【borrowing_rate：" + model.BorrowingRate + "】【repayment_mode：" + model.RepaymentMode + "】【loan_period：" + model.LoanPeriod + "】【fer_account_id:" + model.account_id + "】【creator_id：" + UserContext.simpleUserInfoModel.Id + "】【pdt_collection：" + model.pdt_collection + "】";
            
            var bm = _projectService.AddSysLog(smodel);
            var b = _projectService.Add(model, UserContext.simpleUserInfoModel.Id);
            if (b.Result && b.Data)
            {
                return Json(new ResultModel(true, "添加成功!"), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(false, "添加失败!"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int projectID)
        {
            return null;
        }
        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AuthorityValidate]
        public JsonResult UpdateProject(ProjectModel model)
        {
            var a = _projectService.ISProtExist(model.ProjectName, model.Id);
            if (a.Result && a.Data > 0)
            {
                return Json(new ResultModel(false, "项目名称已经存在!"), JsonRequestBehavior.AllowGet);
            }
            //获取简单的实体信息
            var smple = _projectService.GetProjectModelSmple(model.Id);
            var result = _projectService.GetModel(model.Id);
            if (!smple.Result || smple.Data == null || !result.Result || result.Data == null)
            {
                return Json(new ResultModel(false, "项目信息不存在!"), JsonRequestBehavior.AllowGet);
            }
            var b = _projectService.Update(model, smple.Data, UserContext.simpleUserInfoModel.Id);
            SysActionLogModel smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 1;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改项目信息表pjt 【项目ID：" + model.Id + "】【name：" + result.Data.ProjectName + "变为" + model.ProjectName + "】【ord_number：" + result.Data.OrderNumber + "变为" + model.OrderNumber + "】【amount：" + result.Data.Amount + "变为" + model.Amount + "】【borrower：" + result.Data.Borrower + "变为" + model.Borrower + "】【weight：" + result.Data.Weight + "变为" + model.Weight + "】【amount_sold：" + result.Data.AmountSold + "变为" + model.AmountSold + "】【amount_able：" + result.Data.AvailableAmount + "变为" + model.AvailableAmount + "】【amount_able_fz：" + result.Data.Amount_able_fz + "变为" + model.Amount_able_fz + "】【launch_ime：" + result.Data.LaunchTime + "变为" + model.LaunchTime + "】【e_time：" + result.Data.EndTime + "变为" + model.EndTime + "】【borrowing_rate：" + result.Data.BorrowingRate + "变为" + model.BorrowingRate + "】【repayment_mode：" + result.Data.RepaymentMode + "变为" + model.RepaymentMode + "】【loan_period：" + result.Data.LoanPeriod + "变为" + model.LoanPeriod + "】【fer_account_id:" + result.Data.account_id + "变为" + model.account_id + "】【pdt_collection:" + result.Data.pdt_collection + "变为" + model.pdt_collection + "】";

            var bm = _projectService.AddSysLog(smodel);



            if (b.Result && b.Data)
            {
                return Json(new ResultModel(true, "修改成功!"), JsonRequestBehavior.AllowGet);
            }

            return Json(new ResultModel(false, "更新失败!"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改项目审核状态
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public JsonResult UpdateProjectStatus(ProjectModel model)
        {
            if (model != null)
            {
                var res = _projectService.GetProjectModelWeightOrvaidate(model.Id);
                if (res.Result && res.Data != null)
                {
                    SysActionLogModel smodel = new SysActionLogModel();
                    smodel.CTime = DateTime.Now;
                    smodel.OpType = 1;
                    smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                    smodel.adminId = UserContext.simpleUserInfoModel.Id;
                    smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改项目审核状态 【项目ID：" + model.Id + "】【pjt_status：" + res.Data.ProjectStatus + "变为" + model.ProjectStatus + "】";
                    var bm = _projectService.AddSysLog(smodel);
                }
                var result = _projectService.UpdateProjectStatus(model, UserContext.simpleUserInfoModel.Id);
                if (result.Result && result.Data)
                {
                    return Json(new ResultModel(true, "审核成功!"), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new ResultModel(false, "审核失败!"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改项目权值
        /// </summary>
        /// <param name="Weight"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>     
        public JsonResult UpdateProjectWeight(int Weight, int ProjectID)
        {
            if (Weight < 0 || Weight > 100)
            {
                return Json(new ResultModel(false, "权值必须是1-100之间整数!"), JsonRequestBehavior.AllowGet);
            }
            if (ProjectID < 0)
            {
                return Json(new ResultModel(false, "项目没有选择!"), JsonRequestBehavior.AllowGet);
            }
            var res = _projectService.GetProjectModelWeightOrvaidate(ProjectID);
            if (res.Result && res.Data != null)
            {
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改项目权值 【项目ID：" + res.Data.Id + "】【weight：" + res.Data.Weight + "变为" + Weight + "】";
                var bm = _projectService.AddSysLog(smodel);
            }
            var result = _projectService.UpdateProjectWeight(Weight, ProjectID);
            if (result.Result && result.Data)
            {
                return Json(new ResultModel(true, "修改成功!"), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel(false, "修改失败!"), JsonRequestBehavior.AllowGet);

        }
        #region Get
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetProjectList(YxLiCai.Admin.Models.ProjuctQueryViewModel queryModel)
        {
            if (queryModel.LaunchTime != DateTime.MinValue && queryModel.EndTime != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(queryModel.EndTime, queryModel.LaunchTime);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            var result = _projectService.GetProjuctList(queryModel.ProjectName, queryModel.ProjectStatus, queryModel.LaunchTime, queryModel.EndTime);

            if (result != null && result.Count > 0)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取权值列表
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public JsonResult GetProjectWeightList(YxLiCai.Admin.Models.ProjuctQueryViewModel queryModel)
        {
            if (queryModel.LaunchTime != DateTime.MinValue && queryModel.EndTime != DateTime.MinValue)
            {
                int d = DateOper.DateDayss(queryModel.EndTime, queryModel.LaunchTime);
                if (d < 0)
                {
                    return Json("还款开始日期不能大于结束日期", JsonRequestBehavior.AllowGet);
                }
            }
            var result = _projectService.GetProjuctList(queryModel.ProjectName, queryModel.LaunchTime, queryModel.EndTime, queryModel.Weight, queryModel.EndWeight);
            if (result.Count > 0)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除
        [AuthorityValidateAttribute]
        public JsonResult DeleteProject(string IdList)
        {
            SysActionLogModel smodel = new SysActionLogModel();
            smodel.CTime = DateTime.Now;
            smodel.OpType = 1;
            smodel.adminName = UserContext.simpleUserInfoModel.UserName;
            smodel.adminId = UserContext.simpleUserInfoModel.Id;
            smodel.Remark = UserContext.simpleUserInfoModel.UserName + "-删除项目【ID为：" + IdList + "】";
            var bm = _projectService.AddSysLog(smodel);
            bool b = _projectService.DeleteProject(IdList);
            if (b) { return Json(new ResultModel(true, "删除成功!"), JsonRequestBehavior.AllowGet); }
            return Json(new ResultModel(false, "删除失败!"), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}
