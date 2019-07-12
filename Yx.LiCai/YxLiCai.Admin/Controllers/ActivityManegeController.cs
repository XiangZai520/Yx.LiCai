using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YxLica.Tools.Dic;
using YxLiCai.Admin.Models;
using YxLiCai.Model;
using YxLiCai.Model.ActivityManage;
using YxLiCai.Model.Authority;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Model.Plat;
using YxLiCai.Model.User;
using YxLiCai.Server.ActivityManege;
using YxLiCai.Server.MenuSet;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 活动管理
    /// </summary>
    public class ActivityManegeController : BaseController
    {
        #region 加息券
        /// <summary>
        /// 新增加息券页面
        /// </summary>
        [AuthorityValidateAttribute]
        public ActionResult AddInterestCoupon()
        {
            return View();
        }
        /// <summary>
        /// 新增加息券异步方法
        /// </summary>
        public ActionResult AddInterestCoupon_ajax()
        {
            ActivityManegeService activityManegeService = new ActivityManegeService();
            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "加息券名称不能为空！"
                });
            }
            if (activityManegeService.GetInterestCouponCountByName(name).Data != 0)
            {
                return Json(new
                {
                    Message = "加息券名称已存在！"
                });
            }
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "面值格式不正确，请重新输入！"
                });
            }
            int enableday = 0;
            try
            {
                enableday = Convert.ToInt32(Request["enableday"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "有效期格式不正确，请重新输入！"
                });
            }
            string usecondition = Request["usecondition"].Trim();

            ResultInfo<bool> result = activityManegeService.InsertActInterestCoupon(name, amount, enableday, usecondition, UserContext.simpleUserInfoModel.Id, DateTime.Now);
            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--新增加息券至表act_interest_coupon";
                SysService.AddSysLog(smodel);
                #endregion

                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "新增失败！"
                });
            }
        }
        /// <summary>
        /// 加息券列表
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult InterestCouponList()
        {
            return View();
        }
        /// <summary>
        /// 加息券列表(数据)
        /// </summary>
        /// <returns></returns>
        public ActionResult InterestCouponList_ajax()
        {
            //name: name, amount: amount, status: status, s_ctime: s_ctime, e_ctime: e_ctime, PageIndex: CurrentPage, PageSize: 10
            string name = Convert.ToString(Request["name"]);
            decimal amount = -1;
            if (Request["amount"].Trim() != "")
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int status = Convert.ToInt32(Request["status"]);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int TotalCount = 0;
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<List<Model.ActivityManage.ActInterestCoupon>> result = activityManegeService.GetInterestCouponList(name, amount, s_ctime, e_ctime, status, PageIndex, PageSize, out TotalCount);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    id = entity.id,
                    name = entity.name,
                    interest_rate = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.interest_rate),
                    use_condition = GetUseCondition(entity.use_condition),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    enable_day = entity.enable_day,
                    status = entity.status,
                    usecount = entity.use_count

                }).ToList();
                return Json(new
                {
                    Rows = rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据",
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
        }
        /// <summary>
        /// 获取使用条件
        /// </summary>
        /// <param name="use_condition"></param>
        /// <returns></returns>
        public string GetUseCondition(string use_condition)
        {
            List<string> uc_s = use_condition.Split(',').ToList();
            string uc_result = "";
            int key;
            if (uc_s != null && uc_s.Count > 0)
            {
                foreach (string s in uc_s)
                {
                    key = Convert.ToInt32(s);
                    uc_result = uc_result + (uc_result == "" ? "" : "，") + (YxLica.Tools.Dic.CommonDic.ProductType.Keys.Contains(key) ? YxLica.Tools.Dic.CommonDic.ProductType[key] : "");
                }
            }

            return uc_result;
        }
        /// <summary>
        /// 修改加息券
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult ModifyInterestCoupon()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            Model.ActivityManage.ActInterestCoupon OutModel = activityManegeService.GetActInterestCouponEntity(id).Data;
            if (OutModel != null)
            {
                return View(OutModel);
            }
            else
            {
                return Content("该加息券不存在！");
            }
        }
        /// <summary>
        /// 修改加息券异步方法
        /// </summary>
        public ActionResult ModifyInterestCoupon_ajax()
        {

            int id = Convert.ToInt32(Request["id"]);
            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "加息券名称不能为空！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (activityManegeService.GetInterestCouponCountByName(name, id).Data != 0)
            {
                return Json(new
                {
                    Message = "加息券名称已存在！"
                });
            }
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "面值格式不正确，请重新输入！"
                });
            }
            int enableday = 0;
            try
            {
                enableday = Convert.ToInt32(Request["enableday"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "有效期格式不正确，请重新输入！"
                });
            }
            string usecondition = Request["usecondition"].Trim();

            ResultInfo<bool> result = activityManegeService.ModifyActInterestCoupon(id, name, amount, enableday, usecondition, UserContext.simpleUserInfoModel.Id, DateTime.Now);
            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改加息券至表act_interest_coupon";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "修改失败！"
                });
            }
        }
        /// <summary>
        /// 审核加息券
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditInterestCoupon()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            Model.ActivityManage.ActInterestCoupon OutModel = activityManegeService.GetActInterestCouponEntity(id).Data;
            if (OutModel != null)
            {
                return View(OutModel);
            }
            else
            {
                return Content("该加息券不存在！");
            }
        }
        /// <summary>
        /// 更新加息券状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateInterestCouponStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.UpdateInterestCouponStatus(id, status, UserContext.simpleUserInfoModel.Id, DateTime.Now, remark);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改加息券至表act_interest_coupon,业务方法：审核加息券";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "操作失败！"
                });
            }

        }
        /// <summary>
        /// 删除加息券
        /// </summary>
        /// <returns></returns>
        public ActionResult DelInterestCoupon()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.DelInterestCoupon(id, UserContext.simpleUserInfoModel.Id, DateTime.Now);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "删除加息券至表act_interest_coupon，修改是否删除的状态";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "删除失败！"
                });
            }

        }
        /// <summary>
        /// 加息券发放使用明细
        /// </summary>
        /// <returns></returns>
        public ActionResult InterestCouponSendDetail()
        {
            return View();
        }
        /// <summary>
        /// 加息券发放使用明细
        /// </summary>
        /// <returns></returns>
        public ActionResult InterestCouponSendDetail_ajax()
        {
            string name = Convert.ToString(Request["name"]);
            string login_name = Convert.ToString(Request["login_name"]);
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int use_status = Convert.ToInt32(Request["use_status"]);
            int TotalCount = 0;

            long user_id = -1;

            if (login_name.Trim() != "")
            {
                YxLiCai.Server.User.UserInfoService userInfoService = new YxLiCai.Server.User.UserInfoService();
                ResultInfo<UserInfoModel> ri = userInfoService.GetUserModelByPhone(login_name);
                if (ri.Result && ri.Data != null)
                {
                    user_id = ri.Data.id;
                }
                else
                {
                    user_id = -2;
                }
            }


            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<List<InterestCouponSendDetailEx>> result = activityManegeService.InterestCouponSendDetail(name, user_id, use_status, s_ctime, e_ctime, PageIndex, PageSize, out TotalCount);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    login_name = GetLoginNameByUserID(entity.user_id),
                    name = entity.name,
                    interest_rate = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.interest_rate),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    e_time = entity.e_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    use_status = (entity.use_status == 1 ? "已使用" : "未使用"),
                    pdt_type = (CommonDic.ProductType.Keys.Contains(entity.pdt_type) ? CommonDic.ActType[entity.pdt_type] : ""),
                    use_date = ((entity.use_date == Convert.ToDateTime("1900-01-01")) ? "" : entity.use_date.ToString("yyyy-MM-dd HH:mm:ss")),
                    IsExpired = (entity.e_time > DateTime.Now ? "否" : "是")

                }).ToList();
                return Json(new
                {
                    Rows = rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据",
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
        }
        #endregion
        #region 特享金
        /// <summary>
        /// 新增特享金
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AddActSpecialAssets()
        {
            return View();
        }
        /// <summary>
        /// 新增特享金异步方法
        /// </summary>
        public ActionResult AddActSpecialAssets_ajax()
        {

            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "特享金名称不能为空！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (activityManegeService.GetSpecialAssetsCountByName(name).Data != 0)
            {
                return Json(new
                {
                    Message = "特享金名称已存在！"
                });
            }
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "面值格式不正确，请重新输入！"
                });
            }
            int enableday = 0;
            try
            {
                enableday = Convert.ToInt32(Request["enableday"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "使用期限格式不正确，请重新输入！"
                });
            }
            decimal rate = 0;
            try
            {
                rate = Convert.ToDecimal(Request["rate"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "特享金收益格式不正确，请重新输入！"
                });
            }
            decimal rate_increase = 0;
            try
            {
                rate_increase = Convert.ToDecimal(Request["rate_increase"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "收益增幅格式不正确，请重新输入！"
                });
            }

            ResultInfo<bool> result = activityManegeService.InsertActSpecialAssets(name, amount, enableday, rate, rate_increase, UserContext.simpleUserInfoModel.Id, DateTime.Now);
            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--新增特享金至表act_special_assets";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "新增失败！"
                });
            }
        }
        /// <summary>
        /// 特享金列表
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult SpecialAssetsList()
        {
            return View();
        }
        /// <summary>
        /// 特享金列表(数据)
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialAssetsList_ajax()
        {
            //name: name, amount: amount, status: status, s_ctime: s_ctime, e_ctime: e_ctime, PageIndex: CurrentPage, PageSize: 10
            string name = Convert.ToString(Request["name"]);
            decimal amount = -1;
            if (Request["amount"].Trim() != "")
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int status = Convert.ToInt32(Request["status"]);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int TotalCount = 0;
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<List<Model.ActivityManage.ActSpecialAssets>> result = activityManegeService.GetSpecialAssetsList(name, amount, s_ctime, e_ctime, status, PageIndex, PageSize, out TotalCount);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    id = entity.id,
                    name = entity.name,
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    rate = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.rate),
                    rate_increase = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.rate_increase),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    enable_day = entity.enable_day,
                    status = entity.status,
                    usecount = entity.send_count

                }).ToList();
                return Json(new
                {
                    Rows = rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据",
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
        }
        /// <summary>
        /// 修改特享金
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult ModifySpecialAssets()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            Model.ActivityManage.ActSpecialAssets OutModel = activityManegeService.GetActSpecialAssetsEntity(id).Data;
            if (OutModel != null)
            {
                return View(OutModel);
            }
            else
            {
                return Content("该特享金不存在！");
            }
        }
        /// <summary>
        /// 修改特享金异步方法
        /// </summary>
        public ActionResult ModifyActSpecialAssets_ajax()
        {
            int id = Convert.ToInt32(Request["id"]);
            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "特享金名称不能为空！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (activityManegeService.GetSpecialAssetsCountByName(name, id).Data != 0)
            {
                return Json(new
                {
                    Message = "特享金名称已存在！"
                });
            }
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "面值格式不正确，请重新输入！"
                });
            }
            int enableday = 0;
            try
            {
                enableday = Convert.ToInt32(Request["enableday"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "使用期限格式不正确，请重新输入！"
                });
            }
            decimal rate = 0;
            try
            {
                rate = Convert.ToDecimal(Request["rate"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "特享金收益格式不正确，请重新输入！"
                });
            }
            decimal rate_increase = 0;
            try
            {
                rate_increase = Convert.ToDecimal(Request["rate_increase"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "收益增幅格式不正确，请重新输入！"
                });
            }
            ResultInfo<bool> result = activityManegeService.ModifyActSpecialAssets(id, name, amount, enableday, rate, rate_increase, UserContext.simpleUserInfoModel.Id, DateTime.Now);
            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改特享金至表act_special_assets";
                SysService.AddSysLog(smodel);
                #endregion

                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "修改失败！"
                });
            }
        }
        /// <summary>
        /// 审核特享金页面
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult AuditSpecialAssets()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            Model.ActivityManage.ActSpecialAssets OutModel = activityManegeService.GetActSpecialAssetsEntity(id).Data;
            if (OutModel != null)
            {
                return View(OutModel);
            }
            else
            {
                return Content("该特享金不存在！");
            }
        }
        /// <summary>
        /// 更新特享金状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateSpecialAssetsStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.UpdateSpecialAssetsStatus(id, status, UserContext.simpleUserInfoModel.Id, DateTime.Now, remark);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "--修改特享金至表act_special_assets,业务方法：更新特享金状态";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "操作失败！"
                });
            }

        }
        /// <summary>
        /// 特享金发放明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialAssetsSendDetail()
        {
            return View();
        }
        /// <summary>
        /// 特享金发放明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SpecialAssetsSendDetail_ajax()
        {
            string name = Convert.ToString(Request["name"]);
            string login_name = Convert.ToString(Request["login_name"]);
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int TotalCount = 0;

            long user_id = -1;

            if (login_name.Trim() != "")
            {
                YxLiCai.Server.User.UserInfoService userInfoService = new YxLiCai.Server.User.UserInfoService();
                ResultInfo<UserInfoModel> ri = userInfoService.GetUserModelByPhone(login_name);
                if (ri.Result && ri.Data != null)
                {
                    user_id = ri.Data.id;
                }
                else
                {
                    user_id = -2;
                }
            }


            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<List<SpecialAssetsSendDetailEx>> result = activityManegeService.SpecialAssetsSendDetail(name, user_id, s_ctime, e_ctime, PageIndex, PageSize, out TotalCount);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    login_name = GetLoginNameByUserID(entity.user_id),
                    name = entity.name,
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    type = (CommonDic.ActType.Keys.Contains(entity.type) ? CommonDic.ActType[entity.type] : ""),
                    rate = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.rate),
                    enable_day = entity.enable_day,
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    e_time = entity.e_time.ToString("yyyy-MM-dd HH:mm:ss")

                }).ToList();
                return Json(new
                {
                    Rows = rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据",
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }
        }
        /// <summary>
        /// 根据用户id获取登录名称
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string GetLoginNameByUserID(long user_id)
        {
            YxLiCai.Server.User.UserInfoService userInfoService = new YxLiCai.Server.User.UserInfoService();
            ResultInfo<UserInfoModel> ri = userInfoService.GetUserModel(user_id);
            if (ri.Result && ri.Data != null && ri.Data.LoginName.Length>0)
            {
                return ri.Data.LoginName.Substring(0, 3) + "***" + ri.Data.LoginName.Substring(7, 4);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 删除特享金
        /// </summary>
        /// <returns></returns>
        public ActionResult DelSpecialAssets()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.DelSpecialAssets(id, UserContext.simpleUserInfoModel.Id, DateTime.Now);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "删除特享金至表act_special_assets，修改是否删除的状态";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "删除失败！"
                });
            }

        }
        #endregion
        #region 活动
        /// <summary>
        /// 添加活动
        /// </summary>
        /// <returns></returns>
        public ActionResult AddActivity()
        {
            ActivityManegeService activityManegeService = new ActivityManegeService();
            List<ActPromotionItem> outModel = activityManegeService.GetAllICAndSA().Data;
            return View(outModel);
        }
        /// <summary>
        /// 添加活动
        /// </summary>
        /// <returns></returns>
        public ActionResult AddActivity_ajax()
        {
            //name: name, max_user: max_user, limit_num: limit_num, 
            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "活动名称不能为空！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (activityManegeService.GetActivityCountByName(name).Data != 0)
            {
                return Json(new
                {
                    Message = "活动名称已存在！"
                });
            }
            int max_user_num = 0;
            try
            {
                max_user_num = Convert.ToInt32(Request["max_user_num"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "参与人上限格式不正确，请重新输入！"
                });
            }
            int limit_num = 0;
            try
            {
                limit_num = Convert.ToInt32(Request["limit_num"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "赠送上限格式不正确，请重新输入！"
                });
            }
            //budget: budget, s_time: s_time, e_time: e_time, ad_content: ad_content, url: url, item_array: item_array
            decimal budget = 0;
            try
            {
                budget = Convert.ToDecimal(Request["budget"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "预算格式不正确，请重新输入！"
                });
            }
            DateTime s_time;
            try
            {
                s_time = Convert.ToDateTime(Request["s_time"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "开始日期格式不正确，请重新输入！"
                });
            }
            DateTime e_time;
            try
            {
                e_time = Convert.ToDateTime(Request["e_time"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "结束日期格式不正确，请重新输入！"
                });
            }
            string ad_content = Convert.ToString(Request["ad_content"]);
            string url = Convert.ToString(Request["url"]);
            List<string> item_array;
            try
            {
                item_array = Request["item_array"].Split(',').ToList();
                List<string> _s;
                int i;
                if (item_array != null && item_array.Count > 0)
                {
                    foreach (string s in item_array)
                    {
                        _s = s.Split('_').ToList();
                        if (_s != null && _s.Count > 0)
                        {
                            foreach (string _s_s in _s)
                            {
                                i = Convert.ToInt32(_s_s);
                            }
                        }
                    }
                }
            }
            catch
            {
                return Json(new
                {
                    Message = "赠送项选择有误！"
                });
            }
            int type = Convert.ToInt32(Request["type"]);
            //插入项目主表


            ResultInfo<int> result = activityManegeService.InsertActPromotion(name, type, max_user_num, limit_num, budget, s_time, e_time, ad_content, url, DateTime.Now, UserContext.simpleUserInfoModel.Id);
            if (result.Result && result.Data > 0)
            {
                int act_id = result.Data;
                //插入子表
                List<ActPromotionItem> list_temp = activityManegeService.GetAllICAndSA().Data;
                List<ActPromotionItem> list_item = new List<ActPromotionItem>();
                List<ActPromotionItem> _list_item;
                ActPromotionItem item;
                DateTime c_time = DateTime.Now;
                int creator_id = UserContext.simpleUserInfoModel.Id;
                item_array = Request["item_array"].Split(',').ToList();
                string[] _s;

                if (item_array != null && item_array.Count > 0)
                {
                    foreach (string s in item_array)
                    {
                        _s = s.Split('_');
                        if (_s != null && _s.Length == 3)
                        {
                            item = new ActPromotionItem();
                            item.act_id = act_id;
                            item.c_time = c_time;
                            item.creator_id = creator_id;
                            item.item_type = Convert.ToInt32(_s[0]);
                            item.item_id = Convert.ToInt32(_s[1]);
                            item.item_qty = Convert.ToInt32(_s[2]);
                            _list_item = list_temp.Where(a => a.item_type == item.item_type && a.item_id == item.item_id).ToList();
                            if (_list_item != null && _list_item.Count > 0)
                            {
                                item.item_name = _list_item[0].item_name;
                            }
                            list_item.Add(item);
                        }
                    }
                }
                bool b = true;
                if (list_item != null && list_item.Count > 0)
                {
                    foreach (ActPromotionItem _item in list_item)
                    {
                        b = activityManegeService.InsertActPromotionItem(_item.act_id, _item.item_type, _item.item_id, _item.item_name, _item.item_qty, _item.c_time, _item.creator_id).Data;
                        if (!b)
                        {
                            break;
                        }
                    }
                }
                if (b)
                {
                    #region 添加日志
                    SysActionLogModel smodel = new SysActionLogModel();
                    smodel.CTime = DateTime.Now;
                    smodel.OpType = 1;
                    smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                    smodel.adminId = UserContext.simpleUserInfoModel.Id;
                    smodel.Remark = UserContext.simpleUserInfoModel.UserName + "插入一条记录至活动主表act_promotion，插入一条记录至活动子表act_promotion_item";
                    SysService.AddSysLog(smodel);
                    #endregion
                    return Content("ok");
                }
                else
                {
                    return Json(new
                    {
                        Message = "新增失败！"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Message = "新增失败！"
                });
            }
        }
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ActivityList()
        {
            return View();
        }
        /// <summary>
        /// 活动列表(异步获取数据)
        /// </summary>
        /// <returns></returns>
        public ActionResult ActivityList_ajax()
        {
            //name: name, type: type, status: status, s_ctime: s_ctime, e_ctime: e_ctime, PageIndex: CurrentPage, PageSize: 10
            string name = Convert.ToString(Request["name"]);
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int status = Convert.ToInt32(Request["status"]);
            int type = Convert.ToInt32(Request["type"]);
            int CurrentPage = Convert.ToInt32(Request["CurrentPage"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int IsAct = Convert.ToInt32(Request["IsAct"]);
            int TotalCount = 0;
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<List<Model.ActivityManage.ActPromotion>> result = activityManegeService.GetActivityList(name, type, s_ctime, e_ctime, status, CurrentPage, PageSize, IsAct, out TotalCount);

            if (result.Result && result.Data != null)
            {
                var rows = result.Data.ConvertAll(entity => new
                {
                    id = entity.id,
                    name = entity.name,
                    type = (CommonDic.ActType.Keys.Contains(entity.type) ? CommonDic.ActType[entity.type] : ""),
                    limit_num = entity.limit_num,
                    ad_content = entity.ad_content,
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    max_user_num = entity.max_user_num,
                    curt_user_num = entity.curt_user_num,
                    url = entity.url,
                    budget = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.budget),
                    s_time = entity.s_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    e_time = entity.e_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    status = entity.status

                }).ToList();
                return Json(new
                {
                    Rows = rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, CurrentPage)
                });
            }
            else
            {
                return Json(new
                {
                    Message = "没有数据",
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, CurrentPage)
                });
            }
        }
        /// <summary>
        /// 修改活动
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyActivity()
        {
            ActivityManegeService activityManegeService = new ActivityManegeService();
            int id = Convert.ToInt32(Request["id"]);
            ModifyActivityOut outModel = new ModifyActivityOut();
            outModel.actPromotion = activityManegeService.GetActPromotionEntityByID(id).Data;
            if (outModel.actPromotion != null)
            {
                outModel.All_Item = activityManegeService.GetAllICAndSA().Data;
                outModel.Choose_Item = activityManegeService.GetActPromotionItemByActID(id).Data;
                return View(outModel);
            }
            else
            {
                return Content("该活动不存在！");
            }
        }
        /// <summary>
        /// 修改活动
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyActivity_ajax()
        {
            int id = Convert.ToInt32(Request["id"]);
            string name = Request["name"].Trim();
            if (name == "")
            {
                return Json(new
                {
                    Message = "活动名称不能为空！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (activityManegeService.GetActivityCountByName(name, id).Data != 0)
            {
                return Json(new
                {
                    Message = "活动名称已存在！"
                });
            }
            int max_user_num = 0;
            try
            {
                max_user_num = Convert.ToInt32(Request["max_user_num"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "参与人上限格式不正确，请重新输入！"
                });
            }
            int limit_num = 0;
            try
            {
                limit_num = Convert.ToInt32(Request["limit_num"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "赠送上限格式不正确，请重新输入！"
                });
            }
            //budget: budget, s_time: s_time, e_time: e_time, ad_content: ad_content, url: url, item_array: item_array
            decimal budget = 0;
            try
            {
                budget = Convert.ToDecimal(Request["budget"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "预算格式不正确，请重新输入！"
                });
            }
            DateTime s_time;
            try
            {
                s_time = Convert.ToDateTime(Request["s_time"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "开始日期格式不正确，请重新输入！"
                });
            }
            DateTime e_time;
            try
            {
                e_time = Convert.ToDateTime(Request["e_time"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "结束日期格式不正确，请重新输入！"
                });
            }
            string ad_content = Convert.ToString(Request["ad_content"]);
            string url = Convert.ToString(Request["url"]);
            List<string> item_array;
            try
            {
                item_array = Request["item_array"].Split(',').ToList();
                List<string> _s;
                int i;
                if (item_array != null && item_array.Count > 0)
                {
                    foreach (string s in item_array)
                    {
                        _s = s.Split('_').ToList();
                        if (_s != null && _s.Count > 0)
                        {
                            foreach (string _s_s in _s)
                            {
                                i = Convert.ToInt32(_s_s);
                            }
                        }
                    }
                }
            }
            catch
            {
                return Json(new
                {
                    Message = "赠送项选择有误！"
                });
            }
            int type = Convert.ToInt32(Request["type"]);
            //修改项目主表


            ResultInfo<bool> result = activityManegeService.ModifyActPromotion(id, name, type, max_user_num, limit_num, budget, s_time, e_time, ad_content, url, DateTime.Now, UserContext.simpleUserInfoModel.Id);

            if (result.Result && result.Data)
            {
                //删除现有子项目
                ResultInfo<bool> ri = activityManegeService.DelActPromotionItemByActID(id);
                if (ri.Result && ri.Data)
                {
                    //重新插入子表
                    List<ActPromotionItem> list_temp = activityManegeService.GetAllICAndSA().Data;
                    List<ActPromotionItem> list_item = new List<ActPromotionItem>();
                    List<ActPromotionItem> _list_item;
                    ActPromotionItem item;
                    DateTime c_time = DateTime.Now;
                    int creator_id = UserContext.simpleUserInfoModel.Id;
                    item_array = Request["item_array"].Split(',').ToList();
                    string[] _s;

                    if (item_array != null && item_array.Count > 0)
                    {
                        foreach (string s in item_array)
                        {
                            _s = s.Split('_');
                            if (_s != null && _s.Length == 3)
                            {
                                item = new ActPromotionItem();
                                item.act_id = id;
                                item.c_time = c_time;
                                item.creator_id = creator_id;
                                item.item_type = Convert.ToInt32(_s[0]);
                                item.item_id = Convert.ToInt32(_s[1]);
                                item.item_qty = Convert.ToInt32(_s[2]);
                                _list_item = list_temp.Where(a => a.item_type == item.item_type && a.item_id == item.item_id).ToList();
                                if (_list_item != null && _list_item.Count > 0)
                                {
                                    item.item_name = _list_item[0].item_name;
                                }
                                list_item.Add(item);
                            }
                        }
                    }
                    bool b = true;
                    if (list_item != null && list_item.Count > 0)
                    {
                        foreach (ActPromotionItem _item in list_item)
                        {
                            b = activityManegeService.InsertActPromotionItem(_item.act_id, _item.item_type, _item.item_id, _item.item_name, _item.item_qty, _item.c_time, _item.creator_id).Data;
                            if (!b)
                            {
                                break;
                            }
                        }
                    }
                    if (b)
                    {
                        #region 添加日志
                        SysActionLogModel smodel = new SysActionLogModel();
                        smodel.CTime = DateTime.Now;
                        smodel.OpType = 1;
                        smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                        smodel.adminId = UserContext.simpleUserInfoModel.Id;
                        smodel.Remark = UserContext.simpleUserInfoModel.UserName + "修改一条记录至活动主表act_promotion，删除一条记录至活动子表act_promotion_item，重新插入一条记录至活动子表act_promotion_item";
                        SysService.AddSysLog(smodel);
                        #endregion
                        return Content("ok");
                    }
                    else
                    {
                        return Json(new
                        {
                            Message = "修改失败！"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Message = "修改失败！"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Message = "修改失败！"
                });
            }
        }
        /// <summary>
        /// 审核活动
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditActivity()
        {
            ActivityManegeService activityManegeService = new ActivityManegeService();
            int id = Convert.ToInt32(Request["id"]);
            ModifyActivityOut outModel = new ModifyActivityOut();
            outModel.actPromotion = activityManegeService.GetActPromotionEntityByID(id).Data;
            if (outModel.actPromotion != null)
            {
                outModel.All_Item = activityManegeService.GetAllICAndSA().Data;
                outModel.Choose_Item = activityManegeService.GetActPromotionItemByActID(id).Data;
                return View(outModel);
            }
            else
            {
                return Content("该活动不存在！");
            }
        }
        /// <summary>
        /// 更新活动状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateActivityStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.UpdateActivityStatus(id, status, UserContext.simpleUserInfoModel.Id, DateTime.Now, remark);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "修改活动状态至活动主表act_promotion";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "操作失败！"
                });
            }
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <returns></returns>
        public ActionResult DelActivity()
        {
            int id = Convert.ToInt32(Request["id"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<bool> result = activityManegeService.DelActivity(id, UserContext.simpleUserInfoModel.Id, DateTime.Now);

            if (result.Result && result.Data)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 1;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "删除活动至活动主表act_promotion，修改是否删除的状态";
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                return Json(new
                {
                    Message = "删除失败！"
                });
            }

        }
        /// <summary>
        /// 验证是否存在同类型活动同时间段重叠存在
        /// </summary>
        /// <returns></returns>
        public ActionResult IsExistsByTime()
        {
            DateTime s_time = Convert.ToDateTime(Request["s_time"]);
            DateTime e_time = Convert.ToDateTime(Request["e_time"]);
            int type = Convert.ToInt32(Request["type"]);
            int id = -1;
            if (Request["id"] != null)
            {
                id = Convert.ToInt32(Request["id"]);
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            ResultInfo<int> result = activityManegeService.GetActivityCountByTypeAndTime(s_time, e_time, type,id);
            if (result.Result && result.Data == 0)
            {
                return Content("0");
            }
            else
            {
                return Content("1");
            }
        }

        public ActionResult ActivityDetail()
        {
            ActivityManegeService activityManegeService = new ActivityManegeService();
            int id = Convert.ToInt32(Request["id"]);
            ModifyActivityOut outModel = new ModifyActivityOut();
            outModel.actPromotion = activityManegeService.GetActPromotionEntityByID(id).Data;
            if (outModel.actPromotion != null)
            {
                outModel.All_Item = activityManegeService.GetAllICAndSA().Data;
                outModel.Choose_Item = activityManegeService.GetActPromotionItemByActID(id).Data;
                return View(outModel);
            }
            else
            {
                return Content("该活动不存在！");
            }
        }

        #endregion
        #region 活动充值
        /// <summary>
        /// 活动充值页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActRecharge()
        {
            ActivityManegeService activityManegeService=new ActivityManegeService();
            ResultInfo<PlatAccount> result = activityManegeService.GetPlatAccountByType(2);
            PlatAccount outModel = new PlatAccount();
            if (result.Result && result.Data != null)
            {
                outModel = result.Data;
            }
            return View(outModel);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public ActionResult ActRecharge_ajax()
        {
            decimal RechargeAmount = 0;
            try
            {
                RechargeAmount = Convert.ToDecimal(Request["RechargeAmount"]);
            }
            catch
            {
                return Json(new
                {
                    Message = "充值金额格式不正确！"
                });
            }
            ActivityManegeService activityManegeService = new ActivityManegeService();
            PlatAccount result = activityManegeService.GetPlatAccountByType(2).Data;
            if (result != null)
            {

                ResultInfo<bool> ri = activityManegeService.ActRecharge(result.id,DateTime.Now, RechargeAmount,UserContext.simpleUserInfoModel.Id);
                if(ri.Result&&ri.Data)
                {
                    #region 添加日志
                    SysActionLogModel smodel = new SysActionLogModel();
                    smodel.CTime = DateTime.Now;
                    smodel.OpType = 1;
                    smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                    smodel.adminId = UserContext.simpleUserInfoModel.Id;
                    smodel.Remark = UserContext.simpleUserInfoModel.UserName + "充值至表plat_account_log，充值成功";
                    SysService.AddSysLog(smodel);
                    #endregion
                    return Content("ok");
                }
                else
                {
                    return Json(new
                    {
                        Message = "充值失败！"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Message = "充值失败！"
                });
            }
        }
        /// <summary>
        /// 活动充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ActRechargeRec()
        {
            DateTime s_ctime;
            string _s_ctime = Convert.ToString(Request["s_ctime"]);
            if (_s_ctime.Trim() == "")
            {
                _s_ctime = "1900-01-01";
            }
            s_ctime = Convert.ToDateTime(_s_ctime);
            DateTime e_ctime;
            string _e_ctime = Convert.ToString(Request["e_ctime"]);
            if (_e_ctime.Trim() == "")
            {
                _e_ctime = "9999-01-01";
            }
            e_ctime = Convert.ToDateTime(_e_ctime);
            e_ctime = e_ctime.AddDays(1);
            int tab = Convert.ToInt32(Request["tab"]);
            ActivityManegeService activityManegeService = new ActivityManegeService();
            if (tab == 1)
            {
                ResultInfo<List<ActRechargeRecEx>> result = activityManegeService.ActRechargeRec(s_ctime, e_ctime);
                if (result.Result && result.Data != null)
                {
                    List<ActRechargeRecEx> list = result.Data;
                    var rows = list.ConvertAll(entity => new {
                        c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                        amount=YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                        status="成功",
                        opname = GetOpName(entity.from_id)
                    });
                    return Json(new {
                        Rows = rows
                    });
                }
                return Content("");
            }
            else if (tab == 2)
            {
                ResultInfo<List<ActPaidRecEx>> result = activityManegeService.ActPaidRec(s_ctime, e_ctime);
                if (result.Result && result.Data != null)
                {
                    List<ActPaidRecEx> list = result.Data;
                    var rows = list.ConvertAll(entity => new
                    {
                        type = entity.type,
                        amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount)
                    });
                    return Json(new
                    {
                        Rows = rows
                    });
                }
                return Content("");
            }
            else
            {
                return Content("");
            }

        }
        public string GetOpName(int accountid)
        {
            YxLiCai.Service.Account.AdminAuthenticationService adminAuthenticationService = new Service.Account.AdminAuthenticationService();
            ResultInfo<AccountModel> result = adminAuthenticationService.GetAccountInfoByID(accountid);
            if (result.Result && result.Data != null)
            {
                return result.Data.LoginName;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
