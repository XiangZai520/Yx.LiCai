﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace YxLiCai.Tools
{
    public class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        public static bool SendEmailTo(string body, string emailAddress, bool throwOnError = false)
        {
            return SendEmailTo(body, emailAddress, "来自yxlc预警：" + Environment.MachineName, "", false, null);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        public static bool SendEmailTo(string body, string emailAddress, string copyto, bool throwOnError = false)
        {
            return SendEmailTo(body, emailAddress, "来自yxlc预警:" + Environment.MachineName, copyto, false, null);
        }

        public static bool SendEmailTo<ExportList>(string body, string emailAddress, string title, string copyto, bool isBodyHtml, string attachment, Dictionary<string, string> dicHead, IList<ExportList> exportList)
        {
            using (var ms = new MemoryStream())
            {
                //生成excel文件
                //ExcelHelper.ExportExcel(ms, exportList, dicHead);
                return SendEmailTo(body, emailAddress, title, copyto, isBodyHtml, stream: ms, attachName: attachment);
            }

        }

        /// <summary>
        ///  发送邮件<B>(Dictionary<string, string> dicHead, IList<B> b)
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="emailAddress">收件人地址</param>
        /// <param name="title">标题</param>
        /// <param name="copyto">抄送</param>
        /// <param name="isBodyHtml">邮件格式</param>
        /// <param name="throwOnError">当发送邮件过程中出现异常，是否抛出异常。</param>
        /// <param name="displayName">显示的名称</param>
        public static bool SendEmailTo(string body, string emailAddress, string title, string copyto, bool isBodyHtml, Stream stream = null, bool throwOnError = false, string displayName = "e休理财预警", string attachName = "附件.xls")
        {
            
            try
            {
                string address = "";
                string[] mailNames = emailAddress.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
                var from = new MailAddress(YxLiCai.Tools.Const.SystemConst.EmailFromAdress, displayName);
                var client = new SmtpClient
                {
                    Host = "smtp.exmail.qq.com",
                    Credentials = new NetworkCredential(YxLiCai.Tools.Const.SystemConst.EmailFromAdress, ETS.Util.EncodeAndDecode.DecodeBase64(Encoding.UTF8, YxLiCai.Tools.Const.SystemConst.EmailPwd)),
                    Port = 25
                };
                var mail = new MailMessage();
                mail.From = from;
                foreach (string name in mailNames)
                {
                    if (name != string.Empty)
                    {
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mail.To.Add(new MailAddress(address, displayName));
                    }
                }
                mail.Subject = title;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = isBodyHtml;
                //抄送
                if (!string.IsNullOrEmpty(copyto)) mail.CC.Add(copyto);
                //附件
                if (stream != null)
                {
                    mail.Attachments.Add(new Attachment(stream, attachName));
                }
                // 回复至
                mail.ReplyToList.Add(YxLiCai.Tools.Const.SystemConst.EmailFromAdress);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                return true;
            }
            catch
            {
                if (throwOnError)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
