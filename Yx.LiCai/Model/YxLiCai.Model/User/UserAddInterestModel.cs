using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserAddInterestModel
    {
        #region Model
        private int _id;
        private long _user_id;
        private int _act_id;
        private int _interest_id;
        private decimal _interest_rate = 0.00000000M;
        private int _enable_day = 0;
        private DateTime _c_time;
        private DateTime? _use_time;
        private DateTime _e_time;
        private int _use_status = 0;
        private int? _invest_id;
        private int? _version;
        private string _remark;
        private int? _creator_id;
        private DateTime? _m_time;
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
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 活动id
        /// </summary>
        public int act_id
        {
            set { _act_id = value; }
            get { return _act_id; }
        }
        /// <summary>
        /// 加息券规则id
        /// </summary>
        public int interest_id
        {
            set { _interest_id = value; }
            get { return _interest_id; }
        }
        /// <summary>
        /// 加息券利率
        /// </summary>
        public decimal interest_rate
        {
            set { _interest_rate = value; }
            get { return _interest_rate; }
        }
        /// <summary>
        /// 有效期
        /// </summary>
        public int enable_day
        {
            set { _enable_day = value; }
            get { return _enable_day; }
        }
        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? use_time
        {
            set { _use_time = value; }
            get { return _use_time; }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime e_time
        {
            set { _e_time = value; }
            get { return _e_time; }
        }
        /// <summary>
        /// 使用状态0未使用1已使用
        /// </summary>
        public int use_status
        {
            set { _use_status = value; }
            get { return _use_status; }
        }
        /// <summary>
        /// 投资id
        /// </summary>
        public int? invest_id
        {
            set { _invest_id = value; }
            get { return _invest_id; }
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
        /// 创建人id
        /// </summary>
        public int? creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        #endregion Model
    }
}
