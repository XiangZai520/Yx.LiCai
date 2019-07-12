using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace YxLiCai.Tools.SafeEncrypt
{
    /// <summary>
    /// MD5加密服务
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// 使用MD5加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>返回加密后的字节</returns>
        public static byte[] MD5Encrypt(byte[] data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            return bytes;
        }

        /// <summary>
        /// 使用MD5加密字符串
        /// </summary>
        /// <param name="data">要加密的字符</param>
        /// <returns>返回加密后的字符</returns>
        public static string MD5StringEncrypt(string data)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(data);
            bytes = MD5Encrypt(bytes);
            data = System.Text.Encoding.Default.GetString(bytes);
            return data;
        }

        /// <summary>
        /// 使用MD5加密字符串。
        /// 字节数组中的偏移量，即从该位置开始使用数据。4
        /// 数组中用作数据的字节数。8
        /// </summary>
        /// <param name="data">要加密的字符</param>
        /// <returns></returns>
        public static string MD5StringEncrypt4_8(string data)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(data);
            bytes = MD5Encrypt(bytes);
            data = BitConverter.ToString(bytes, 4, 8);
            data = data.Replace("-", "");
            return data;
        }


        /// <summary>
        /// 使用MD5加密字符串
        /// </summary>
        /// <param name="data">要加密的字符</param>
        /// <param name="encode">要使用的编码</param>
        /// <returns>返回加密后的字符</returns>
        public static string MD5StringEncrypt(string data, System.Text.Encoding encode)
        {
            byte[] bytes = encode.GetBytes(data);
            bytes = MD5Encrypt(bytes);
            data = encode.GetString(bytes);
            return data;
        }

        ///  <summary> 
        /// MD5加密算法 
        ///  </summary> 
        ///  <param name="str">字符串 </param> 
        ///  <param name="code">加密方式,16或32 </param> 
        ///  <returns> </returns> 
        public static string MD5Convert(string str)
        {
            return MD5Convert(str, 16);
        }
        ///  <summary> 
        /// MD5加密算法 
        ///  </summary> 
        ///  <param name="str">字符串 </param> 
        ///  <param name="code">加密方式,16或32 </param> 
        ///  <returns> </returns> 
        public static string MD5Convert(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper().Substring(8, 16);
            }
            else//32位加密  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
            }
        }
    }
}