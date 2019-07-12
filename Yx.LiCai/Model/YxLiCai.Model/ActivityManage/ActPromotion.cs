using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ActivityManage
{
    /// <summary>
    /// 活动主表
    /// </summary>
    public class ActPromotion
    {
        public ActPromotion()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _ad_content;
		private DateTime _s_time;
		private DateTime _e_time;
		private int _status=0;
		private int _type=0;
        private int _limit_num = 0;
        private int _max_user_num = 0;
        private int _curt_user_num = 0;
		private DateTime _c_time=DateTime.Now;
		private int? _creator_id=0;
		private DateTime? _m_time;
		private int? _m_id=0;
		private int? _version=0;
		private string _remark;
        private decimal _budget;
        private string _url;
        private int _is_delete = 0;

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
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ad_content
		{
			set{ _ad_content=value;}
			get{return _ad_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime s_time
		{
			set{ _s_time=value;}
			get{return _s_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime e_time
		{
			set{ _e_time=value;}
			get{return _e_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int limit_num
        {
            set { _limit_num = value; }
            get { return _limit_num; }
        }
		/// <summary>
		/// 
		/// </summary>
        public int max_user_num
		{
            set { _max_user_num = value; }
            get { return _max_user_num; }
		}
        /// <summary>
        /// 
        /// </summary>
        public int curt_user_num
        {
            set { _curt_user_num = value; }
            get { return _curt_user_num; }
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
        //private decimal _budget;
        //private string _url;
        /// <summary>
        /// 
        /// </summary>
        public string url
        {
            set { _url = value; }
            get { return _url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal budget
        {
            set { _budget = value; }
            get { return _budget; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int is_delete
        {
            set { _is_delete = value; }
            get { return _is_delete; }
        }
		#endregion Model
    }
}
