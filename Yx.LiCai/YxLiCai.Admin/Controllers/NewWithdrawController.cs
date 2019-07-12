using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Server.WithdrawManager;

namespace YxLiCai.Admin.Controllers
{
    /// <summary>
    /// 提现操作
    /// </summary>
    public class NewWithdrawController : BaseController
    {
        #region 用户提现

        /// <summary>
        /// 用户提现
        /// </summary>
        /// <returns></returns>
        public ActionResult UserWithdraw()
        {
            return View();
        }
        /// <summary>
        /// 用户提现
        /// </summary>
        public ActionResult UserWithdraw_ajax()
        {
            string LoginName = Convert.ToString(Request["LoginName"]);
            string MyRealName = Convert.ToString(Request["MyRealName"]);
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
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<UserWithDrawEx>> result = newWithdrawService.GetUserWithDrawList(LoginName, MyRealName, s_ctime, e_ctime, status, 0, PageIndex, PageSize, out TotalCount);
            if (result.Result && result.Data != null)
            {
                List<UserWithDrawEx> list = result.Data;
                var Rows = list.ConvertAll(entity => new
                {
                    id = entity.id,
                    login_name =YxLica.Tools.Common.GetHideStr(entity.login_name,3,4),
                    real_name = entity.real_name,
                    bank_name = entity.bank_name,
                    bank_card = YxLica.Tools.Common.GetHideStr(entity.bank_card,6,4),
                    bank_phone = YxLica.Tools.Common.GetHideStr(entity.bank_phone,3,4),
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    amount_balance = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_balance),
                    amount_principal = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_principal),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    auditor_name = entity.auditor_name,
                    audit_time = (entity.audit_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.audit_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    loan_name = entity.loan_name,
                    loan_time = (entity.loan_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.loan_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    status = entity.status


                }).ToList();
                return Json(new
                {
                    Rows = Rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }

            return Json(new
            {
                Message = "没有数据"
            });
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateWithdrawStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
            {
                int auditor_id = UserContext.simpleUserInfoModel.Id;
                string auditor_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime audit_time = DateTime.Now;
                ResultInfo<bool> result = newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                if (result.Result && result.Data)
                {
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
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }
        }
        /// <summary>
        /// 放款
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
            {
                if (ri_info.Data.amount > ri_info.Data.account_amount_blance)
                {
                    return Json(new
                    {
                        Message = "用户账户余额不足，放款失败！"
                    });
                }
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取提现用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserWithDrawEx userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        string card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        string card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.WithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }
        }
        /// <summary>
        /// 重新放款
        /// </summary>
        /// <returns></returns>
        public ActionResult WithdrawReLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 4)
            {
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取提现用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserWithDrawEx userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        string card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        string card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.WithdrawReLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }

        }
        /// <summary>
        /// 全部通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassAllWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserWithDraw(0, 0);
            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            if (result.Result && result.Data != null && result.Data.Count > 0)
            {
                List<int> list = result.Data;
                foreach (int id in list)
                {
                    ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                    {
                        newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }
            return Content("ok");
        }
        /// <summary>
        /// 全部放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanAllWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserWithDraw(1, 0);

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;

            if (result.Result && result.Data != null&&result.Data.Count>0)
            {
                List<int> list = result.Data;
                UserWithDrawEx userWithDrawEx;
                string card_top;
                string card_last;
                foreach (int id in list)
                {
                    ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                    {
                        if (ri_info.Data.amount > ri_info.Data.account_amount_blance)
                        {
                            return Json(new
                            {
                                Message = "用户账户余额不足，放款失败！"
                            });
                        }
                        //获取提现用户信息
                        userWithDrawEx = ri_info.Data;
                        if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                        {
                            card_top = userWithDrawEx.bank_card.Substring(0, 6);
                            card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                            newWithdrawService.WithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        }

                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }

            return Content("ok");
        }
        /// <summary>
        /// 批量通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassChooseWithdraw()
        {
            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            NewWithdrawService newWithdrawService = new NewWithdrawService();

            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            foreach (int id in list)
            {
                ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                {
                    newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanChooseWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();

            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;


            UserWithDrawEx userWithDrawEx;
            string card_top;
            string card_last;

            foreach (int id in list)
            {
                ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                {
                    if (ri_info.Data.amount > ri_info.Data.account_amount_blance)
                    {
                        return Json(new
                        {
                            Message = "用户账户余额不足，放款失败！"
                        });
                    }
                    //获取提现用户信息
                    userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        newWithdrawService.WithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                    }

                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 查看审核未通过原因
        /// </summary>
        /// <returns></returns>
        public ActionResult LookWithdrawDenyRemark()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            string remark = "";
            if (ri_info.Result && ri_info.Data != null)
            {
                remark = ri_info.Data.remark;
            }
            if (remark==null)
            {
                remark = "";
            }
            return Content(remark);
        }

        #endregion
        #region 用户赎回
        /// <summary>
        /// 用户赎回
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRedemption()
        {
            return View();
        }
        /// <summary>
        /// 用户赎回
        /// </summary>
        public ActionResult UserRedemption_ajax()
        {
            string LoginName = Convert.ToString(Request["LoginName"]);
            string MyRealName = Convert.ToString(Request["MyRealName"]);
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
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<UserRedemptionEx>> result = newWithdrawService.GetUserRedemptionList(LoginName, MyRealName, s_ctime, e_ctime, status, PageIndex, PageSize, out TotalCount);
            if (result.Result && result.Data != null)
            {
                List<UserRedemptionEx> list = result.Data;
                var Rows = list.ConvertAll(entity => new
                {
                    id = entity.id,
                    login_name = YxLica.Tools.Common.GetHideStr(entity.login_name,3,4),
                    real_name = entity.real_name,
                    bank_name = entity.bank_name,
                    bank_card = YxLica.Tools.Common.GetHideStr(entity.bank_card,6,4),
                    bank_phone = YxLica.Tools.Common.GetHideStr(entity.bank_phone,3,4),
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    amount_penalty = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_penalty),
                    amount_Actual = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_Actual),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    auditor_name = entity.auditor_name,
                    audit_time = (entity.audit_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.audit_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    loan_name = entity.loan_name,
                    loan_time = (entity.loan_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.loan_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    status = entity.status


                }).ToList();
                return Json(new
                {
                    Rows = Rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }

            return Json(new
            {
                Message = "没有数据"
            });
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateRedemptionStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
            {
                int auditor_id = UserContext.simpleUserInfoModel.Id;
                string auditor_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime audit_time = DateTime.Now;
                ResultInfo<bool> result = newWithdrawService.UpdateRedemptionStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                if (result.Result && result.Data)
                {
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
            else
            {
                return Json(new
                {
                    Message = "赎回状态不正确！"
                });
            }
        }
        /// <summary>
        /// 放款
        /// </summary>
        /// <returns></returns>
        public ActionResult RedemptionLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
            {
                if (ri_info.Data.amount_Actual > ri_info.Data.account_amount_blance)
                {
                    return Json(new
                    {
                        Message = "用户账户余额不足，放款失败！"
                    });
                }
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取赎回用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserRedemptionEx userRedemptionEx = ri_info.Data;
                    if (userRedemptionEx.bank_card != "" && userRedemptionEx.bank_card.Length > 6)
                    {
                        string card_top = userRedemptionEx.bank_card.Substring(0, 6);
                        string card_last = userRedemptionEx.bank_card.Substring(userRedemptionEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.RedemptionLoan(id, loan_id, loan_name, loan_time, userRedemptionEx.user_id, userRedemptionEx.bank_card, card_top, card_last, userRedemptionEx.amount_Actual);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "赎回状态不正确！"
                });
            }
        }
        /// <summary>
        /// 重新放款（赎回）
        /// </summary>
        /// <returns></returns>
        public ActionResult RedemptionReLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 4)
            {
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取赎回用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserRedemptionEx userRedemptionEx = ri_info.Data;
                    if (userRedemptionEx.bank_card != "" && userRedemptionEx.bank_card.Length > 6)
                    {
                        string card_top = userRedemptionEx.bank_card.Substring(0, 6);
                        string card_last = userRedemptionEx.bank_card.Substring(userRedemptionEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.RedemptionReLoan(id, loan_id, loan_name, loan_time, userRedemptionEx.user_id, userRedemptionEx.bank_card, card_top, card_last, userRedemptionEx.amount_Actual);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }

        }
        /// <summary>
        /// 全部通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassAllRedemption()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserRedemption(0);
            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            if (result.Result && result.Data != null&&result.Data.Count>0)
            {
                List<int> list = result.Data;
                foreach (int id in list)
                {
                    ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                    {
                        newWithdrawService.UpdateRedemptionStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }
            return Content("ok");
        }
        /// <summary>
        /// 全部放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanAllRedemption()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserRedemption(1);

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;

            if (result.Result && result.Data != null&&result.Data.Count>0)
            {
                List<int> list = result.Data;
                UserRedemptionEx userRedemptionEx;
                string card_top;
                string card_last;
                foreach (int id in list)
                {
                    ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                    {
                        if (ri_info.Data.amount_Actual > ri_info.Data.account_amount_blance)
                        {
                            return Json(new
                            {
                                Message = "用户账户余额不足，放款失败！"
                            });
                        }
                        //获取赎回用户信息
                        userRedemptionEx = ri_info.Data;
                        if (userRedemptionEx.bank_card != "" && userRedemptionEx.bank_card.Length > 6)
                        {
                            card_top = userRedemptionEx.bank_card.Substring(0, 6);
                            card_last = userRedemptionEx.bank_card.Substring(userRedemptionEx.bank_card.Length - 4, 4);
                            newWithdrawService.RedemptionLoan(id, loan_id, loan_name, loan_time, userRedemptionEx.user_id, userRedemptionEx.bank_card, card_top, card_last, userRedemptionEx.amount_Actual);
                        }

                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }
            return Content("ok");
        }
        /// <summary>
        /// 批量通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassChooseRedemption()
        {
            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            NewWithdrawService newWithdrawService = new NewWithdrawService();

            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            foreach (int id in list)
            {
                ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                {
                    newWithdrawService.UpdateRedemptionStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanChooseRedemption()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();

            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;


            UserRedemptionEx userRedemptionEx;
            string card_top;
            string card_last;

            foreach (int id in list)
            {
                ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                {
                    if (ri_info.Data.amount_Actual > ri_info.Data.account_amount_blance)
                    {
                        return Json(new
                        {
                            Message = "用户账户余额不足，放款失败！"
                        });
                    }
                    //获取赎回用户信息
                    userRedemptionEx = ri_info.Data;
                    if (userRedemptionEx.bank_card != "" && userRedemptionEx.bank_card.Length > 6)
                    {
                        card_top = userRedemptionEx.bank_card.Substring(0, 6);
                        card_last = userRedemptionEx.bank_card.Substring(userRedemptionEx.bank_card.Length - 4, 4);
                        newWithdrawService.RedemptionLoan(id, loan_id, loan_name, loan_time, userRedemptionEx.user_id, userRedemptionEx.bank_card, card_top, card_last, userRedemptionEx.amount_Actual);
                    }

                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 查看审核未通过原因
        /// </summary>
        /// <returns></returns>
        public ActionResult LookRedemptionDenyRemark()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserRedemptionEx> ri_info = newWithdrawService.GetUserRedemptionInfoByID(id);
            string remark = "";
            if (ri_info.Result && ri_info.Data != null)
            {
                remark = ri_info.Data.remark;
            }
            if (remark == null)
            {
                remark = "";
            }
            return Content(remark);
        }
        #endregion
        #region 保理提现
        /// <summary>
        /// 保理提现
        /// </summary>
        /// <returns></returns>
        public ActionResult FactoringWithdraw()
        {
            return View();
        }
        /// <summary>
        /// 用户提现
        /// </summary>
        public ActionResult FactoringWithdraw_ajax()
        {
            string LoginName = Convert.ToString(Request["LoginName"]);
            string MyRealName = Convert.ToString(Request["MyRealName"]);
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
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<UserWithDrawEx>> result = newWithdrawService.GetUserWithDrawList(LoginName, MyRealName, s_ctime, e_ctime, status, 1, PageIndex, PageSize, out TotalCount);
            if (result.Result && result.Data != null)
            {
                List<UserWithDrawEx> list = result.Data;
                var Rows = list.ConvertAll(entity => new
                {
                    id = entity.id,
                    login_name = YxLica.Tools.Common.GetHideStr(entity.login_name,3,4),
                    real_name = entity.real_name,
                    bank_name = entity.bank_name,
                    bank_card = YxLica.Tools.Common.GetHideStr(entity.bank_card,6,4),
                    bank_phone = YxLica.Tools.Common.GetHideStr(entity.bank_phone,3,4),
                    amount = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount),
                    //amount_balance = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_balance),
                    //amount_principal = YxLiCai.Tools.Const.SystemConst.MoenyConvert(entity.amount_principal),
                    c_time = entity.c_time.ToString("yyyy-MM-dd HH:mm:ss"),
                    auditor_name = entity.auditor_name,
                    audit_time = (entity.audit_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.audit_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    loan_name = entity.loan_name,
                    loan_time = (entity.loan_time.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : entity.loan_time.ToString("yyyy-MM-dd HH:mm:ss")),
                    status = entity.status


                }).ToList();
                return Json(new
                {
                    Rows = Rows,
                    PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
                });
            }

            return Json(new
            {
                Message = "没有数据"
            });
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateFactoringWithdrawStatus()
        {
            int id = Convert.ToInt32(Request["id"]);
            int status = Convert.ToInt32(Request["status"]);
            string remark = Request["remark"].Trim();
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
            {
                int auditor_id = UserContext.simpleUserInfoModel.Id;
                string auditor_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime audit_time = DateTime.Now;
                ResultInfo<bool> result = newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                if (result.Result && result.Data)
                {
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
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }
        }
        /// <summary>
        /// 放款
        /// </summary>
        /// <returns></returns>
        public ActionResult FactoringWithdrawLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
            {
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取提现用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserWithDrawEx userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        string card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        string card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.FactoringWithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }
        }
        /// <summary>
        /// 重新放款
        /// </summary>
        /// <returns></returns>
        public ActionResult FactoringWithdrawReLoan()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 4)
            {
                int loan_id = UserContext.simpleUserInfoModel.Id;
                string loan_name = UserContext.simpleUserInfoModel.LoginName;
                DateTime loan_time = DateTime.Now;

                //获取提现用户信息
                if (ri_info.Result && ri_info.Data != null)
                {
                    UserWithDrawEx userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        string card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        string card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        ResultInfo<bool> result = newWithdrawService.FactoringWithdrawReLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        if (result.Result && result.Data)
                        {
                            return Content("ok");
                        }
                        return Json(new
                        {
                            Message = "放款失败！"
                        });
                    }
                }

                return Json(new
                {
                    Message = "用户信息不完整！"
                });
            }
            else
            {
                return Json(new
                {
                    Message = "提现状态不正确！"
                });
            }

        }
        /// <summary>
        /// 全部通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassAllFactoringWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserWithDraw(0, 1);
            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            if (result.Result && result.Data != null&&result.Data.Count>0)
            {
                List<int> list = result.Data;
                foreach (int id in list)
                {
                    ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                    {
                        newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }
            return Content("ok");
        }
        /// <summary>
        /// 全部放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanAllFactoringWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<List<int>> result = newWithdrawService.GetAllUserWithDraw(1, 1);

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;

            if (result.Result && result.Data != null&&result.Data.Count>0)
            {
                List<int> list = result.Data;
                UserWithDrawEx userWithDrawEx;
                string card_top;
                string card_last;
                foreach (int id in list)
                {
                    ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                    if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                    {
                        if (ri_info.Data.amount > ri_info.Data.account_amount_blance)
                        {
                            return Json(new
                            {
                                Message = "用户账户余额不足，放款失败！"
                            });
                        }
                        //获取提现用户信息
                        userWithDrawEx = ri_info.Data;
                        if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                        {
                            card_top = userWithDrawEx.bank_card.Substring(0, 6);
                            card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                            newWithdrawService.FactoringWithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                        }

                    }
                }
            }
            else
            {
                return Json(new
                {
                    Message = "没有可操作数据！"
                });
            }

            return Content("ok");
        }
        /// <summary>
        /// 批量通过
        /// </summary>
        /// <returns></returns>
        public ActionResult PassChooseFactoringWithdraw()
        {
            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            NewWithdrawService newWithdrawService = new NewWithdrawService();

            int status = 1;
            string remark = "";
            int auditor_id = UserContext.simpleUserInfoModel.Id;
            string auditor_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime audit_time = DateTime.Now;

            foreach (int id in list)
            {
                ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 0)
                {
                    newWithdrawService.UpdateWithdrawStatus(id, status, auditor_id, audit_time, auditor_name, remark);
                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 批量放款
        /// </summary>
        /// <returns></returns>
        public ActionResult LoanChooseFactoringWithdraw()
        {
            NewWithdrawService newWithdrawService = new NewWithdrawService();

            string ids = Request["ids"];
            List<int> list = new List<int>();
            List<string> list_temp = ids.Split(',').ToList();
            if (list_temp != null && list_temp.Count > 0)
            {
                foreach (string item in list_temp)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            int loan_id = UserContext.simpleUserInfoModel.Id;
            string loan_name = UserContext.simpleUserInfoModel.LoginName;
            DateTime loan_time = DateTime.Now;


            UserWithDrawEx userWithDrawEx;
            string card_top;
            string card_last;

            foreach (int id in list)
            {
                ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
                if (ri_info.Result && ri_info.Data != null && ri_info.Data.status == 1)
                {
                    //获取提现用户信息
                    userWithDrawEx = ri_info.Data;
                    if (userWithDrawEx.bank_card != "" && userWithDrawEx.bank_card.Length > 6)
                    {
                        card_top = userWithDrawEx.bank_card.Substring(0, 6);
                        card_last = userWithDrawEx.bank_card.Substring(userWithDrawEx.bank_card.Length - 4, 4);
                        newWithdrawService.FactoringWithdrawLoan(id, loan_id, loan_name, loan_time, userWithDrawEx.user_id, userWithDrawEx.bank_card, card_top, card_last, userWithDrawEx.amount);
                    }

                }
            }


            return Content("ok");
        }
        /// <summary>
        /// 查看审核未通过原因
        /// </summary>
        /// <returns></returns>
        public ActionResult LookFactoringWithdrawDenyRemark()
        {
            int id = Convert.ToInt32(Request["id"]);
            NewWithdrawService newWithdrawService = new NewWithdrawService();
            ResultInfo<UserWithDrawEx> ri_info = newWithdrawService.GetUserWithDrawInfoByID(id);
            string remark = "";
            if (ri_info.Result && ri_info.Data != null)
            {
                remark = ri_info.Data.remark;
            }
            if (remark == null)
            {
                remark = "";
            }
            return Content(remark);
        }
        #endregion
    }
}
