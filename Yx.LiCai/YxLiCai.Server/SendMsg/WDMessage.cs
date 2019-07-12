using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Model;
using YxLiCai.Tools;

namespace YxLiCai.Server.SendMsg
{
    /// <summary>
    /// 错误值枚举
    /// </summary>
    public enum ErrorCodee
    {
        余额不足 = -10,
        账号关闭 = -11,
        短信内容超过500字或为空 = -12,
        手机号码超过200个或合法的手机号码为空或者手机号码与通道代码正则不匹配 = -13,
        用户名不存在 = -16,
        msg_id超过50个字符或没有传msg_id字段 = -14,
        访问ip错误 = -18,
        密码错误或者业务代码错误或者通道关闭或者业务关闭 = -19,
        小号错误 = -20
    }
    public class WDMessage : BaseMessage
    {
        #region 变量
        //发送短信参数地址
        private string urlAddress = "http://cloud.baiwutong.com:8080/post_sms.do";
        //发送短信参数用户信息
        private string postData = "id={0}&MD5_td_code={1}&mobile={2}&msg_content={3}&msg_id={4}&ext={5}"; // 要发放的数据
        //用户名
        private readonly string sms_UID = System.Configuration.ConfigurationManager.AppSettings["SMS_UID_New"];
        //接口密码
        private readonly string sms_KEY = System.Configuration.ConfigurationManager.AppSettings["SMS_KEY_New"];
        #endregion

        /// <summary>
        /// 根据key获取枚举值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        private string GetEnumValue(string key)
        {
            string name = Enum.Parse(typeof(ErrorCodee), key).ToString();
            return name; 
        }

        /// <summary>
        /// 发送短信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        public override ResultInfo<string> SendMessage(string phone, string content)
        {
            ResultInfo<string> result = new ResultInfo<string>();
            result.Result = true;
            result.Message = "";
            result.Data = "";
            try
            {
                //发送错误日志邮件
                if (YxLiCai.Tools.Const.SystemConst.IsSendMail == "false")
                {
                    return result;
                }
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }
                string smsUrl = string.Format(postData, sms_UID, sms_KEY, phone, content, "", "");

                string str = "";
               System.Threading.Tasks.Task.Factory.StartNew(() =>
               {
                str = Tools.Util.UrlResponse.GetResponseString(urlAddress, smsUrl);
                    YxLiCai.Tools.LogHelper.Write("短信发送记录日志", DateTime.Now + "电话:" + phone + "--发送内容:" + content + "--返回值:" + str);
                    if (str.Contains("#"))
                    {
                        result.Result = true;
                        User.UserInfoService.AddUserSendMesg(phone, content);
                    }
                    else
                    {
                        result.Message = GetEnumValue(str);
                        YxLiCai.Tools.LogHelper.Write("短信发送错误记录日志", DateTime.Now + "电话:" + phone + "--发送内容:" + content + "--返回值:" + str + "返回信息：" + GetEnumValue(str));
                    }
                }); 
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;
        }
    }
}
