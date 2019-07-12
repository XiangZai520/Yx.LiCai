using YxLiCai.Tools.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.Enums
{ 
    public enum SendCheckCodeStatus
    {
        [DisplayText("正在发送")]
        Sending,

        [DisplayText("手机号码无效")]
        InvlidPhoneNumber,

        [DisplayText("发送失败")]
        SendFailure,
        [DisplayText("该用户已注册")]
        AlreadyExists
    }

    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ProjectType
    { 
        [DisplayText("专业项目")]
        专业项目=1,
        [DisplayText("债券转让")]
        债券转让=2,
        [DisplayText("优选计划")]
        优选计划=3,
        [DisplayText("风险租赁")]
        风险租赁=4
       
    }
 
    public enum ModifyPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("两次密码不能相同")]
        PwdIsSame,
    }

    public enum ForgetPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("验证码不能为空")]
        checkCodeIsEmpty,
        [DisplayText("验证码错误")]
        checkCodeWrong,
        [DisplayText("您要找回的密码正是当前密码")]
        PwdIsSave,
    }  
   
}
