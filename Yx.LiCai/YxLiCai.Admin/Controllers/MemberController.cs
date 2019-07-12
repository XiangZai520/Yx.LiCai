using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLica.Tools.Pay.LLWYPay;
using YxLiCai.Admin.Models;
using YxLiCai.Model;
using YxLiCai.Model.ExtendModel;
using YxLiCai.Model.User;
using YxLiCai.Server.Member;
using YxLiCai.Server.MenuSet;
using YxLiCai.Server.SendMsg;
using YxLiCai.Server.User;
using YxLiCai.Server.UserAccumulatedEarnings;
using YxLiCai.Tools;

namespace YxLiCai.Admin.Controllers
{
    public class MemberController : BaseController
    {
        /// <summary>
        /// 会员列表页
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 异步获取会员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMemberInfoList_ajax()
        {
            string LoginName = Convert.ToString(Request["LoginName"]);
            string MyRealName = Convert.ToString(Request["MyRealName"]);
            string Phone = Convert.ToString(Request["Phone"]);
            DateTime S_RegTime;
            string _S_RegTime = Convert.ToString(Request["S_RegTime"]);
            if (_S_RegTime.Trim() == "")
            {
                _S_RegTime = "1900-01-01";
            }
            S_RegTime = Convert.ToDateTime(_S_RegTime);
            DateTime E_RegTime;
            string _E_RegTime = Convert.ToString(Request["E_RegTime"]);
            if (_E_RegTime.Trim() == "")
            {
                _E_RegTime = "9999-01-01";
            }
            E_RegTime = Convert.ToDateTime(_E_RegTime);
            E_RegTime = E_RegTime.AddDays(1);
            int Status = Convert.ToInt32(Request["Status"]);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int TotalCount = 0;
            MemberManageService memberManageService = new MemberManageService();
            ResultInfo<List<MemberInfoEx>> ri = memberManageService.GetMemberInfo(LoginName, MyRealName, Phone, S_RegTime, E_RegTime, Status, PageIndex, PageSize, out TotalCount);


            return Json(new
            {
                Rows = ri.Data,
                PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
            });
        }


        /// <summary>
        /// 会员详情页
        /// </summary>
        /// <returns></returns>
        //[AuthorityValidateAttribute]
        public ActionResult MemberDetail()
        {
            long UserID = Convert.ToInt64(Request["UserID"]);
            MemberDetailOutModel outModel = new MemberDetailOutModel();
            YxLiCai.Server.User.UserInfoService userInfoService = new YxLiCai.Server.User.UserInfoService();
            //用户数据
            outModel.userInfo = userInfoService.GetUserModel(UserID).Data;
            if (outModel.userInfo == null)
            {
                outModel.userInfo = new Model.User.UserInfoModel();
            }
            if (!string.IsNullOrEmpty(outModel.userInfo.LoginName) && outModel.userInfo.LoginName.Length > 8)
            {
                outModel.userInfo.LoginName = outModel.userInfo.LoginName.Substring(0, 3) + "***" + outModel.userInfo.LoginName.Substring(7, 4);
            }
            if (!string.IsNullOrEmpty(outModel.userInfo.Phone) && outModel.userInfo.Phone.Length > 8)
            {
                outModel.userInfo.Phone = outModel.userInfo.Phone.Substring(0, 3) + "***" + outModel.userInfo.Phone.Substring(7, 4);
            }
            if (!string.IsNullOrEmpty(outModel.userInfo.MyRealCard) && outModel.userInfo.MyRealCard.Length > 14)
            {
                outModel.userInfo.MyRealCard = outModel.userInfo.MyRealCard.Substring(0, 6) + "***" + outModel.userInfo.MyRealCard.Substring((outModel.userInfo.MyRealCard.Length - 4), 4);
            }
            //银行卡数据
            YxLiCai.Model.User.UserBankCardModel UserBankCard = userInfoService.GetBankCard(UserID).Data;
            if (UserBankCard != null)
            {
                outModel.BankCardNo = UserBankCard.BankCard;
                outModel.BankName = UserBankCard.BankName;
                if (!string.IsNullOrEmpty(outModel.BankCardNo) && outModel.BankCardNo.Length > 8)
                {
                    outModel.BankCardNo = outModel.BankCardNo.Substring(0, 6) + "***" + outModel.BankCardNo.Substring((outModel.BankCardNo.Length - 4), 4);
                }
            }
            //投资记录
            YxLiCai.Server.Order.OrderInfoService orderInfoService = new Server.Order.OrderInfoService();
            outModel.PurchaseRecordList = orderInfoService.GetMemberPurchaseRecord(UserID).Data;
            //提现记录
            MemberManageService memberManageService = new MemberManageService();
            outModel.WithdrawRecordList = memberManageService.GetWithDrawListByUserID(UserID).Data;
            //赎回记录
            outModel.RedemptionRecordList = memberManageService.GetRedemptionRecordListByUserID(UserID).Data;
            //资产数据
            UserCountService userCountService = new UserCountService();
            UserCountMonthService userCountMonthService = new UserCountMonthService();
            UserCountSeasonService userCountSeasonService = new UserCountSeasonService();
            UserCountYearService userCountYearService = new UserCountYearService();
            UserAccumulatedEarningsService userMoney = new UserAccumulatedEarningsService();

            //outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(userCountService.GetZongMoney(UserID).Data);
            //outModel.ZiYouMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert((userCountMonthService.GetZiMoney(UserID).Data + userCountSeasonService.GetZiMoney(UserID).Data + userCountYearService.GetZiMoney(UserID).Data));
            //
            outModel.TotalMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(userCountService.GetAllMyMoney(UserID).Data);
            outModel.ZiYouMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(userCountService.GetZongMoney(UserID).Data);
            UserInfoService userService = new UserInfoService();
            var result3 = userService.GetUserSpecialAssets(UserID);
            decimal t_amount_invest = result3.Result == true ? YxLiCai.Tools.Const.SystemConst.MoenyConvert(result3.Data) : 0.00m;

            outModel.TeXiangMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(t_amount_invest);
            outModel.TotalIncome = YxLiCai.Tools.Const.SystemConst.MoenyConvert((userMoney.GetUserMonth(UserID) + userMoney.GetUserSeason(UserID, "0") + userMoney.GetUserSeason(UserID, "1") + userMoney.GetUserYear(UserID)));
            outModel.TotalHongBao = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
            outModel.KeShuHuiMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(0);
            outModel.KeTiXianMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(userInfoService.GetUserWithdrawals(UserID).Data);
            return View(outModel);
        }

