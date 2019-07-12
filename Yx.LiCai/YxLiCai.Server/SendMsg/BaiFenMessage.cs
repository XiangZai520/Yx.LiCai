using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YxLiCai.Model;
using YxLiCai.Tools;

namespace YxLiCai.Server.SendMsg
{
    /// <summary>
    /// 百分短信类
    /// </summary>
    public class BaiFenMessage : BaseMessage
    {
        public static string PostUrl = ConfigurationManager.AppSettings["WebReference.Service.PostUrl"];
        public static string sname = ConfigurationManager.AppSettings["sname"];
        public static string spwd = ConfigurationManager.AppSettings["spwd"];
        public static string scorpid = ConfigurationManager.AppSettings["scorpid"];
        public static string sprdid = ConfigurationManager.AppSettings["sprdid"];

        /// <summary>
        /// 发送短信信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        public override YxLiCai.Model.ResultInfo<string> SendMessage(string phone, string content)
        {
            YxLiCai.Model.ResultInfo<string> result = new YxLiCai.Model.ResultInfo<string>();
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
                string postStrTpl = "sname={0}&spwd={1}&scorpid={2}&sprdid={3}&sdst={4}&smsg={5}";

                content = content + "【E休理财】";
                string smsUrl = string.Format(postStrTpl, sname, spwd, scorpid, sprdid, phone, content);

                string str = "";
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    str = Tools.Util.UrlResponse.GetResponseString(PostUrl, smsUrl);
                    YxLiCai.Tools.LogHelper.Write("短信发送记录日志", DateTime.Now + "电话:" + phone + "--发送内容:" + content + "--返回值:" + str);
                    if (str.Contains("提交成功"))//判断百分短信
                    {
                        result.Result = true;
                        User.UserInfoService.AddUserSendMesg(phone, content);
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
