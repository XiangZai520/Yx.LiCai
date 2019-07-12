using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.Web;

namespace YxLiCai.DataHelper.DBUtility
{
    /// <summary>
    /// 数据操作类中处理跟踪信息的相关对象
    /// </summary>
    public class TraceHelper
    {
        #region 常量
        private const string UrlParamName = "trace";        //默认开启监测的Url参数名称
        private const string UrlParamValue = "true";        //默认开启监测的Url参数值
        #endregion

        #region 私有函数
        /// <summary>
        /// 判断当前请求是否启用跟踪
        /// </summary>
        /// <returns></returns>
        private static bool RequestIsTrace()
        {
            bool IsTrace = false;
            //对于控制台等没有HTTP上下文的项目不输出调试信息
            if (HttpContext.Current != null)
            {
                string TraceParam = HttpContext.Current.Request.QueryString[UrlParamName] != null ? HttpContext.Current.Request.QueryString[UrlParamName].ToString() : "";

                if (!string.IsNullOrEmpty(TraceParam))
                {
                    if (TraceParam.ToLower() == UrlParamValue)
                    {
                        IsTrace = true;
                    }
                }

                if (IsTrace == true)
                {
                    string IP = GetIP();
                    if (IP.IndexOf("172.16.16") > -1 || IP.IndexOf("219.141.178") > -1 || IP == "127.0.0.1" || IP.IndexOf("10.168.5") > -1 || IP.IndexOf("114.255.58") > -1)
                    {
                        IsTrace = true;
                    }
                    else
                    {
                        IsTrace = false;
                    }
                }
            }

            return IsTrace;
        }

        /// <summary>
        /// 构造SQL参数字符串
        /// </summary>
        /// <param name="Params">SQL参数数组</param>
        /// <returns></returns>
        private static string BuildParameterString(DbParameter[] Params)
        {
            StringBuilder myBuilder = new StringBuilder();

            if (Params != null && Params.Length > 0)
            {
                for (int i = 0; i < Params.Length; i++)
                {
                    myBuilder.Append(BuildParameter(Params[i]));
                }
            }

            return myBuilder.ToString();
        }

        /// <summary>
        /// 构造单个参数字符串
        /// </summary>
        /// <param name="Param">当前参数</param>
        /// <returns></returns>
        private static string BuildParameter(DbParameter Param)
        {
            string DeclareFormat = "DECLARE {0} {1} ";
            string ValueFormat = "SET {0} = {1} ";

            string returnValue = "";

            switch (Param.DbType)
            {
                case DbType.Boolean:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "BIT");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, Param.Value);
                    break;
                case DbType.Date:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "DATE");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, "'" + Param.Value + "'");
                    break;
                case DbType.DateTime:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "DATETIME");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, "'" + Param.Value + "'");
                    break;
                case DbType.Int16:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "SMALLINT");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, Param.Value);
                    break;
                case DbType.Int32:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "INT");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, Param.Value);
                    break;
                case DbType.Int64:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "BIGINT");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, Param.Value);
                    break;
                case DbType.String:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "VARCHAR(" + Param.Size.ToString() + ")");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, "'" + Param.Value + "'");
                    break;
                case DbType.Time:
                    returnValue = string.Format(DeclareFormat, Param.ParameterName, "TIME");
                    returnValue += string.Format(ValueFormat, Param.ParameterName, "'" + Param.Value + "'");
                    break;
                default:
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        private static string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_CIP"];

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !IsIP(result))
            {
                return "127.0.0.1";
            }

            return result;

        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 公共函数
        /// <summary>
        /// 判断当前请求是否显示跟踪信息
        /// </summary>
        /// <param name="Trace">跟踪信息上下文</param>
        /// <param name="Context">HTTP请求上下文</param>
        /// <returns></returns>
        public static bool IsTrace()
        {
            return RequestIsTrace();
            //return false;
        }

        /// <summary>
        /// 构造参数字符串
        /// </summary>
        /// <param name="Params">SQL的参数数组</param>
        /// <returns></returns>
        public static string BuildParameters(DbParameter[] Params)
        {
            return BuildParameterString(Params);
        }
        #endregion
    }
}
