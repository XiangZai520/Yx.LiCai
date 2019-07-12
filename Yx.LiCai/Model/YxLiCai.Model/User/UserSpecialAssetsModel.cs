using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{ 
    /// <summary>
    /// user_special_assets:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserSpecialAssetsModel
    {
        public UserSpecialAssetsModel()
        { }
        #region Model
        private int _id;
        private long? _user_id;
        private int? _act_id;
        private int _special_id;
        private decimal _amount = 0.00000000M;
        private DateTime _c_time;
        private DateTime _u_time;
        private DateTime _e_time;
        private int _enable_day = 0;
        private int _use_status = 0;
        private decimal _rate = 0.00000000M;
        private decimal _rate_increase = 0.00000000M;
        private decimal _rate_added = 0.00000000M;
        private decimal _interest_added = 0.00000000M;
        private decimal _interest_paid = 0.00000000M;
        private DateTime? _m_time;
        private int? _creator_id;
        private int? _version;
        private string _remark;
        private long? _invited_user_id;
        private string _invited_user_name;
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
        /// 活动id
        /// </summary>
        public int? act_id
        {
            set { _act_id = value; }
            get { return _act_id; }
        }
        /// <summary>
        /// 特享金规则id
        /// </summary>
        public int special_id
        {
            set { _special_id = value; }
            get { return _special_id; }
        }
        /// <summary>
        /// 面值
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
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
        public DateTime u_time
        {
            set { _u_time = value; }
            get { return _u_time; }
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
        /// 过期期限
        /// </summary>
        public int enable_day
        {
            set { _enable_day = value; }
            get { return _enable_day; }
        }
        /// <summary>
        /// 使用状态1已使用2已过期
        /// </summary>
        public int use_status
        {
            set { _use_status = value; }
            get { return _use_status; }
        }
        /// <summary>
        /// 特享金基础利率
        /// </summary>
        public decimal rate
        {
            set { _rate = value; }
            get { return _rate; }
        }
        /// <summary>
        /// 每月递增利率
        /// </summary>
        public decimal rate_increase
        {
            set { _rate_increase = value; }
            get { return _rate_increase; }
        }
        /// <summary>
        /// 特享金增加利率
        /// </summary>
        public decimal rate_added
        {
            set { _rate_added = value; }
            get { return _rate_added; }
        }
        /// <summary>
        /// 已计利息
        /// </summary>
        public decimal interest_added
        {
            set { _interest_added = value; }
            get { return _interest_added; }
        }
        /// <summary>
        /// 已付利息
        /// </summary>
        public decimal interest_paid
        {
            set { _interest_paid = value; }
            get { return _interest_paid; }
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
        /// 被邀请人id
        /// </summary>
        public long? invited_user_id
        {
            set { _invited_user_id = value; }
            get { return _invited_user_id; }
        }
        /// <summary>
        /// 被邀请人账号
        /// </summary>
        public string invited_user_name
        {
            set { _invited_user_name = value; }
            get { return _invited_user_name; }
        }
        #endregion Model
    }
}
