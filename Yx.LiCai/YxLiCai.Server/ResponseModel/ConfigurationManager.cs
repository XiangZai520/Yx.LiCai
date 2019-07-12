using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Tools;

namespace YxLiCai.Server.Common
{
    public class ConfigurationManagerServer
    {
        public List<KeyValuePair> GetConfig(string configName, string nodeName)
        {
            var filePath = Config.XmlDataPath() + configName + ".config";
            var configAll = System.IO.File.ReadAllText(filePath).Split('*').ToList();

            //删除换行
            try
            {
                return XmlHelper.XmlDeserialize<KeyValuePairList>(configAll.First(m => m.Contains(nodeName)).Replace("\n", "").Replace("\r", ""),
                    nodeName, Encoding.UTF8).list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
