using System;
using System.Text;
using System.IO;
using System.Net;
using YxLiCai.Model;
using YxLiCai.Tools;
namespace YxLiCai.Server.SendMsg
{
    public class Send
    {
        #region 变量
        //发送短信参数地址
        private string urlAddress = "http://cloud.baiwutong.com:8080/post_sms.do?";
        //发送短信参数用户信息
        private string postData = "id={0}&MD5_td_code={1}&mobile={2}&msg_content={3}&msg_id={4}&ext={5}"; // 要发放的数据
        //用户名
        private readonly string sms_UID = System.Configuration.ConfigurationManager.AppSettings["SMS_UID_New"];
        //接口密码
        private readonly string sms_KEY = System.Configuration.ConfigurationManager.AppSettings["SMS_KEY_New"];
        #endregion

        #region 公共方法

        /// <summary>
        /// 发送短信（多个接收人时，手机号码需要用半角逗号隔开）
        ///131*******, 132*******,133********
        /// </summary>
        /// <param name="toMobile">接收人手机号</param>
        /// <param name="Content">短信内容</param>
        /// <returns></returns>
        public ResultInfo<string> SendMsg(string toMobile, string Content)
        {
            ResultInfo<string> result = new ResultInfo<string>();
            result.Result = false;
            result.Message = "未知错误!";
            result.Data = string.Empty; 
            try
            {
                //发送错误日志邮件
                if (YxLiCai.Tools.Const.SystemConst.IsSendMail == "false")
                {
                    return result;
                }     
                if (string.IsNullOrEmpty(Content))
                { 
                    return result;
                }
                string smsUrl = urlAddress + string.Format(postData, sms_UID, sms_KEY, toMobile, Content, "", "");

                string str = "";
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    str = GetHtmlFromUrl(smsUrl);
                    YxLiCai.Tools.LogHelper.Write("短信发送记录日志", DateTime.Now + "电话:" + toMobile + "--发送内容:" + Content + "--返回值:" + str);
                    if (str.Contains("#"))
                    {
                        result.Result = true;
                        result.Message = "发送成功";
                        User.UserInfoService.AddUserSendMesg(toMobile, Content);
                    }
                    else
                    {
                        result.Result = false;
                        result.Message = GetEnumValue(str);
                        YxLiCai.Tools.LogHelper.Write("短信发送错误记录日志", DateTime.Now + "电话:" + toMobile + "--发送内容:" + Content + "--返回值:" + str + "返回信息：" + GetEnumValue(str));
                    }
                });
                result.Result = true;
                result.Message = "发送成功";
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;

        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 发送短信方法：调用时只需要把拼成的URL传给该函数即可。判断返回值即可
        /// </summary>
        /// <param name="url">发送短信的地址</param>
        /// <returns></returns>
        private string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            else
            {
                string targeturl = url.Trim().ToString();
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "POST";
                hr.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);
                return ser.ReadToEnd();
            }
            //return strRet;
        }
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostDataGetHtml(string uri, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uRI = new Uri(uri);
                HttpWebRequest req = WebRequest.Create(uRI) as HttpWebRequest;
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                Stream inStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

                return htmlResult;
            }
            catch (Exception ex)
            {
                return "网络错误：" + ex.Message.ToString();
            }
        }

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
        #endregion
    }
    
}