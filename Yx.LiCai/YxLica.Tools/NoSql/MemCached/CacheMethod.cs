using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Memcached.ClientLibrary;

namespace YxLiCai.Tools.NoSql.MemCached
{
    public class CacheMethod
    {
        static CacheMethod()
        {
        }
        /// <summary>
        /// 向缓存池存入值,数据缓存默认天数
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="Value">缓存内容</param>
        /// <returns></returns>
        public static bool setMemcachedValue(string poolName, string key, string Value)
        {
            DateTime d1 = DateTime.Now.AddDays(double.Parse(CacheConfig.CacheDays));
            return CacheMethod.setMemCacheValues(poolName, key, Value, d1);
        }
        /// <summary>
        /// 向缓存池存入值,数据缓存默认小时数
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存内容</param>
        /// <returns></returns>
        public static bool setMemcacheValueInHour(string poolName, string key, string value)
        {
            DateTime d1 = DateTime.Now.AddHours(double.Parse(CacheConfig.CacheHours));
            return CacheMethod.setMemCacheValues(poolName, key, value, d1);
        }
        /// <summary>
        /// 向缓存池存入值,数据缓存默认分钟数
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存内容</param>
        /// <returns></returns>
        public static bool setMemcacheValueInMinutes(string poolName, string key, string value)
        {
            DateTime d1 = DateTime.Now.AddMinutes(double.Parse(CacheConfig.CacheMinutes));
            return CacheMethod.setMemCacheValues(poolName, key, value, d1);
        }
        /// <summary>
        /// 向缓存池存入值,数据缓存指定时间
        /// 默认缓存1天
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存内容</param>
        /// <param name="cacheTime">缓存时间值</param>
        /// <param name="cacheType">1-天为单位,2小时为单位,3-分钟为单位</param>
        /// <returns></returns>
        public static bool SetMemValues(string poolName, string key, string value, int cacheTime, int cacheType)
        {
            DateTime d1 = DateTime.Now.AddDays(1.0);
            switch (cacheType)
            {
                case 1:
                    d1 = DateTime.Now.AddDays((double)cacheTime);
                    break;
                case 2:
                    d1 = DateTime.Now.AddHours((double)cacheTime);
                    break;
                case 3:
                    d1 = DateTime.Now.AddMinutes((double)cacheTime);
                    break;
            }
            return CacheMethod.setMemCacheValues(poolName, key, value, d1);
        }
        /// <summary>
        /// 向缓存池存入值,数据缓存指定时间
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存内容</param>
        /// <param name="ExpiredTime">失效时间</param>
        /// <returns></returns>
        public static bool SetMemValuesExpiredTime(string poolName, string key, string value, DateTime ExpiredTime)
        {
            return CacheMethod.setMemCacheValues(poolName, key, value, ExpiredTime);
        }
        /// <summary>
        /// 删除指定缓存池的指定键的值
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        public static void DeleteMemcache(string poolName, string key)
        {
            try
            {
                SockIOPool instance = SockIOPool.GetInstance(poolName);
                instance.SetServers(CacheConfig.getMemcachedServerList());
                instance.SocketTimeout = 3000;
                instance.SocketConnectTimeout = 3000;
                instance.Initialize();
                MemcachedClient memcachedClient = new MemcachedClient();
                memcachedClient.PoolName = poolName;
                memcachedClient.EnableCompression = false;
                if (!memcachedClient.KeyExists(key))
                    return;
                memcachedClient.Delete(key);
            }
            catch { }
        }
        /// <summary>
        /// 释放缓存池所有数据
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        public static void FlushAllCache(string poolName)
        {
            try
            {
                new MemcachedClient()
                {
                    PoolName = poolName,
                    EnableCompression = false
                }.FlushAll();
            }
            catch { }
        }
        /// <summary>
        /// 获取缓存池中值
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static string GetMemCacheValues(string poolName, string key)
        {
            try
            {
                string str = "";
                SockIOPool instance = SockIOPool.GetInstance(poolName);
                instance.SetServers(CacheConfig.getMemcachedServerList());
                instance.SocketTimeout = 3000;
                instance.SocketConnectTimeout = 3000;
                instance.Initialize();
                object obj = new MemcachedClient()
                {
                    PoolName = poolName,
                    EnableCompression = false
                }.Get(key);
                if (obj != null)
                    str = obj.ToString();
                return str;
            }
            catch { return string.Empty; }
        }
        /// <summary>
        /// 向缓存池写入数据,指定失效时间
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存内容</param>
        /// <param name="d1">缓存失效时间</param>
        /// <returns></returns>
        private static bool setMemCacheValues(string poolName, string key, string value, DateTime d1)
        {
            try
            {
                SockIOPool instance = SockIOPool.GetInstance(poolName);
                instance.SetServers(CacheConfig.getMemcachedServerList());
                instance.SocketTimeout = 3000;
                instance.SocketConnectTimeout = 3000;
                instance.Initialize();
                return new MemcachedClient()
                {
                    PoolName = poolName,
                    EnableCompression = false
                }.Set(key, (object)value, d1);
            }
            catch { return false; }
        }
        /// <summary>
        /// 仅向缓存池插入键
        /// </summary>
        /// <param name="poolName">缓存池名</param>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static long MemCacheIncrement(string poolName, string key)
        {
            try
            {
                SockIOPool instance = SockIOPool.GetInstance(poolName);
                instance.SetServers(CacheConfig.getMemcachedServerList());
                instance.SocketTimeout = 3000;
                instance.SocketConnectTimeout = 3000;
                instance.Initialize();
                return new MemcachedClient()
                {
                    PoolName = poolName,
                    EnableCompression = false
                }.Increment(key);
            }
            catch { return -1; }
        }
    }
}