        /// <summary>
        ///黑名单页面
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult BlackList()
        {
            return View();
        }
        /// <summary>
        /// 黑名单页面（异步获取数据）
        /// </summary>
        /// <returns></returns>
        public ActionResult BlackList_ajax()
        {
            string LoginName = Convert.ToString(Request["LoginName"]);
            string MyRealName = Convert.ToString(Request["MyRealName"]);
            string OprateName = Convert.ToString(Request["OprateName"]);
            DateTime S_RegTime;
            string _S_RegTime = Convert.ToString(Request["S_RegTime"]);
            if (_S_RegTime.Trim() == "")
            {
                _S_RegTime = "1900-01-01";
            }
            S_RegTime = Convert.ToDateTime(_S_RegTime);
            DateTime E_RegTime;
            string _E_RegTime = Convert.ToString(Request["E_RegTime"]);
            if (_E_RegTime.Trim() == "")
            {
                _E_RegTime = "9999-01-01";
            }
            E_RegTime = Convert.ToDateTime(_E_RegTime);
            E_RegTime = E_RegTime.AddDays(1);
            int PageIndex = Convert.ToInt32(Request["PageIndex"]);
            int PageSize = Convert.ToInt32(Request["PageSize"] == null ? "10" : Request["PageSize"]);
            int TotalCount = 0;
            MemberManageService memberManageService = new MemberManageService();
            ResultInfo<List<UserBlackListEx>> ri = memberManageService.GetBlackList(LoginName, MyRealName, S_RegTime, E_RegTime, OprateName, PageIndex, PageSize, out TotalCount);


            return Json(new
            {
                Rows = ri.Data,
                PageHTML = YxLiCai.Admin.PublicCode.PagerOprate.CreatePagerHTML(TotalCount, PageIndex)
            });
        }

        /// <summary>
        /// 取消拉黑
        /// </summary>
        /// <returns></returns>
        [AuthorityValidateAttribute]
        public ActionResult CancleBlackList_ajax()
        {
            long userid = Convert.ToInt64(Request["userid"]);
            int accountid = UserContext.simpleUserInfoModel.Id;
            MemberManageService memberManageService = new MemberManageService();
            bool result = memberManageService.CancleBlackList(userid, accountid);
            if (result)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 8;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "---取消用户拉黑：用户ID：" + userid;
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                //return Content("{Message:'取消失败请重试！'}");
                return Json(new
                {
                    Message = "取消失败请重试！"
                });
            }
        }

