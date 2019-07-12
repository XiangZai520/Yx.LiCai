/*
 * 序列化工具类
 * 作者：平扬
 * 时间：2014-1-11
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace YxLiCai.Tools
{
    public static class SerializeHelper
    {
        #region 【json格式化】
        /// <summary>
        /// 将对象序列化为Json格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JsonSerializer(object data)
        {
            return JsonSerializer(data, true);
        }
        /// <summary>
        /// 将对象序列化为Json格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string JsonSerializer(object data, bool SerializeNullValues)
        {
            fastJSON.JSONParameters jsonParam = new fastJSON.JSONParameters();
            jsonParam.SerializeNullValues = SerializeNullValues;
            jsonParam.ShowReadOnlyProperties = true;
            jsonParam.EnableAnonymousTypes = true;
            return fastJSON.JSON.Instance.ToJSON(data, jsonParam);
        }

        /// <summary>
        /// 将Json反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json)
        {
            return fastJSON.JSON.Instance.ToObject<T>(json);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object JsonDeserialize(string json)
        {
            return fastJSON.JSON.Instance.Parse(json);
        }
        #endregion

        #region 实体类和XML转化

        /// <summary>
        /// 对象转换成XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static string ToXml<T>(this T t, string rootName = null) where T : class,new()
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(t.GetType());
            try
            {
                //序列化对象  
                xml.Serialize(Stream, t);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
    }
}
