using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server.SendMsg
{
    /// <summary>
    /// 短信工厂
    /// </summary>
    public sealed class MessageFactory
    { 
        #region 变量
        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 短信消息
        /// </summary>
        private static BaseMessage baseMessage = null;
        #endregion

        #region 短信息发送类实例
        /// <summary>
        /// 短信息发送类实例
        /// </summary>
        public static BaseMessage Instance
        {
            get
            {
                if (baseMessage == null)
                {
                    lock (lockHelper)
                    {
                        if (baseMessage == null)
                        {
                            try
                            {
                                baseMessage =
                                  (BaseMessage)
                                      Activator.CreateInstance(
                                          Type.GetType("YxLiCai.Server.SendMsg." + System.Configuration.ConfigurationManager.AppSettings["WhiceMessage"]));
                               
                            }
                            catch
                            {
                                throw new Exception("请检查web.config节点短信息通道节点[WhiceMessage]是否正确");
                            }
                        }
                    }
                }
                return baseMessage;
            }
        }
        #endregion

    }

    /// <summary>
    /// 短信息发送类
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        public abstract YxLiCai.Model.ResultInfo<string> SendMessage(string phone, string content);
    } 
}
