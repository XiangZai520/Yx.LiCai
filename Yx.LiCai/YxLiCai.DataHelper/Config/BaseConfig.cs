using System;
using System.Xml;
using System.Web;
using System.Configuration;
using System.Web.Caching;
namespace YxLiCai.DataHelper.Config
{
    /// <summary>
    /// 基础配置文件
    /// </summary>
    internal static class BaseConfig
    {
        
        #region 数据层类型
        /// <summary>
        /// 数据层类型
        /// </summary>
        //private static readonly string _BaseType = ConfigurationManager.ConnectionStrings["DataType"].ToString();
        private static readonly string _BaseType = "SqlServer";
        /// <summary>
        /// 数据层类型
        /// </summary>
        public static string BaseType
        {
            get
            {
                return _BaseType;
            }
            
        }
        #endregion

        #region 存储过程配置文件
        /// <summary>
        /// 存储过程配置文件
        /// </summary>
        private static string XmlPath = string.Empty;
        /// <summary>
        /// 存储过程配置文件
        /// </summary>
        public static XmlDocument ProcConfig
        {
            get
            {
                if (HttpContext.Current.Cache["XmlConfig"] == null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(XmlPath);
                    HttpRuntime.Cache.Insert("XmlConfig", xmlDocument, new CacheDependency(XmlPath),Cache.NoAbsoluteExpiration,Cache.NoSlidingExpiration,CacheItemPriority.High,null);
                    DataHelper.Parameters.SqlServerParameter.ClearCachePrameters();
                }
                return HttpRuntime.Cache["XmlConfig"] as XmlDocument;
            }
        }
        #endregion

        #region 静态构造函数
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BaseConfig()
        {
            if (HttpContext.Current != null)
            {
                XmlPath = HttpContext.Current.Server.MapPath("~/App_Data/DataBase.xml");
            }
        }
        #endregion
    }
}
