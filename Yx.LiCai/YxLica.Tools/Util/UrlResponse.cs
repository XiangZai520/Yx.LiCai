using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
 

namespace YxLiCai.Tools.Util
{
    /// <summary>
    /// Web请求操作
    /// </summary>
    /// <remarks>
    /// 作者：何剑伟.Jorben
    /// </remarks>
    public class UrlResponse
    {
        #region POST public static string GetResponseString(string apiUrl, string postData, int timeOut = 20000)
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="apiUrl">请求地址</param>
        /// <param name="postData">参数字符串</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>string</returns>
        public static string GetResponseString(string apiUrl, string postData, int timeOut = 20000)
        {
            string errorMessage;
            return GetResponseString(apiUrl, postData, Encoding.UTF8, out errorMessage, null, timeOut);
        }
        #endregion

         #region POST public static string GetResponseString(string apiUrl, string postData, int timeOut = 20000)
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="apiUrl">请求地址</param>
        /// <param name="postData">参数字符串</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>string</returns>
        public static string GetResponseStringASCII(string apiUrl, string postData, int timeOut = 20000)
        {
            string errorMessage;
            return GetResponseString(apiUrl, postData, Encoding.ASCII, out errorMessage, null, timeOut);
        }
        #endregion
        #region POST public static string GetResponseString(string apiUrl, string postData, Encoding encoding, out string errorMessage, int timeOut = 20000)
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="apiUrl">请求地址</param>
        /// <param name="postData">参数字符串</param>
        /// <param name="encoding">接收编码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>string</returns>
        public static string GetResponseString(string apiUrl, string postData, Encoding encoding, out string errorMessage, int timeOut = 20000)
        {
            return GetResponseString(apiUrl, postData, encoding, out errorMessage, null, timeOut);
        }
        #endregion

        #region POST public static string GetResponseString(string apiUrl, string postData, Encoding encoding, out string errorMessage, CookieCollection cookieCollection, int timeOut = 20000)
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="apiUrl">请求地址</param>
        /// <param name="postData">参数字符串</param>
        /// <param name="encoding">接收编码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="cookieCollection">带cookie信息</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>string</returns>
        public static string GetResponseString(string apiUrl, string postData, Encoding encoding, out string errorMessage, CookieCollection cookieCollection, int timeOut = 20000)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = "POST";
            /*
             * 祥仔注释掉新加属性不管用
            httpWebRequest.SendChunked = true;
            httpWebRequest.TransferEncoding = "utf-8;";*/
            httpWebRequest.ContentType = "application/x-www-form-urlencoded;";
            //祥仔新加str
            byte[] byData;
            byData = System.Text.Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = byData.Length;
            //end
            httpWebRequest.Timeout = timeOut;
            //创建Cooike并发送Cooike
            if (cookieCollection != null)
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(cookieCollection);
                httpWebRequest.CookieContainer = cookieContainer;
            }

            errorMessage = string.Empty;
            HttpWebResponse response = null;

            try
            {
                var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(postData);
                streamWriter.Close();

                response = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return errorMessage;
        }
        #endregion

        #region GET public static string GetResponseString(string apiUrl)
        /// <summary>
        /// GET
        /// </summary>
        /// <param name="apiUrl">请求地址</param>
        /// <returns></returns>
        public static string GetResponseString(string apiUrl)
        {
            string errorMessage;
            return GetResponseString(apiUrl, Encoding.UTF8, out errorMessage);
        }
        #endregion

        #region GET public static string GetResponseString(string apiUrl, Encoding encoding, out string errorMessage, int timeOut = 20000)
        /// <summary>
        /// GET
        /// </summary>
        /// <param name="apiUrl">URL</param>
        /// <param name="encoding">接收编码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="timeOut">超时时间（默认值20s）</param>
        /// <returns></returns>
        public static string GetResponseString(string apiUrl, Encoding encoding, out string errorMessage, int timeOut = 20000)
        {
            errorMessage = string.Empty;

            try
            {
                var request = WebRequest.Create(apiUrl);
                request.Method = "GET";
                request.Timeout = timeOut;
                var jsonStream = new StreamReader(request.GetResponse().GetResponseStream(), encoding);
                
                return jsonStream.ReadToEnd();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw ex;
            }

            return string.Empty;
        }
        #endregion


        #region 获取网页文件内容 public static string GetUrlContent(string url, Encoding encoding = null)
        /// <summary>
        /// 获取网页文件内容
        /// </summary>
        /// <param name="url">目标网页Url</param>
        /// <param name="encoding">是目标网页的编码，如果为null，则自动分析网页的编码</param>
        /// <returns></returns>
        public static string GetUrlContent(string url, Encoding encoding = null)
        {
            var webClient = new WebClient { Credentials = CredentialCache.DefaultCredentials }; //创建WebClient实例webClient
            //有的网页可能下不下来，有种种原因比如需要cookie,编码问题等等
            //这是就要具体问题具体分析比如在头部加入cookie
            //webClient.Headers.Add("Cookie", cookie);
            //这样可能需要一些重载方法。根据需要写就可以了

            //获取或设置用于对向 Internet 资源的请求进行身份验证的网络凭据。

            //如果服务器要验证用户名,密码
            //webClient.Credentials = new NetworkCredential(struser, strpassword);;

            //从资源下载数据并返回字节数组。
            var dataBuffer = webClient.DownloadData(url);
            var webData = Encoding.Default.GetString(dataBuffer);

            //获取网页字符编码描述信息
            var charSetMatch = Regex.Match(webData, "<meta([^<]*)charset=([^<\"]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var webCharSet = charSetMatch.Groups[2].Value;

            if (!string.IsNullOrWhiteSpace(webCharSet) && encoding == null)
            {
                encoding = Encoding.GetEncoding(webCharSet);
            }

            if (encoding != null && encoding != Encoding.Default)
            {
                webData = encoding.GetString(dataBuffer);
            }

            return webData;
        }
        #endregion
    }
}