        /// <summary>
        /// 重置登录、交易密码
        /// </summary>
        /// <returns></returns>
        //[AuthorityValidateAttribute]
        public ActionResult ReSetPw()
        {
            string PwType = Request["PwType"].ToString();
            long userid = Convert.ToInt64(Request["userid"]);
            YxLiCai.Server.User.UserInfoService userInfoService = new YxLiCai.Server.User.UserInfoService();
            //用户数据
            UserInfoModel userInfo = userInfoService.GetUserModel(userid).Data;
            bool result = false;
            //更新密码
            string PwTypeName = "";
            if (userInfo != null)
            {
                string phone = userInfo.Phone;
                //生成密码
                string NewPw = Validate_Code.GetRandomCode(6);
                string NewPw_MD5 = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(NewPw, 32); ;

                if (PwType.Trim() == "LoginPw")
                {
                    PwTypeName = "登录密码";
                    result = userInfoService.UpdateUserPassWord(userid, NewPw_MD5).Data;
                }
                else if (PwType.Trim() == "TradePw")
                {
                    PwTypeName = "交易密码";
                    result = userInfoService.UpdateUserSallpassword(userid, NewPw_MD5).Data;
                }
                if (result)
                {
                    //发送短信 
                    string MsgContent = PwTypeName + "重置成功！新密码：" + NewPw; 
                    YxLiCai.Server.SendMsg.MessageFactory.Instance.SendMessage(phone, MsgContent); 
                }

            }
            if (result)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 8;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "---重置用户" + PwTypeName + "用户ID：" + userid;
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                //return Content("{Message:'重置失败！'}");
                return Json(new
                {
                    Message = "重置失败！"
                });
            }
        }
        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <returns></returns>
        public ActionResult JoinBlacklist()
        {
            string Remark = Request["Remark"].ToString();
            long userid = Convert.ToInt64(Request["userid"]);
            MemberManageService memberManageService = new YxLiCai.Server.Member.MemberManageService();
            bool result = memberManageService.JoinUserBlacklist(Remark, userid, UserContext.simpleUserInfoModel.Id, UserContext.simpleUserInfoModel.LoginName).Data;
            if (result)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 8;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "----会员加入黑名单用户ID：" + userid;
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                //return Content("{Message:'拉黑失败！'}");
                return Json(new
                {
                    Message = "拉黑失败！"
                });
            }
        }
        /// <summary>
        /// 解除绑定银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult UnbindBankCard()
        {
            long userid = Convert.ToInt64(Request["userid"]);
            bool result = false;
            UserBankCardService userBankCardService = new UserBankCardService();
            UserBankCardModel userBankCardModel = userBankCardService.GetEntity(userid).Data;
            if (userBankCardModel != null)
            {
                //调用接口 解除绑定银行卡
                YxLiCai.Tools.Pay.Yeepay.YJPay yjPay = new Tools.Pay.Yeepay.YJPay();

                string str = yjPay.unbind(userBankCardModel.FirstNum, userBankCardModel.LastNum, userid.ToString(), 2);
                YxLiCai.Tools.LogHelper.Write("解除绑定卡请求", "userid:" + userid.ToString() + "--返回结果:" + str);
                if (str.IndexOf("error") < 0)
                {
                    //更新本地数据库 为未绑定（伪删除）
                    result = userBankCardService.UnbindBankCard(userid).Data;
                    UserInfoService user = new UserInfoService();
                    ResultInfo<UserInfoModel> ri_uim = user.GetUserModel(userid);
                    if (ri_uim.Result)
                    {
                        SmallUserInfo SmallUser = new SmallUserInfo();
                        SmallUser.Id = ri_uim.Data.id;
                        SmallUser.Account = ri_uim.Data.Phone;
                        SmallUser.IsBindBank = ri_uim.Data.IsBindBank;
                        SmallUser.IsJiaoYIPW = 0;
                        if (!string.IsNullOrEmpty(ri_uim.Data.Sallpassword))
                        {
                            SmallUser.IsJiaoYIPW = 1;
                        }
                        SmallUser.IsRealCard = ri_uim.Data.IsRealCard;
                        SmallUser.MyCode = ri_uim.Data.MyCode;
                        SmallUser.MyRealName = ri_uim.Data.MyRealName;
                        SmallUser.MyRealCard = ri_uim.Data.MyRealCard;
                        SmallUser.BankName = ri_uim.Data.BankName;
                        SmallUser.LastBankNum = ri_uim.Data.LastBankNum;
                        string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, userid);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                        redis.Replace(Userkey, SmallUser, DateTime.Now.AddDays(5));
                    }
                }
            }
            if (result)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 8;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "---会员解除银行卡解除成功用户ID：" + userid;
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                //return Content("{Message:'解除绑定失败！'}");
                return Json(new
                {
                    Message = "解除绑定失败！"
                });
            }
        }
        /// <summary>
        /// 解除绑定银行卡(连连支付)
        /// </summary>
        /// <returns></returns>
        public ActionResult UnbindBankCard_LL()
        {
            string userid = Request["userid"];
            string no_agree = Request["no_agree"];
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(no_agree) || userid=="0")
            {
                return Json(new
                {
                    Message = "信息不完整！"
                });
            }

            bool result = false;
            UserBankCardService userBankCardService = new UserBankCardService();
            UserBankCardModel userBankCardModel = userBankCardService.GetEntity(Convert.ToInt64(userid)).Data;
            if (userBankCardModel != null)
            {
                //解绑用户银行卡
                string str = LLPay.RelieveBankID(userid.ToString(), no_agree);
                LL_Message LL = SerializeHelper.JsonDeserialize<LL_Message>(str);
                LogHelper.Write("解除绑定卡请求", "userid:" + userid.ToString() + "--返回结果:" + str);
                if (LL.ret_code == "0000" && LL.ret_msg == "交易成功")
                {
                    //更新本地数据库 为未绑定（伪删除）
                    result = userBankCardService.UnbindBankCard(Convert.ToInt64(userid)).Data;
                    UserInfoService user = new UserInfoService();
                    ResultInfo<UserInfoModel> ri_uim = user.GetUserModel(Convert.ToInt64(userid));
                    if (ri_uim.Result)
                    {
                        SmallUserInfo SmallUser = new SmallUserInfo();
                        SmallUser.Id = ri_uim.Data.id;
                        SmallUser.Account = ri_uim.Data.Phone;
                        SmallUser.IsBindBank = ri_uim.Data.IsBindBank;
                        SmallUser.IsJiaoYIPW = 0;
                        if (!string.IsNullOrEmpty(ri_uim.Data.Sallpassword))
                        {
                            SmallUser.IsJiaoYIPW = 1;
                        }
                        SmallUser.IsRealCard = ri_uim.Data.IsRealCard;
                        SmallUser.MyCode = ri_uim.Data.MyCode;
                        SmallUser.MyRealName = ri_uim.Data.MyRealName;
                        SmallUser.MyRealCard = ri_uim.Data.MyRealCard;
                        SmallUser.BankName = ri_uim.Data.BankName;
                        SmallUser.LastBankNum = ri_uim.Data.LastBankNum;
                        string Userkey = string.Format(YxLiCai.Tools.Const.RedisCacheKey.CaptchaUserReadsModel, userid);
                        var redis = new YxLiCai.Tools.NoSql.RedisCache.RedisCache();
                        redis.Replace(Userkey, SmallUser, DateTime.Now.AddDays(5));
                    }
                }
            }
            if (result)
            {
                #region 添加日志
                SysActionLogModel smodel = new SysActionLogModel();
                smodel.CTime = DateTime.Now;
                smodel.OpType = 8;
                smodel.adminName = UserContext.simpleUserInfoModel.UserName;
                smodel.adminId = UserContext.simpleUserInfoModel.Id;
                smodel.Remark = UserContext.simpleUserInfoModel.UserName + "---会员解除银行卡解除成功用户ID：" + userid;
                SysService.AddSysLog(smodel);
                #endregion
                return Content("ok");
            }
            else
            {
                //return Content("{Message:'解除绑定失败！'}");
                return Json(new
                {
                    Message = "解除绑定失败！"
                });
            }
        }
    }
}
public class LL_Message
{
    public string ret_code { get; set; }
    public string ret_msg { get; set; }
    public string sign_type { get; set; }
    public string sign { get; set; }
}