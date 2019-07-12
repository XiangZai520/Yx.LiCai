using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace YxLiCai.Tools.SafeEncrypt
{
    /// <summary>
    /// DES���ܷ���
    /// </summary>
    public class DES
    {
        private const string DES_KEY = "FDSFIojslsk;fjlk;)*(+nmjdsf$#@dsf54641#&*(()";

        /// <summary>
        /// �����ַ���
        /// ע��:��Կ����Ϊ��λ
        /// </summary>
        /// <param name="inputString">�ַ���</param>
        /// <param name="encryptKey">��Կ</param>
        /// <returns></returns>
        public static string DesEncrypt(string inputString, string encryptKey)
        {

            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                inputString = Convert.ToBase64String(ms.ToArray());
            }
            catch { }
            return inputString;
        }


        /// <summary>
        /// �����ַ���
        /// ע��:��Կ����Ϊ��λ
        /// </summary>
        /// <param name="inputString">�����ܵ��ַ���</param>
        /// <param name="decryptKey">������Կ</param>
        /// <returns></returns>
        public static string DesDecrypt(string inputString, string decryptKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[inputString.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(inputString);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                inputString = encoding.GetString(ms.ToArray());
            }
            catch { }
            return inputString;
        }


        /// <summary>
        /// 3des�����ַ���
        /// </summary>
        /// <param name="plainText">����</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>���ܺ󲢾�base63������ַ���</returns>
        /// <remarks>���أ�ָ�����뷽ʽ</remarks>
        public static string Encrypt3DES(string plainText, Encoding encoding)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
            var DES = new
                TripleDESCryptoServiceProvider();
            var hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(DES_KEY));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = encoding.GetBytes(plainText);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock
                                              (Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// 3des�����ַ���
        /// </summary>
        /// <param name="plainText">����</param>
        /// <returns>���ܺ󲢾�base63������ַ���</returns>
        public static string Encrypt3DES(string plainText)
        {
            return Encrypt3DES(plainText, Encoding.Default);
        }

        /// <summary>
        /// 3des�����ַ���
        /// </summary>
        /// <param name="entryptText">����</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string Decrypt3DES(string entryptText)
        {
            return Decrypt3DES(entryptText, Encoding.Default);
        }

        /// <summary>
        /// 3des�����ַ���
        /// </summary>
        /// <param name="entryptText">����</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <remarks>��̬������ָ�����뷽ʽ</remarks>
        public static string Decrypt3DES(string entryptText, Encoding encoding)
        {
            var DES = new
                TripleDESCryptoServiceProvider();
            var hashMD5 = new MD5CryptoServiceProvider();

            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(DES_KEY));
            DES.Mode = CipherMode.ECB;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result;
            try
            {
                byte[] Buffer = Convert.FromBase64String(entryptText);
                result = encoding.GetString(DESDecrypt.TransformFinalBlock
                                                (Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {
                throw (new Exception("Invalid Key or input string is not a valid base64 string", e));
            }
            return result;
        }


    }
}