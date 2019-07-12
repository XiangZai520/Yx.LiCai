using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace YxLiCai.Tools
{
    public class Config
    {
        public static string LogPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["LogPath"];
        }
        public static string XmlDataPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["DataPath"];
        }
        public static string UploadPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
        }
        public static List<string> BingSearchStrategyList()
        {
            var list = System.Configuration.ConfigurationManager.AppSettings["BingSearchStrategyList"];
            if (list.Trim().Length == 0) return new List<string>();
            return list.Trim().Split(' ').ToList();
        }
        public static string FinancialFigurePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["FinancialFigurePath"];
        }
 
    }
}
