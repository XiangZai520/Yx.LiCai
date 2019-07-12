using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserInvitationModel
    {
        #region Model
        private int _id;
        private long? _user_id;
        private long? _invited_user_id;
        private DateTime? _c_time;
        private DateTime? _m_time;
        private int? _creator_id;
        private int? _version;
        private string _remark;
        private int _act_id = 0;
        private string _invited_login_name;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public long? user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 被邀请人id
        /// </summary>
        public long? invited_user_id
        {
            set { _invited_user_id = value; }
            get { return _invited_user_id; }
        }
        /// <summary>
        /// 邀请时间
        /// </summary>
        public DateTime? c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        /// <summary>
        /// 创建人id
        /// </summary>
        public int? creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int? version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 活动ID
        /// </summary>
        public int act_id
        {
            set { _act_id = value; }
            get { return _act_id; }
        }
        /// <summary>
        /// 被邀请人登录名
        /// </summary>
        public string invited_login_name {
            set { _invited_login_name = value; }
            get { return _invited_login_name; }
        }

        #endregion Model
    }
}
