using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.DirectoryServices.Protocols;

namespace YxLiCai.Tools
{
    public class FtpHelper
    {
        #region 文件上传
        /// <summary>
        /// Ftp上传文件到指定位置
        /// </summary>
        /// <param name="AimPath">目标路径</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="ftpServerIP">Ftp服务器地址：如ftp://192.168.9.254:21</param>
        /// <param name="ftpUserID">用户名</param>
        /// <param name="ftpPassword">用户密码</param>
        /// <returns></returns>
        public static bool UploadFtp(string AimPath, string filepath, string ftpServerIP, string ftpUserID, string ftpPassword)
        {

            FileInfo fileInf = new FileInfo(filepath);
            string uri = ftpServerIP + "/" + AimPath + "/" + fileInf.Name;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            try
            {
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.ReadWriteTimeout = 10000;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                // 缓存区打下 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {
                reqFTP.Abort();
                return false;
            }

        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹:实现级联创建
        /// 返回：true成功，false失败
        /// </summary>
        /// <param name="AimPathStr">目标目录名，相对路径：如/a/a1/a2</param>
        /// <param name="ftpServerStr">Ftp服务器地址：如ftp://192.168.9.254:21</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns></returns>
        public static bool CreateFtpCascadeDirectory(string AimPathStr, string ftpServerStr, string ftpUserID, string ftpPassword)
        {
            string[] AimPathArray = AimPathStr.TrimStart('/').Split('/');
            string AimUriCache = string.Empty;
            for (int i = 0; i < AimPathArray.Length; i++)
            {
                AimUriCache += "/" + AimPathArray[i];
                if (CreateFtpDirectory(AimUriCache, ftpServerStr, ftpUserID, ftpPassword))
                {
                    continue;
                }
                else
                {
                    if (CreateFtpDirectory(AimUriCache, ftpServerStr, ftpUserID, ftpPassword))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 创建文件夹 返回：true成功，false失败
        /// </summary>
        /// <param name="AimPathStr">目标目录名，相对路径：如/a</param>
        /// <param name="ftpServerStr">Ftp服务器地址：如ftp://192.168.9.254:21</param>
        /// <param name="ftpUserID">用户名</param>
        /// <param name="ftpPassword">用户密码</param>
        /// <returns></returns>
        public static bool CreateFtpDirectory(string AimPathStr, string ftpServerStr, string ftpUserID, string ftpPassword)
        {
            Uri BaseUri = new Uri(ftpServerStr);
            Uri uri = new Uri(BaseUri, AimPathStr);
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
            reqFTP.KeepAlive = true;
            reqFTP.Timeout = 2000;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse FtpResponse = (FtpWebResponse)reqFTP.GetResponse();
                if (FtpResponse.StatusCode == FtpStatusCode.PathnameCreated)
                {
                    FtpResponse.Close();
                    return true;
                }
                else
                {
                    FtpResponse.Close();
                    return false;
                }
            }
            catch (WebException e)
            {
                FtpWebResponse FtpResponse = (FtpWebResponse)e.Response;
                if (FtpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    //文件已存在，返回True
                    FtpResponse.Close();
                    return true;
                }
                else
                {
                    FtpResponse.Close();
                    return false;
                }
            }
        }
        #endregion

        #region 文件删除
        /// <summary>  
        /// FTP删除文件  
        /// </summary>  
        /// <param name="filePath">ftp删除文件路径,相对于根目录的相对路径：如/a/a1/a2.jpg</param>  
        /// <param name="ftpServerStr">Ftp服务器地址：如ftp://192.168.9.254:21</param>  
        /// <param name="ftpUserID">ftpUserID用户名</param>  
        /// <param name="ftpPassword">ftpPassword密码</param>  
        /// <returns></returns>  
        public static bool DeleteFile(string filePath, string ftpServerStr, string ftpUserID, string ftpPassword)
        {
            bool sRet = false;
            FtpWebResponse Respose = null;
            FtpWebRequest reqFTP = null;
            try
            {
                //根据uri创建FtpWebRequest对象    
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(string.Format(@"{0}{1}", ftpServerStr, filePath)));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                //默认为true是上传完后不会关闭FTP连接    
                reqFTP.KeepAlive = false;
                //执行删除操作  
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                Respose = (FtpWebResponse)reqFTP.GetResponse();
                if (Respose.StatusCode == FtpStatusCode.FileActionOK)
                {
                    Respose.Close();
                    sRet = true;
                }
                else
                {
                    Respose.Close();
                    sRet = false;
                }
            }
            catch (WebException e)
            {
                FtpWebResponse Response = (FtpWebResponse)e.Response;
                if (Response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Response.Close();
                    sRet = true;
                }
                else
                {
                    Response.Close();
                    sRet = false;
                }
            }
            return sRet;
        }
        #endregion
    }
}
