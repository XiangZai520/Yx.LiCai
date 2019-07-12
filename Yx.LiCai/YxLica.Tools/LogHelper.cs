using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace YxLiCai.Tools
{
   public class LogHelper
    {
       public static Logger logger = NLog.LogManager.GetCurrentClassLogger();
       public static object logLock = new object();
       
       /// <summary>
       /// 异常捕获日志
       /// </summary>
       /// <param name="ex">异常对象（必须为属性类）</param>
       /// <param name="rmark">操作简要描述</param>
       public static void LogWriter(Exception ex, string rmark="")
       {
           try
           {
               string logstr = "\r\n-----------------start----------------------\r\n";
               logstr = logstr + DateTime.Now.ToString() + "\r\n";
               MethodBase m = new StackTrace().GetFrame(1).GetMethod();
               ParameterInfo[] pm = m.GetParameters();
               string classname = m.DeclaringType.ToString();
               string propertyName = m.Name;
               logstr = logstr + "备注:" + rmark + "\r\n";
               //写类名
               logstr = logstr + "函数类名:" + classname + "\r\n";
               //写函数方法
               logstr = logstr + "函数名称为:" + propertyName + "\r\n";
               for (int i = 0; i < pm.Length; i++)
               {
                   logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
               }
               logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
               logstr += "--------------------end---------------------\r\n";
               //写日志
               logger.Info(logstr);
           }
           catch (Exception)
           {
               throw;
           }

       }
       /// <summary>
       /// 带参数 异常的日志
       /// </summary>
       /// <param name="dec">异常参数对象（必须为属性类）</param>
       /// <param name="ex">日志异常对象</param>
       public static void LogWriter(object dec, Exception ex)
       {
           try
           {
               string logstr = "\r\n-----------------start----------------------\r\n";
               logstr = logstr + DateTime.Now.ToString() + "\r\n";
               MethodBase m = new StackTrace().GetFrame(1).GetMethod();
               ParameterInfo[] pm = m.GetParameters();
               string classname = m.DeclaringType.ToString();
               string propertyName = m.Name;
               //写类名
               logstr = logstr + "函数类名:" + classname + "\r\n";
               //写函数方法
               logstr = logstr + "函数名称为:" + propertyName + "\r\n";
               for (int i = 0; i < pm.Length; i++)
               {
                   logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
               }             
               logstr = logstr + "函数参数值:" + JsonConvert.SerializeObject(dec) +"\r\n";
               logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
               logstr += "--------------------end---------------------\r\n";
               //写日志
               logger.Info(logstr);
           }
           catch (Exception)
           {
               throw;
           }

       }
       /// <summary>
       /// 带参数 描述 异常日志捕获
       /// </summary>
       /// <param name="dec">所要捕获的参数（必须为属性类）</param>
       /// <param name="ex">异常对象</param>
       /// <param name="rmark">描述</param>
       public static void LogWriter(object dec, Exception ex, string rmark)
       {
           try
           {
               string logstr = "\r\n-----------------start----------------------\r\n";
               logstr = logstr + DateTime.Now.ToString() + "\r\n";
               MethodBase m = new StackTrace().GetFrame(1).GetMethod();
               ParameterInfo[] pm = m.GetParameters();
               string classname = m.DeclaringType.ToString();
               string propertyName = m.Name;
               //写备注
               logstr = logstr + "备注:" + rmark + "\r\n";
               //写类名
               logstr = logstr + "函数类名:" + classname + "\r\n";
               //写函数方法
               logstr = logstr + "函数名称为:" + propertyName + "\r\n";
               for (int i = 0; i < pm.Length; i++)
               {
                   logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
               }
               logstr = logstr + "函数参数值:" + JsonConvert.SerializeObject(dec) + "\r\n";
               logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
               logstr += "--------------------end---------------------\r\n";
               //写日志
               logger.Info(logstr);
           }
           catch (Exception)
           {
               throw;
           }
       }
       /// <summary>
       /// 捕获全局异常
       /// </summary>
       /// <param name="error"></param>
       public static void LogWriterFromFilter(Exception error)
       {
           try
           {
               string logstr = "\r\n-----------------start----------------------\r\n";
               logstr = logstr + DateTime.Now.ToString() + "\r\n";
               //异常发生地址
               logstr = logstr + "异常发生地址:" + HttpContext.Current.Request.Url.AbsoluteUri.ToString() + "\r\n";
               logstr = logstr + "请求类型:" + HttpContext.Current.Request.RequestType.ToString() + "\r\n";
               
               logstr = logstr + "异常:" + error.ToString() + "\r\n";
               logstr = logstr + "IP:" + YxLiCai.Tools.Util.Helper.GetIP() + "\r\n";
               logstr += "--------------------end---------------------\r\n";
               //发送错误日志邮件
               if (YxLiCai.Tools.Const.SystemConst.IsSendMail == "true")
               {
                   string emailToAddress = YxLiCai.Tools.Const.SystemConst.EmailToAdress;
                   EmailHelper.SendEmailTo(logstr, emailToAddress);
               }              
               //写日志
               logger.Info(logstr);
           }
           catch (Exception)
           {
               throw;
           } 
       }
       /// <summary>
       /// 写日志文件
       /// </summary>
       /// <param name="Type"></param>
       /// <param name="Ptxt"></param>
       public static void Write(string Type, string Ptxt)
       {
           lock (logLock)
           {
               string path = System.AppDomain.CurrentDomain.BaseDirectory;
               path += "\\Log";
               if (!System.IO.Directory.Exists(path))
               {
                   System.IO.Directory.CreateDirectory(path);
               }
               path += "\\" + Type + "(" + DateTime.Now.ToString("yyyyMMdd") + ").log";
               System.IO.File.AppendAllText(path, Ptxt + "\r\n");
           }
       }
    }
} 