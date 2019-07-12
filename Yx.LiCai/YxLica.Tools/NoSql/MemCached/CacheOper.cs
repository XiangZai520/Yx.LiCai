using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.NoSql.MemCached
{
    public class CacheOper
    {
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="PerKey">缓存key</param>
        /// <param name="Data">数据</param>
        /// <param name="CM">配置对象</param>
        public static void SetCache(string poolName, string PerKey, string Data, CacheModel CM)
        {
            if (CM != null)
            {
                if (CM.Cachetype != 4)
                {
                    CacheMethod.SetMemValues(poolName, PerKey, Data, CM.CacheTime, CM.Cachetype);
                }
                else
                {
                    CacheMethod.SetMemValuesExpiredTime(poolName, PerKey, Data, CM.ExpiredTime);
                }
            }
            else
            {
                CacheMethod.setMemcachedValue(poolName, PerKey, Data);
            }

        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="poolName">池名</param>
        /// <param name="PerKey">缓存中的键</param>
        public static string GetCache(string poolName, string PerKey)
        {
            string data = string.Empty;
            try
            {
                object po = CacheMethod.GetMemCacheValues(poolName, PerKey);
                if (po != null && !string.IsNullOrEmpty(po.ToString()))
                {
                    data = po.ToString();
                }
            }
            catch { }
            return data;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="Key">缓存key</param>
        /// <returns></returns>
        public static string GetCache(string Key)
        {
            string data = string.Empty;
            try
            {
                object po = CacheMethod.GetMemCacheValues("POOL", Key);
                if (po != null && !string.IsNullOrEmpty(po.ToString()))
                {
                    data = po.ToString();
                }
            }
            catch { }
            return data;
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="data">数据</param>
        /// <param name="CacheTime">时间</param>
        /// <param name="Cachetype">1-天为单位,2小时为单位,3-分钟为单位4-指定时间</param>
        /// <param name="ExpiredTime">指定时间</param>
        public static void SetCache(string key, string data, int CacheTime, int Cachetype, DateTime ExpiredTime)
        {
            string poolName = "POOL";
            if (Cachetype != 4)
            {
                CacheMethod.SetMemValues(poolName,key,data, CacheTime,Cachetype);
            }
            else
            {
                CacheMethod.SetMemValuesExpiredTime(poolName, key, data,ExpiredTime);
            }
        }
    }
}