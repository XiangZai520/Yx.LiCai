using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace YxLiCai.Tools.SafeEncrypt
{
    /// <summary>
    /// MD5���ܷ���
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// ʹ��MD5��������
        /// </summary>
        /// <param name="data">Ҫ���ܵ�����</param>
        /// <returns>���ؼ��ܺ���ֽ�</returns>
        public static byte[] MD5Encrypt(byte[] data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            return bytes;
        }

        /// <summary>
        /// ʹ��MD5�����ַ���
        /// </summary>
        /// <param name="data">Ҫ���ܵ��ַ�</param>
        /// <returns>���ؼ��ܺ���ַ�</returns>
        public static string MD5StringEncrypt(string data)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(data);
            bytes = MD5Encrypt(bytes);
            data = System.Text.Encoding.Default.GetString(bytes);
            return data;
        }

        /// <summary>
        /// ʹ��MD5�����ַ�����
        /// �ֽ������е�ƫ���������Ӹ�λ�ÿ�ʼʹ�����ݡ�4
        /// �������������ݵ��ֽ�����8
        /// </summary>
        /// <param name="data">Ҫ���ܵ��ַ�</param>
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
        /// ʹ��MD5�����ַ���
        /// </summary>
        /// <param name="data">Ҫ���ܵ��ַ�</param>
        /// <param name="encode">Ҫʹ�õı���</param>
        /// <returns>���ؼ��ܺ���ַ�</returns>
        public static string MD5StringEncrypt(string data, System.Text.Encoding encode)
        {
            byte[] bytes = encode.GetBytes(data);
            bytes = MD5Encrypt(bytes);
            data = encode.GetString(bytes);
            return data;
        }

        ///  <summary> 
        /// MD5�����㷨 
        ///  </summary> 
        ///  <param name="str">�ַ��� </param> 
        ///  <param name="code">���ܷ�ʽ,16��32 </param> 
        ///  <returns> </returns> 
        public static string MD5Convert(string str)
        {
            return MD5Convert(str, 16);
        }
        ///  <summary> 
        /// MD5�����㷨 
        ///  </summary> 
        ///  <param name="str">�ַ��� </param> 
        ///  <param name="code">���ܷ�ʽ,16��32 </param> 
        ///  <returns> </returns> 
        public static string MD5Convert(string str, int code)
        {
            if (code == 16) //16λMD5���ܣ�ȡ32λ���ܵ�9~25�ַ���  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper().Substring(8, 16);
            }
            else//32λ����  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
            }
        }
    }
}