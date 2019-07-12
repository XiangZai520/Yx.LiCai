﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{ 
    /// <summary>
    /// UserRateCoupon:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserRateCouponModel
    {
        public UserRateCouponModel()
        { }
        #region Model
        private int _id;
        private long _user_id;
        private int? _act_id;
        private int _interest_id;
        private decimal _interest_rate;
        private int _enable_day;
        private DateTime _c_time;
        private DateTime? _use_time;
        private DateTime _e_time;
        private int _use_status;
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
        public int? act_id
        {
            set { _act_id = value; }
            get { return _act_id; }
        }
        /// <summary>
        /// 加息券id
        /// </summary>
        public int interest_id
        {
            set { _interest_id = value; }
            get { return _interest_id; }
        }
        /// <summary>
        /// 加息额度
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
        /// 获得时间
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
        /// 0未使用1使用
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
        /// 
        /// </summary>
        public int? version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        #endregion Model


    }
}
