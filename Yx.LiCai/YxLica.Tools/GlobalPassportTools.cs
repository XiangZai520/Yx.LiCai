using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLica.Tools.Pay.LLWYPay;
using System.Net.Http;
using System.Net.Http.Headers;

namespace YxLica.Tools
{
    public class RequestModel
    {
        public string app_key { get; set; }
        public string parameter { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
    }

    public class GlobalPassportTools
    {
        private static string md5_key =ConfigurationManager.AppSettings["global_md5_key"];  
        public static string app_key = ConfigurationManager.AppSettings["global_app_key"];
        private static string global_api_url = ConfigurationManager.AppSettings["global_api_url"];
        //得到时间戳
        public static String getCurrentDateTimeStr()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// 调用接口
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="parameter">接口参数</param>
        /// <returns></returns>
        public static string GetApiResult(string method,string parameter)
        {
            string url = global_api_url + method;
            YxLica.Tools.RequestModel mod = new YxLica.Tools.RequestModel();
            mod.app_key = YxLica.Tools.GlobalPassportTools.app_key;
            mod.parameter = parameter;
            mod.timestamp = YxLica.Tools.GlobalPassportTools.getCurrentDateTimeStr();
            mod.sign = YxLica.Tools.GlobalPassportTools.addSign(mod); 
            string str_result = new HttpClient().PostAsJsonAsync<RequestModel>(url, mod).Result.Content.ReadAsStringAsync().Result; 
            return str_result;
        }
 
        //签名入口
        public static String addSign(RequestModel model)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("app_key", model.app_key);
            sParaTemp.Add("parameter", model.parameter);
            sParaTemp.Add("timestamp", model.timestamp); 
            // 生成签名原串
            String sign_src = genSignData(sParaTemp);  
            sign_src += "&key=" + md5_key; 
            try
            {
                string sign = Md5Algorithm.getInstance().md5Digest(
                    Encoding.UTF8.GetBytes(sign_src)); 
                return sign;

            }
            catch (Exception e)
            { 
                return "";
            }
        }
        // 生成签名原串
        public static String genSignData(SortedDictionary<string, string> sParaTemp)
        {
            StringBuilder content = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParaTemp)
            {
                //"sign"不参与签名
                if ("sign".Equals(temp.Key))
                {
                    continue;
                }
                // 空串不参与签名
                if (isnull(temp.Value))
                {
                    continue;
                }
                content.Append("&" + temp.Key + "=" + temp.Value);
            }
            String signSrc = content.ToString();
            if (signSrc.StartsWith("&"))
            {
                signSrc = signSrc.Substring(1);
            }
            return signSrc;
        }

        //验签入口
        public static bool checkSign(SortedDictionary<string, string> sParaTemp)
        { 
            if (sParaTemp == null)
            {
                return false;
            }
            string app_key;
            sParaTemp.TryGetValue("app_key", out app_key);

            String sign;
            if (!sParaTemp.TryGetValue("sign", out sign))
            {
                return false;
            }

            // 生成签名原串
            String sign_src = genSignData(sParaTemp);
            sign_src += "&key=" + md5_key;
            try
            {
                if (sign.Equals(Md5Algorithm.getInstance().md5Digest(
                    Encoding.UTF8.GetBytes(sign_src))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //判断字符串是否为空
        public static bool isnull(String str)
        {

            if (null == str || str.ToLower().Equals("null") || str.Equals(""))
            {
                return true;
            }
            else
                return false;
        }

        //有序字典（最简单的有序字典）转为json
        public static string dictToJson(SortedDictionary<string, string> dict)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            foreach (KeyValuePair<string, string> temp in dict)
            {
                json.Append("\"" + temp.Key + "\"" + ":" + "\"" + temp.Value + "\"");
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("}");
            string content = json.ToString();
            return content;
        }

    }
}
