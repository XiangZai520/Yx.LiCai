using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Configuration;
namespace YxLiCai.Tools.NoSql.MemCached
{
    /// <summary>
    ///CacheConfig 的摘要说明
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// 应用程序目录
        /// </summary>
        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private static string filestr = string.Empty;
        private static string CacheServerstr = string.Empty;
        public static string CacheDays = System.Configuration.ConfigurationManager.AppSettings["CacheDays"] ?? "2";
        public static readonly string CacheHours = System.Configuration.ConfigurationManager.AppSettings["CacheHours"] ?? "1";
        public static readonly string CacheMinutes = System.Configuration.ConfigurationManager.AppSettings["CacheMintes"] ?? "30";
        static CacheConfig()
        {

        }
        /// <summary>
        ///  给定一个节点的属性值
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="poolname">池名称</param>
        /// <param name="key">节点属性值</param>
        /// <returns>返回节点对象</returns>
        public static CacheModel GetCacheModel(string version, string poolname, string key)
        {
            XmlDocument BaseXmldoc = new XmlDocument();
            XmlDocument xmldoc = new XmlDocument();
            string filestr = string.Empty;
            filestr = path + "App_Data\\" + version + "\\CacheConfig.xml";
            BaseXmldoc.LoadXml(File.ReadAllText(filestr));
            //节点对象
            XmlNode xmlnode = null;
            xmlnode = BaseXmldoc.SelectSingleNode("CONFIG/CACHES[@POOLNAME='" + poolname + "']/CACHE[@KEY=\"" + key + "\"]");

            //扩展配置文件列表
            List<string> ExtendXmlList = new List<string>();
            if (xmlnode == null)
            {
                XmlNodeList ExtendNodeList = BaseXmldoc.SelectNodes("CONFIG/EXTENDS/INCLUDE");
                if (ExtendNodeList != null && ExtendNodeList.Count > 0)
                {
                    foreach (XmlNode node in ExtendNodeList)
                    {
                        if (node.Attributes["SRC"] != null)
                        {
                            string ExtendFilePath = node.Attributes["SRC"].Value;
                            ExtendXmlList.Add(ExtendFilePath);
                        }
                    }
                    foreach (string filePath in ExtendXmlList)
                    {
                        try
                        {
                            string ExtendXmlContent = File.ReadAllText(path + "App_Data\\" + version+"\\" + filePath);
                            xmldoc.LoadXml(ExtendXmlContent);
                            xmlnode = xmldoc.SelectSingleNode("CONFIG/CACHES[@POOLNAME='" + poolname + "']/CACHE[@KEY=\"" + key + "\"]");
                        }
                        catch { }
                        if (xmlnode != null)
                        {
                            break;
                        }
                    }
                }
            }

            if (xmlnode != null)
            {
                CacheModel pm = new CacheModel();
                pm.Key = key;
                pm.Cachetype = int.Parse(xmlnode.Attributes["CACHETYPE"].Value);
                if (pm.Cachetype == 4)
                {
                    pm.ExpiredTime = DateTime.Parse(xmlnode.Attributes["CACHETIME"].Value);
                }
                else
                {
                    pm.CacheTime = int.Parse(xmlnode.Attributes["CACHETIME"].Value);
                }
                return pm;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 服务器简单调用数据缓存配置获取
        /// </summary>
        /// <param name="poolname">池名称</param>
        /// <param name="key">节点属性值</param>
        /// <returns>返回节点对象</returns>
        public static CacheModel GetCommonCacheModel(string poolname, string key)
        {
            XmlDocument BaseXmldoc = new XmlDocument();
            string filestr = string.Empty;
            filestr = path + "App_Data\\CommonDataCache.xml";
            BaseXmldoc.LoadXml(File.ReadAllText(filestr));
            //节点对象
            XmlNode xmlnode = null;
            xmlnode = BaseXmldoc.SelectSingleNode("CONFIG/CACHES[@POOLNAME='" + poolname + "']/CACHE[@KEY=\"" + key + "\"]");
            if (xmlnode != null)
            {
                CacheModel pm = new CacheModel();
                pm.Key = key;
                pm.Cachetype = int.Parse(xmlnode.Attributes["CACHETYPE"].Value);
                if (pm.Cachetype == 4)
                {
                    pm.ExpiredTime = DateTime.Parse(xmlnode.Attributes["CACHETIME"].Value);
                }
                else
                {
                    pm.CacheTime = int.Parse(xmlnode.Attributes["CACHETIME"].Value);
                }
                return pm;
            }
            else
            {
                return null;
            }
        }

        #region 缓存服务器列表
        /// <summary>
        /// 获取缓存服务器列表
        /// </summary>
        /// <returns></returns>
        public static List<XmlNode> FindCacheServerList()
        {
            XmlDocument BaseXmldoc = new XmlDocument();
            string CacheServerstr = string.Empty;
            CacheServerstr = path + "App_Data\\CacheServer.xml";
            BaseXmldoc.LoadXml(File.ReadAllText(CacheServerstr));
            List<XmlNode> xmlnodelist = new List<XmlNode>();
            //最终返回的节点对象
            XmlNodeList ilist = BaseXmldoc.SelectNodes("CONFIG/LISTS/LIST");
            foreach (XmlNode xn in ilist)
            {
                xmlnodelist.Add(xn);
            }
            return xmlnodelist;
        }
        /// <summary>
        /// 返回缓存服务器列表,缓存服务器列表配置在.config中的AppSettings节点,节点名MemcachedList,多个服务器用,分割
        /// </summary>
        /// <returns></returns>
        public static string[] getMemcachedServerList()
        {

            List<XmlNode> xmlnodelist = FindCacheServerList();

            if (xmlnodelist != null && xmlnodelist.Count > 0)
            {
                string[] MemcachedLists = new string[xmlnodelist.Count];
                for (int i = 0; i < xmlnodelist.Count; i++)
                {
                    XmlNode node = xmlnodelist[i];
                    MemcachedLists[i] = node.Attributes["IP"].Value + ":" + node.Attributes["PORT"].Value;
                }
                return MemcachedLists;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}