/*
 * 手机、验证码实体类
 * 作者：bb
 * 时间：2015年6月1日 10:05:57
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.User
{
    public class Sys_MsgCode
    {
        public Sys_MsgCode()
        { }
        #region Model
        private int _id;
        private string _sendmsg;
        private string _sendaccount;       
        private DateTime _senddate;
        #endregion
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string SendMsg
        {
            set { _sendmsg = value; }
            get { return _sendmsg; }
        }
        /// <summary>
        /// 发送账号
        /// </summary>
        public string SendAccount
        {
            set { _sendaccount = value; }
            get { return _sendaccount; }
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendData
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 过期时间，单位分钟
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// 短信验证是否过期。true:过期，false:不过期。
        /// </summary>
        public bool IsMobileLater
        {
            get
            {
                if (SendData.AddMinutes(Minute) > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
