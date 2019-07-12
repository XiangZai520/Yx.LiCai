/*
 * 融资方用户个人信息
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.UserFinancingManagement;
using YxLiCai.Server.UserFinancingManagement;
namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 融资方用户个人信息
    /// </summary>
    public class FinancingController : Controller
    {
        private UserInfo_FinancingManagement_Services user;
        //
        // GET: /UserFinancing/
        public FinancingController()
        {
            user = new UserInfo_FinancingManagement_Services();
        }

        #region 给融资方开辟账户
        /// <summary>
        /// 融资方个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddFerUser()
        {
            return View();
        }
        #endregion

        #region 融资方个人信息页面视图
        /// <summary>
        /// 融资方个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            if (UserContext.simpleUserInfoModel.Id > 0)
            {
                ResultInfo<int> resu = user.ISUserExist(UserContext.simpleUserInfoModel.Id);
                ResultInfo<UserInfo_FinancingManagement_Model> result = user.GetUserModel(UserContext.simpleUserInfoModel.Id);
                if (resu.Result && resu.Data > 0)
                {
                    if (result.Result && result.Data != null)
                    {
                        return View(result.Data);
                    }
                    else
                    {
                        result.Data.financier_id = UserContext.simpleUserInfoModel.Id;
                        result.Data.isbank_card = 0;
                        result.Data.isreal_card = 0;
                        result.Data.myreal_card = "";
                        result.Data.myreal_name = "";
                        result.Data.phone = "";
                    }
                }
                else { return View(result.Data); }
            }
            return View();
        }
        #endregion

        #region 给融资方开户
        /// <summary>
        /// 给融资方开户
        /// </summary>
        /// <returns></returns>
        public JsonResult AddUserInfo(string Cname, string PWD, string Caddress, string LGname)
        {
            if (string.IsNullOrEmpty(Cname) || string.IsNullOrEmpty(PWD) || string.IsNullOrEmpty(Caddress) || string.IsNullOrEmpty(LGname))
            {
                return Json(new ResultModel<bool>(2, "信息填写不完整", true), JsonRequestBehavior.AllowGet);
            }
            UserInfo_FinancingManagement_Services userInfo_FinancingManagement_Services = new UserInfo_FinancingManagement_Services();
            ResultInfo<UserInfo_FinancingManagement_Model> ri = userInfo_FinancingManagement_Services.GetUserModel(LGname);
            if (ri.Result == false)
            {
                return Json(new ResultModel<bool>(4, "系统错误", true), JsonRequestBehavior.AllowGet);
            }
            if (ri.Data != null)
            {
                return Json(new ResultModel<bool>(3, "登录名已经存在", true), JsonRequestBehavior.AllowGet);
            }
            UserInfo_FinancingManagement_Model model = new UserInfo_FinancingManagement_Model();
            model.isbank_card = 0;
            model.isreal_card = 0;
            model.financier_name = Cname;
            model.phone = "";
            model.spassword = "";
            model.company_address = Caddress;
            model.myreal_name = "";
            model.myreal_card = "";
            model.bank_name = "";
            model.bank_card = "";
            model.bank_code = "";
            model.first_num = "";
            model.last_num = "";
            model.bank_phone = "";
            model.requestid = "";
            model.status = 1;
            model.pwd = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(PWD, 32);
            model.l_name = LGname;

            var result = user.AddUser(model);
            Fina_User_Count_Model modelC = new Fina_User_Count_Model();

            modelC.fer_id = result.Data;
            modelC.c_time = DateTime.Now;
            modelC.m_time = DateTime.Now;
            modelC.amount = 0;
            modelC.amount_user = 0;
            modelC.amount_user_fz = 0;
            modelC.amount_repay = 0;
            modelC.amount_repay_fz = 0;
            modelC.amount_paid = 0;
            modelC.amount_borrow = 0;
            modelC.interest_payable = 0;
            modelC.interest_paid = 0;
            modelC.version = 0;
            modelC.remark = "";
            var resultC = user.AddFina_User_Count_Model(modelC);
            if (result.Data > 0 && result.Result && resultC.Data > 0 && resultC.Result)
            {
                return Json(new ResultModel<bool>(1, "开户成功", true), JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel<bool>(2, "开户失败", true), JsonRequestBehavior.AllowGet);
        }
        #endregion


        
    }
}
