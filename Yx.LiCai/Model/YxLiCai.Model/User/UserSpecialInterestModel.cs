using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserSpecialInterestModel
    {
        #region Model
        private int _id;
        private DateTime? _c_time;
        private int? _creator_id;
        private DateTime? _m_time;
        private int? _user_special_id;
        private int? _status;
        private decimal? _interest;
        private int? _version;
        private string _remark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
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
        /// 修改时间
        /// </summary>
        public DateTime? m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        /// <summary>
        /// 用户特享金id
        /// </summary>
        public int? user_special_id
        {
            set { _user_special_id = value; }
            get { return _user_special_id; }
        }
        /// <summary>
        /// 类型,0:未付息1:待付息2:已付息
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 已计利息
        /// </summary>
        public decimal? interest
        {
            set { _interest = value; }
            get { return _interest; }
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
        #endregion Model
    }
}
