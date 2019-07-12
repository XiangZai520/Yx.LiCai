using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ActivityManage
{
    /// <summary>
    /// 活动加息券
    /// </summary>
    public class ActInterestCoupon
    {
        public ActInterestCoupon()
		{}
		#region Model
		private int _id;
		private decimal _interest_rate=0.00000000M;
		private int _enable_day=0;
		private string _use_condition;
		private DateTime _c_time=DateTime.Now;
		private int? _creator_id=0;
		private DateTime? _m_time;
		private int? _m_id=0;
		private int? _version=0;
		private string _remark= "";
        private string _name = "";
        private int _status = 0;
        private int _is_delete = 0;
        private int _send_count = 0;
        private int _use_count = 0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int send_count
        {
            set { _send_count = value; }
            get { return _send_count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int use_count
        {
            set { _use_count = value; }
            get { return _use_count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int is_delete
        {
            set { _is_delete = value; }
            get { return _is_delete; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
		/// <summary>
		/// 
		/// </summary>
		public decimal interest_rate
		{
			set{ _interest_rate=value;}
			get{return _interest_rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int enable_day
		{
			set{ _enable_day=value;}
			get{return _enable_day;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string use_condition
		{
			set{ _use_condition=value;}
			get{return _use_condition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime c_time
		{
			set{ _c_time=value;}
			get{return _c_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? creator_id
		{
			set{ _creator_id=value;}
			get{return _creator_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? m_time
		{
			set{ _m_time=value;}
			get{return _m_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? m_id
		{
			set{ _m_id=value;}
			get{return _m_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model
    }
}
